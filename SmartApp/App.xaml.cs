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

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((config, services) =>
                {
                    services.AddSingleton (new IotHubManager(new IotHubManagerOptions
                    {
                        IotHubConnectionString = config.Configuration.GetConnectionString("IotHub")!,
                        EventHubEndpoint = config.Configuration.GetConnectionString("EventHubEndpoint")!,
                        EventHubName = config.Configuration.GetConnectionString("EventHubName")!,
                        ConsumerGroup = config.Configuration.GetConnectionString("ConsumerGroup")!
                    }));

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
            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}
