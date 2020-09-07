// --------------------------------------------------------------------------------
/*  Copyright © 2020, Yasgar Technology Group, Inc.
    Any unauthorized review, use, disclosure or distribution is prohibited.

    Purpose: Main Processor Class for CSV import and GPX production

    Description: 

*/
// --------------------------------------------------------------------------------

using CsvHelper;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using YTG.CSVtoGPX.Code;
using System.Threading.Tasks;

namespace YTG.CSVtoGPX
{
    public class ProcessFile
    {


        #region Fields

        public event ProgressHandler ProgressEvent;
        public event CSVProgressCompleteHandler ProgressCompleteEvent;
        private static int thisCounter = 0;
        private static object syncObj = new object();


        #endregion // Fields

        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ProcessFile(string filePath)
        {
            this.FilePath = filePath;
        }

        #endregion // Constructors

        #region Properties

        private string FilePath { get; }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Process the file selected on the Main Window
        /// </summary>
        public void Process()
        {
            List<Models.RawImport> _rawImport = new List<Models.RawImport>();

            long LogCount = 10;

            lock (syncObj)
            {
                thisCounter = 0; // reset
            }

            try
            {
                OnProgressEvent(new ProgressEventArgs(LogCount++, "Beginning CSV File Process!"));

                using (var reader = new StreamReader(this.FilePath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    _rawImport = csv.GetRecords<Models.RawImport>().ToList();
                }

                ConcurrentBag<Models.GPX.gpx> _gpxs = new ConcurrentBag<Models.GPX.gpx>();

                Models.GPX.gpx _gpx = new Models.GPX.gpx();
                _gpx.author = "Yasgar Technology Group Inc.";
                _gpx.desc = "Custom GPX file made for Jack";
                _gpx.name = this.FilePath;
                _gpx.time = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:ssZ");
                _gpx.creator = "YTGI";

                Parallel.ForEach(_rawImport, (_item) =>
                {
                    // Leave out ones with no coords
                    if (!string.IsNullOrWhiteSpace(_item.Latitude.ToString()))
                    {
                        DateTime _time = DateTime.Now;
                        if (string.IsNullOrEmpty(_item.LastUpdate))
                        { _item.LastUpdate = DateTime.Now.ToString(); }
                        else
                        {
                            if (DateTime.TryParse(_item.LastUpdate.ToString(), out DateTime _tempTime))
                            { _time = _tempTime; }
                        }

                        Models.GPX.Wpt _wpt = new Models.GPX.Wpt();
                        _wpt.cmt = _item.Description;
                        _wpt.src = _item.Source;
                        if (string.IsNullOrWhiteSpace(_item.Information.Trim(256))) { _wpt.desc = _item.Information.Trim(256); } else { _wpt.desc = _item.Description; }
                        _wpt.lat = _item.Latitude.ToString("##.#00000000");
                        _wpt.lon = _item.Longitude.ToString("##.#00000000");
                        _wpt.name = _item.Name;
                        _wpt.time = _time.ToString("yyyy-MM-ddThh:mm:ssZ");

                        _wpt.sym = "Crossing"; // https://freegeographytools.com/2008/garmin-gps-unit-waypoint-icons-table

                        _gpx.WayPoints.Add(_wpt);

                        OnProgressEvent(new ProgressEventArgs(LogCount++, "Name: " + _wpt.name + " - Added to file!"));

                        lock (syncObj)
                        {
                            thisCounter++;
                        }

                    }
                });

                _gpx.bounds = FillBounds(_rawImport);

                System.Xml.Serialization.XmlSerializer xmlS = new System.Xml.Serialization.XmlSerializer(typeof(Models.GPX.gpx), "http://www.topografix.com/GPX/1/0");
                Utf8StringWriter sw = new Utf8StringWriter();
                System.Xml.XmlWriter xw = System.Xml.XmlWriter.Create(sw, new System.Xml.XmlWriterSettings { OmitXmlDeclaration = false, Indent = true });
                xmlS.Serialize(xw, _gpx);

                string _outputPath = Path.GetDirectoryName(FilePath);
                string _filename = Path.GetFileNameWithoutExtension(FilePath);
                _outputPath = Path.Combine(_outputPath, DateTime.Now.ToDateTimeTimeStampString() + "-" + _filename + ".gpx");

                File.WriteAllText(_outputPath, sw.ToString());

                OnProgressEvent(new ProgressEventArgs(LogCount++, "Processed " + thisCounter.ToString() + " records from the CSV!"));

                OnProgressEvent(new ProgressEventArgs(LogCount++, "Process Ended: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString()));

                OnProgressCompleteEvent();

                return;

            }
            catch (Exception ex)
            {
                OnProgressEvent(new ProgressEventArgs("An Error Occurred: " + ex.Message + " - " + ex.StackTrace));
                OnProgressCompleteEvent();
            }
        }

        /// <summary>
        /// Fills the bounds object for GPX file production based on imported data
        /// </summary>
        /// <param name="raw"></param>
        /// <returns></returns>
        public Models.GPX.Bounds FillBounds(List<Models.RawImport> raw)
        {
            Models.GPX.Bounds _bounds = new Models.GPX.Bounds();
            try
            {
                var _lats = (from lat in raw
                             orderby lat.Latitude ascending
                             select lat);

                _bounds.minlat = _lats.FirstOrDefault().Latitude.ToString("##.#00000000");
                _bounds.maxlat = _lats.LastOrDefault().Latitude.ToString("##.#00000000");

                var _longs = (from lng in raw
                              orderby lng.Longitude ascending
                              select lng);

                _bounds.minlon = _longs.FirstOrDefault().Longitude.ToString("##.#00000000");
                _bounds.maxlon = _longs.LastOrDefault().Longitude.ToString("##.#00000000");

                return _bounds;

            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        ///     A class only to override encoding with UTF8.
        /// </summary>
        public class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }

        #endregion // Methods

        #region Events

        /// <summary>
        /// Event raised for file processing progress
        /// </summary>
        /// <param name="e"></param>
        public void OnProgressEvent(ProgressEventArgs e)
        {
            if (ProgressEvent != null)
            { ProgressEvent(this, e); }
        }

        /// <summary>
        /// Event raised when process completed
        /// </summary>
        public void OnProgressCompleteEvent()
        {
            if (ProgressCompleteEvent != null)
            { ProgressCompleteEvent(this); }
        }

        #endregion // Events

    }
}
