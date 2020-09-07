using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;
using System.Xml.Serialization;

namespace YTG.CSVtoGPX.Models.GPX
{
    public class Wpt
    {
        [XmlAttribute]
        public string lat { get; set; }
        [XmlAttribute]
        public string lon { get; set; }

        public string time { get; set; }
        public string name { get; set; }
        public string desc { get; set; }
        public string cmt { get; set; }
        public string url { get; set; }
        public string urlname { get; set; }
        public string sym { get; set; }
        public string type { get; set; }
        public string src { get; set; }

    }
}
