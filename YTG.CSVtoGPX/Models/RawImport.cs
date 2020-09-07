using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace YTG.CSVtoGPX.Models
{
    public class RawImport
    {

        public string Name { get; set; }
        public string Description { get; set; }

        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        public string Source { get; set; }
        public string Information { get; set; }

        public string LastUpdate { get; set; }


    }
}
