using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using SushieUser.Models;
using System.Text.Json;
using CommunityToolkit.Mvvm.Input;
using SushieUser.Helper;
namespace SushieUser.ViewModels
{
    public partial class MenuViewModels : ObservableObject
    {
        [ObservableProperty]
        ObservableCollection<SushieItem> sushieItems;

        [ObservableProperty]
        ObservableCollection<SushieItem> sushieCards;

        [ObservableProperty]
        ObservableCollection<Category> sushieCategories;

        private Category _selectedCategory;

        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (_selectedCategory != value)
                {
                    SetProperty(ref _selectedCategory, value);

                    // Ваша логика, которая выполняется при изменении
                    OnCategoryChanged();
                }
            }
        }

        public ApiClient apiClient => SinglTone.Instance.ApiClient;

        public bool Auth = SinglTone.Instance.Auth;

        public MenuViewModels()
        {
            LoadData();
        }

        async void LoadData()
        {
            var responseSushieItems = await apiClient.GetProduct();
            SushieItems = new ObservableCollection<SushieItem>(responseSushieItems);

            var responseCategoriesItems = await apiClient.GetCategories();
            SushieCategories = new ObservableCollection<Category>(responseCategoriesItems);
        }

        public async void OnCategoryChanged()
        {
            var responseSushieItems = await apiClient.GetCategoriesProducts(SelectedCategory.Id);
            SushieItems = new ObservableCollection<SushieItem>(responseSushieItems);
        }
    }
}
