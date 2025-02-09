using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SushieUser.Helper;
using SushieUser.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushieUser.ViewModels
{
    public partial class CardViewModel : ObservableObject
    {
        [ObservableProperty]
        ObservableCollection<CartItem> sushieItems;

        public ApiClient apiClient => SinglTone.Instance.ApiClient;

        public CardViewModel()
        {
            LoadData();
        }

        [RelayCommand]
        async void LoadData()
        {
            try
            {
                var responseSushieItems = await apiClient.GetCart();

                SushieItems = new ObservableCollection<CartItem>(responseSushieItems);
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Ошибка", $"Ошибка при загрузки корзины: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        async void Plus(CartItem item)
        {
            item.qualinty++;
            SushieItem sushieItem = (SushieItem)item;
            await apiClient.AddCart(sushieItem, item.qualinty);
            LoadData();
        }

        [RelayCommand]
        async void Minus(CartItem item)
        {
            item.qualinty--;

            SushieItem sushieItem = (SushieItem)item;

            await apiClient.AddCart(sushieItem, item.qualinty);

            LoadData();
        }

        [RelayCommand]
        async void Apply()
        {
            SushieItems = new ObservableCollection<CartItem>();
        }
    }
}
