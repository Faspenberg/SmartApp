using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Models;
using Shared.Services;
using SmartApp.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace SmartApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private static IHost? AppHost { get; set; }
        private string iothub;
        private string eventhub;
        private string eventhubname;
        private string consumergroup;


        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((config, services) =>
                {

                    iothub = config.Configuration.GetConnectionString("IotHub")!;
                    eventhub = config.Configuration.GetConnectionString("EventHubEndpoint")!;
                    eventhubname = config.Configuration.GetConnectionString("EventHubName")!;
                    consumergroup = config.Configuration.GetConnectionString("ConsumerGroup")!;

                    services.AddSingleton(new IotHubManager());

                    services.AddTransient<HttpClient>();
                    services.AddSingleton<DateAndTimeService>();
                    services.AddSingleton<WeatherService>();
                    services.AddSingleton<HomeViewModel>();
                    services.AddSingleton<SettingsViewModel>();
                    services.AddSingleton<AddDeviceViewModel>();
                    services.AddSingleton<DevicesViewModel>();
                    services.AddSingleton<MainWindowViewModel>();
                    services.AddSingleton<MainWindow>();
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost!.StartAsync();

            var mainWindow = AppHost!.Services.GetRequiredService<MainWindow>();
            var iotHubManager = AppHost!.Services.GetRequiredService<IotHubManager>();
            iotHubManager.Initialize(new IotHubManagerOptions
            {
                IotHubConnectionString = iothub,
                EventHubEndpoint = eventhub,
                EventHubName = eventhubname,
                ConsumerGroup = consumergroup,
            });

            mainWindow.Show();
            

            base.OnStartup(e);
        }
    }
}
