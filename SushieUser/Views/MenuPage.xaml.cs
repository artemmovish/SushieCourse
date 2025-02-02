using SushieUser.Models;
using SushieUser.ViewModels;

namespace SushieUser.Views;

public partial class MenuPage : ContentPage
{
    public MenuPage()
	{
		InitializeComponent();
		var vm = new MenuViewModels();
        BindingContext = vm;
	}
}