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
using Android.Content.PM;
using LAPhil.Droid;
using Android.Text.Format;

namespace SharedLibraryAndroid
{
    [Activity(Label = "LA Phil", MainLauncher = false, Icon = "@mipmap/icon", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MyTicketsActivity : Activity
    {   
        private UpComingAdapter _Adapter;
        private Context context;
        private RecyclerView upcomingRecyclerView;
        private Ticket[] ticketsList;
        private Ticket[] ticketsSkipList;
        private Button btnViewTicket, btnLearnAbout;
        private ImageView profileImg;
        private TextView lblUpcomingEvent, lblName, lblDatetime, headerTitle;
        private LinearLayout lytTickets;
        private Event[] eventList;
        private Ticket nextTicket;
        private Event nextTicketEvent;
        private ProgressBar progressBar;
        TicketsService ticketsService = ServiceContainer.Resolve<TicketsService>();
        EventService eventService = ServiceContainer.Resolve<EventService>();

		protected override void OnCreate(Bundle savedInstanceState)
		{
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MyTicketsView);
            OverridePendingTransition(Resource.Animation.No_animation, Resource.Animation.No_animation);
            context = this;
            ticketsList = new Ticket[] { };
            initView(context);

            var lytCustomBottom = (LinearLayout)FindViewById(Resource.Id.lytCustomBottom);
            var tabBarView = new TabBarView(context);
            lytCustomBottom.AddView(tabBarView);

            if (UserModel.Instance.tickets == null)
            {
                progressBar.Visibility = Android.Views.ViewStates.Visible;
                _ = LoadTickets();

            }
            else if (UserModel.Instance.tickets.Length == 0)
            {
                lytTickets.Visibility = ViewStates.Gone;
                lblUpcomingEvent.Text = "No tickets to display.";

            }
            else if (UserModel.Instance.tickets.Length > 0)
            {
                eventList = UserModel.Instance.ticketsEvent;
                GetMyTickets(UserModel.Instance.tickets);
                GetEventList(UserModel.Instance.ticketsEvent);
            }
            btnViewTicket.Click += (sender, e) =>
            {
                if (ticketsList.Length > 0)
                {
                    UserModel.Instance.selectedTickets= ticketsList[0];
                    UserModel.Instance.SelectedEvent = nextTicketEvent;
                    StartActivity(new Intent(this, typeof(TicketsDetailActivity)));
                }

            };
            profileImg.Click += (sender, e) =>
            {

                UserModel.Instance.SelectedEvent = nextTicketEvent;
                Intent intent = new Intent(this, typeof(ProgramNotesActivity));
                StartActivity(intent);
            };

            btnLearnAbout.Click += (sender, e) =>
            {

                UserModel.Instance.SelectedEvent = nextTicketEvent;
                Intent intent = new Intent(this, typeof(ProgramNotesActivity));
                StartActivity(intent);
            };
		}

        async Task LoadTickets()
        {
            var currentAccount = ServiceContainer.Resolve<LoginService>().CurrentAccount();
            //var currentAccount = loginService.CurrentAccount();


            // the MADE api currently returns ALL the tickets for both
            // HWB and LAPHIL. Our apps though only know about
            // events for thier respective purpose. AKA HWB only knows
            // about HWB events. So we need to filter out tickets
            // that might be for the <other> venue.
            var tickets = await ticketsService.GetTickets(currentAccount);
            var events = await eventService.CurrentEvents();
            var eventIds = new HashSet<string>(events.Select(x => x.TicketingSystemId));
            var allowedTickets = tickets.Where(x => eventIds.Contains(x.PerformanceId)).ToArray();
            eventList = events;
            GetMyTickets(allowedTickets);
            GetEventList(events);
        }

        private void GetMyTickets(Ticket[] ticketsData)
        {
            progressBar.Visibility = Android.Views.ViewStates.Gone;
            lytTickets.Visibility = ViewStates.Visible;
            UserModel.Instance.tickets = ticketsData;
            ticketsList = ticketsData;
            if (ticketsData != null && ticketsData.Length != 0)
            {
                lytTickets.Visibility = ViewStates.Visible;
                if (ticketsList.Length > 1)
                {

                   ticketsSkipList = ticketsList.Skip(1).ToArray();
                    _Adapter = new UpComingAdapter(ticketsSkipList, context,eventList);
                    LinearLayoutManager layoutManager = new LinearLayoutManager(context);
                    upcomingRecyclerView.SetLayoutManager(layoutManager);
                    // set RecyclerView's adapter
                    upcomingRecyclerView.SetAdapter(_Adapter);
                    _Adapter.ItemClick += OnItemClick;

                }

                lblUpcomingEvent.Text = "Upcoming Tickets";
                var timeString = ticketsList[0].Date;

                var date = timeString.DateTime.ToString($"ddd / MMM dd, yyyy / {TimeFormat}");
                lblName.Text = ticketsList[0].Name;
                lblDatetime.Text = date;
                var candidateTicket = ticketsList[0];

                if (nextTicket == null || nextTicket.PerformanceId != candidateTicket.PerformanceId)
                {
                    nextTicket = candidateTicket;
                }

            }
            else
            {
                progressBar.Visibility = Android.Views.ViewStates.Gone;
                lytTickets.Visibility = ViewStates.Gone;
                lblUpcomingEvent.Text = "No tickets to display.";
            }
        }

        private void GetEventList(Event[] obj)
        {
            eventList = obj;

            if (eventList.Length != 0)
            {
                UserModel.Instance.ticketsEvent = obj;
                nextTicketEvent = obj.Where(x => x.TicketingSystemId == nextTicket.PerformanceId).FirstOrDefault();
                try
                {
                    if (nextTicketEvent != null)
                    {
                        Glide.With(context).Load(nextTicketEvent.ImageUrl).Thumbnail(0.1f).Into(profileImg);
                    }
                }
                catch (Exception)
                { }
            }
        }



        void OnItemClick(object sender, int position)
        {
            UserModel.Instance.selectedTickets = ticketsSkipList[position];
            UserModel.Instance.SelectedEvent = nextTicketEvent;
            StartActivity(new Intent(this, typeof(TicketsDetailActivity)));
        }

        private void initView(Context mContext)
        {
            upcomingRecyclerView = FindViewById<RecyclerView>(Resource.Id.rv_upcoming);
            profileImg = FindViewById<ImageView>(Resource.Id.profile_img);
            btnViewTicket = FindViewById<Button>(Resource.Id.btnViewTicket);
            btnLearnAbout = FindViewById<Button>(Resource.Id.btnLearnAbout);
            lblUpcomingEvent = FindViewById<TextView>(Resource.Id.lbl_event);
            lblName = FindViewById<TextView>(Resource.Id.lblName);
            lblDatetime = FindViewById<TextView>(Resource.Id.lblDateTime);
            headerTitle = FindViewById<TextView>(Resource.Id.headerTitle);
            lytTickets = FindViewById<LinearLayout>(Resource.Id.lytTickets);
            progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar1);

            btnViewTicket.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Normal);
            btnLearnAbout.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Normal);
            lblUpcomingEvent.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            lblName.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            headerTitle.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            lblDatetime.SetTypeface(Utility.MediumTypeface(mContext), TypefaceStyle.Normal);
        }
 
        //protected override void OnResume()
        //{
        //    base.OnResume();
        //    Task.Delay(400);
        //    GC.Collect(0);
        //}
		public override void OnBackPressed()
		{
            base.OnBackPressed();
            FinishAffinity();
		}
        public string TimeFormat
        {
            get
            {
                if (DateFormat.Is24HourFormat(context))
                    return "H:mm tt";

                return "h:mm tt";
            }
        }
	}
}
