using Newtonsoft.Json;
using Shared.Models.DataDeviceModels;
using Shared.Services;
using System;
using System.Collections.Generic;
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

namespace SpeakerDevice
{
    public partial class MainWindow : Window
    {
        private readonly DeviceManager _deviceManager;

        public MainWindow(DeviceManager deviceManager)
        {
            InitializeComponent();
            _deviceManager = deviceManager;
            Task.WhenAll(SetDeviceTypeAsync(), SendTelemetryDataAsync(), CheckConnectivityAsync(),
                TogglePrinterStateAsync());
        }


        private async Task TogglePrinterStateAsync()
        {

            while (true)
            {

                if (_deviceManager.AllowSending())
                {
                    SpeakerOnIcon.Visibility = Visibility.Visible;
                    SpeakerOffIcon.Visibility = Visibility.Collapsed;
                }
                else
                {
                    SpeakerOnIcon.Visibility = Visibility.Collapsed;
                    SpeakerOffIcon.Visibility = Visibility.Visible;

                }

                await Task.Delay(1000);
            }
        }

        private async Task CheckConnectivityAsync()
        {
            while (true)
            {
                ConnectivityStatus.Text = await NetworkManager.CheckConnectivityAsync();
                await Task.Delay(1000);

            }
        }

        private async Task SetDeviceTypeAsync()
        {
            var deviceType = "Speaker";

            await _deviceManager.SendDeviceTypeAsync(deviceType);
        }




        private async Task SendTelemetryDataAsync()
        {

            while (true)
            {
                if (_deviceManager.Configuration.AllowSending)
                {
                    var dataModel = new SpeakerDataModel()
                    {
                        IsActive = true,
                        Volume = "Medium",
                        Location = "LivingRoom",
                        CurrentTime = DateTime.Now
                    };


                    var latestMessageJson = JsonConvert.SerializeObject(new
                    {
                        Volume = dataModel.Volume,
                        CurrentTime = dataModel.CurrentTime,
                        DeviceOn = dataModel.IsActive,
                        ContainerName = dataModel.ContainerName,
                    });


                    var operationalStatusJson = JsonConvert.SerializeObject(dataModel.IsActive);


                    if (await _deviceManager.SendMessageAsync(latestMessageJson) &&
                        await _deviceManager.SendOperationalStatusAsync(operationalStatusJson))
                        CurrentMessageSent.Text = $"Message sent successfully: {latestMessageJson}";


                    var telemetryInterval = _deviceManager.Configuration.TelemetryInterval;

                    await Task.Delay(telemetryInterval);
                }
            }


        }
    }
}
