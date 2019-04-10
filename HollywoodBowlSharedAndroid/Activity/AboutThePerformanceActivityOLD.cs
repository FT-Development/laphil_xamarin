
using Android.Content.PM;
using Android.OS;
using Android.App;
using Android.Content;
using Android.Widget;
using LAPhil.Events;
using System;
using Android.Text;
using Com.Bumptech.Glide;
using System.Threading.Tasks;
using Android.Text.Format;

namespace HollywoodBowl.Droid                  
{    
    [Activity(Label = "HollywoodBowl", MainLauncher = false, Icon = "@mipmap/icon", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class AboutThePerformanceActivityOLD : Activity
    {
        private Context mContext;
        private TextView lblTicketName, lblHeader, lblTicketDateTime, lbltag, lblRulesApply, lblaboutThePer, lblPartOf, lblDescription;
        private ImageView btnBack, imgThumbnail;
        private Button btnBuyTicket;
        private TabBarView tabBarView;
        public Event Event { get; set; }

		
		protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AboutThePerformanceView);
            OverridePendingTransition(Resource.Animation.No_animation, Resource.Animation.No_animation);
            mContext = this;
            Event = UserModel.Instance.SelectedEvent;
            InitView(mContext);

            if (Event.Program != null && Event.Program.Name != null)
            {
                if (((int)Build.VERSION.SdkInt) >= 24)
                {
                    lblTicketName.TextFormatted = Html.FromHtml(Event.Program.Name, FromHtmlOptions.ModeLegacy);
                }
                else
                {
                    lblTicketName.TextFormatted = Html.FromHtml(Event.Program.Name);
                }
            }

            lbltag.Text = Event.Program.ProducerName;
            if (Event.Program != null &&
                    Event.Program.ExtraMessageUrl != null &&
                    Event.Program.ExtraMessageUrl != string.Empty)
            {
                lblRulesApply.Visibility = Android.Views.ViewStates.Visible;
                lblRulesApply.Text = Event.Program.ExtraMessage;
            }else
            {
                lblRulesApply.Visibility = Android.Views.ViewStates.Gone;
            }
            if (Event.Series != null &&
                  Event.Series.WebUrl != null &&
                   Event.Series.WebUrl != string.Empty)
            {
                lblPartOf.Text = "PART OF " + Event.Series.Name;
            }

            lblRulesApply.Click += (sender, e) =>
            {
                if (Event.Program != null &&
                    Event.Program.ExtraMessageUrl != null &&
                    Event.Program.ExtraMessageUrl != string.Empty)
                {
                    string newUrl = Event.Program.ExtraMessageUrl;
                    var uri = Android.Net.Uri.Parse(newUrl);
                    var intent = new Intent(Intent.ActionView, uri);
                    StartActivity(intent);
                }
            };

            lblPartOf.Click += (sender, e) =>
            {
                if (Event.Series != null &&
                   Event.Series.WebUrl != null &&
                    Event.Series.WebUrl != string.Empty)
                {
                    string newUrl = Event.Series.WebUrl;
                    var uri = Android.Net.Uri.Parse(newUrl);
                    var intent = new Intent(Intent.ActionView, uri);
                    StartActivity(intent);
                }

            };


            if (((int)Build.VERSION.SdkInt) >= 24)
            {
                lblDescription.TextFormatted = Html.FromHtml(Event.Description, FromHtmlOptions.ModeLegacy);
            }
            else
            {
                lblDescription.TextFormatted = Html.FromHtml(Event.Description);
            }

            try
            {
                Glide.With(mContext).Load(Event.ImageUrl).Thumbnail(0.1f).Into(imgThumbnail);
            }
            catch (Exception)
            { }

            //var timeString = Event.StartTime;
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(Event.StartTime.DateTime, System.TimeZoneInfo.Local);
            var date1 = localTime.ToString($"ddd / MMM dd, yyyy / {TimeFormat}");
           
            lblTicketDateTime.Text = date1;

            btnBuyTicket.Click += (sender, e) =>
            {
                if (Event.BuyUrl == null)
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
                    string url = Event.BuyUrl;
                    string newUrl = url.Replace("https://", "https://lapatester:p@ssw0rd@");
                    var uri = Android.Net.Uri.Parse(newUrl);
                    var intent = new Intent(Intent.ActionView, uri);
                    StartActivity(intent);
                }
            };

            btnBack.Click += (sender, e) =>
            {
                Finish();
            };

        }
        public string TimeFormat
        {
            get
            {
                if (DateFormat.Is24HourFormat(mContext))
                    return "H:mm tt";

                return "h:mm tt";
            }
        }
        private void InitView(Context context)
        {
            btnBack = FindViewById<ImageView>(Resource.Id.btnBack);
            imgThumbnail = FindViewById<ImageView>(Resource.Id.imgThumbnail);
            lblTicketName = FindViewById<TextView>(Resource.Id.lblTicketName);
            lblTicketDateTime = FindViewById<TextView>(Resource.Id.lblTicketDateTime);
            btnBuyTicket = FindViewById<Button>(Resource.Id.btnBuyTicket);
            lbltag = FindViewById<TextView>(Resource.Id.lbltag);
            lblRulesApply = FindViewById<TextView>(Resource.Id.lblRulesApply);
            lblaboutThePer = FindViewById<TextView>(Resource.Id.lblaboutThePer);
            lblPartOf = FindViewById<TextView>(Resource.Id.lblPartOf);
            lblDescription = FindViewById<TextView>(Resource.Id.lblDescription);
            var lytCustomBottom = (LinearLayout)FindViewById(Resource.Id.lytCustomBottom);
            tabBarView = new TabBarView(mContext);
            lytCustomBottom.AddView(tabBarView);

            btnBuyTicket.SetTypeface(Utility.BoldTypeface(context), Android.Graphics.TypefaceStyle.Bold);
            lblTicketName.SetTypeface(Utility.RegularTypeface(context), Android.Graphics.TypefaceStyle.Normal);
            lblTicketDateTime.SetTypeface(Utility.RegularTypeface(context), Android.Graphics.TypefaceStyle.Normal);
            lbltag.SetTypeface(Utility.RegularTypeface(context), Android.Graphics.TypefaceStyle.Bold);
            lblRulesApply.SetTypeface(Utility.RegularTypeface(context), Android.Graphics.TypefaceStyle.Normal);
            lblaboutThePer.SetTypeface(Utility.BoldTypeface(context), Android.Graphics.TypefaceStyle.Bold);
            lblPartOf.SetTypeface(Utility.RegularTypeface(context), Android.Graphics.TypefaceStyle.Normal);
            lblDescription.SetTypeface(Utility.RegularTypeface(context), Android.Graphics.TypefaceStyle.Normal);

            lblHeader = FindViewById<TextView>(Resource.Id.lblHeader);
            lblHeader.SetTypeface(Utility.BoldTypeface(context), Android.Graphics.TypefaceStyle.Bold);
        }

        protected override void OnResume()
        {
            base.OnResume();
            //Task.Delay(200);
            //GC.Collect(0);
        }
    }

}
