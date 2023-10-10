using CommunityToolkit.Mvvm.ComponentModel;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartApp.MVVM.ViewModels
{
    public partial class DeviceItemViewModel : ObservableObject
    {

        private DeviceItem _deviceItem;

        public DeviceItemViewModel(DeviceItem deviceItem)
        {
            _deviceItem = deviceItem;
            Location = deviceItem.Location ?? "";
            IsActive = deviceItem.IsActive;
            Icon = SetDeviceIcon();

        }

        public string DeviceId => _deviceItem.DeviceId = null!;

        public string? DeviceType => _deviceItem.DeviceType;

        public string? Vendor => _deviceItem.Vendor;

        private string SetDeviceIcon()
        {
            switch (DeviceType?.ToLower())
            {
                case "lamp":
                    return "\uf0eb";

                case "fan":
                    return "\ue004";

                case "speaker":
                    return "\uf6a9";

                default:
                    return "\uf2db";
            }
        }

        [ObservableProperty]

        string location;

        [ObservableProperty]

        bool isActive;

        [ObservableProperty]

        string icon;
    }
}
