﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Models;

namespace Shared.Models.Entities
{
    public class SmartAppSettings
    {
        [Key] 
        public int Id { get; set; }
        public string IotHubConnectionString { get; set; } = null!;
        public string EventHubEndpoint { get; set; } = null!;
        public string EventHubName { get; set; } = null!;
        public string ConsumerGroup { get; set; } = null!;



        public static implicit operator SmartAppSettings(IotHubManagerOptions dto)
        {
            return new SmartAppSettings()
            {
                IotHubConnectionString = dto.IotHubConnectionString,
                EventHubEndpoint = dto.EventHubEndpoint,
                EventHubName = dto.EventHubName,
                ConsumerGroup = dto.ConsumerGroup,
            };
        }


    }
}
