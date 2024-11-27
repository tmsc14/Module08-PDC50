namespace Module08.View;
using Module08.ViewModel;

public partial class UserPage : ContentPage
{
	public UserPage()
	{
		InitializeComponent();
		BindingContext = new UserViewModel();
	}
}