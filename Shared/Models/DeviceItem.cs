﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class DeviceItem : INotifyPropertyChanged
    {
        public DeviceItem()
        {
        }

        public string? DeviceId { get; set; }

        public string? DeviceType { get; set; }

        public string? Location { get; set; }

        public string? Vendor { get; set; }

        public bool _isActive;

        public bool IsActive { get { return _isActive; } set { if (IsActive != value) { _isActive = value; OnPropertyChanged(); } } }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
