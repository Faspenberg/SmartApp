using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Contexts;
using Shared.Models;
using Shared.Models.Entities;
using Shared.Services;
using SmartApp.MVVM.ViewModels;

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
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                })
               .ConfigureServices((config, services) =>
               {
                   services.AddTransient<HttpClient>();
                   services.AddDbContext<SmartAppDbContext>((provider, options) =>
                   {
                       options.UseSqlite($"DataSource=Database.sqlite.db", x => x.MigrationsAssembly(nameof(Shared)));
                   }, ServiceLifetime.Scoped);


                   using (var scope = services.BuildServiceProvider().CreateScope())
                   {
                       var dbContext = scope.ServiceProvider.GetRequiredService<SmartAppDbContext>();

                       if (!dbContext.DatabaseExists())
                           dbContext.Database.Migrate();

                       if (!dbContext.ConnectionStringsExists())
                       {
                           dbContext.Settings.Add(new SmartAppSettings
                           {
                               Id = 1,
                               IotHubConnectionString = config.Configuration.GetConnectionString("IotHub")!,
                               EventHubEndpoint = config.Configuration.GetConnectionString("EventHubEndpoint")!,
                               EventHubName = config.Configuration.GetConnectionString("EventHubName")!,
                               ConsumerGroup = config.Configuration.GetConnectionString("ConsumerGroup")!
                           });

                           dbContext.SaveChanges();
                       }
                   }

                   services.AddSingleton<IotHubManager>();
                   services.AddSingleton<SmartAppDbService>();
                   services.AddSingleton<DateAndTimeService>();
                   services.AddSingleton<WeatherService>();
                   services.AddSingleton<HomeViewModel>();
                   services.AddSingleton<SettingsViewModel>();
                   services.AddSingleton<AddDeviceViewModel>();
                   services.AddSingleton<DevicesViewModel>();
                   services.AddSingleton<SetupViewModel>();
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
