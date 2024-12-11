using Module08.ViewModel;

namespace Module08.View
{
    public partial class StudentDetailsPage : ContentPage
    {
        public StudentDetailsPage()
        {
            InitializeComponent();
            BindingContext = new StudentDetailsViewModel();
        }
    }
}

