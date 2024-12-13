using Module08.ViewModel;
using Module08.Model;
using Module08.View;

namespace Module08
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(StudentDetailsPage), typeof(StudentDetailsPage));
            Routing.RegisterRoute(nameof(AddGradePage), typeof(AddGradePage));
            Routing.RegisterRoute(nameof(EditGradePage), typeof(EditGradePage));
            Routing.RegisterRoute(nameof(AddAttendancePage), typeof(AddAttendancePage));
            Routing.RegisterRoute(nameof(EditAttendancePage), typeof(EditAttendancePage));
        }
    }
}
