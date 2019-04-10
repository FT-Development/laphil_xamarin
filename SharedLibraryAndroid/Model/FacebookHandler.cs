using System;
using Org.Json;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;

namespace SharedLibraryAndroid.Model
{
    public class FacebookHandler
    {
        private ICallbackManager callbackManager;

        public FacebookHandler(Android.Content.Context context)
        {
            FacebookSdk.ApplicationId = "2343424585877717";
            FacebookSdk.SdkInitialize(context);
        }

        public void StartLogin(Android.App.Activity activity, Action<bool, string> callback)
        {
            if (AccessToken.CurrentAccessToken == null) {
                getAccessToken(activity, callback);
            }else {
                getUserData(callback);
            }
        }

        private void getAccessToken(Android.App.Activity activity, Action<bool, string> callback) 
        {

            FacebookLoginCallback loginCallback = new FacebookLoginCallback((status, message) =>
            {
                if (!status) {
                    callback(status, message);
                    return;
                }
                getUserData(callback);
            });
            callbackManager = CallbackManagerFactory.Create();

            LoginManager.Instance.RegisterCallback(callbackManager, loginCallback);

            LoginManager.Instance.LogOut();
            LoginManager.Instance.SetLoginBehavior(LoginBehavior.NativeWithFallback);

            string[] permissions = { "email" };
            LoginManager.Instance.LogInWithReadPermissions(activity, permissions);

        }

        private void getUserData(Action<bool, string> callback)
        {
            FacebookUserDataCallback userDataCallback = new FacebookUserDataCallback((status, userData) => { 

                if (!status) {
                    callback(status, "Can't process now. Please try again later.");
                    return;
                }

                string email = userData.OptString("email");
                if (email.Length  == 0) {
                    callback(false, "Can't get an email address associated with the account you used. Please switch to some other account.");
                    return;
                }

                callback(true, email);

            });

            GraphRequest.NewGraphPathRequest(AccessToken.CurrentAccessToken, "me?fields=email,name,id", userDataCallback).ExecuteAsync();

        }

        public void OnActivityResult(int requestCode, int resultCode, Android.Content.Intent data) {
            callbackManager.OnActivityResult(requestCode, resultCode, data);
        }

    }

    public class FacebookLoginCallback : Java.Lang.Object, IFacebookCallback
    {
        private Action<bool, string> Callback;

        public FacebookLoginCallback(Action<bool, string> callback)
        {
            Callback = callback;
        }

        public void OnCancel()
        {
            Callback(false, "Canceled");
        }

        public void OnError(FacebookException error)
        {
            Callback(false, "Can't process now. Please try again later.");
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            Callback(true, "Success");
        }
    }

    public class FacebookUserDataCallback : Java.Lang.Object, GraphRequest.ICallback
    {
        private Action<bool, JSONObject> Callback;

        public FacebookUserDataCallback(Action<bool, JSONObject> callback)
        {
            Callback = callback;
        }

        public void OnCompleted(JSONObject userObject, GraphResponse response)
        {
            Callback(true, userObject);
        }

        public void OnCompleted(GraphResponse response)
        {
            Callback(true, response.JSONObject);
        }
    }
}
