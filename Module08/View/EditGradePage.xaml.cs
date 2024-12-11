using Module08.ViewModel;

namespace Module08.View
{
    public partial class EditGradePage : ContentPage
    {
        public EditGradePage()
        {
            InitializeComponent();
            BindingContext = new EditGradeViewModel();
        }
    }
}
