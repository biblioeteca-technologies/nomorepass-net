using ExampleXamarinForms.QrViews;
using nomorepass_net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace ExampleXamarinForms
{
    public partial class MainPage : ContentPage
    {
        public string User { get; set; }
        public string Password { get; set; }


        NoMorePass noMorePass = new NoMorePass();
        ZXingBarcodeImageView Qr;
        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = this;
        }

        public async void QrButtonSend_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new QrToSendPass());
        }

        private void QrButtonGet_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new QrToGetPass());
        }
    }
}
