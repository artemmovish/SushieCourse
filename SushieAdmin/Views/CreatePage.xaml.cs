using SushieAdmin.ViewModels;
using System.Globalization;

namespace SushieAdmin.Views;

public partial class CreatePage : ContentPage
{
	public CreatePage()
	{
		InitializeComponent();

		BindingContext = new CreateViewModel();
	}
}
