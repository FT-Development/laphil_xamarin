using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Content;
using Android.Widget;
using Android.Support.V4.View;
using LAPhil.User;
using LAPhil.Application;
using Android.Graphics;
using LAPhil.Events;
namespace HollywoodBowl.Droid
{
    public class TicketsDetailView : Fragment
    {
        private TicketsBarCodePagerAdapter _Adapter;
        private Context mContext;
        private ViewPager viewPager;
        public Ticket myTickets;
        public Event Event { get; set; }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.TicketDetailView, container, false);
            mContext = this.Activity;
            viewPager = view.FindViewById<ViewPager>(Resource.Id.viewPagerBarCode);
            var btnLearnAbout = view.FindViewById<Button>(Resource.Id.btnLearnAbout);
            ImageView btnBackDetail = view.FindViewById<ImageView>(Resource.Id.btnBackDetail);
            var visitWebSite = view.FindViewById<TextView>(Resource.Id.visitWebSite);
            var lblLearnMore = view.FindViewById<TextView>(Resource.Id.lblLearnMore);

            visitWebSite.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            lblLearnMore.SetTypeface(Utility.RegularTypeface(mContext), TypefaceStyle.Normal);
            btnBackDetail.Click += (sender, e) =>
            {
                FragmentManager.PopBackStack();
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
                //ProgramNotesView fragment = new ProgramNotesView();
                //fragment.selectedEvent = Event;
                //var transaction = FragmentManager.BeginTransaction();
                //transaction.Replace(Resource.Id.Main_FragmentContainer, fragment);
                //transaction.AddToBackStack(null);
                //transaction.Commit();
            AboutThePerformanceView fragment = new AboutThePerformanceView();
            fragment.Event = Event;
            var transaction = FragmentManager.BeginTransaction();
            transaction.Add(Resource.Id.Main_FragmentContainer, fragment);
            transaction.SetTransition(FragmentTransit.FragmentFade);
            transaction.AddToBackStack(null);
            transaction.Commit();

            };
            GetTicketDetails();
            return view.RootView;
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
            Activity.RunOnUiThread(() => {
                _Adapter = new TicketsBarCodePagerAdapter(mContext, obj, Arguments.GetString("ticketName"));
            viewPager.Adapter = _Adapter;
          });
        }
    }
}
