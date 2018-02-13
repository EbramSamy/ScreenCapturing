using Plugin.ScreenCapturing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SampleScreenShot
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var stream1 = new MemoryStream(await CrossScreenCapturing.Current.CaptureAsync(webView));
            IncidentImageData.Source = ImageSource.FromStream(() => stream1);
        }

   
    }
}
