using SushieAdmin.ViewModels;

namespace SushieAdmin.Views;

public partial class CreatePage : ContentPage
{
	public CreatePage()
	{
		InitializeComponent();

		BindingContext = new CreateViewModel();
	}
}