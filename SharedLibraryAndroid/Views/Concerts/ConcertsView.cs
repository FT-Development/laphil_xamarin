using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Support.V7.Widget;
using Android.Content;
using Android.Widget;
using LAPhil.Events;
using LAPhil.Application;
using LAPhil.Droid;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Linq;

namespace SharedLibraryAndroid
{
 public class ConcertsView : Fragment
    {
        private ConcertsAdapter _Adapter;
        private Context mContext;
        public EventService events;
        List<Event> eventList;
        private TextView upComing;
        private RecyclerView concertRecyclerView;

        private ProgressBar progressBar;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.ConcertsView, container, false);
            mContext = this.Activity;

            eventList = new List<Event>();
            concertRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.concertsRecyclerview);
            LinearLayoutManager layoutManager = new LinearLayoutManager(mContext);
            concertRecyclerView.SetLayoutManager(layoutManager);
            upComing = (TextView)view.FindViewById(Resource.Id.upComing);
            var seeAllText = (TextView)view.FindViewById(Resource.Id.seeAllText);
            upComing.SetTypeface(Utility.BoldTypeface(mContext),Android.Graphics.TypefaceStyle.Bold);
            seeAllText.SetTypeface(Utility.RegularTypeface(mContext), Android.Graphics.TypefaceStyle.Normal);
            var seeCalendar = view.FindViewById<LinearLayout>(Resource.Id.btnCalendar);
            progressBar = view.FindViewById<ProgressBar>(Resource.Id.progress_bar);

            seeCalendar.Click += (sender, e) =>
            {
                CalendarFragment fragment = new CalendarFragment();
                var transaction = FragmentManager.BeginTransaction();
                transaction.Add(Resource.Id.Main_FragmentContainer, fragment);
                transaction.AddToBackStack("CalendarFragment");
                transaction.Commit();

            };
            // Signature specifies Task  


            if(UserModel.Instance.events != null){
                if(UserModel.Instance.events.Count > 0){
                    progressBar.Visibility = ViewStates.Gone;
                    upComing.Text = "Upcoming Events";

                    Handler h = new Handler();
                    Action myAction = () =>
                    {
                        _ = GetLocalEventList();

                    };
                    h.PostDelayed(myAction, 10);
                }else{
                    GetAllEvents(); 
                }
            }else{
                GetAllEvents();    
            }

            return view.RootView;
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
            await Task.Delay(5);
            // The method has no return statement.  
            _Adapter = new ConcertsAdapter(UserModel.Instance.events, mContext);
            _Adapter.ItemClick += OnItemClick;
            _Adapter.ItemonClick += OnItembuyNowClick;
            concertRecyclerView.SetAdapter(_Adapter);
            //_Adapter.NotifyDataSetChanged();
        }

        private async Task GetEventList(Event[] obj)
        {
            if (await ShouldReloadData(obj) == false)
                return;

            eventList = (obj).OfType<Event>().ToList();

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
                upComing.Text = "No Upcoming Events found.";
            }
        }

        void OnItemClick(object sender, int position)
        {
            ProgramNotesView fragment = new ProgramNotesView();
            fragment.selectedEvent = UserModel.Instance.events[position];
            var transaction = FragmentManager.BeginTransaction();
            transaction.Add(Resource.Id.Main_FragmentContainer, fragment);
            transaction.AddToBackStack(null);
            transaction.Commit();
        }
        void OnItembuyNowClick(object sender, int position)
        {
            if (UserModel.Instance.events[position].BuyUrl == null)
            {
                Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(this.Activity);
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
                string url = UserModel.Instance.events[position].BuyUrl;
                string newUrl = url.Replace("https://", "https://lapatester:p@ssw0rd@");
                var uri = Android.Net.Uri.Parse(newUrl);
                var intent = new Intent(Intent.ActionView, uri);
                StartActivity(intent);
            }
        }
    }
}