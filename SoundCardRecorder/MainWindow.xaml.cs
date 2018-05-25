using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CSCore;
using CSCore.SoundIn;
using CSCore.Codecs.WAV;
using CSCore.Codecs.MP3;
using CSCore.MediaFoundation;

namespace SoundCardRecorder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WasapiCapture capture;
        private MediaFoundationEncoder writer;
        public MainWindow()
        {
            InitializeComponent();

            //if nessesary, you can choose a device here
            //to do so, simply set the device property of the capture to any MMDevice
            //to choose a device, take a look at the sample here: http://cscore.codeplex.com/
            //initialize the selected device for recording
        }

        private void StartRecordBtn_Clicked(object sender, RoutedEventArgs ev)
        {
            capture = new WasapiLoopbackCapture();
            capture.Initialize();
            DateTime date = DateTime.Now;
            string filename = string.Format(@"F:\Share\DailyMeetingRecord\{0:yyyy_MM_dd}.mp3", date);
            //create a wavewriter to write the data to
            writer = MediaFoundationEncoder.CreateMP3Encoder(capture.WaveFormat, filename);
            byte[] buffer = new byte[capture.WaveFormat.BytesPerSecond];
            //setup an eventhandler to receive the recorded data
            capture.DataAvailable += (s, e) =>
            {
                //save the recorded audio
                writer.Write(e.Data, e.Offset, e.ByteCount);
            };
            //start recording
            capture.Start();
            this.StartRecordBtn.IsEnabled = false;
            this.StopRecordBtn.IsEnabled = true;
        }

        private void StopRecordBtn_Clicked(object sender, RoutedEventArgs e)
        {
            //stop recording
            if (null != capture)
            {
                capture.Stop();
                writer.Dispose();
                writer = null;
                capture.Dispose();
                capture = null;
                this.StartRecordBtn.IsEnabled = true;
                this.StopRecordBtn.IsEnabled = false;

            }
        }
    }
}
