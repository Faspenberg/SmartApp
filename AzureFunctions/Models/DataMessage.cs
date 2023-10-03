using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureFunctions.Models
{
    public class DataMessage
    {
        public string id { get; set; } = Guid.NewGuid().ToString();
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public int _ts { get; set; }
    }
}
