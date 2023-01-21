using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Accelerometer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccelerometerPage : ContentPage
    {
        private ISimpleAudioPlayer _simpleAudioPlayer;
        SensorSpeed speed = SensorSpeed.UI;
        bool IsBusy = false;

        float xDiff = 0;
        float zDiff = 0;
        float yDiff = 0;

        public AccelerometerPage()
        {
            InitializeComponent();
            _simpleAudioPlayer = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            Stream beepStream = GetType().Assembly.GetManifestResourceStream("Accelerometer.beep.mp3");

            //TODO Fix the beepStream null issue.
            bool isSuccess = _simpleAudioPlayer.Load(beepStream);
            Xamarin.Essentials.Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
            Xamarin.Essentials.Accelerometer.Start(speed);
        }

        private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {

            var data = e.Reading;

            if ((xDiff - data.Acceleration.X) > .003 || (zDiff - data.Acceleration.Z) > .003 || (yDiff - data.Acceleration.Y) > .003)
            {

                if (!IsBusy)
                {
                    IsBusy = true;
                    Device.BeginInvokeOnMainThread(() =>
                {
                    {
                        ReadingsText.Text = $"Reading: X: {data.Acceleration.X}, Y: {data.Acceleration.Y}, Z: {data.Acceleration.Z}";
                    }
                    //System.Threading.Thread.Sleep(2000);
                    _simpleAudioPlayer.Play();
                    IsBusy = false;
                });
                    xDiff = data.Acceleration.X;
                    yDiff = data.Acceleration.Y;
                    zDiff = data.Acceleration.Z;
                }
                else
                {
                    Debug.WriteLine("BUSY");
                }
            }
        }

        private void MonitorSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            if (Xamarin.Essentials.Accelerometer.IsMonitoring)
                Xamarin.Essentials.Accelerometer.Stop();
            else
                Xamarin.Essentials.Accelerometer.Start(speed);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            _simpleAudioPlayer.Play();
        }
    }
}