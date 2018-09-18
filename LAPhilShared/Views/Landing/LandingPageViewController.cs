using Foundation;
using System;
using UIKit;

using Xamarin.Auth;
using System.Json;

namespace LAPhil.iOS
{
    public partial class LandingPageViewController : UIViewController
    {
        public LandingPageViewController (IntPtr handle) : base (handle)
        {
        }
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            btnClose.TouchUpInside += CloseView;
            btnFB.TouchUpInside += fbLogin_TouchUpInside;
        }

        private void CloseView(object sender, EventArgs e)
        {
            Console.WriteLine("Pop CloseView ");
            this.DismissViewController(true,null);
        }

        private void fbLogin_TouchUpInside(object sender, EventArgs e)
        {
            var auth = new OAuth2Authenticator(
                clientId: "1843461312586790",
                scope: "",
                authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
                redirectUrl: new Uri("http://www.facebook.com/connect/login_success.html"));

            auth.Completed += Auth_Completed;
            var ui = auth.GetUI();
            PresentViewController(ui, true, null);
        }

        private async void Auth_Completed(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (e.IsAuthenticated)
            {
                var request = new OAuth2Request("GET",
                                                new Uri("https://graph.facebook.com/me?fields=name,picture,cover,birthday")
                                                , null, e.Account);
                var response = await request.GetResponseAsync();
                var user = JsonValue.Parse(response.GetResponseText());
                var fbName = user["name"];
                var fbCover = user["cover"]["source"];
                var fbProfile = user["picture"]["data"]["url"];

                lblName.Text = fbName.ToString();
                imgCover.Image = UIImage.LoadFromData(NSData.FromUrl(new NSUrl(fbCover)));
                imgCover.ContentMode = UIViewContentMode.ScaleAspectFit;
                imgProfile.Image = UIImage.LoadFromData(NSData.FromUrl(new NSUrl(fbProfile)));
                imgProfile.ContentMode = UIViewContentMode.ScaleAspectFit;
            }
            //DismissViewController(true, null);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

    }
}