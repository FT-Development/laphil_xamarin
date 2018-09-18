using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using Android.App;
using Android.Content;
using Android.Support.Design.Widget;
using Android.Graphics;
using System.Collections.Generic;
using Android.Graphics.Drawables;
using Android.Widget;
using LAPhil.Events;
using Android.Support.V7.Widget;
using Android.Views;
using LAPhil.Application;
using System.Threading.Tasks;
using System.Linq;
using System;
using LAPhil.Droid;
using System.Collections;

namespace SharedLibraryAndroid
{
    [Activity(Label = "LA Phil", MainLauncher = false, Icon = "@mipmap/icon", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class CalendarActivity : Activity
    {
        private CalendarAdapter _Adapter;
        private Context mContext;
        private RecyclerView concertRecyclerView;
        private ProgressBar progressBar;
        private List<Event> eventList;
        private TabBarView tabBarView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CalendarView);
            mContext = this;
            eventList = new List<Event>();
            var currentDate = DateTime.Now;
            concertRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerCalendar);
            ImageView btnBack = FindViewById<ImageView>(Resource.Id.backbtn_id);
            var header = FindViewById<TextView>(Resource.Id.header);
            var filterText = FindViewById<TextView>(Resource.Id.filterText);
            filterText.SetTypeface(Utility.RegularTypeface(mContext), Android.Graphics.TypefaceStyle.Normal);
            header.SetTypeface(Utility.BoldTypeface(mContext), Android.Graphics.TypefaceStyle.Bold);
            progressBar = FindViewById<ProgressBar>(Resource.Id.progress_bar);
            // instantiate the layout manager


            UserModel.Instance.SelectedTab = "Concerts";
            var lytCustomBottom = (LinearLayout)FindViewById(Resource.Id.lytCustomBottom);
            tabBarView = new TabBarView(mContext);
            lytCustomBottom.AddView(tabBarView);

            var filterBtn = FindViewById<LinearLayout>(Resource.Id.btn_filter);
            filterBtn.Click += (sender, e) =>
            {
                UserModel.Instance.FilterEventType = "";
                UserModel.Instance.FilterStartDate = "";
                UserModel.Instance.FilterEndDate = "";
                var activity = new Intent(mContext, typeof(FilterActivity));
                StartActivity(activity);
            };
            btnBack.Click += (sender, e) =>
            {
                Finish();
                OverridePendingTransition(Resource.Animation.Slide_in_left, Resource.Animation.Slide_out_right);
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

        }
        private void FetchAllEvents()
        {
            var eventsService = ServiceContainer.Resolve<EventService>();
            eventsService
                .CurrentEvents()
                .Subscribe(results =>
                {
                    GetAllEvent(results);
                });
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            UserModel.Instance.FilterEventType = "";
            Finish();
            OverridePendingTransition(Resource.Animation.Slide_in_left, Resource.Animation.Slide_out_right);

        }
        private async Task GetLocalEventList()
        {
            await Task.Delay(5);
            eventList = UserModel.Instance.allEvents;
            // The method has no return statement.  
            RecyclerSectionItemDecoration sectionItemDecoration = new RecyclerSectionItemDecoration(0, true, eventList,mContext);
            concertRecyclerView.AddItemDecoration(sectionItemDecoration);
            _Adapter = new CalendarAdapter(eventList, mContext);
            LinearLayoutManager layoutManager = new LinearLayoutManager(mContext);
            concertRecyclerView.SetLayoutManager(layoutManager);
            _Adapter.ItemClick += OnItemClick;
            concertRecyclerView.SetAdapter(_Adapter);
            progressBar.Visibility = ViewStates.Gone;
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

        private void GetAllEvent(Event[] obj)
        {
            UserModel.Instance.allEvents = (obj).OfType<Event>().ToList();
            _ = GetEventList(obj);
        }

        private async Task GetEventList(Event[] obj)
        {
            if (await ShouldReloadData(obj) == false)
                return;

            eventList = (obj).OfType<Event>().ToList();

            if (eventList.Count() > 0)
            {
                concertRecyclerView.Visibility = ViewStates.Visible;
                progressBar.Visibility = ViewStates.Gone;
                RecyclerSectionItemDecoration sectionItemDecoration = new RecyclerSectionItemDecoration(100, true, eventList,mContext);
                concertRecyclerView.AddItemDecoration(sectionItemDecoration);

                _Adapter = new CalendarAdapter(eventList, mContext);
                LinearLayoutManager layoutManager = new LinearLayoutManager(mContext);
                concertRecyclerView.SetLayoutManager(layoutManager);
                _Adapter.ItemClick += OnItemClick;
                concertRecyclerView.SetAdapter(_Adapter);

            }
            else
            {
                concertRecyclerView.Visibility = ViewStates.Gone;
               // Toast.MakeText(mContext, "There are no events available\nPlease select another date range", ToastLength.Long).Show();
                ShowAlertNoEvents();
            }
        }

        void OnItemClick(object sender, int position)
        {
            UserModel.Instance.SelectedEvent = eventList[position];

            Intent intent;
            if (UserModel.Instance.SelectedEvent.ShouldOverrideDetails())
            {
                UserModel.Instance.isFromConcert = true;
                intent = new Intent(this, typeof(WebViewActivity));
                intent.PutExtra("url", UserModel.Instance.SelectedEvent.GetOverrideUrl());
                intent.PutExtra("header", UserModel.Instance.SelectedEvent.Program.Name);
            }
            else
            {                
                UserModel.Instance.isFromConcert = false;
                intent = new Intent(this, typeof(ProgramNotesActivity));
            }

            StartActivity(intent);

        }
        public void FilterData()
        {
            if((UserModel.Instance.FilterEventType != null && UserModel.Instance.FilterEventType != "")
               && (UserModel.Instance.FilterStartDate != "" && UserModel.Instance.FilterEndDate != ""))
            {
                List<Event> objAllEvent = UserModel.Instance.allEvents;
                ArrayList list = new ArrayList();
                for (int i = 0; i < objAllEvent.Count; i++)
                {
                    if (objAllEvent[i].Program.categories != null&&UserModel.Instance.filterEventList!=null)
                    {
                        for (int j = 0; j < UserModel.Instance.filterEventList.Count; j++) { 
                            if (objAllEvent[i].Program.categories.Contains(UserModel.Instance.filterEventList[j]))
                        {
                            if ((objAllEvent[i].StartTime.Date.CompareTo(Convert.ToDateTime(UserModel.Instance.FilterStartDate)) >= 0) &&
                            (objAllEvent[i].StartTime.Date.CompareTo(Convert.ToDateTime(UserModel.Instance.FilterEndDate)) < 1))
                            {
                                Event tmpObj = objAllEvent[i];
                                list.Add(tmpObj);
                            }
                        }
                    }
                    }
                }
                Event[] array = list.ToArray(typeof(Event)) as Event[];
                _ = GetEventList(array);
            }
            else if (UserModel.Instance.FilterEventType != null && UserModel.Instance.FilterEventType != "")
            {
                List<Event> objAllEvent = UserModel.Instance.allEvents;
              // Console.WriteLine("Obj Count : " + objAllEvent.Count);

                ArrayList list = new ArrayList();
                for (int i = 0; i < objAllEvent.Count; i++)
                {
                    if (objAllEvent[i].Program.categories != null&& UserModel.Instance.filterEventList != null)
                    {
                        for (int j = 0; j < UserModel.Instance.filterEventList.Count; j++)
                        {
                            if (objAllEvent[i].Program.categories.Contains(UserModel.Instance.filterEventList[j]))
                            {
                                Event tmpObj = objAllEvent[i];
                                list.Add(tmpObj);
                            }
                        }
                    }
                }
                Event[] array = list.ToArray(typeof(Event)) as Event[];
                _ = GetEventList(array);
            }
            else if (UserModel.Instance.FilterStartDate != null && UserModel.Instance.FilterStartDate != "")
            {
                List<Event> objAllEvent = UserModel.Instance.allEvents;
                ArrayList list = new ArrayList();
                for (int i = 0; i < objAllEvent.Count; i++){
                    if ((objAllEvent[i].StartTime.Date.CompareTo(Convert.ToDateTime(UserModel.Instance.FilterStartDate)) >= 0) &&
                        (objAllEvent[i].StartTime.Date.CompareTo(Convert.ToDateTime(UserModel.Instance.FilterEndDate)) < 1))
                    {
                        Event tmpObj = objAllEvent[i];
                        list.Add(tmpObj);
                    }   
                }

                Event[] array = list.ToArray(typeof(Event)) as Event[];
                _ = GetEventList(array);
            }
            else
            {
                List<Event> listEvetOld = UserModel.Instance.allEvents;
                Event[] oldList = listEvetOld.ToArray() as Event[];
                _ = GetEventList(oldList);

            }
        }
        protected override void OnResume()
        {
            base.OnResume();
            if (UserModel.Instance.isFromSeeConcetFilter)
            {
                UserModel.Instance.isFromSeeConcetFilter = false;
                UserModel.Instance.isFilterByDate = false;
                FilterData();
            }
        
        }
        private void ShowAlertNoEvents()
        {
            Dialog dialog = new Dialog(mContext);
            dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
            dialog.SetContentView(Resource.Layout.NoEventsView);
            dialog.Window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
            dialog.SetCancelable(true);

            TextView welcomePopupDescText = (TextView)dialog.FindViewById(Resource.Id.noEventsPopupDescText);
            TextView welcomePopupHeaderText = (TextView)dialog.FindViewById(Resource.Id.noEventsPopupHeaderText);
            welcomePopupDescText.SetTypeface(Utility.RegularTypeface(mContext), TypefaceStyle.Normal);
            welcomePopupHeaderText.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            TextView btnOkay = (TextView)dialog.FindViewById(Resource.Id.btnOkay);
            btnOkay.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            dialog.Show();
            btnOkay.Click += delegate {
                
                dialog.Dismiss();

                List<Event> listEvetOld = UserModel.Instance.allEvents;
                Event[] oldList = listEvetOld.ToArray() as Event[];
                _ = GetEventList(oldList);

            };
        }
		//protected override void OnDestroy()
		//{
  //          base.OnDestroy();
  //          GC.Collect(0);
		//}
	}
}
