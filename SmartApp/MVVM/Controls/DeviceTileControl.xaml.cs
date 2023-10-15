using Microsoft.Azure.Devices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Rest;
using Newtonsoft.Json;
using Shared.Models;
using Shared.Services;
using SmartApp.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartApp.MVVM.Controls
{

    public partial class DeviceTileControl : UserControl
    {
        private readonly IotHubManager _iotHubManager;



        public DeviceTileControl(IServiceProvider serviceProvider)
        {
            InitializeComponent();
        }


        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var deviceItem = button!.DataContext as DeviceItemViewModel;

            if (deviceItem != null)
            {
                if (deviceItem.IsActive)
                {
                    var directMethodResponse = new DirectMethodResponse()
                    {
                        DeviceId = deviceItem.DeviceId!,
                        MethodName = "stop"
                    };

                    await _iotHubManager.SendMethodAsync(directMethodResponse);
                }

                if (!deviceItem.IsActive)
                {
                    var directMethodResponse = new DirectMethodResponse()
                    {
                        DeviceId = deviceItem.DeviceId!,
                        MethodName = "start"
                    };

                    await _iotHubManager.SendMethodAsync(directMethodResponse);
                }
            }


        }


    }
}




