using Module08.ViewModel;

namespace Module08.View
{
    public partial class AddGradePage : ContentPage
    {
        public AddGradePage()
        {
            InitializeComponent();
            BindingContext = new AddGradeViewModel();
        }
    }
}

