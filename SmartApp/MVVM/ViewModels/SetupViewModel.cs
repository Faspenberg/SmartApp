using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartApp.MVVM.ViewModels
{
    public partial class SetupViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly WeatherService _weatherService;

        private string _weatherUpdate;
        private string _iotHubConnectionString;
        private string _eventHubEndPoint;
        private string _eventHubName;
        private string _consumerGroup;


        public string WeatherUpdate
        {
            get { return _weatherUpdate; }
            set { SetProperty(ref _weatherUpdate, value); }
        }

        public string IotHubConnectionString
        {
            get { return _iotHubConnectionString; }
            set { SetProperty(ref _iotHubConnectionString, value); }
        }

        public string EventHubEndpoint
        {
            get { return _eventHubEndPoint; }
            set { SetProperty(ref _eventHubEndPoint, value); }
        }

        public string EventHubName
        {
            get { return _eventHubName; }
            set { SetProperty(ref _eventHubName, value); }
        }

        public string ConsumerGroup
        {
            get { return _consumerGroup; }
            set { SetProperty(ref _consumerGroup, value); }
        }


        public SetupViewModel(IServiceProvider serviceProvider, WeatherService weatherService)
        {
            _serviceProvider = serviceProvider;
            _weatherService = weatherService;


        }

        [RelayCommand]
        private void NavigateToSettings()
        {
            var mainWindowViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            mainWindowViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<SettingsViewModel>();
        }


    }





}


