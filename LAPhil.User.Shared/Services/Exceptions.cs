using System;
//#if __ANDROID__
//using Android.Content;
//using LAPhil.Application;
//#endif

//#if __HBANDROID__
//using HollywoodBowl.Droid;
//#endif

//#if __LAPHILANDROID__
//using SharedLibraryAndroid;
//#endif

using LAPhil.Auth;
#if __IOS__
using Foundation;
using UIKit;
#endif


namespace LAPhil.User
{
    // this is weird as we are effectively duplication LAPhil.Auth.InvalidUsernameOrPassword
    // but it allows us to not seemingly include a random lib `using LAPhil.Auth`
    // where we `using HollywoodBowl.Services`. There would be no at-first-sight
    // reason it's included when handling errors at the call site, we'd just 
    // suddenly be using `LAPhil.Auth.InvalidUsernameOrPassword`
    public class InvalidUsernameOrPassword : LAPhil.Auth.InvalidUsernameOrPassword
    {
        public InvalidUsernameOrPassword()


        {
//#if __ANDROID__
//            LoginActivity activity = (LoginActivity)ServiceContainer.Resolve<LAPhil.Platform.CurrentActivityService>().Activity;
//            Intent message = new Intent("InvalidUsernameOrPassword");
//            Android.Support.V4.Content.LocalBroadcastManager.GetInstance(activity.mContext).SendBroadcast(message);
//#endif


#if __IOS__
            //1-Feb
            Console.WriteLine("InvalidUsernameOrPassword.....");
            NSNotificationCenter.DefaultCenter.PostNotificationName((NSString)"PostLoginException", null);
#endif
        }
#if __IOS__
        private void PresentViewController(UIAlertController okAlertController, bool v, object p)
        {
            
        }
#endif
    }

    public class InvalidToken: LAPhil.Auth.InvalidToken
    {
        public InvalidToken()
        {
        }
    }

    public class UnknownAuthException : LAPhil.Auth.AuthException
    {
        public UnknownAuthException()
        {
        }
    }
}
