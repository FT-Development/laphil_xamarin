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
  public  class SignupSecondPage : Fragment
    {
        private Context mContext;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.signup_view2, container, false);
            mContext = this.Activity;

            var btnContinueEmail = view.FindViewById<Button>(Resource.Id.btnContinueEmail);

            btnContinueEmail.Click += (sender, e) =>
            {
                SignupThirdPage fragment = new SignupThirdPage();
                var transaction = FragmentManager.BeginTransaction();
                transaction.Replace(Resource.Id.SignUp_FragmentContainer, fragment);
                transaction.AddToBackStack(null);
                transaction.Commit();
            };
            return view.RootView;

        }
    }
}

