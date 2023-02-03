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
	public partial class QrToSendPass : ContentPage
	{
        //public string User { get; set; }
        //public string Password { get; set; }

        NoMorePass noMorePass = new NoMorePass();
        ZXingBarcodeImageView Qr;
        public QrToSendPass ()
		{
			InitializeComponent ();
            this.BindingContext = this;
        }

        private async void QrButtonSend_Clicked(object sender, EventArgs e)
        {
            //Generate QR to send User / Pass / Site to the phone

            var user = EntryQR.Text;
            var password = EntryQRPass.Text;

            if (user == null || user == "")
                user = "usertest";
            if (password == null || password == "")
                password = "mypassword";


            var QrCode = await noMorePass.GetQrSend(
               "Test password",
               user,
               password,
            new Dictionary<string, object>());

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

            Dictionary<string, object> res = null;
            await Task.Delay(TimeSpan.FromSeconds(10)).ContinueWith((t) =>
            {
                if (res == null)
                {
                    noMorePass.stopped = true;
                }
            }).ContinueWith((t) =>
            {
                if (noMorePass.stopped == true)
                {
                    //Aborted!
                    noMorePass.Stop();

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        stkQR.Children.Remove(Qr);
                    });
                }
            });

            res = await noMorePass.Send();
            res = null;
        }
    }
}