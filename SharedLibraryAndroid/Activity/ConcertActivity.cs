using Android.Content.PM;
using Android.OS;
using Android.App;
using Android.Content;
using System.Collections.Generic;
using Android.Widget;
using LAPhil.Events;
using Android.Support.V7.Widget;
using Android.Views;
using LAPhil.Application;
using System.Threading.Tasks;
using System.Linq;
using System;
using LAPhil.Droid;

namespace SharedLibraryAndroid
{
    [Activity(Label = "LA Phil", MainLauncher = false, Icon = "@mipmap/icon", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ConcertActivity : Activity
    {
        private ConcertsAdapter _Adapter;
        private Context mContext;
        public EventService events;
        List<Event> eventList;
        private TextView upComing;
        private RecyclerView concertRecyclerView;
        private ProgressBar progressBar;
        private TabBarView tabBarView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ConcertsView);
            OverridePendingTransition(Resource.Animation.No_animation, Resource.Animation.No_animation);
            mContext = this;

            eventList = new List<Event>();
            concertRecyclerView = FindViewById<RecyclerView>(Resource.Id.concertsRecyclerview);
            LinearLayoutManager layoutManager = new LinearLayoutManager(mContext);
            concertRecyclerView.SetLayoutManager(layoutManager);
            upComing = (TextView)FindViewById(Resource.Id.upComing);
            var seeAllText = (TextView)FindViewById(Resource.Id.seeAllText);
            upComing.SetTypeface(Utility.BoldTypeface(mContext), Android.Graphics.TypefaceStyle.Bold);
            seeAllText.SetTypeface(Utility.RegularTypeface(mContext), Android.Graphics.TypefaceStyle.Normal);
            var seeCalendar = FindViewById<LinearLayout>(Resource.Id.btnCalendar);
            progressBar = FindViewById<ProgressBar>(Resource.Id.progress_bar);

            var lytCustomBottom = (LinearLayout)FindViewById(Resource.Id.lytCustomBottom);
            tabBarView = new TabBarView(mContext);
            lytCustomBottom.AddView(tabBarView);

            seeCalendar.Click += (sender, e) =>
            {
                StartActivity(new Intent(this, typeof(CalendarActivity)));

            };

            if (UserModel.Instance.events != null)
            {
                if (UserModel.Instance.events.Count > 0)
                {
                    progressBar.Visibility = ViewStates.Visible;
                    upComing.Text = "Upcoming Events";
                    //Handler h = new Handler();
                    //Action myAction = () =>
                    //{
                        _ = GetLocalEventList();
                      //};
                    //h.PostDelayed(myAction, 10);
                }
                else
                {
                    GetAllEvents();
                }
            }
            else
            {
                GetAllEvents();
            }


        }

        private void GetAllEvents()
        {
            var eventsService = ServiceContainer.Resolve<EventService>();

            eventsService
            .UpcomingEvents()
            .Subscribe(results =>
            {
                _ = GetEventList(results);

            });
        }

        EventComparer eventComparer = new EventComparer();
        private Task<bool> ShouldReloadData(Event[] obj)
        {
            if (eventList == null)
                return Task.FromResult(true);

            return Task.Run(() =>
            {
                var deletions = eventList.Except(obj, eventComparer).Count();
                var insertions = obj.Except(eventList, eventComparer).Count();

                if (deletions == 0 && insertions == 0)
                    return false;

                return true;
            });
        }

        private async Task GetLocalEventList()
        {
            progressBar.Visibility = ViewStates.Gone;
            // The method has no return statement. 
            await Task.Delay(300);
            _Adapter = new ConcertsAdapter(UserModel.Instance.events, mContext);
            _Adapter.ItemClick += OnItemClick;
            _Adapter.ItemonClick += OnItembuyNowClick;
            concertRecyclerView.SetAdapter(_Adapter);
            //_Adapter.NotifyDataSetChanged();
        }

        private async Task GetEventList(Event[] obj)
        {
            progressBar.Visibility = ViewStates.Gone;
            if (obj == null || obj.Length == 0)
            {
                upComing.Text = "No Upcoming Events found.";
            }
            if (await ShouldReloadData(obj) == false)
            {
              return; 
            }
               
            eventList = (obj).OfType<Event>().ToList();
            progressBar.Visibility = ViewStates.Gone;
            if (eventList.Count() > 0)
            {
                UserModel.Instance.events = eventList;
                progressBar.Visibility = ViewStates.Gone;
                _Adapter = new ConcertsAdapter(eventList, mContext);
                _Adapter.ItemClick += OnItemClick;
                _Adapter.ItemonClick += OnItembuyNowClick;
                concertRecyclerView.SetAdapter(_Adapter);
                upComing.Text = "Upcoming Events";
            }
            else
            {
                progressBar.Visibility = ViewStates.Gone;
                upComing.Text = "No Upcoming Events found.";
            }
        }

        void OnItemClick(object sender, int position)
        {

            UserModel.Instance.isFromConcert = true;
            UserModel.Instance.SelectedEvent = UserModel.Instance.events[position];

            Intent intent;
            if (UserModel.Instance.SelectedEvent.ShouldOverrideDetails()) {
                intent = new Intent(this, typeof(WebViewActivity));
                intent.PutExtra("url", UserModel.Instance.SelectedEvent.GetOverrideUrl());
                intent.PutExtra("header", UserModel.Instance.SelectedEvent.Program.Name);
            }else {
                intent = new Intent(this, typeof(ProgramNotesActivity));    
            }

            StartActivity(intent);
            OverridePendingTransition(Resource.Animation.Slide_in_right, Resource.Animation.slide_out_left);
            //Finish();
        }
        void OnItembuyNowClick(object sender, int position)
        {
            var selectedEvent = UserModel.Instance.events[position];
            if (selectedEvent.ShouldOverrideDetails()) 
            {
                Intent intent = new Intent(this, typeof(WebViewActivity));
                intent.PutExtra("url", selectedEvent.GetOverrideUrl());
                intent.PutExtra("header", selectedEvent.Program.Name);
                StartActivity(intent);
                OverridePendingTransition(Resource.Animation.Slide_in_right, Resource.Animation.slide_out_left);
            }
            else if (selectedEvent.BuyUrl == null)
            {
                Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                AlertDialog alert = dialog.Create();
                alert.SetTitle("LA Phil");
                alert.SetMessage("No tickets available");
                alert.SetButton("Okay", (c, ev) =>
                {

                });
                alert.Show();
            }
            else
            {
                string url = selectedEvent.BuyUrl;
                string newUrl = url.Replace("https://", "https://lapatester:p@ssw0rd@");
                var uri = Android.Net.Uri.Parse(newUrl);
                var intent = new Intent(Intent.ActionView, uri);
                StartActivity(intent);
            }
        }
		public override void OnBackPressed()
		{
            base.OnBackPressed();
            FinishAffinity();
		}
		//protected override void OnResume()
        //{
        //    base.OnResume();
        //    Task.Delay(400);
        //    GC.Collect(0);
        //}
	}
}