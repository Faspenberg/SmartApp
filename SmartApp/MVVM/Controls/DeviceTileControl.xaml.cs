using Microsoft.Azure.Devices;
using Newtonsoft.Json;
using Shared.Models;
using Shared.Services;
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


        public DeviceTileControl()
        {
            InitializeComponent();
            _iotHubManager = new IotHubManager();
        }





        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                DirectMethodResponse methodRequest = new DirectMethodResponse
                {
                    DeviceId = methodRequest.DeviceId, 
                    MethodName = methodRequest.MethodName,  
                    ResponseTimeout = methodRequest.ResponseTimeout,               
                };

                var result = await _iotHubManager.SendMethodAsync(methodRequest);

               
                if (result != null)
                {
                    
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                
            }
        }


    }
}




