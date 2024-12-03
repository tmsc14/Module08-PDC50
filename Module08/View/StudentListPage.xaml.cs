using Microsoft.Maui.Controls;
using Module08.ViewModel;

namespace Module08.View
{
	public partial class StudentListPage : ContentPage
	{
		private readonly StudentListViewModel _viewModel;

		public StudentListPage()
		{
			InitializeComponent();
			_viewModel = new StudentListViewModel();
			BindingContext = _viewModel;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			_viewModel.LoadStudentsCommand.Execute(null);
		}
	}
}