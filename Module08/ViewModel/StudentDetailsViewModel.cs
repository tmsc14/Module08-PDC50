using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module08.Model;
using Module08.Services;
using Microsoft.Maui.Controls;
using System.Windows.Input;

namespace Module08.ViewModel
{
    [QueryProperty(nameof(Student), "Student")]
    public class StudentDetailsViewModel : BindableObject
    {
        private Student _student;

        public Student Student
        {
            get => _student;
            set
            {
                _student = value;
                OnPropertyChanged();
            }
        }

        public ICommand GoBackCommand { get; }

        public StudentDetailsViewModel()
        {
            GoBackCommand = new Command(async () => await GoBack());
        }

        private async Task GoBack()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}