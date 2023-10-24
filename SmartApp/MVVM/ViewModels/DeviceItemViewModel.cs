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
            IsActive = deviceItem.IsActive;            
            Icon = SetIcon();
        }

        public string DeviceId => _deviceItem.DeviceId ?? "";

        public string DeviceType => _deviceItem.DeviceType ?? "";

        public string Location => _deviceItem.Location ?? "";

        [ObservableProperty]
        bool isActive;

        [ObservableProperty]
        string icon;

        public void Initialize()
        {
            SetIcon();
        }


        private string SetIcon()
        {
            switch (DeviceType?.ToLower())
            {
                case "lamp":
                    
                    return "\uf0eb";

                case "fan":
                    return "\uf863";

                case "speaker":
                    return "\uf6a9";

                default:
                    return "\ue55d";
            }

        }
    }
}
