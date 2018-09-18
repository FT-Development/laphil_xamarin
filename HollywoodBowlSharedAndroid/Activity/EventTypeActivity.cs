using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Widget;
using LAPhil.Events;

namespace HollywoodBowl.Droid
{
    [Activity(Label = "HollywoodBowl", MainLauncher = false, Icon = "@mipmap/icon", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    class EventTypeActivity : AppCompatActivity
    {
        private EventTypeAdapter eventTypeAdapter;
        private Context mContext;
        private List<String> eventlist;
        private RecyclerView eventTypeRecyclerview;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.ActivityEventType);
            mContext = this;
            eventTypeRecyclerview = FindViewById<RecyclerView>(Resource.Id.eventTypeRecyclerview);
            LinearLayoutManager layoutManager = new LinearLayoutManager(mContext);
            eventTypeRecyclerview.SetLayoutManager(layoutManager);
            var btnBack = FindViewById<ImageView>(Resource.Id.btnBack);
            var header = FindViewById<TextView>(Resource.Id.header);
            var btnConfirm = FindViewById<TextView>(Resource.Id.btnConfirm);
            header.SetTypeface(Utility.BoldTypeface(mContext), Android.Graphics.TypefaceStyle.Bold);
            btnConfirm.SetTypeface(Utility.BoldTypeface(mContext), Android.Graphics.TypefaceStyle.Bold);

            eventlist = new List<String>();
            GetAllEventList();

            btnBack.Click += (sender, e) =>
            {
                Finish();
            };

            btnConfirm.Click += (sender, e) =>
            {
                StartActivity(new Intent(this, typeof(FilterActivity)));
                UserModel.Instance.isFromSeeConcetFilter = true;
                Finish();
            };
        }
        void OnItemClick(object sender, int position)
        {
            UserModel.Instance.FilterEventType = eventlist[position];
           // UserModel.Instance.filterEventList.Add(eventlist[position]);
            if (UserModel.Instance.filterEventList != null)
            {
                if (!UserModel.Instance.filterEventList.Contains(eventlist[position]))
                {
                    UserModel.Instance.filterEventList.Add(eventlist[position]);
                }
            }
        }

        private void GetAllEventList()
        {
            List<Event> obj = UserModel.Instance.allEvents;

            eventlist.Clear();
            for (int i = 0; i < obj.Count; i++)
            {
                if (obj[i].Program.categories != null)
                {
                    for (int k = 0; k < obj[i].Program.categories.Count; k++)
                    {
                        var eventTypeName = obj[i].Program.categories[k];

                        if (!eventlist.Contains(eventTypeName))
                        {
                            eventlist.Add(eventTypeName);
                        }
                    }
                }
            }
            eventTypeAdapter = new EventTypeAdapter(eventlist, mContext);
            eventTypeRecyclerview.SetAdapter(eventTypeAdapter);
            eventTypeAdapter.ItemClick += OnItemClick;
        }
    }
}
