using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Support.V7.Widget;
using Android.Content;
using Android.Widget;
using System.Net.Http;
using LAPhil.Events;
using Newtonsoft.Json;
using System.Linq;
using LAPhil.Application;
using System.Threading.Tasks;

namespace HollywoodBowl.Droid
{ 
    class CalendarFragment : Fragment
    {
        private CalendarAdapter _Adapter;
        private Context mContext;
        private RecyclerView concertRecyclerView;
        private ProgressBar progressBar;
        private List<Event> eventList;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.CalendarView, container, false);
            mContext = this.Activity;
            eventList = new List<Event>();
            var currentDate = DateTime.Now;
            concertRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerCalendar);
            ImageView btnBack = view.FindViewById<ImageView>(Resource.Id.backbtn_id);
            var header = view.FindViewById<TextView>(Resource.Id.header);
           
            header.SetTypeface(Utility.BoldTypeface(mContext), Android.Graphics.TypefaceStyle.Bold);
            progressBar = view.FindViewById<ProgressBar>(Resource.Id.progress_bar);
           
            // instantiate the layout manager
            LinearLayoutManager layoutManager = new LinearLayoutManager(mContext);
            concertRecyclerView.SetLayoutManager(layoutManager);

            var filterBtn = view.FindViewById<LinearLayout>(Resource.Id.btn_filter);
            filterBtn.Click += (sender, e) =>
            {
                var activity = new Intent(mContext, typeof(FilterActivity));
                StartActivity(activity);
            };
            btnBack.Click += (sender, e) =>
            {
                if (FragmentManager.BackStackEntryCount > 0)
                    FragmentManager.PopBackStack();
            };


            if (UserModel.Instance.allEvents != null)
            {
                if (UserModel.Instance.allEvents.Count > 0)
                {
                    progressBar.Visibility = ViewStates.Gone;
                    Handler h = new Handler();
                    Action myAction = () =>
                    {
                        _ = GetLocalEventList();

                    };
                    h.PostDelayed(myAction, 10);
                }
                else
                {
                    FetchAllEvents();
                }
            }
            else
            {
                FetchAllEvents();
            }
            return view.RootView;
        }


        private void FetchAllEvents()
        {
            var eventsService = ServiceContainer.Resolve<EventService>();
            eventsService
                .CurrentEvents()
                .Subscribe(results =>
                {
                    _ = GetEventList(results);

                });
        }


        private async Task GetLocalEventList()
        {
            await Task.Delay(5);
            // The method has no return statement.  
            _Adapter = new CalendarAdapter(UserModel.Instance.allEvents, mContext);
            _Adapter.ItemClick += OnItemClick;
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
            if (await ShouldReloadData(obj) == false)
                return;

            eventList = (obj).OfType<Event>().ToList();

            if (eventList.Count() > 0)
            {
                UserModel.Instance.allEvents = eventList;
                progressBar.Visibility = ViewStates.Gone;
                _Adapter = new CalendarAdapter(eventList, mContext);
                _Adapter.ItemClick += OnItemClick;
                concertRecyclerView.SetAdapter(_Adapter);
            }
            else
            {
            }
        }

        void OnItemClick(object sender, int position)
        {
            AboutThePerformanceView fragment = new AboutThePerformanceView();
            fragment.Event = UserModel.Instance.allEvents[position];
            var transaction = FragmentManager.BeginTransaction();
            transaction.Add(Resource.Id.Main_FragmentContainer, fragment);
            transaction.SetTransition(FragmentTransit.FragmentFade);
            transaction.AddToBackStack(null);
            transaction.Commit();
        }
    }
}
