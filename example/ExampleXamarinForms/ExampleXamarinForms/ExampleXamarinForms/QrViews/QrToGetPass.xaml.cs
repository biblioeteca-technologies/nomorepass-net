using nomorepass_net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace ExampleXamarinForms.QrViews
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class QrToGetPass : ContentPage
	{
        public string User { get; set; }
        public string Password { get; set; }

        NoMorePass noMorePass = new NoMorePass();
        ZXingBarcodeImageView Qr;
        public QrToGetPass ()
		{
			InitializeComponent ();
            this.BindingContext= this;
		}

        private async void QrButtonSend_Clicked(object sender, EventArgs e)
        {
            int expiry = (int)(DateTime.UtcNow.AddSeconds(20) - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            noMorePass.SetExpiry(expiry); // 20 sec expiry

            QrGenerator.IsEnabled = false;

            // QR to send a password back
            var QrCode = await noMorePass.GetQrText("xamarin");

            Qr = new ZXingBarcodeImageView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
            };
            Qr.BarcodeFormat = ZXing.BarcodeFormat.QR_CODE;
            Qr.BarcodeOptions.Width = 200;
            Qr.BarcodeOptions.Height = 200;
            Qr.BarcodeValue = QrCode;
            stkQR.Children.Add(Qr);

            Dictionary<string, string> res2 = null;
            await Task.Delay(TimeSpan.FromSeconds(10)).ContinueWith((t) =>
            {
                if (res2 == null)
                {
                    noMorePass.stopped = true;
                }
            }).ContinueWith((t) =>
            {
                if (noMorePass.stopped == true)
                {
                    noMorePass.Stop();
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        stkQR.Children.Remove(Qr);
                    });
                }
            });

            noMorePass.stopped = false;

            var resp2 = await noMorePass.Start();
            if (resp2 != null && resp2.ContainsKey("error"))
            {
                //Error
            }
            else
            {
                if (resp2 != null)
                {
                    User = resp2["user"];
                    Password = resp2["password"];

                    EntryQR.Text = User;
                    EntryQRPass.Text = Password;
                    QrGenerator.IsEnabled = true;
                }
            }
            resp2 = null;
        }
    }
}