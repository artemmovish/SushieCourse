using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SushieUser.Helper;
using SushieUser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushieUser.ViewModels
{
    partial class ProfileViewModel : ObservableObject
    {
        [ObservableProperty]
        RegisterRequest registerRequest = new RegisterRequest();

        [ObservableProperty]
        bool isRegister = false;

        [ObservableProperty]
        bool isLogin = true;

        [ObservableProperty]
        bool isProfile = false;

        public ApiClient apiClient => SinglTone.Instance.ApiClient;

        public ProfileViewModel()
        {

        }

        [RelayCommand]
        public void LogRegSwitch()
        {
            if (IsRegister)
            {
                IsRegister = false;
                IsLogin = true;
            }
            else
            {
                IsRegister = true;
                IsLogin = false;
            }
        }

        [RelayCommand]
        public async Task SignInfromReg()
        {
            try
            {
                await apiClient.Register(RegisterRequest);

            }
            catch (Exception)
            {
                return;
            }

            if (apiClient.CheckToken())
            {
                IsProfile = true;
                IsRegister = false;
                IsLogin = false;
                RegisterRequest = await apiClient.GetProfile();
            }
        }

        [RelayCommand]
        public async Task SignInfromLog()
        {
            var authRequest = new AuthRequest()
            {
                Login = RegisterRequest.Login,
                Password = RegisterRequest.Password,
                Telephone = RegisterRequest.Telephone
            };

            await apiClient.Login(authRequest);

            if (apiClient.CheckToken())
            {
                IsProfile = true;
                IsRegister = false;
                IsLogin = false;
                RegisterRequest = await apiClient.GetProfile();
            }
        }
    }
}
