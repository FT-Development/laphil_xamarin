using System;
using Facebook.LoginKit;
using Facebook.CoreKit;

namespace LAPhilShared
{
    public class FacebookHandler
    {
        public FacebookHandler()
        {
        }

        public void Login(Action<bool, string> callback)
        {
            if (AccessToken.CurrentAccessToken == null)
            {
                getAccessToken(callback);
            }else 
            {
                getUserData(callback);
            }
        }

        private void getAccessToken(Action<bool, string> callback) 
        { 
            var loginManager = new LoginManager();
            loginManager.LoginBehavior = LoginBehavior.Native;

            string[] permissions = { "email" };

            loginManager.LogInWithReadPermissions(permissions, null, (result, error) =>
            {
                if (error != null || result == null)
                {
                    callback(false, "Can't process now. Please try again later.");
                }
                else if (result.IsCancelled)
                {
                    callback(false, "Canceled");
                }
                else
                {
                    getUserData(callback);
                }
            });

        }

        private void getUserData(Action<bool, string> callback)
        {

            var request = new GraphRequest("/me", new Foundation.NSDictionary("fields", "id,name,email"), AccessToken.CurrentAccessToken.TokenString, null, "GET");
            request.Start((connection, result, error) =>
            {
                if (error != null || result == null) 
                {
                    callback(false, "Can't process now. Please try again later.");
                    return;
                }

                var userInfo = result as Foundation.NSDictionary;
                string email = userInfo["email"].ToString();
                if (email == null || email.Length == 0)
                {
                    callback(false, "Can't process now. Please try again later.");
                    return;
                }

                callback(true, email);

            });

        }
    }
}
