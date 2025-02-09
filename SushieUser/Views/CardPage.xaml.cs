using SushieUser.ViewModels;
using System.Threading.Tasks;

namespace SushieUser.Views;

public partial class CardPage : ContentPage
{
	public CardPage()
	{
		InitializeComponent();

		BindingContext = new CardViewModel();

    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Application.Current.MainPage.DisplayAlert("Уведомление", "Заказ успешно выполнен", "OK");
    }
}