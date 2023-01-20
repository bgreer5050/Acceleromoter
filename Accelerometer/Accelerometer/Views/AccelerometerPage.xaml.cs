using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
        float xDiff = 0;
        float zDiff = 0;
        float yDiff = 0;

        public AccelerometerPage()
        {
            InitializeComponent();
            _simpleAudioPlayer = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            Stream beepStream = GetType().Assembly.GetManifestResourceStream("Accelerometer.beep.mp3");
            bool isSuccess = _simpleAudioPlayer.Load(beepStream);
            Xamarin.Essentials.Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
            Xamarin.Essentials.Accelerometer.Start(speed);


        }

        private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() => {
                var data = e.Reading;
                
                if((xDiff - data.Acceleration.X) > .007 || (zDiff - data.Acceleration.Z) > .007)
                {
                    ReadingsText.Text = $"Reading: X: {data.Acceleration.X}, Y: {data.Acceleration.Y}, Z: {data.Acceleration.Z}";
                }


                
            });
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