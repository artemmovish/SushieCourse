using SushieAdmin.ViewModels;

namespace SushieAdmin.Views;

public partial class ChangePage : ContentPage
{
	public ChangePage()
	{
		InitializeComponent();
		BindingContext = new ChangeViewModel();
	}
}