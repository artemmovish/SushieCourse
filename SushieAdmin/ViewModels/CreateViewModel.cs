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

        [ObservableProperty]
        string imagePath;

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
                var responseCategoriesItems = await Task.Run(() => apiClient.GetCategories());
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Categories = new ObservableCollection<Category>(responseCategoriesItems);
                });
            }
            catch (Exception ex)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage.DisplayAlert("Ошибка", $"Ошибка при загрузке категорий: {ex.Message}", "OK");
                });
            }

            try
            {
                var responseSushieItems = await Task.Run(() => apiClient.GetProduct());
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    SushieItems = new ObservableCollection<SushieItem>(responseSushieItems);
                });
            }
            catch (Exception ex)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage.DisplayAlert("Ошибка", $"Ошибка при загрузке продуктов: {ex.Message}", "OK");
                });
            }
        }


        [RelayCommand]
        async void SetImage()
        {
            try
            {
                // Запуск FilePicker для выбора изображения
                var result = await FilePicker.Default.PickAsync(new PickOptions
                {
                    PickerTitle = "Выберите изображение",
                    FileTypes = FilePickerFileType.Images // Ограничиваем выбор только изображениями
                });

                // Если пользователь выбрал файл
                if (result != null)
                {
                    // Получаем путь к выбранному файлу
                    ImagePath = result.FullPath;

                    // Можно также прочитать файл как поток, если нужно
                    using var stream = await result.OpenReadAsync();
                    // Дополнительная обработка изображения, если необходимо
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок (например, если пользователь отменил выбор)
                await Shell.Current.DisplayAlert("Ошибка", $"Не удалось выбрать изображение: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        async Task CreateProduct()
        {
            try
            {
                SushieItem.category_id = (int)CategoryFromSushieItem.Id;
                await apiClient.CreateProductAsync(SushieItem, ImagePath);
                SushieItems.Add(SushieItem);
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Ошибка", $"Ошибка при создании продукта: {ex.Message}", "OK");
            }
        }
        [RelayCommand]
        async Task CreateCategory()
        {
            try
            {
                await apiClient.CreatCategories(Category);

                Categories.Add(Category);
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Ошибка", $"Ошибка при создании категории: {ex.Message}", "OK");
            }
        }
    }
}
