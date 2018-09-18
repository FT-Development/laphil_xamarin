﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using Android.Support.V7.App;
using LAPhil.Application;
using LAPhil.Auth;
using LAPhil.User;
using System.Threading.Tasks;
using LAPhil.Urls;
using Android.Support.V4.Content;
using Android.Graphics;
using System.Net.Http;
using Newtonsoft.Json;

namespace HollywoodBowl.Droid
{
    [Activity(Label = "LoginActivity", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class LoginActivity : AppCompatActivity
    {
        public Context mContext { get; set; }
        private EditText lblEmail, lblPassword;
        private ProgressBar progressBar;
        private TextView btnForgotPass;
      //  private Receiver mReceiver;
        LAPhilUrlService urlService = LAPhil.Application.ServiceContainer.Resolve<LAPhilUrlService>();
        LoginService loginService = ServiceContainer.Resolve<LoginService>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_login);
            OverridePendingTransition(Resource.Animation.Open_bottom_top, Resource.Animation.Close_bottom_top);

            mContext = this;

            // Create your application here
            var btnSignUp = FindViewById<TextView>(Resource.Id.btnSignUp);
            var btnLogin = FindViewById<TextView>(Resource.Id.btnLogin);
            var btnFbLogin = FindViewById<TextView>(Resource.Id.btnFbLogin);
            var btnCross = FindViewById<ImageView>(Resource.Id.btnCross);
            lblEmail = FindViewById<EditText>(Resource.Id.lblEmail);
            lblPassword = FindViewById<EditText>(Resource.Id.lblPassWord);
            btnForgotPass = FindViewById<TextView>(Resource.Id.btnForgotPass);
            progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar1);
            lblEmail.SetTypeface(Utility.RegularTypeface(mContext), TypefaceStyle.Normal);
            btnForgotPass.SetTypeface(Utility.RegularTypeface(mContext), TypefaceStyle.Normal);
            lblPassword.SetTypeface(Utility.RegularTypeface(mContext), TypefaceStyle.Normal);
            var lblHeaderTextView = FindViewById<TextView>(Resource.Id.headerTitle);
            lblHeaderTextView.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);

            btnSignUp.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            btnLogin.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            btnFbLogin.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);

        //    mReceiver = new Receiver(this, progressBar);
          //  LocalBroadcastManager.GetInstance(this).RegisterReceiver(mReceiver, new IntentFilter("InvalidUsernameOrPassword"));

            btnLogin.Click += (sender, e) =>
            {
              
                if (lblEmail.Text.Trim().Length == 0 || !Utility.IsValidEmail(lblEmail.Text.Trim()))
                {
                    Toast.MakeText(ApplicationContext, "Enter valid email ", ToastLength.Long).Show();
                    return;
                }
                if (lblPassword.Text.Trim().Length == 0)
                {
                    Toast.MakeText(ApplicationContext, "Enter password ", ToastLength.Long).Show();
                    return;
                }

                progressBar.Visibility = Android.Views.ViewStates.Visible;

              _= Login(lblEmail.Text, lblPassword.Text);

            };
            btnFbLogin.Click += (sender, e) =>
            {
                Utility.SetBooleanSharedPreference("isLogin", true);
                Finish();
            };
            btnSignUp.Click += (sender, e) =>
            {
                Intent intent = new Intent(this, typeof(SignupActivity));
                StartActivity(intent);
                //Finish();
            };
            btnCross.Click += (sender, e) =>
            {
                UserModel.Instance.SelectedTab = "Concerts";
                StartActivity(new Intent(mContext, typeof(ConcertActivity)));
                Finish();
                OverridePendingTransition(Resource.Animation.Open_top_bottom, Resource.Animation.Close_top_bottom);


            };
            btnForgotPass.Click += (sender, e) =>
            {
                String urlsForgotPassword = urlService.WebForgotPassword;
                Intent intent = new Intent(this, typeof(ForgotPasswordActivity));
                intent.PutExtra("url", urlsForgotPassword);
                StartActivity(intent);
            };
        }

        private async Task Login(string username, string password)
        {
            //JWT token;
            try
            {
                var data = new
                {
                    username = username,
                    password = password
                };
              //  AuthResponse response = null;
                var json = JsonConvert.SerializeObject(data);
                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                var url = "https://my.hollywoodbowl.com/api/jwt/authorization";
                var client = new HttpClient();
                var response =  client.PostAsync(new Uri(url), stringContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    var account = loginService.Login(username, password)
                                          .Subscribe((Account obj) => GetAccountDetail(obj));
               
                }
                else
                {
                    progressBar.Visibility = Android.Views.ViewStates.Gone;
                    Toast.MakeText(mContext, "Invalid username or password", ToastLength.Long).Show();
                }            
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception :: ERROR ---- {0}", e);
            }
        }

        private void GetAccountDetail(Account obj)
        {
            Utility.SetBooleanSharedPreference("isLogin", true);

            loginService.Save(obj);

            //Task.Delay(100);
            loginService.SetCurrentAccount(obj);

            //Task.Delay(200);
            loginService.Rx.Complete(obj);

            //Task.Delay(500);
            UserModel.Instance.SelectedTab = "MyTickets";
            StartActivity(new Intent(mContext, typeof(MyTicketsActivity)));
            Finish();

        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            UserModel.Instance.SelectedTab = "Concerts";
            StartActivity(new Intent(mContext, typeof(ConcertActivity)));
            OverridePendingTransition(Resource.Animation.Open_top_bottom, Resource.Animation.Close_top_bottom);

        }

    }
}