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
    public partial class CreateViewModel : ObservableObject
    {
        [ObservableProperty]
        ObservableCollection<SushieItem> sushieItems;

        [ObservableProperty]
        SushieItem sushieItem;

        [ObservableProperty]
        Category categoryFromSushieItem;

        [ObservableProperty]
        ObservableCollection<Category> categories;
        
        [ObservableProperty]
        Category category;

        public ApiClient apiClient => SinglTone.Instance.ApiClient;

        public CreateViewModel()
        {
            LoadData();
            Console.WriteLine("fsdf");
            apiClient.Login();
            SushieItem = new SushieItem();
            CategoryFromSushieItem = new Category();
            Category = new Category();
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

        [RelayCommand]
        async void CreateProduct()
        {
            try
            {
                SushieItem.category_id = (int)CategoryFromSushieItem.Id;
                await apiClient.CreatProducts(SushieItem);
                LoadData();
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Ошибка", $"Ошибка при создании продукта: {ex.Message}", "OK");
            }
        }
        [RelayCommand]
        async void CreateCategory()
        {
            try
            {
                await apiClient.CreatCategories(Category);
                LoadData();
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Ошибка", $"Ошибка при создании категории: {ex.Message}", "OK");
            }
        }
    }
}
