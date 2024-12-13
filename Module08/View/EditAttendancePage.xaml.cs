using Module08.ViewModel;

namespace Module08.View;

public partial class EditAttendancePage : ContentPage
{
    public EditAttendancePage()
    {
        InitializeComponent();
        BindingContext = new EditAttendanceViewModel();
    }
}
