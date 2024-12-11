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
        private Student _selectedStudent;
        private string _errorMessage;
        private string _statusMessage;

        // Input fields
        private string _studentIdInput;
        private string _fullNameInput;
        private string _gradeClassInput;
        private string _contactNoInput;
        private DateTime _dateOfBirthInput = DateTime.Today;
        private string _genderInput;
        private string _addressInput;
        private string _emailInput;
        private string _emergencyContactInput;
        private string _statusInput;

        public ObservableCollection<Student> Students
        {
            get => _students;
            set
            {
                _students = value;
                OnPropertyChanged();
            }
        }

        public Student SelectedStudent
        {
            get => _selectedStudent;
            set
            {
                _selectedStudent = value;
                OnPropertyChanged();
                UpdateEntryFields();
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

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        // Input properties
        public string StudentIdInput
        {
            get => _studentIdInput;
            set
            {
                _studentIdInput = value;
                OnPropertyChanged();
            }
        }

        public string FullNameInput
        {
            get => _fullNameInput;
            set
            {
                _fullNameInput = value;
                OnPropertyChanged();
            }
        }

        public string GradeClassInput
        {
            get => _gradeClassInput;
            set
            {
                _gradeClassInput = value;
                OnPropertyChanged();
            }
        }

        public string ContactNoInput
        {
            get => _contactNoInput;
            set
            {
                _contactNoInput = value;
                OnPropertyChanged();
            }
        }

        public DateTime DateOfBirthInput
        {
            get => _dateOfBirthInput;
            set
            {
                _dateOfBirthInput = value;
                OnPropertyChanged();
            }
        }

        public string GenderInput
        {
            get => _genderInput;
            set
            {
                _genderInput = value;
                OnPropertyChanged();
            }
        }

        public string AddressInput
        {
            get => _addressInput;
            set
            {
                _addressInput = value;
                OnPropertyChanged();
            }
        }

        public string EmailInput
        {
            get => _emailInput;
            set
            {
                _emailInput = value;
                OnPropertyChanged();
            }
        }

        public string EmergencyContactInput
        {
            get => _emergencyContactInput;
            set
            {
                _emergencyContactInput = value;
                OnPropertyChanged();
            }
        }

        public string StatusInput
        {
            get => _statusInput;
            set
            {
                _statusInput = value;
                OnPropertyChanged();
            }
        }

        public StudentListViewModel()
        {
            _studentService = new StudentService();
            Students = new ObservableCollection<Student>();
            LoadStudentsCommand = new Command(async () => await LoadStudents());
            AddStudentCommand = new Command(async () => await AddStudent());
            UpdateStudentCommand = new Command(async () => await UpdateStudent());
            DeleteStudentCommand = new Command(async () => await DeleteStudent());
        }

        public ICommand LoadStudentsCommand { get; }
        public ICommand AddStudentCommand { get; }
        public ICommand UpdateStudentCommand { get; }
        public ICommand DeleteStudentCommand { get; }

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

        private async Task AddStudent()
        {
            if (!string.IsNullOrWhiteSpace(StudentIdInput) && !string.IsNullOrWhiteSpace(FullNameInput))
            {
                bool answer = await Application.Current.MainPage.DisplayAlert(
                    "Confirm Add",
                    $"Add student {FullNameInput}?",
                    "Yes", "No");

                if (answer)
                {
                    var newStudent = new Student
                    {
                        StudentID = StudentIdInput,
                        FullName = FullNameInput,
                        GradeClass = GradeClassInput,
                        ContactNo = ContactNoInput,
                        DateOfBirth = DateOfBirthInput.ToString("yyyy-MM-dd"),
                        Gender = GenderInput,
                        Address = AddressInput,
                        Email = EmailInput,
                        EmergencyContact = EmergencyContactInput,
                        Status = StatusInput
                    };

                    var result = await _studentService.AddStudentAsync(newStudent);
                    await LoadStudents();
                    ClearInputs();
                    StatusMessage = $"Student {FullNameInput} added successfully";
                    await Application.Current.MainPage.DisplayAlert("Success", $"Student {FullNameInput} has been added", "OK");
                }
            }
        }

        public async Task UpdateStudent()
        {
            if (SelectedStudent != null)
            {
                bool answer = await Application.Current.MainPage.DisplayAlert(
                    "Confirm Update",
                    $"Update details for student {FullNameInput}?",
                    "Yes", "No");

                if (answer)
                {
                    SelectedStudent.FullName = FullNameInput;
                    SelectedStudent.GradeClass = GradeClassInput;
                    SelectedStudent.ContactNo = ContactNoInput;
                    SelectedStudent.DateOfBirth = DateOfBirthInput.ToString("yyyy-MM-dd");
                    SelectedStudent.Gender = GenderInput;
                    SelectedStudent.Address = AddressInput;
                    SelectedStudent.Email = EmailInput;
                    SelectedStudent.EmergencyContact = EmergencyContactInput;
                    SelectedStudent.Status = StatusInput;

                    var result = await _studentService.UpdateStudentAsync(SelectedStudent);
                    await LoadStudents();
                    StatusMessage = $"Student {FullNameInput} updated successfully";
                    await Application.Current.MainPage.DisplayAlert("Success", $"Student {FullNameInput} has been updated", "OK");
                    ClearInputs();
                }
            }
        }

        private async Task DeleteStudent()
        {
            if (SelectedStudent != null)
            {
                bool answer = await Application.Current.MainPage.DisplayAlert(
                    "Confirm Delete",
                    $"Are you sure you want to delete student {SelectedStudent.FullName}?",
                    "Yes", "No");

                if (answer)
                {
                    var result = await _studentService.DeleteStudentAsync(SelectedStudent.StudentID);
                    await LoadStudents();
                    ClearInputs();
                    StatusMessage = $"Student {SelectedStudent.FullName} deleted successfully";
                    await Application.Current.MainPage.DisplayAlert("Success", "Student has been deleted", "OK");
                }
            }
        }

        private void ClearInputs()
        {
            StudentIdInput = string.Empty;
            FullNameInput = string.Empty;
            GradeClassInput = string.Empty;
            ContactNoInput = string.Empty;
            DateOfBirthInput = DateTime.Today;
            GenderInput = string.Empty;
            AddressInput = string.Empty;
            EmailInput = string.Empty;
            EmergencyContactInput = string.Empty;
            StatusInput = string.Empty;
        }

        private void UpdateEntryFields()
        {
            if (SelectedStudent != null)
            {
                StudentIdInput = SelectedStudent.StudentID;
                FullNameInput = SelectedStudent.FullName;
                GradeClassInput = SelectedStudent.GradeClass;
                ContactNoInput = SelectedStudent.ContactNo;
                DateOfBirthInput = DateTime.Parse(SelectedStudent.DateOfBirth);
                GenderInput = SelectedStudent.Gender;
                AddressInput = SelectedStudent.Address;
                EmailInput = SelectedStudent.Email;
                EmergencyContactInput = SelectedStudent.EmergencyContact;
                StatusInput = SelectedStudent.Status;
            }
            else
            {
                ClearInputs();
            }
        }
    }
}