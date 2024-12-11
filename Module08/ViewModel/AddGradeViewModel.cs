using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Module08.Model;
using Module08.Services;

namespace Module08.ViewModel
{
    [QueryProperty(nameof(StudentId), "StudentId")]
    public class AddGradeViewModel : BindableObject
    {
        private readonly GradeService _gradeService;
        private string _studentId;
        private string _subject;
        private string _quarter;
        private string _score;
        private string _schoolYear;

        public string StudentId
        {
            get => _studentId;
            set
            {
                _studentId = value;
                OnPropertyChanged();
            }
        }

        public string Subject
        {
            get => _subject;
            set
            {
                _subject = value;
                OnPropertyChanged();
            }
        }

        public string Quarter
        {
            get => _quarter;
            set
            {
                _quarter = value;
                OnPropertyChanged();
            }
        }

        public string Score
        {
            get => _score;
            set
            {
                _score = value;
                OnPropertyChanged();
            }
        }

        public string SchoolYear
        {
            get => _schoolYear;
            set
            {
                _schoolYear = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public AddGradeViewModel()
        {
            _gradeService = new GradeService();
            SaveCommand = new Command(async () => await SaveGrade());
            CancelCommand = new Command(async () => await Cancel());
        }

        private async Task SaveGrade()
        {
            if (string.IsNullOrWhiteSpace(Subject) ||
                string.IsNullOrWhiteSpace(Quarter) ||
                string.IsNullOrWhiteSpace(Score) ||
                string.IsNullOrWhiteSpace(SchoolYear))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "All fields are required",
                    "OK");
                return;
            }

            try
            {
                var newGrade = new Grade
                {
                    StudentID = StudentId,
                    Subject = Subject,
                    Quarter = Quarter,
                    Score = decimal.Parse(Score),
                    SchoolYear = SchoolYear
                };

                var result = await _gradeService.AddGradeAsync(newGrade);
                await Application.Current.MainPage.DisplayAlert(
                    "Success",
                    "Grade added successfully",
                    "OK");
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    $"Failed to add grade: {ex.Message}",
                    "OK");
            }
        }

        private async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
