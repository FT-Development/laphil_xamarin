using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using LAPhil.Droid;

namespace SharedLibraryAndroid
{
    [Activity(Label = "LA Phil", MainLauncher = false, Icon = "@mipmap/icon", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class CalendarDateFilterActivity : Activity
    {
        private Context mContext;
        private DateTime startDateCondition;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SelectCalendarDateActivity);
            mContext = this;
            var nextYear = DateTime.Now.AddYears(3);
            var startDate = "";
            var endDate = "";
            var dates1 = new List<DateTime>() { };
            var calendar = FindViewById<MonoDroid.TimesSquare.CalendarPickerView>(Resource.Id.calendar_view);
            var lblEndDate = FindViewById<TextView>(Resource.Id.lblEndDate);
            var lblStartDate = FindViewById<TextView>(Resource.Id.lblStartDate);
            var header = FindViewById<TextView>(Resource.Id.header);
            var btnSubmit = FindViewById<TextView>(Resource.Id.btnSubmit);
            var btnClearDate = FindViewById<TextView>(Resource.Id.btnClearDate);
            var btnBack = FindViewById<ImageView>(Resource.Id.btnBack);
            var dateLayout = FindViewById<LinearLayout>(Resource.Id.dateLayout);
            lblEndDate.SetTypeface(Utility.BoldTypeface(mContext), Android.Graphics.TypefaceStyle.Bold);
            lblStartDate.SetTypeface(Utility.BoldTypeface(mContext), Android.Graphics.TypefaceStyle.Bold);
            header.SetTypeface(Utility.BoldTypeface(mContext), Android.Graphics.TypefaceStyle.Bold);
            btnSubmit.SetTypeface(Utility.BoldTypeface(mContext), Android.Graphics.TypefaceStyle.Bold);
            btnClearDate.SetTypeface(Utility.BoldTypeface(mContext), Android.Graphics.TypefaceStyle.Bold);

            btnSubmit.Click += (s, e) =>
            {
                UserModel.Instance.isFilterByDate = true;
                UserModel.Instance.isFromSeeConcetFilter = true;
                StartActivity(new Intent(this, typeof(FilterActivity)));
                Finish();
            };

            btnClearDate.Click += (s, e) =>
            {
                startDate = "";
                endDate = "";
                lblEndDate.Text = "Choose End Date";
                lblStartDate.Text = "Choose Start Date";
                dateLayout.SetBackgroundColor(Resources.GetColor(Resource.Color.app_color_Brown));
                calendar.Init(DateTime.Now, nextYear)
                  .InMode(MonoDroid.TimesSquare.CalendarPickerView.SelectionMode.Range)
                  .WithSelectedDates(dates1);
                btnClearDate.Visibility = Android.Views.ViewStates.Gone;
                btnSubmit.Visibility = Android.Views.ViewStates.Gone;
            };

            btnBack.Click += (sender, e) =>
            {
                Finish();
            };

            var startDate1 = "";
            calendar.Init(DateTime.Now, nextYear)
                .InMode(MonoDroid.TimesSquare.CalendarPickerView.SelectionMode.Range)
                .WithSelectedDates(dates1);
            calendar.OnDateSelected +=
                        (s, e) =>
                        {
                            if (startDate.Equals(""))
                            {
                                dateLayout.SetBackgroundColor(Resources.GetColor(Resource.Color.app_color_Brown));
                                startDate = e.SelectedDate.ToShortDateString();
                                var myDateTime = e.SelectedDate.ToString("yyyyMMddTHHmmss");
                                DateTime d = DateTime.ParseExact(myDateTime, "yyyyMMdd'T'HHmmss", null);
                                var date2 = d.ToString("MMM dd, yyyy");
                                lblStartDate.Text = date2;
                                startDateCondition = d.Date;
                                startDate1 = date2;
                                endDate = "";
                                lblEndDate.Text = "Choose End Date";
                                UserModel.Instance.FilterStartDate = e.SelectedDate.Date.ToString();
                                btnSubmit.Visibility = Android.Views.ViewStates.Gone;
                                btnClearDate.Visibility = Android.Views.ViewStates.Visible;
                            }
                            else
                            {
                                dateLayout.SetBackgroundColor(Resources.GetColor(Resource.Color.app_textColor_Blue));
                                var myDateTime = e.SelectedDate.ToString("yyyyMMddTHHmmss");
                                DateTime d = DateTime.ParseExact(myDateTime, "yyyyMMdd'T'HHmmss", null);
                                var date2 = d.ToString("MMM dd, yyyy");

                                if (d.Date <= startDateCondition)
                                {
                                    lblEndDate.Text = startDate1;
                                    lblStartDate.Text = date2;
                                    UserModel.Instance.FilterEndDate = startDate;
                                    UserModel.Instance.FilterStartDate = e.SelectedDate.Date.ToString();
                                    endDate = startDate1;

                                }
                                else
                                {
                                    lblEndDate.Text = date2;
                                    UserModel.Instance.FilterEndDate = e.SelectedDate.Date.ToString();
                                    endDate = e.SelectedDate.ToShortDateString();
                                }
                                startDate = "";
                                btnSubmit.Visibility = Android.Views.ViewStates.Visible;
                            }
                        };

        }
    }
}
