using DataAccess.Contexts;
using Microsoft.Azure.Devices;
using Microsoft.EntityFrameworkCore;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Azure.Devices.Shared;

namespace DataAccess.Services
{
    public class IotHubManager
    {
        private System.Timers.Timer _timer;
        private bool isConfigured;
        private readonly ZeniAppDbContext _dbContext;
        private RegistryManager? _registryManager;
        private ServiceClient? _serviceClient;

        public IotHubManager(ZeniAppDbContext dbContext)
        {
            _dbContext = dbContext;
            DeviceList = new List<DeviceItem>();
            _timer = new System.Timers.Timer(5000);
            _timer.Elapsed += async (s, e) => await GetAllDevicesAsync();
            _timer.Start();
        }

        public List<DeviceItem> DeviceList { get; private set; }
        public event Action? DeviceListUpdated;
        public async Task InitializeAsync()
        {
            try
            {
                if (!isConfigured)
                {
                    await Task.Delay(1000);
                    var connectionString = "HostName=kyh-smartUnit-iotUppgift.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=7ML/rHjdnkMKOoZcQ3OoyDufPOxkx453+AIoTOFv98Y=";
                    var settings = await _dbContext.Settings.FirstOrDefaultAsync();
                    if (settings != null)
                    {
                        _registryManager = RegistryManager.CreateFromConnectionString(connectionString);
                        _serviceClient = ServiceClient.CreateFromConnectionString(connectionString);
                        isConfigured = true;  
                    }
                }
               
                isConfigured = true;
            }
            catch (Exception ex ) { Debug.Write(ex.Message); }
        }

        private async Task GetAllDevicesAsync()
        {
            try
            {
                if (isConfigured)
                {
                    var listUpdated = false;
                    var twins = new List<Twin>();
                    var result = _registryManager?.CreateQuery("SELECT * FROM Devices");

                    foreach (var item in await result!.GetNextAsTwinAsync())
                    {
                        twins.Add(item);

                    }

                    foreach (var device in twins)
                    {
                        if (!DeviceList.Any(x => x.DeviceId == device.DeviceId))
                        {
                            DeviceList.Add(new DeviceItem { DeviceId = device.DeviceId, });
                            listUpdated = true;
                        }

                        for(int i = DeviceList.Count; i > 0; i--)
                        {
                            if (!twins.Any(x => x.DeviceId == DeviceList[i].DeviceId))
                            {
                                DeviceList.RemoveAt(i);
                                listUpdated = true;
                            }
                        }

                        if (listUpdated)
                            DeviceListUpdated?.Invoke();


                    }
                }


            }
            catch (Exception ex ) { Debug.WriteLine(ex.Message); }
        }
    }
}
