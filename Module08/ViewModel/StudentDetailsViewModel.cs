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
        private string _sortBy;
        private string _filterBy;
        private string _filterValue;
        private ObservableCollection<Attendance> _studentAttendance;
        private readonly AttendanceService _attendanceService;
        private string _attendanceStats;
        private DateTime _startDate = DateTime.Today.AddMonths(-1);
        private DateTime _endDate = DateTime.Today;

        public ObservableCollection<Attendance> StudentAttendance
        {
            get => _studentAttendance;
            set
            {
                _studentAttendance = value;
                OnPropertyChanged();
            }
        }

        public string AttendanceStats
        {
            get => _attendanceStats;
            set
            {
                _attendanceStats = value;
                OnPropertyChanged();
            }
        }

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                FilterAttendance();
                OnPropertyChanged();
            }
        }

        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                FilterAttendance();
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
        public ICommand EditAttendanceCommand { get; }
        public ICommand DeleteAttendanceCommand { get; }

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
            EditAttendanceCommand = new Command<Attendance>(async (attendance) => await EditAttendance(attendance));
            DeleteAttendanceCommand = new Command<Attendance>(async (attendance) => await DeleteAttendance(attendance));
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

        private void CalculateAttendanceStats()
        {
            if (!StudentAttendance.Any())
            {
                AttendanceStats = "No attendance records";
                return;
            }

            int totalDays = StudentAttendance.Count;
            int presentDays = StudentAttendance.Count(a => a.Status == "Present");
            decimal attendanceRate = (decimal)presentDays / totalDays * 100;

            AttendanceStats = $"Attendance Rate: {Math.Round(attendanceRate, 2)}%\n" +
                              $"Present: {presentDays}\n" +
                              $"Total Days: {totalDays}";
        }

        private void FilterAttendance()
        {
            if (StudentAttendance == null) return;

            var filteredAttendance = StudentAttendance
                .Where(a => a.Date >= StartDate && a.Date <= EndDate)
                .OrderByDescending(a => a.Date)
                .ToList();

            StudentAttendance = new ObservableCollection<Attendance>(filteredAttendance);
            CalculateAttendanceStats();
        }

        private async Task EditAttendance(Attendance attendance)
        {
            if (attendance != null)
            {
                await Shell.Current.GoToAsync($"EditAttendancePage?AttendanceId={attendance.AttendanceID}");
            }
        }

        private async Task DeleteAttendance(Attendance attendance)
        {
            if (attendance != null)
            {
                bool answer = await Application.Current.MainPage.DisplayAlert(
                    "Confirm Delete",
                    $"Delete attendance record for {attendance.Date:MM/dd/yyyy}?",
                    "Yes", "No");

                if (answer)
                {
                    await _attendanceService.DeleteAttendanceAsync(attendance.AttendanceID);
                    await LoadAttendance();
                }
            }
        }
    }
}