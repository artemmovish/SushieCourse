
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

namespace SushieAdmin.ViewModels
{
    public partial class ChangeViewModel : ObservableObject
    {

        [ObservableProperty]
        ObservableCollection<SushieItem> sushieItems;

        [ObservableProperty]
        ObservableCollection<Category> categories;

        public ApiClient apiClient => SinglTone.Instance.ApiClient;

        public ChangeViewModel()
        {
            LoadData();
        }

        [RelayCommand]
        async Task Update()
        {
            LoadData();
        }
        [RelayCommand]
        async Task Edit(SushieItem item)
        {
            apiClient.ChangeProducts(item);
        }

        [RelayCommand]
        async Task Delete(int id)
        {        
            foreach (var item in sushieItems)
            {
                if (item.id == id)
                {
                    apiClient.DeleteProducts(id);
                    SushieItems.Remove(item);
                    break;
                }
            }
        }

        async Task LoadData()
        {
            try
            {
                var responseCategoriesItems = await apiClient.GetCategories();
                Categories = new ObservableCollection<Category>(responseCategoriesItems);
            }
            catch (Exception ex)
            {

                Application.Current.MainPage.DisplayAlert("Ошибка", $"Ошибка при создании загрузки категорий: {ex.Message}", "OK");

            }

            try
            {
                var responseSushieItems = await apiClient.GetProduct();
                SushieItems = new ObservableCollection<SushieItem>(responseSushieItems);
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Ошибка", $"Ошибка при создании загрузки продуктов: {ex.Message}", "OK");
            }

        }
    }
}
