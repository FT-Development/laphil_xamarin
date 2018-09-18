using Foundation;
using System;
using System.Text.RegularExpressions; 
using UIKit;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using LAPhilShared;
using System.Diagnostics.Contracts;
using LAPhil.Auth;
using System.Threading.Tasks;
using LAPhil.User;
using LAPhil.Application;
using LAPhil.Events;
using Xamarin.Auth;
using System.Json;

using CoreFoundation;
using System.Threading;
//using Xamarin.Forms;

using CoreGraphics;



//  FB Facebook Login Help : 
//  http://www.c-sharpcorner.com/article/facebook-login-using-xamarin-auth-in-xamarin-ios/

namespace LAPhil.iOS
{
    public partial class LogInPageViewController : UIViewController
    {
        NSObject observer;
        LoginService loginService = ServiceContainer.Resolve<LoginService>();

        public LogInPageViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            btnClose.TouchUpInside += CloseView;
            btnClose.TouchUpOutside += CloseView;

            btnLogin.TouchUpInside += actionLogin;
            btnFB.TouchUpInside += fbLogin_TouchUpInside;

        }


        public void SetMyAccountData(User.Account account)
        {
            
            Console.WriteLine(">>>>  myAccount Username: {0}", account.Username);
            Console.WriteLine(">>>>  myAccount Token: {0}", account.Token.TokenString);


            DispatchQueue.GetGlobalQueue(DispatchQueuePriority.Default).DispatchAsync(() =>
            {

                DispatchQueue.MainQueue.DispatchAsync(() => {
                    NSUserDefaults.StandardUserDefaults.SetString("1", "isLoginOpen");
                    //LAPhilShared.SharedClass.StopLoaderIndicator(this.loadingIndeicator);//GIF_Animation_STOP

                    //var okAlertController = UIAlertController.Create("LA Phil", "Welcome "+ account.Username , UIAlertControllerStyle.Alert);
                    ////Add Actions

                    //okAlertController.AddAction(UIAlertAction.Create("Okay", UIAlertActionStyle.Default, alert => {
                        Task.Delay(500);
                        loginService.Save(account);
                        Task.Delay(500);
                        loginService.SetCurrentAccount(account);
                        Task.Delay(500);
                        loginService.Rx.Complete(account);
                    //}));
                    LAPhilShared.SharedClass.StopLoaderIndicator(this.loadingIndeicator);//GIF_Animation_STOP

                    //UITabBarController tabBarController = (UITabBarController)this.TabBarController;
                    //tabBarController.SelectedIndex = 2;
                    //UITabBarController tabBarController = (UITabBarController)this.rootViewController;
                    //tabBarController.SelectedIndex = 2;
                    //var storyboard = UIStoryboard.FromName("Main", null);
                    //var myTabBarRootViewController = storyboard.InstantiateViewController("tabBarRootViewController") as tabBarRootViewController;
                    //myTabBarRootViewController.SelectedIndex = 2;
                    //PresentViewController(okAlertController, true, null);
                });
            });

        }

        //Action
        private void actionLogin(object sender, EventArgs e)
        {
            Console.WriteLine("Name : {0}",txtEmail.Text);
            Console.WriteLine("Password : {0}", txtPassword.Text);

            string email = txtEmail.Text;  
            if(email.Length == 0)
            {
                var okAlertController = UIAlertController.Create("LA Phil", "Please enter email", UIAlertControllerStyle.Alert);
                okAlertController.AddAction(UIAlertAction.Create("Okay", UIAlertActionStyle.Default, null));
                PresentViewController(okAlertController, true, null);
                txtEmail.BecomeFirstResponder();
                return;
            }else if (txtPassword.Text.Length == 0)
            {
                var okAlertController = UIAlertController.Create("LA Phil", "Please enter password", UIAlertControllerStyle.Alert);
                okAlertController.AddAction(UIAlertAction.Create("Okay", UIAlertActionStyle.Default, null));
                PresentViewController(okAlertController, true, null);
                txtPassword.BecomeFirstResponder();
                return;
            }else if (!Regex.Match(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").Success) {  
                var okAlertController = UIAlertController.Create("LA Phil", "Please enter correct email", UIAlertControllerStyle.Alert);
                okAlertController.AddAction(UIAlertAction.Create("Okay", UIAlertActionStyle.Default, null));
                PresentViewController(okAlertController, true, null);
                txtEmail.BecomeFirstResponder();
                return;
            }

            LAPhilShared.SharedClass.StartLoaderIndicator(this.loadingIndeicator);//GIF_Animation_START
            try
            {
                observer = NSNotificationCenter.DefaultCenter.AddObserver((NSString)"PostLoginException", PostLoginException);
                loginService.Login(txtEmail.Text, txtPassword.Text).Subscribe((User.Account obj) => SetMyAccountData(obj));
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception :: ERROR ---- {0}", exception);

                if (exception is UnknownAuthException)
                {
                    var errorMessage = exception.Message;

                    var okAlertController = UIAlertController.Create("LA Phil", errorMessage, UIAlertControllerStyle.Alert);
                    okAlertController.AddAction(UIAlertAction.Create("Okay", UIAlertActionStyle.Default, null));
                    PresentViewController(okAlertController, true, null);
                }
                else
                {
                    var okAlertController = UIAlertController.Create("LA Phil", "Wrong email or password", UIAlertControllerStyle.Alert);
                    okAlertController.AddAction(UIAlertAction.Create("Okay", UIAlertActionStyle.Default, null));
                    PresentViewController(okAlertController, true, null);
                }
                txtEmail.BecomeFirstResponder();
                LAPhilShared.SharedClass.StopLoaderIndicator(this.loadingIndeicator);//GIF_Animation_STOP
                return;
                //if (e is User.InvalidUsernameOrPassword)
                //InvalidCredentialsError(hidden: false);
                //else if (e is UnknownAuthException)
                //InvalidCredentialsError(hidden: false);
            }
        }

        void PostLoginException(NSNotification obj)
        {
            var okAlertController = UIAlertController.Create("LA Phil", "Invalid Email Or Password", UIAlertControllerStyle.Alert);
            okAlertController.AddAction(UIAlertAction.Create("Okay", UIAlertActionStyle.Default, null));
            PresentViewController(okAlertController, true, null);
            txtEmail.BecomeFirstResponder();
            LAPhilShared.SharedClass.StopLoaderIndicator(this.loadingIndeicator);//GIF_Animation_STOP

        }

        private void CloseView(object UIButton, EventArgs e)
        {
            Console.WriteLine("Pop CloseView ");
            var isLoginOpen = NSUserDefaults.StandardUserDefaults.StringForKey("isLoginOpen");
            if(isLoginOpen != "comefromMore")
            {
                NSUserDefaults.StandardUserDefaults.SetString("0", "isLoginOpen");
                NSNotificationCenter.DefaultCenter.PostNotificationName((NSString)"setIndex", null);    
            }

            loginService.LoginCancel();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            NSUserDefaults.StandardUserDefaults.SetString("0", "isLoginOpen");
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
                imgProfile.Image = UIImage.LoadFromData(NSData.FromUrl(new NSUrl(fbProfile)));
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

