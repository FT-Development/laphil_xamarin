using System;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Android.App;
using Android.Content;
using Android.Content.PM;

namespace HollywoodBowl.Droid
{
    [Activity(Label = "HollywoodBowl", MainLauncher = false, Icon = "@mipmap/icon", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class FilterActivity : AppCompatActivity
    {
        private TextView btnChooseEvent, btnChooseDate, textEventName, textSelectDate, btnSeeConcerts;
        private ImageView btnCross;
        private Context mContext;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Activity_Filter);
            mContext = this;
            InitView();
            btnChooseEvent.Click += (sender, e) =>
            {
                var activity = new Intent(this, typeof(EventTypeActivity));
                StartActivity(activity);
                Finish();

            };
            btnChooseDate.Click += (sender, e) =>
            {
                var activity = new Intent(this, typeof(CalendarDateFilterActivity));
                StartActivity(activity);
                Finish();

            };
            btnCross.Click += (sender, e) =>
            {
                Finish();
            };
            btnSeeConcerts.Click += (sender, e) =>
            {
                UserModel.Instance.isFromSeeConcetFilter = true;
                UserModel.Instance.isFilterByDate = false;
                Finish();

            };
            textEventName.Click += (sender, e) =>
            {
                UserModel.Instance.FilterEventType = "";
                textEventName.Visibility = Android.Views.ViewStates.Gone;
                btnChooseEvent.Visibility = Android.Views.ViewStates.Visible;
                if (UserModel.Instance.FilterStartDate == "" && UserModel.Instance.FilterEndDate == "")
                {
                    btnSeeConcerts.Visibility = Android.Views.ViewStates.Gone;
                }

            };
            textSelectDate.Click += (sender, e) =>
            {
                UserModel.Instance.isFilterByDate = false;
                UserModel.Instance.FilterStartDate = "";
                UserModel.Instance.FilterEndDate = "";
                textSelectDate.Visibility = Android.Views.ViewStates.Gone;
                btnChooseDate.Visibility = Android.Views.ViewStates.Visible;

                if (UserModel.Instance.FilterEventType == "")
                {
                    btnSeeConcerts.Visibility = Android.Views.ViewStates.Gone;
                }
            };
        }
        private void InitView()
        {
            btnCross = FindViewById<ImageView>(Resource.Id.btnCross);
            btnChooseEvent = FindViewById<TextView>(Resource.Id.lblChooseEvent);
            btnChooseDate = FindViewById<TextView>(Resource.Id.lblChooseDate);
            textEventName = FindViewById<TextView>(Resource.Id.lblChooseEventName);
            textSelectDate = FindViewById<TextView>(Resource.Id.lblSelectDate);
            btnSeeConcerts = FindViewById<TextView>(Resource.Id.btnSeeConcerts);
            var header = FindViewById<TextView>(Resource.Id.header);
            header.SetTypeface(Utility.BoldTypeface(mContext), Android.Graphics.TypefaceStyle.Bold);

            btnSeeConcerts.SetTypeface(Utility.BoldTypeface(mContext), Android.Graphics.TypefaceStyle.Bold);
            textSelectDate.SetTypeface(Utility.BoldTypeface(mContext), Android.Graphics.TypefaceStyle.Bold);
            btnChooseDate.SetTypeface(Utility.BoldTypeface(mContext), Android.Graphics.TypefaceStyle.Bold);
            textEventName.SetTypeface(Utility.BoldTypeface(mContext), Android.Graphics.TypefaceStyle.Bold);
            btnChooseEvent.SetTypeface(Utility.BoldTypeface(mContext), Android.Graphics.TypefaceStyle.Bold);

        }
        protected override void OnResume()
        {
            base.OnResume();
            if ((UserModel.Instance.FilterEventType != null && UserModel.Instance.FilterEventType != "") &&
               (UserModel.Instance.isFilterByDate))
            {
                var filterEvents = String.Join(", ", UserModel.Instance.filterEventList);
                //Events
                btnChooseEvent.Visibility = Android.Views.ViewStates.Gone;
                textEventName.Visibility = Android.Views.ViewStates.Visible;
                textEventName.Text = filterEvents;

                //Date
                btnChooseDate.Visibility = Android.Views.ViewStates.Gone;
                textSelectDate.Visibility = Android.Views.ViewStates.Visible;
                DateTime startDateTime = Convert.ToDateTime(UserModel.Instance.FilterStartDate);
                DateTime endDateTime = Convert.ToDateTime(UserModel.Instance.FilterEndDate);
                textSelectDate.Text = startDateTime.ToString("MMM dd, yyyy") + " -> " + endDateTime.ToString("MMM dd, yyyy");

                btnSeeConcerts.Visibility = Android.Views.ViewStates.Visible;
            }
            else if (UserModel.Instance.FilterEventType != null && UserModel.Instance.FilterEventType != "")
            {
                var filterEvents = String.Join(", ", UserModel.Instance.filterEventList);
                btnChooseEvent.Visibility = Android.Views.ViewStates.Gone;
                textEventName.Visibility = Android.Views.ViewStates.Visible;
                textEventName.Text = filterEvents;
                btnSeeConcerts.Visibility = Android.Views.ViewStates.Visible;
            }
            else if (UserModel.Instance.isFilterByDate)
            {
                btnChooseDate.Visibility = Android.Views.ViewStates.Gone;
                textSelectDate.Visibility = Android.Views.ViewStates.Visible;
                DateTime startDateTime = Convert.ToDateTime(UserModel.Instance.FilterStartDate);
                DateTime endDateTime = Convert.ToDateTime(UserModel.Instance.FilterEndDate);
                textSelectDate.Text = startDateTime.ToString("MMM dd, yyyy") + " -> " + endDateTime.ToString("MMM dd, yyyy");
                btnSeeConcerts.Visibility = Android.Views.ViewStates.Visible;
            }
            else
            {
                btnSeeConcerts.Visibility = Android.Views.ViewStates.Gone;
                btnChooseEvent.Visibility = Android.Views.ViewStates.Visible;
                textEventName.Visibility = Android.Views.ViewStates.Gone;
            }
        }
    }
}
