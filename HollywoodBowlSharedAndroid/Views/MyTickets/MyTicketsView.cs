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


namespace HollywoodBowl.Droid
{
    public class MyTicketsView : Fragment
    {
        private UpComingAdapter _Adapter;
        private Context context;
        private RecyclerView upcomingRecyclerView;
        private Ticket[] ticketsList;
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

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.MyTicketsView, container, false);
            context = this.Activity;
            ticketsList = new Ticket[] { };
            initView(view, context);

            if (UserModel.Instance.tickets == null)
            {
                progressBar.Visibility = Android.Views.ViewStates.Visible;
                _ = LoadTickets();

            }else if(UserModel.Instance.tickets.Length==0)
            {
                lytTickets.Visibility = ViewStates.Gone;
                lblUpcomingEvent.Text = "No tickets to display.";  

            }else if(UserModel.Instance.tickets.Length>0)
            {
                GetMyTickets(UserModel.Instance.tickets);
                GetEventList(UserModel.Instance.ticketsEvent);
            }
            btnViewTicket.Click += (sender, e) =>
            {
                if (ticketsList.Length > 0)
                {
                    TicketsDetailView fragment = new TicketsDetailView();
                    Bundle args = new Bundle();
                    args.PutString("ticketName", ticketsList[0].Name);
                    fragment.Arguments = args;
                    fragment.myTickets = ticketsList[0];
                    fragment.Event = nextTicketEvent;
                    var transaction = FragmentManager.BeginTransaction();
                    transaction.Add(Resource.Id.Main_FragmentContainer, fragment);
                    transaction.AddToBackStack(null);
                    transaction.Commit();
                }

            };
            profileImg.Click += (sender, e) =>
            {
             
            AboutThePerformanceView fragment = new AboutThePerformanceView();
            fragment.Event = nextTicketEvent;
            var transaction = FragmentManager.BeginTransaction();
            transaction.Add(Resource.Id.Main_FragmentContainer, fragment);
            transaction.SetTransition(FragmentTransit.FragmentFade);
            transaction.AddToBackStack(null);
            transaction.Commit();

            };

            btnLearnAbout.Click += (sender, e) =>
            {
            AboutThePerformanceView fragment = new AboutThePerformanceView();
            fragment.Event = nextTicketEvent;
            var transaction = FragmentManager.BeginTransaction();
            transaction.Add(Resource.Id.Main_FragmentContainer, fragment);
            transaction.SetTransition(FragmentTransit.FragmentFade);
            transaction.AddToBackStack(null);
            transaction.Commit();

            };
          
            return view.RootView;

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

            GetMyTickets(allowedTickets);
            GetEventList(events);
        }

        private void GetMyTickets(Ticket[] ticketsData)
        {
            progressBar.Visibility = Android.Views.ViewStates.Gone;
            lytTickets.Visibility = ViewStates.Visible;
            UserModel.Instance.tickets = ticketsData;
            ticketsList = ticketsData;
            if (ticketsData != null&& ticketsData.Length!=0)
            {
                lytTickets.Visibility = ViewStates.Visible;
                if (ticketsList.Length > 1)
                {  
                  
                    ticketsList.Skip(1);
                    _Adapter = new UpComingAdapter(ticketsList, context,eventList);
                    LinearLayoutManager layoutManager = new LinearLayoutManager(context);
                    upcomingRecyclerView.SetLayoutManager(layoutManager);
                    // set RecyclerView's adapter
                    upcomingRecyclerView.SetAdapter(_Adapter);
                    _Adapter.ItemClick += OnItemClick;

                }
                  
                 
                    lblUpcomingEvent.Text = "Upcoming Tickets";
                    var timeString = ticketsList[0].Date;
                    // DateTime dt = Convert.ToDateTime(timeString);
                    var myDateTime = timeString.Date.ToString("yyyyMMddTHHmmssZ");
                    DateTime d = DateTime.ParseExact(myDateTime, "yyyyMMdd'T'HHmmss'Z'", null);
                    var date = d.ToString("ddd / MMM dd, yyyy / H:mm tt");
                    lblName.Text = ticketsList[0].Name;
                    lblDatetime.Text = date;
                    var candidateTicket = ticketsList[0];

                    if (nextTicket == null || nextTicket.PerformanceId != candidateTicket.PerformanceId)
                    {
                        nextTicket = candidateTicket;
                    }
                   
            }
            else
            {   progressBar.Visibility = Android.Views.ViewStates.Gone;
                lytTickets.Visibility = ViewStates.Gone;
                lblUpcomingEvent.Text = "No tickets to display.";
            }
        }

        private void GetEventList(Event[] obj)
        {
            eventList = obj;

            if (eventList.Length != 0)
            {   
                UserModel.Instance.ticketsEvent=obj;
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
            TicketsDetailView fragment = new TicketsDetailView();
            Bundle args = new Bundle();
            args.PutString("ticketName", ticketsList[position].Name);
            fragment.Arguments = args;
            fragment.Event = nextTicketEvent;
            fragment.myTickets = ticketsList[position];
            var transaction = FragmentManager.BeginTransaction();
            transaction.Add(Resource.Id.Main_FragmentContainer, fragment);
            transaction.AddToBackStack(null);
            transaction.Commit();

        }

        private void initView(View view, Context mContext)
        {
            upcomingRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.rv_upcoming);
            profileImg = view.FindViewById<ImageView>(Resource.Id.profile_img);
            btnViewTicket = view.FindViewById<Button>(Resource.Id.btnViewTicket);
            btnLearnAbout = view.FindViewById<Button>(Resource.Id.btnLearnAbout);
            lblUpcomingEvent = view.FindViewById<TextView>(Resource.Id.lbl_event);
            lblName = view.FindViewById<TextView>(Resource.Id.lblName);
            lblDatetime = view.FindViewById<TextView>(Resource.Id.lblDateTime);
            headerTitle = view.FindViewById<TextView>(Resource.Id.headerTitle);
            lytTickets = view.FindViewById<LinearLayout>(Resource.Id.lytTickets);
            progressBar = view.FindViewById<ProgressBar>(Resource.Id.progressBar1);

            btnViewTicket.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Normal);
            btnLearnAbout.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Normal);
            lblUpcomingEvent.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            lblName.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            headerTitle.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            lblDatetime.SetTypeface(Utility.MediumTypeface(mContext), TypefaceStyle.Normal);
        }
    }
}
