﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.DataDeviceModels
{
    public class LampDataModel
    {
        public string ContainerName { get; set; } = "lampData";

        public bool IsActive { get; set; }

        public DateTime CurrentTime { get; set; }

        public string Location { get; set; }
    }
}
