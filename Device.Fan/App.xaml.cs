
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Models.Devices;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Device.Fan
{
    public partial class App : Application
    {
        public static IHost? host { get; set; }

        public App()
        {

            DeviceRegistrationSetup();

            host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                })
                .ConfigureServices((config, services) =>
                {
                    services.AddSingleton<MainWindow>();
                    services.AddSingleton(new DeviceConfiguration(config.Configuration.GetConnectionString("Device")!));
                    services.AddSingleton<DeviceManager>();
                    services.AddSingleton<NetworkManager>();
                })
                .Build();
        }

        private async void DeviceRegistrationSetup()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var root = configurationBuilder.Build();
            var connectionString = root.GetConnectionString("FanDevice");

            if (string.IsNullOrEmpty(connectionString))
            {
                var newDeviceId = "fan_device";
                var deviceType = "fan";

                var registrationManager = new RegistrationManager();
                connectionString = await registrationManager.RegisterDevice(newDeviceId, deviceType);

                configurationBuilder = new ConfigurationBuilder();
                root = configurationBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
                var configSource = new Dictionary<string, string>
                {
                    { "ConnectionStrings:FanDevice", connectionString }
                };
                root = configurationBuilder.AddInMemoryCollection(configSource).Build();
            }
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await host!.StartAsync();

            var mainWindow = host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}