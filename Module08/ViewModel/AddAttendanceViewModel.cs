using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Module08.Services;
using Module08.Model;

namespace Module08.ViewModel
{
    [QueryProperty(nameof(StudentId), "StudentId")]
    public class AddAttendanceViewModel : BindableObject
    {
        private readonly AttendanceService _attendanceService;
        private string _studentId;
        private DateTime _date = DateTime.Today;
        private string _status;
        private string _remarks;

        public string StudentId
        {
            get => _studentId;
            set
            {
                _studentId = value;
                OnPropertyChanged();
            }
        }

        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged();
            }
        }

        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        public string Remarks
        {
            get => _remarks;
            set
            {
                _remarks = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public AddAttendanceViewModel()
        {
            _attendanceService = new AttendanceService();
            SaveCommand = new Command(async () => await SaveAttendance());
            CancelCommand = new Command(async () => await Cancel());
        }

        private async Task SaveAttendance()
        {
            if (string.IsNullOrWhiteSpace(Status))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Status is required", "OK");
                return;
            }

            var attendance = new Attendance
            {
                StudentID = StudentId,
                Date = Date,
                Status = Status,
                Remarks = Remarks
            };

            await _attendanceService.AddAttendanceAsync(attendance);
            await Application.Current.MainPage.DisplayAlert("Success", "Attendance added successfully", "OK");
            await Shell.Current.GoToAsync("..");
        }

        private async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
