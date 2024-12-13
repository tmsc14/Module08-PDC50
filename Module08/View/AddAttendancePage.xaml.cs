using Module08.ViewModel;

namespace Module08.View;

public partial class AddAttendancePage : ContentPage
{
    public AddAttendancePage()
    {
        InitializeComponent();
        BindingContext = new AddAttendanceViewModel();
    }
}
