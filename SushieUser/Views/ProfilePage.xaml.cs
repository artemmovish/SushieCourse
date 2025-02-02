using SushieUser.ViewModels;

namespace SushieUser.Views;

public partial class ProfilePage : ContentPage
{
	public ProfilePage()
	{
		InitializeComponent();
		BindingContext = new ProfileViewModel();
	}
}