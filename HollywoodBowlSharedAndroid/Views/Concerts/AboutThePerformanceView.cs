using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Content;
using Android.Widget;
using LAPhil.Events;
using Android.Text;
using Com.Bumptech.Glide;

namespace HollywoodBowl.Droid
{
    public class AboutThePerformanceView : Fragment
    {
        private Context mContext;
        private TextView lblTicketName,lblHeader,lblTicketDateTime, lbltag,lblRulesApply,lblaboutThePer,lblPartOf,lblDescription;
        private ImageView btnBack,imgThumbnail;
        private Button btnBuyTicket;
        public Event Event { get; set; }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.AboutThePerformanceView, container, false);
            mContext = this.Activity;
            InitView(view, mContext);

            if(Event.Program != null&&Event.Program.Name!=null){
                lblTicketName.Text = Event.Program.Name;
            }

            lbltag.Text = Event.Program.ProducerName;
            if (Event.Program != null &&
                    Event.Program.ExtraMessageUrl != null &&
                    Event.Program.ExtraMessageUrl != string.Empty)
            { 
                lblRulesApply.Text = Event.Program.ExtraMessage;
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
                    Event.Series.WebUrl != string.Empty){
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
            }catch (Exception)
            { }

            var timeString = Event.StartTime;
            // DateTime dt = Convert.ToDateTime(timeString);
            var myDateTime = timeString.Date.ToString("yyyyMMddTHHmmssZ");
            DateTime d = DateTime.ParseExact(myDateTime, "yyyyMMdd'T'HHmmss'Z'", null);
            var date = d.ToString("ddd / MMM dd, yyyy / H:mm tt");
            lblTicketDateTime.Text = date;


            btnBuyTicket.Click += (sender, e) =>
            {
                if (Event.BuyUrl == null)
                {
                    Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(this.Activity);
                    AlertDialog alert = dialog.Create();
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
                FragmentManager.PopBackStack();
            };

            return view.RootView;
        }

        private void InitView(View view, Context context)
        {
            btnBack = view.FindViewById<ImageView>(Resource.Id.btnBack);
            imgThumbnail = view.FindViewById<ImageView>(Resource.Id.imgThumbnail);
            lblTicketName = view.FindViewById<TextView>(Resource.Id.lblTicketName);
            lblTicketDateTime = view.FindViewById<TextView>(Resource.Id.lblTicketDateTime);
            btnBuyTicket = view.FindViewById<Button>(Resource.Id.btnBuyTicket);
            lbltag = view.FindViewById<TextView>(Resource.Id.lbltag);
            lblRulesApply = view.FindViewById<TextView>(Resource.Id.lblRulesApply);
            lblaboutThePer = view.FindViewById<TextView>(Resource.Id.lblaboutThePer);
            lblPartOf = view.FindViewById<TextView>(Resource.Id.lblPartOf);
            lblDescription = view.FindViewById<TextView>(Resource.Id.lblDescription);

            btnBuyTicket.SetTypeface(Utility.BoldTypeface(mContext), Android.Graphics.TypefaceStyle.Bold);

            lblTicketName.SetTypeface(Utility.BoldTypeface(mContext), Android.Graphics.TypefaceStyle.Bold);
            lblTicketDateTime.SetTypeface(Utility.RegularTypeface(mContext), Android.Graphics.TypefaceStyle.Normal);
            lbltag.SetTypeface(Utility.RegularTypeface(mContext), Android.Graphics.TypefaceStyle.Bold);
            lblRulesApply.SetTypeface(Utility.RegularTypeface(mContext), Android.Graphics.TypefaceStyle.Normal);
            lblaboutThePer.SetTypeface(Utility.BoldTypeface(mContext), Android.Graphics.TypefaceStyle.Bold);
            lblPartOf.SetTypeface(Utility.RegularTypeface(mContext), Android.Graphics.TypefaceStyle.Normal);
            lblDescription.SetTypeface(Utility.RegularTypeface(mContext), Android.Graphics.TypefaceStyle.Normal);


            lblHeader = view.FindViewById<TextView>(Resource.Id.lblHeader);
            lblHeader.SetTypeface(Utility.BoldTypeface(mContext), Android.Graphics.TypefaceStyle.Bold);
        }

    }
}
