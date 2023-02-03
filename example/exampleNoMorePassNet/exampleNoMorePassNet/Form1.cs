using nomorepass_net;

namespace exampleNoMorePassNet
{
    public partial class Form1 : Form
    {
        NoMorePass noMorePass = new NoMorePass();
        public Form1()
        {
            InitializeComponent();
        }


        private async void TestButton_Click(object sender, EventArgs e)
        {
            //Generate QR to send User / Pass / Site to the phone
            int expiry = (int)(DateTime.UtcNow.AddSeconds(20) - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            noMorePass.SetExpiry(expiry); // 20 sec expiry

            noMorePass.stopped = false;

            var user = UserBox.Text;
            var password = PasswordBox.Text;

            if (user == null || user == "")
                user = "usertest";
            if (password == null || password == "")
                password = "mypassword";


            var test1 = await noMorePass.GetQrSend(
               "Test password",
               user,
               password,
            new Dictionary<string, object>());
            Zen.Barcode.CodeQrBarcodeDraw qrTest = Zen.Barcode.BarcodeDrawFactory.CodeQr;
            QrBox.Image = qrTest.Draw(test1, 100);

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

                    QrBox.Image = null;          //elimina el qr de la pantalla
                }
            });

            res = await noMorePass.Send();
            res = null;
        }

        private async void QRPasswordBack_Click(object sender, EventArgs e)
        {
            noMorePass.stopped = false;

            int expiry = (int)(DateTime.UtcNow.AddSeconds(20) - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            noMorePass.SetExpiry(expiry); // 20 sec expiry

            // QR to send a password back
            var test2 = await noMorePass.GetQrText("prueba");

            Zen.Barcode.CodeQrBarcodeDraw qrTest = Zen.Barcode.BarcodeDrawFactory.CodeQr;
            QrBox2.Image = qrTest.Draw(test2, 100);


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

                    QrBox2.Image = null;          //elimina el qr de la pantalla

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
                    UserBoxBack.Text = resp2["user"];
                    PasswordBoxBack.Text = resp2["password"];
                }
            }
            resp2 = null;
        }
    }
}