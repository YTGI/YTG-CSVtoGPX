// --------------------------------------------------------------------------------
/*  Copyright © 2020, Yasgar Technology Group, Inc.
    Any unauthorized review, use, disclosure or distribution is prohibited.

    Purpose: UI to Convert CSV file to GPX compliant file

    Description: This is a project done for a hobby project and was not written
                 for any commercial purposes.

*/
// --------------------------------------------------------------------------------



using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace YTG.CSVtoGPX
{

    public delegate void ProgressHandler(object o, ProgressEventArgs e);
    public delegate void ProgressCompleteHandler(object o);
    public delegate void CSVProgressCompleteHandler(object o);

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Fields

        private delegate void LoadMessageDelegate(string message);
        private delegate void WriteLogDelegate();
        private delegate void FillGridDelegate();
        private Microsoft.Win32.OpenFileDialog m_OpenFileDialog;
        private ObservableCollection<string> m_LogMessages;

        #endregion // Fields

        private void InitializeForm()
        {
            this.txtVersion.Text = "Version: " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// Process the data when Member Search Process is complete
        /// </summary>
        /// <param name="o"></param>
        private void ProgressComplete(object o)
        {
            if (o.GetType().Name == "ProcessSheet")
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Background, new WriteLogDelegate(WriteLogFile));
            }
        }

        private void SingleLineProgressComplete(object o)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Background, new WriteLogDelegate(WriteLogFile));
        }


        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(txtFilePath.Text))
                {
                    System.Windows.MessageBox.Show("Please select the CSV input file!");
                    return;
                }

                this.Cursor = Cursors.Wait;

                this.LogMessages.Clear();
                lstResults.ItemsSource = LogMessages;

                this.LogMessages.Add("Process Started: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
                this.LogMessages.Add("Source CSV file: " + txtFilePath.Text);

                ProcessFile FileProcess = new ProcessFile(txtFilePath.Text);
                FileProcess.ProgressEvent += new ProgressHandler(ShowProgressEvents);
                FileProcess.ProgressCompleteEvent += new CSVProgressCompleteHandler(SingleLineProgressComplete);


                Thread _thread = new Thread(() => FileProcess.Process());
                _thread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, ex.Message);
            }
        }

        private void LoadMessage(string message)
        {
            LogMessages.Insert(0, message);
        }

        private void ShowProgressEvents(object o, ProgressEventArgs e)
        {
            // this.LogMessages.Add(e.Description);
            this.Dispatcher.BeginInvoke(DispatcherPriority.Background, new LoadMessageDelegate(LoadMessage), e.Description);
        }

        private void btnFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                m_OpenFileDialog = new Microsoft.Win32.OpenFileDialog();
                m_OpenFileDialog.Filter = "CSV Files|*.csv";
                Nullable<bool> result = m_OpenFileDialog.ShowDialog();

                if (result == true)
                {
                    txtFilePath.Text = m_OpenFileDialog.FileName;

                    lstResults.Visibility = System.Windows.Visibility.Visible;


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, ex.Message);
            }
        }

        /// <summary>
        /// Write the Log Messages to a log file
        /// </summary>
        private void WriteLogFile()
        {
            string LogFileName;
            string LogFilePath;

            LogFilePath = System.IO.Path.GetDirectoryName(txtFilePath.Text) + "\\";
            string datestring = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("0#") + DateTime.Now.Day.ToString("0#") +
                                                                DateTime.Now.Hour.ToString("0#") + DateTime.Now.Minute.ToString("0#");
            LogFileName = System.IO.Path.GetFileNameWithoutExtension(txtFilePath.Text) + "-" + datestring;

            FileInfo LogFile = new FileInfo(LogFilePath + LogFileName + ".log");

            if (File.Exists(LogFile.Name))
            { File.Delete(LogFile.Name); }

            StreamWriter LogWriter = LogFile.AppendText();
            foreach (string log in this.LogMessages)
            {
                LogWriter.WriteLine(log);
            }
            // LogFile.Create();
            LogWriter.Flush();
            LogWriter.Close();

            // this.LogMessages.Clear();
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// Log messages during processing
        /// </summary>
        private ObservableCollection<string> LogMessages
        {
            get
            {
                if (m_LogMessages == null)
                { m_LogMessages = new ObservableCollection<string>(); }
                return m_LogMessages;
            }
            set { m_LogMessages = value; }
        }


    }
}
