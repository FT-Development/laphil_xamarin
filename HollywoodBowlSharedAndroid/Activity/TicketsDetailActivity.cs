using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Support.V7.Widget;
using Android.Content;
using Android.Widget;
using LAPhil.Events;
using Android.Graphics;
using LAPhil.Application;
using System.Linq;
using LAPhil.User;
using Com.Bumptech.Glide;
using System.Threading.Tasks;
using System.Reactive.Linq;
using Android.Support.V4.View;
using Android.Content.PM;

namespace HollywoodBowl.Droid
{
      [Activity(Label = "HollywoodBowl", MainLauncher = false, Icon = "@mipmap/icon", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
        public class TicketsDetailActivity : Activity
        {

        private TicketsBarCodePagerAdapter _Adapter;
        private Context mContext;
        private ViewPager viewPager;
        public Ticket myTickets;
        private TabBarView tabBarView;
        public Event Event { get; set; }
        public String TicketsName { get; set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.TicketDetailView);
            OverridePendingTransition(Resource.Animation.No_animation, Resource.Animation.No_animation);
            mContext = this;
            myTickets= UserModel.Instance.selectedTickets;
            Event = UserModel.Instance.SelectedEvent;
            TicketsName=  myTickets.Name;
            viewPager = FindViewById<ViewPager>(Resource.Id.viewPagerBarCode);
            var btnLearnAbout = FindViewById<TextView>(Resource.Id.btnLearnAbout);
            ImageView btnBackDetail = FindViewById<ImageView>(Resource.Id.btnBackDetail);
            var visitWebSite = FindViewById<TextView>(Resource.Id.visitWebSite);
            var lblLearnMore = FindViewById<TextView>(Resource.Id.lblLearnMore);
            var ticketsMasterText = FindViewById<TextView>(Resource.Id.ticketsMasterText);
            var header = FindViewById<TextView>(Resource.Id.header);


            var lytCustomBottom = (LinearLayout)FindViewById(Resource.Id.lytCustomBottom);
            tabBarView = new TabBarView(mContext);
            lytCustomBottom.AddView(tabBarView);

            btnLearnAbout.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);

            header.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            visitWebSite.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            lblLearnMore.SetTypeface(Utility.RegularTypeface(mContext), TypefaceStyle.Normal);
            ticketsMasterText.SetTypeface(Utility.RegularTypeface(mContext), TypefaceStyle.Normal);
            btnBackDetail.Click += (sender, e) =>
            {
                Finish();
            };

            visitWebSite.Click += (sender, e) =>
            {
                string newUrl = "https://my.hollywoodbowl.com";
                var uri = Android.Net.Uri.Parse(newUrl);
                var intent = new Intent(Intent.ActionView, uri);
                StartActivity(intent);
            };
            btnLearnAbout.Click += delegate
            {
                UserModel.Instance.SelectedEvent = Event;
                StartActivity(new Intent(this, typeof(ProgramNotesActivity)));

            };
            GetTicketDetails();;
        }
        private void GetTicketDetails()
        {
            var loginService = ServiceContainer.Resolve<LoginService>();
            var ticketsService = ServiceContainer.Resolve<TicketsService>();

            ticketsService.GetTicketDetail(loginService.CurrentAccount(), myTickets)
                          .Subscribe((TicketDetail[] obj) => GetMyTicketDetail(obj));
        }

        private void GetMyTicketDetail(TicketDetail[] obj)
        {
           RunOnUiThread(() => {
                _Adapter = new TicketsBarCodePagerAdapter(mContext, obj,TicketsName);
                viewPager.Adapter = _Adapter;
            });
        }
    }
}
