using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class DirectMethodResponse
    {
        public string DeviceId { get; set; } = null!;

        public string MethodName { get; set; } = null!;


        public string? Message { get; set; }

        public object? Payload { get; set; }
    }
}
