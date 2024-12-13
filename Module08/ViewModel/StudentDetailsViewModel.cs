﻿using System;
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
        private string _sortBy;
        private string _filterBy;
        private string _filterValue;
        private ObservableCollection<Attendance> _studentAttendance;
        private readonly AttendanceService _attendanceService;

        public ObservableCollection<Attendance> StudentAttendance
        {
            get => _studentAttendance;
            set
            {
                _studentAttendance = value;
                OnPropertyChanged();
            }
        }

        public Student Student
        {
            get => _student;
            set
            {
                _student = value;
                OnPropertyChanged();
                LoadGrades();
                LoadAttendance();
            }
        }

        public string SortBy
        {
            get => _sortBy;
            set
            {
                _sortBy = value;
                SortGrades();
                OnPropertyChanged();
            }
        }

        public string FilterBy
        {
            get => _filterBy;
            set
            {
                _filterBy = value;
                ApplyFilter();
                OnPropertyChanged();
            }
        }

        public string FilterValue
        {
            get => _filterValue;
            set
            {
                _filterValue = value;
                ApplyFilter();
                OnPropertyChanged();
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
        public ICommand BulkDeleteCommand { get; }
        public ICommand AddAttendanceCommand { get; }

        public StudentDetailsViewModel()
        {
            _gradeService = new GradeService();
            _attendanceService = new AttendanceService();
            StudentGrades = new ObservableCollection<Grade>();
            StudentAttendance = new ObservableCollection<Attendance>();

            GoBackCommand = new Command(async () => await GoBack());
            AddGradeCommand = new Command(async () => await NavigateToAddGrade());
            RefreshGradesCommand = new Command(async () => await LoadGrades());
            EditGradeCommand = new Command<Grade>(async (grade) => await EditGrade(grade));
            DeleteGradeCommand = new Command<Grade>(async (grade) => await DeleteGrade(grade));
            BulkDeleteCommand = new Command<string>(async (criteria) => await BulkDeleteGrades(criteria));
            AddAttendanceCommand = new Command(async () => await NavigateToAddAttendance());
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

        private void SortGrades()
        {
            if (StudentGrades == null) return;

            var sortedGrades = _sortBy?.ToLower() switch
            {
                "score" => StudentGrades.OrderByDescending(g => g.Score).ToList(),
                "quarter" => StudentGrades.OrderBy(g => g.Quarter).ToList(),
                "subject" => StudentGrades.OrderBy(g => g.Subject).ToList(),
                _ => StudentGrades.ToList()
            };

            StudentGrades = new ObservableCollection<Grade>(sortedGrades);
        }

        private void ApplyFilter()
        {
            if (string.IsNullOrEmpty(FilterBy) || string.IsNullOrEmpty(FilterValue))
            {
                LoadGrades();
                return;
            }

            var filteredGrades = FilterBy?.ToLower() switch
            {
                "quarter" => StudentGrades.Where(g => g.Quarter.ToLower().Contains(FilterValue.ToLower())).ToList(),
                "subject" => StudentGrades.Where(g => g.Subject.ToLower().Contains(FilterValue.ToLower())).ToList(),
                _ => StudentGrades.ToList()
            };

            StudentGrades = new ObservableCollection<Grade>(filteredGrades);
        }

        private async Task BulkDeleteGrades(string criteria)
        {
            try
            {
                bool confirm = await Application.Current.MainPage.DisplayAlert(
                    "Confirm Deletion",
                    $"Delete all grades matching the criteria: {criteria}?",
                    "Yes", "No");

                if (confirm)
                {
                    var gradesToDelete = StudentGrades.Where(g =>
                        criteria == "below75" ? g.Score < 75 :
                        criteria == "below80" ? g.Score < 80 : false).ToList();

                    foreach (var grade in gradesToDelete)
                    {
                        await _gradeService.DeleteGradeAsync(grade.GradeID);
                    }

                    await LoadGrades();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error",
                    $"Failed to delete grades: {ex.Message}", "OK");
            }
        }

        private async Task NavigateToAddAttendance()
        {
            if (Student != null)
            {
                await Shell.Current.GoToAsync("AddAttendancePage?StudentId=" + Student.StudentID);
            }
        }

        private async Task LoadAttendance()
        {
            if (Student != null)
            {
                try
                {
                    var attendance = await _attendanceService.GetStudentAttendanceAsync(Student.StudentID);
                    StudentAttendance.Clear();
                    foreach (var record in attendance)
                    {
                        StudentAttendance.Add(record);
                    }
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        $"Failed to load attendance: {ex.Message}",
                        "OK");
                }
            }
        }
    }
}