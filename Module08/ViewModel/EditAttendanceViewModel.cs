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
    [QueryProperty(nameof(AttendanceId), "AttendanceId")]
    public class EditAttendanceViewModel : BindableObject
    {
        private readonly AttendanceService _attendanceService;
        private int _attendanceId;
        private DateTime _date = DateTime.Today;
        private string _status;
        private string _remarks;

        public int AttendanceId
        {
            get => _attendanceId;
            set
            {
                _attendanceId = value;
                LoadAttendance();
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

        public EditAttendanceViewModel()
        {
            _attendanceService = new AttendanceService();
            SaveCommand = new Command(async () => await SaveAttendance());
            CancelCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
        }

        private async Task LoadAttendance()
        {
            try
            {
                var attendance = await _attendanceService.GetAttendanceByIdAsync(AttendanceId);
                if (attendance != null)
                {
                    Date = attendance.Date;
                    Status = attendance.Status;
                    Remarks = attendance.Remarks;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error",
                    $"Failed to load attendance: {ex.Message}", "OK");
                await Shell.Current.GoToAsync("..");
            }
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
                AttendanceID = AttendanceId,
                Date = Date,
                Status = Status,
                Remarks = Remarks
            };

            await _attendanceService.UpdateAttendanceAsync(attendance);
            await Application.Current.MainPage.DisplayAlert("Success", "Attendance updated successfully", "OK");
            await Shell.Current.GoToAsync("..");
        }
    }
}
