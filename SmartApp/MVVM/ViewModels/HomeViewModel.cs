﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Azure.Devices;
using Microsoft.Extensions.DependencyInjection;
using Shared.Models;
using Shared.Services;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartApp.MVVM.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly DateAndTimeService _dateAndTimeService;
        private readonly WeatherService _weatherService;
        private readonly IotHubManager _ioutHubManager;

        public HomeViewModel(IServiceProvider serviceProvider, DateAndTimeService dateAndTimeService, WeatherService weatherService, IotHubManager ioutHubManager)
        {
            _serviceProvider = serviceProvider;
            _dateAndTimeService = dateAndTimeService;
            _weatherService = weatherService;
            _ioutHubManager = ioutHubManager;

           

            UpdateDateAndTime();
            UpdateWeather();
            UpdateDeviceList();


            _ioutHubManager.DevicesUpdated += UpdateDeviceList;

        }

        private void UpdateDeviceList()
        {
            try
            {
                var devices = _ioutHubManager.CurrentDevices.Select(device => new DeviceItemViewModel(device)).ToList();
                DevicesList = new ObservableCollection<DeviceItemViewModel>(devices);
            } catch (Exception ex) { Debug.WriteLine(ex.Message); }

        }

        [ObservableProperty]
        ObservableCollection<DeviceItemViewModel> devicesList = new ObservableCollection<DeviceItemViewModel>();

        [ObservableProperty]
        private string? _title = "Home";

        [ObservableProperty]
        private string? _currentTime = "--:--";

        [ObservableProperty]
        private string? _currentDate;

        [ObservableProperty]
        private string? _currentWeatherCondition = "\uf5c7";

        [ObservableProperty]
        private string? _currentOutsideTemperature = "--";

        [ObservableProperty]
        private string? _currentOutsideTemperatureUnit = "°C";

        [ObservableProperty]
        private string? _currentInsideTemperature = "--";

        [ObservableProperty]
        private string? _currentInsideTemperatureUnit = "°C";

        [RelayCommand]
        private void NavigateToSettings()
        {
            var mainWindowViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            mainWindowViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<SettingsViewModel>();
        }

        
        private void UpdateWeather()
        {
            _weatherService.WeatherUpdated += () =>
            {
                CurrentWeatherCondition = _weatherService.CurrentWeatherCondition;
                CurrentOutsideTemperature = _weatherService.CurrentOutsideTemperature;
                CurrentInsideTemperature = _weatherService.CurrentInsideTemperature;
            };
        }

        private void UpdateDateAndTime()
        {
            _dateAndTimeService.TimeUpdated += () =>
            {
                CurrentDate = _dateAndTimeService.CurrentDate;
                CurrentTime = _dateAndTimeService.CurrentTime;
            };
        }

        [RelayCommand]
        private async Task StartStopButton(object parameter)
        {
            try
            {
                if (parameter is DeviceItemViewModel device)
                {
                    if (device.IsActive)
                    {
                        var deviceId = device.DeviceId;

                       
                        if (!string.IsNullOrEmpty(deviceId))
                        {
                            await _ioutHubManager.SendMethodAsync(new DirectMethodResponse
                            {
                                DeviceId = deviceId,
                                MethodName = "stop"
                            });
                        }
                    }
                    else
                    {
                       
                        var deviceId = device.DeviceId;
                        if (!string.IsNullOrEmpty(deviceId))
                        {
                            await _ioutHubManager.SendMethodAsync(new DirectMethodResponse
                            {
                                DeviceId = deviceId,
                                MethodName = "start"
                            });


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
