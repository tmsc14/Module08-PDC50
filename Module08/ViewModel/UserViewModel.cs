using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module08.Model;
using Module08.Services;

namespace Module08.ViewModel
{
    public class UserViewModel : BindableObject
    {
        private readonly UserService userService;
        public ObservableCollection<User> Users { get; set; }
        s
        public UserViewModel() 
        {

        }
    }
}
