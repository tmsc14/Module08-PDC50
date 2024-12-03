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

		public ObservableCollection<Student> Students
		{
			get => _students;
			set
			{
				_students = value;
				OnPropertyChanged();
			}
		}

		public ICommand LoadStudentsCommand { get; }

		public StudentListViewModel()
		{
			_studentService = new StudentService();
			Students = new ObservableCollection<Student>();
			LoadStudentsCommand = new Command(async () => await LoadStudents());
		}

		private async Task LoadStudents()
		{
			var students = await _studentService.GetStudentsAsync();
			Students.Clear();
			foreach (var student in students)
			{
				Students.Add(student);
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}