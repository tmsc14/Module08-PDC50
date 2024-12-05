using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Module08.Model;
using Module08.Services;
using Microsoft.Maui.Controls;

namespace Module08.ViewModel
{
    public class StudentListViewModel : BindableObject
    {
        private readonly StudentService _studentService;
        private ObservableCollection<Student> _students;
        private string _errorMessage;

        public ObservableCollection<Student> Students
        {
            get => _students;
            set
            {
                _students = value;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public StudentListViewModel()
        {
            _studentService = new StudentService();
            Students = new ObservableCollection<Student>();
            LoadStudentsCommand = new Command(async () => await LoadStudents());
        }

        public ICommand LoadStudentsCommand { get; }

        private async Task LoadStudents()
        {
            try
            {
                ErrorMessage = "Loading students...";
                var students = await _studentService.GetStudentsAsync();

                if (students == null || !students.Any())
                {
                    ErrorMessage = "No students found or error loading data";
                    return;
                }

                Students.Clear();
                foreach (var student in students)
                {
                    Students.Add(student);
                }
                ErrorMessage = $"Loaded {students.Count} students successfully";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
            }
        }
    

    //public event PropertyChangedEventHandler PropertyChanged;
    //protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    //{
    //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    //}
    //}
    }
}