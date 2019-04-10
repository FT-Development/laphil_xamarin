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
using Android;
using Android.Support.V4.Content;
using Android.Support.V4.App;
using Android.Runtime;
using Com.Gimbal.Android;
using Android.Locations;

namespace HollywoodBowl.Droid
{
    [Activity(Label = "HollywoodBowl", MainLauncher = false, Icon = "@mipmap/icon", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
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

        private int REQUEST_LOCATION = 991;

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
            var lytCustomBottom = (LinearLayout)FindViewById(Resource.Id.lytCustomBottom);
            tabBarView = new TabBarView(mContext);
            lytCustomBottom.AddView(tabBarView);
            upComing.SetTypeface(Utility.BoldTypeface(mContext), Android.Graphics.TypefaceStyle.Bold);
            seeAllText.SetTypeface(Utility.RegularTypeface(mContext), Android.Graphics.TypefaceStyle.Normal);
            var seeCalendar = FindViewById<LinearLayout>(Resource.Id.btnCalendar);
            progressBar = FindViewById<ProgressBar>(Resource.Id.progress_bar);

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
                    Handler h = new Handler();
                    Action myAction = () =>
                    {
                        _ = GetLocalEventList();
                    };
                    h.PostDelayed(myAction, 10);
                }
                else
                {
                    progressBar.Visibility = ViewStates.Gone;
                    GetAllEvents();
                }
            }
            else
            {
                GetAllEvents();
            }

            CheckLocationPermissions();

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
        private async Task GetLocalEventList()
        {
          
            // The method has no return statement.
            await Task.Delay(300);
            progressBar.Visibility = ViewStates.Gone;
            _Adapter = new ConcertsAdapter(UserModel.Instance.events, mContext);
            _Adapter.ItemClick += OnItemClick;
            _Adapter.ItemonClick += OnItembuyNowClick;
            concertRecyclerView.SetAdapter(_Adapter);
            //_Adapter.NotifyDataSetChanged();
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

        private async Task GetEventList(Event[] obj)
        {
           
            upComing.Text = "Upcoming Events";
            progressBar.Visibility = ViewStates.Gone;

            if (obj==null || obj.Length == 0)
            {
               upComing.Text = "No Upcoming Events found.";
            }
            if (await ShouldReloadData(obj) == false)
            {
               return;
            }
              
            eventList = (obj).OfType<Event>().ToList();

            if (eventList.Count() > 0)
            {
                UserModel.Instance.events = eventList;
                _Adapter = new ConcertsAdapter(eventList, mContext);
                _Adapter.ItemClick += OnItemClick;
                _Adapter.ItemonClick += OnItembuyNowClick;
                concertRecyclerView.SetAdapter(_Adapter);
                upComing.Text = "Upcoming Events";
            }
            else
            {
                upComing.Text = "No Upcoming Events found.";
            }
        }

        void OnItemClick(object sender, int position)
        {
            UserModel.Instance.isFromConcert = true;
            UserModel.Instance.SelectedEvent = UserModel.Instance.events[position];

            Intent intent;
            if (UserModel.Instance.SelectedEvent.ShouldOverrideDetails())
            {
                intent = new Intent(this, typeof(WebViewActivity));
                intent.PutExtra("url", UserModel.Instance.SelectedEvent.GetOverrideUrl());
                intent.PutExtra("header", UserModel.Instance.SelectedEvent.Program.Name);
            }
            else
            {
                intent = new Intent(this, typeof(ProgramNotesActivity));
            }

            StartActivity(intent);
            OverridePendingTransition(Resource.Animation.Slide_in_right, Resource.Animation.slide_out_left);

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

                var analytics = Firebase.Analytics.FirebaseAnalytics.GetInstance(this);
                Bundle bundle = new Bundle();
                bundle.PutString("Program", selectedEvent.Program.Name);
                analytics.LogEvent("BuyNow", bundle);
            }
            if (selectedEvent.BuyUrl == null)
            {
                Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this);
                Android.App.AlertDialog alert = dialog.Create();
                alert.SetTitle("HollywoodBowl");
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

                var analytics = Firebase.Analytics.FirebaseAnalytics.GetInstance(this);
                Bundle bundle = new Bundle();
                bundle.PutString("Program", UserModel.Instance.events[position].Program.Name);
                analytics.LogEvent("BuyNow", bundle);
            }

        }
       
		public override void OnBackPressed()
		{
            base.OnBackPressed();
            FinishAffinity();
		}

        private void CheckLocationPermissions()
        {
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) == (int)Permission.Granted)
            {
                // We have permission, go ahead and use the location.
                //Com.Gimbal.Android.PlaceManager.Instance.StartMonitoring();
                Gimbal.Start();
            }
            else
            {
                ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.AccessFineLocation }, REQUEST_LOCATION);
            }

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            if (requestCode == REQUEST_LOCATION)
            {
                // Check if the only required permission has been granted
                if ((grantResults.Length == 1) && (grantResults[0] == Permission.Granted))
                {
                    // Location permission has been granted, okay to retrieve the location of the device.
                    //Com.Gimbal.Android.PlaceManager.Instance.StartMonitoring();
                    Gimbal.Start();
                }
            }
            else
            {
                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }
        }

    }

}