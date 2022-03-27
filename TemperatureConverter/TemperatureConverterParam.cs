using System;
using System.Collections.Generic;

namespace TemperatureConverter
{
    public class TemperatureConverterParam
    {
        public string SensorID { get; set; }

        public IEnumerable<Reading> Readings { get; set; }
    }
}
