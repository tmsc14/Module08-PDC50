using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module08.Model;
using Module08.Services;
using Microsoft.Maui.Controls;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace Module08.ViewModel
{
    [QueryProperty(nameof(Student), "Student")]
    public class StudentDetailsViewModel : BindableObject
    {
        private Student _student;
        private ObservableCollection<Grade> _studentGrades;
        private readonly GradeService _gradeService;
        private decimal _gpa;
        private string _gradeSummary;

        public Student Student
        {
            get => _student;
            set
            {
                _student = value;
                OnPropertyChanged();
                LoadGrades();
            }
        }

        public ObservableCollection<Grade> StudentGrades
        {
            get => _studentGrades;
            set
            {
                _studentGrades = value;
                OnPropertyChanged();
            }
        }

        public decimal GPA
        {
            get => _gpa;
            set
            {
                _gpa = value;
                OnPropertyChanged();
            }
        }

        public string GradeSummary
        {
            get => _gradeSummary;
            set
            {
                _gradeSummary = value;
                OnPropertyChanged();
            }
        }

        public ICommand GoBackCommand { get; }
        public ICommand AddGradeCommand { get; }
        public ICommand RefreshGradesCommand { get; }
        public ICommand EditGradeCommand { get; }
        public ICommand DeleteGradeCommand { get; }

        public StudentDetailsViewModel()
        {
            _gradeService = new GradeService();
            StudentGrades = new ObservableCollection<Grade>();

            GoBackCommand = new Command(async () => await GoBack());
            AddGradeCommand = new Command(async () => await NavigateToAddGrade());
            RefreshGradesCommand = new Command(async () => await LoadGrades());
            EditGradeCommand = new Command<Grade>(async (grade) => await EditGrade(grade));
            DeleteGradeCommand = new Command<Grade>(async (grade) => await DeleteGrade(grade));
        }

        private async Task GoBack()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async Task NavigateToAddGrade()
        {
            if (Student != null)
            {
                await Shell.Current.GoToAsync("AddGradePage?StudentId=" + Student.StudentID);
            }
        }

        private async Task LoadGrades()
        {
            if (Student != null)
            {
                try
                {
                    var grades = await _gradeService.GetStudentGradesAsync(Student.StudentID);
                    StudentGrades.Clear();
                    foreach (var grade in grades)
                    {
                        StudentGrades.Add(grade);
                    }

                    // Calculate GPA
                    if (StudentGrades.Any())
                    {
                        GPA = Math.Round(StudentGrades.Average(g => g.Score), 2);

                        // Calculate grade summary
                        var highest = StudentGrades.Max(g => g.Score);
                        var lowest = StudentGrades.Min(g => g.Score);
                        GradeSummary = $"Highest: {highest}, Lowest: {lowest}";
                    }
                    else
                    {
                        GPA = 0;
                        GradeSummary = "No grades available.";
                    }
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        $"Failed to load grades: {ex.Message}",
                        "OK");
                }
            }
        }

        private async Task EditGrade(Grade grade)
        {
            if (grade != null)
            {
                await Shell.Current.GoToAsync($"EditGradePage?GradeId={grade.GradeID}");
            }
        }

        private async Task DeleteGrade(Grade grade)
        {
            if (grade != null)
            {
                bool answer = await Application.Current.MainPage.DisplayAlert(
                    "Confirm Delete",
                    $"Delete grade for {grade.Subject}?",
                    "Yes", "No");

                if (answer)
                {
                    await _gradeService.DeleteGradeAsync(grade.GradeID);
                    await LoadGrades();
                }
            }
        }

        private decimal CalculateGPA()
        {
            if (!StudentGrades.Any()) return 0;

            decimal totalScore = 0;
            foreach (var grade in StudentGrades)
            {
                totalScore += grade.Score;
            }
            return Math.Round(totalScore / StudentGrades.Count, 2);
        }
    }
}