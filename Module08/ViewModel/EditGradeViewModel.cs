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
    [QueryProperty(nameof(GradeId), "GradeId")]
    public class EditGradeViewModel : BindableObject
    {
        private readonly GradeService _gradeService;
        private int _gradeId;
        private string _subject;
        private string _quarter;
        private string _score;
        private string _schoolYear;

        public int GradeId
        {
            get => _gradeId;
            set
            {
                _gradeId = value;
                OnPropertyChanged();
                LoadGrade();
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

        public ICommand UpdateCommand { get; }
        public ICommand CancelCommand { get; }

        public EditGradeViewModel()
        {
            _gradeService = new GradeService();
            UpdateCommand = new Command(async () => await UpdateGrade());
            CancelCommand = new Command(async () => await Cancel());
        }

        private async Task LoadGrade()
        {
            try
            {
                var grade = await _gradeService.GetGradeByIdAsync(GradeId);
                if (grade != null)
                {
                    Subject = grade.Subject;
                    Quarter = grade.Quarter;
                    Score = grade.Score.ToString();
                    SchoolYear = grade.SchoolYear;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error",
                    $"Failed to load grade details: {ex.Message}", "OK");
                await Shell.Current.GoToAsync("..");
            }
        }

        private async Task UpdateGrade()
        {
            if (string.IsNullOrWhiteSpace(Subject) ||
                string.IsNullOrWhiteSpace(Quarter) ||
                string.IsNullOrWhiteSpace(Score) ||
                string.IsNullOrWhiteSpace(SchoolYear))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "All fields are required", "OK");
                return;
            }

            try
            {
                var updatedGrade = new Grade
                {
                    GradeID = GradeId,
                    Subject = Subject,
                    Quarter = Quarter,
                    Score = decimal.Parse(Score),
                    SchoolYear = SchoolYear
                };

                var result = await _gradeService.UpdateGradeAsync(updatedGrade);
                await Application.Current.MainPage.DisplayAlert("Success", "Grade updated successfully", "OK");
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to update grade: {ex.Message}", "OK");
            }
        }

        private async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
