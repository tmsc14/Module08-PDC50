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

        public ICommand GoBackCommand { get; }
        public ICommand AddGradeCommand { get; }
        public ICommand RefreshGradesCommand { get; }

        public StudentDetailsViewModel()
        {
            _gradeService = new GradeService();
            StudentGrades = new ObservableCollection<Grade>();

            GoBackCommand = new Command(async () => await GoBack());
            AddGradeCommand = new Command(async () => await AddGrade());
            RefreshGradesCommand = new Command(async () => await LoadGrades());
        }

        private async Task GoBack()
        {
            await Shell.Current.GoToAsync("..");
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

        private async Task AddGrade()
        {
            if (Student != null)
            {
                try
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Add Grade",
                        "Grade addition will be implemented soon",
                        "OK");
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        $"Failed to add grade: {ex.Message}",
                        "OK");
                }
            }
        }
    }
}