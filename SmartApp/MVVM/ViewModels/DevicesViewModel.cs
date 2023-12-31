﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Shared.Services;


namespace SmartApp.MVVM.ViewModels
{
    public partial class DevicesViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IotHubManager _iotHub;
        public DevicesViewModel(IServiceProvider serviceProvider, IotHubManager iotHub)
        {
            _serviceProvider = serviceProvider;
            _iotHub = iotHub;

            UpdateDeviceList();
            _iotHub.DevicesUpdated += UpdateDeviceList;
        }

        [ObservableProperty]
        private ObservableCollection<DeviceItemViewModel>? _devices;

        [RelayCommand]
        private void NavigateToSettings()
        {
            var mainWindowViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            mainWindowViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<SettingsViewModel>();
        }

        [RelayCommand]
        private async Task RemoveDevice(object parameter)
        {
            try
            {
                if (parameter is DeviceItemViewModel device)
                {
                    
                    await _iotHub.RemoveDevice(device.DeviceId);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void UpdateDeviceList()
        {
            Devices = new ObservableCollection<DeviceItemViewModel>(_iotHub.CurrentDevices.Select(device => new DeviceItemViewModel(device)).ToList());
        }

    }
}
