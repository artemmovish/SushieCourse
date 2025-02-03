using SushieUser.ViewModels;

namespace SushieUser.Views;

public partial class CardPage : ContentPage
{
	public CardPage()
	{
		InitializeComponent();

		BindingContext = new CardViewModel();

    }
}