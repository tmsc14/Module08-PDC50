﻿using System;
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
        private User _selectedUser;
        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
            }
        }

        //Name
        private string _nameInput;
        public string NameInput
        {
            get => _nameInput;
            set
            {
                _nameInput = value;
                OnPropertyChanged();
            }
        }

        //Gender
        private string _genderInput;
        public string GenderInput
        {
            get => _genderInput;
            set
            {
                _genderInput = value;
                OnPropertyChanged();
            }
        }

        //Contact No
        private string _contactNoInput;
        public string ContactNoInput
        {
            get => _contactNoInput;
            set
            {
                _contactNoInput = value;
                OnPropertyChanged();
            }
        }

        public UserViewModel() 
        {
            _userService = new UserService();
            Users = new ObservableCollection<User>();
            LoadUserCommand = new Command(async () => await LoadUsers());
            AddUserCommand = new Command(async () => await AddUser());
            DeleteUserCommand = new Command(async () => await DeleteUser());
        }

        public ICommand LoadUserCommand { get; }
        public ICommand AddUserCommand { get; }
        public ICommand DeleteUserCommand { get; }

        private async Task DeleteUser()
        {
            if (SelectedUser != null)
            {
                var result = await _userService.DeleteUserAsync(SelectedUser.Id);
                await LoadUsers();
            }
        }
        private async Task LoadUsers()
        {
            var users = await _userService.GetUsersAsync();
            Users.Clear();
            foreach (var user in users)
            {
                Users.Add(user);
            }
        }

        private async Task AddUser()
        {
            if (!string.IsNullOrWhiteSpace(NameInput) &&
                !string.IsNullOrWhiteSpace(GenderInput) &&
                !string.IsNullOrWhiteSpace(ContactNoInput))
            {
                var newUser = new User
                {
                    Name = NameInput,
                    Gender = GenderInput,
                    ContactNo = ContactNoInput
                };

                var result = await _userService.AddUserAsync(newUser);
                if (result.Equals("Success", StringComparison.OrdinalIgnoreCase))
                {
                    await LoadUsers();
                }
            }
        }
    }
}
