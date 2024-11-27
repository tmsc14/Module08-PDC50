using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module08.Model;
using Module08.Services;
using System.Windows.Input;

namespace Module08.ViewModel
{
    public class UserViewModel : BindableObject
    {
        private readonly UserService _userService;
        public ObservableCollection<User> Users { get; set; }
        
        public UserViewModel() 
        {
            _userService = new UserService();
            Users = new ObservableCollection<User>();
            LoadUserCommand = new Command(async () => await LoadUsers());

        }

        public ICommand LoadUserCommand { get; set; }

        private async Task LoadUsers()
        {
            var users = await _userService.GetUsersAsync();
            Users.Clear();
            foreach (var user in users)
            {
                Users.Add(user);
            }
        }
    }
}
