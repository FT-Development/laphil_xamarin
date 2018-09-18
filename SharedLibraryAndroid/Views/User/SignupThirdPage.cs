using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using Android.OS;
using Android.Views;
using LAPhil.Droid;
using Android.Content;
using Android.Widget;

namespace SharedLibraryAndroid
{
    class SignupThirdPage : Fragment
    {
        private Context mContext;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.signup_view3, container, false);
            mContext = this.Activity;
            var btnSignUp = view.FindViewById<Button>(Resource.Id.btnSignUp);
            btnSignUp.Click += (sender, e) =>
            {
                Utility.SetBooleanSharedPreference("isLogin", true);
                Activity.Finish();
            };
            return view.RootView;
        }
        public override void OnDestroy()
        { 
            base.OnDestroy();
         
        }
    }
}