using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;
using System.Xml.Serialization;

namespace YTG.CSVtoGPX.Models.GPX
{
    public class Bounds
    {
        [XmlAttribute]
        public string minlat { get; set; }

        [XmlAttribute]
        public string minlon { get; set; }

        [XmlAttribute]
        public string maxlat { get; set; }

        [XmlAttribute]
        public string maxlon { get; set; }

    }
}
