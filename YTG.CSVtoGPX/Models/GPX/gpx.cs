using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using System.Windows.Media.TextFormatting;
using System.Xml.Serialization;

namespace YTG.CSVtoGPX.Models.GPX
{
    public class gpx
    {

        List<Wpt> m_waypoints = new List<Wpt>();

        [XmlAttribute]
        public string creator { get; set; }

        public string name { get; set; }
        public string desc { get; set; }
        public string author { get; set; }
        public string email { get; set; }
        public string time { get; set; }
        public string keywords { get; set; }
        public Bounds bounds { get; set; }

        [XmlElement(ElementName ="wpt")]
        public List<Wpt> WayPoints
        {
            get
            {
                return m_waypoints;
            }
            set
            {
                m_waypoints = value;
            }
        }



    }
}
