using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartApp.MVVM.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IotHubManager _iotHub;


        public SettingsViewModel(IServiceProvider serviceProvider, IotHubManager iotHub)
        {
            _serviceProvider = serviceProvider;            
            _iotHub = iotHub;
        }



        [RelayCommand]
        private void NavigateToHome()
        {
            var mainWindowViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            mainWindowViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<HomeViewModel>();
        }

        [RelayCommand]
        private void ShowAddDevice()
        {
            var mainWindowViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            mainWindowViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<AddDeviceViewModel>();
        }

        [RelayCommand]
        private void ShowAllDevices()
        {
            var mainWindowViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            mainWindowViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<DevicesViewModel>();
        }

        [RelayCommand]
        private void ShowConfiguration()
        {
           
        }

        [RelayCommand]
        private void ExitApplication()
        {
            Environment.Exit(0);
        }



    }
}
