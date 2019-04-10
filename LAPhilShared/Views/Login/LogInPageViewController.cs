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

        private NSObject observer;
        private LoginService loginService = ServiceContainer.Resolve<LoginService>();

        public LogInPageViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            btnClose.TouchUpInside += ActClose;
            btnClose.TouchUpOutside += ActClose;

            btnLogin.TouchUpInside += ActLogin;
            btnFB.TouchUpInside += ActFbLogin;

        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            NSUserDefaults.StandardUserDefaults.SetString("0", "isLoginOpen");
        }

        private string GetAppName() 
        {
            return (NSString)NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleName");
        }

        private void ShowAlert(string title, string message) 
        {
            var okAlertController = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
            okAlertController.AddAction(UIAlertAction.Create("Okay", UIAlertActionStyle.Default, null));
            PresentViewController(okAlertController, true, null);
        }

        //Actions
        private void ActClose(object UIButton, EventArgs e)
        {
            var isLoginOpen = NSUserDefaults.StandardUserDefaults.StringForKey("isLoginOpen");
            if (isLoginOpen != "comefromMore")
            {
                NSUserDefaults.StandardUserDefaults.SetString("0", "isLoginOpen");
                NSNotificationCenter.DefaultCenter.PostNotificationName((NSString)"setIndex", null);
            }
            loginService.LoginCancel();
        }

        private void ActLogin(object sender, EventArgs e)
        {

            string email = txtEmail.Text;
            if (email.Length == 0)
            {
                ShowAlert(GetAppName(), "Please enter email");
                txtEmail.BecomeFirstResponder();
                return;
            }

            if (txtPassword.Text.Length == 0)
            {
                ShowAlert(GetAppName(), "Please enter password");
                txtPassword.BecomeFirstResponder();
                return;
            }

            if (!Regex.Match(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").Success) {
                ShowAlert(GetAppName(), "Please enter correct email");
                txtEmail.BecomeFirstResponder();
                return;
            }

            LAPhilShared.SharedClass.StartLoaderIndicator(this.loadingIndeicator);//GIF_Animation_START
            Login(txtEmail.Text, txtPassword.Text, null);

        }

        private void ActFbLogin(object sender, EventArgs e)
        {
            LAPhilShared.SharedClass.StartLoaderIndicator(this.loadingIndeicator);//GIF_Animation_START

            FacebookHandler facebookHandler = new FacebookHandler();
            facebookHandler.Login((status, data) =>
            {
                if (!status) 
                {
                    if (!data.Equals("Canceled"))
                    {
                        ShowAlert(GetAppName(), data);
                    }
                    return;
                }

                Console.WriteLine("Fb Email: " + data);
                Login(null, null, data);
            });

        }

        //Webservices
        private void Login(string userName, string password, string facebookEmail) 
        {
            try
            {
                NSNotificationCenter.DefaultCenter.AddObserver((NSString)"PostLoginException", (obj) => 
                {
                    SharedClass.StopLoaderIndicator(this.loadingIndeicator);
                    txtEmail.BecomeFirstResponder();
                    ShowAlert(GetAppName(), "Invalid Email Or Password");
                });
                loginService.Login(userName, password, facebookEmail)
                            .Subscribe((User.Account obj) => SetMyAccountData(obj));
            }
            catch (Exception exception)
            {
                SharedClass.StopLoaderIndicator(this.loadingIndeicator);
                txtEmail.BecomeFirstResponder();
                Console.WriteLine("Exception :: ERROR ---- {0}", exception);

                if (exception is UnknownAuthException)
                {
                    ShowAlert(GetAppName(), exception.Message);
                }
                else
                {
                    ShowAlert(GetAppName(), "Invalid Email Or Password");
                }
            }
        }

        public void SetMyAccountData(User.Account account)
        {
            
            Console.WriteLine(">>>>  myAccount Username: {0}", account.Username);
            Console.WriteLine(">>>>  myAccount Token: {0}", account.Token.TokenString);

            DispatchQueue.GetGlobalQueue(DispatchQueuePriority.Default).DispatchAsync(() =>
            {

                DispatchQueue.MainQueue.DispatchAsync(() => {
                    NSUserDefaults.StandardUserDefaults.SetString("1", "isLoginOpen");
                        
                    Task.Delay(500);
                    loginService.Save(account);
                    Task.Delay(500);
                    loginService.SetCurrentAccount(account);
                    Task.Delay(500);
                    loginService.Rx.Complete(account);

                    LAPhilShared.SharedClass.StopLoaderIndicator(this.loadingIndeicator);//GIF_Animation_STOP

                });
            });

        }

    }
}

