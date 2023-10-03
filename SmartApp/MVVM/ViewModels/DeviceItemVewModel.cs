using CommunityToolkit.Mvvm.ComponentModel;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartApp.MVVM.ViewModels
{
    public partial class DeviceItemVewModel : ObservableObject
    {

        private DeviceItem _deviceItem;

        public DeviceItemVewModel(DeviceItem deviceItem)
        {
            _deviceItem = deviceItem;
            Location = deviceItem.Location ?? "";
            IsActive = deviceItem.IsActive;
            Icon = SetDeviceIcon();

        }

        public string DeviceId => _deviceItem.DeviceId;

        public string DeviceType => _deviceItem.DeviceType;

        public string Vendor => _deviceItem.Vendor;

        private string SetDeviceIcon()
        {
            return DeviceType.ToLower() switch
            {
                "light" => "\uf0eb",
                _ => "\uf2db",
            };
        }

        [ObservableProperty]

        string location;

        [ObservableProperty]

        bool isActive;

        [ObservableProperty]

        string icon;
    }
}
