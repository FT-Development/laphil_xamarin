using Foundation;
using System;
using UIKit;
using LAPhil.Application;
using LAPhil.User;


namespace LAPhil.iOS
{
    public partial class SignUpPageViewController : UIViewController
    {
        LoginService loginService = ServiceContainer.Resolve<LoginService>();

        public SignUpPageViewController (IntPtr handle) : base (handle)
        {
        }
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            btnClose.TouchUpInside += CloseView;
            btnBack.TouchUpInside += BackView;
        }

        private void BackView(object sender, EventArgs e)
        {
            Console.WriteLine("Pop BackView ");
            NavigationController.PopViewController(true);
        }
        private void CloseView(object sender, EventArgs e)
        {
            Console.WriteLine("Dismiss CloseView ");
            loginService.LoginCancel();
        }

    }
}