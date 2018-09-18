using Android.App;
using Android.OS;
using Android.Views;
using LAPhil.Droid;
using Android.Support.V7.Widget;
using Android.Content;
using System.Collections.Generic;
using System;
using Android.Widget;
using System.Linq;
using Android.Graphics;
using Com.Bumptech.Glide;
using LAPhil.Events;
using LAPhil.Application;
using System.Net.Http;
using Newtonsoft.Json;
using Android.Content.PM;
using Android.Text.Format;
using Android.Text;
using System.Threading;

namespace SharedLibraryAndroid
{
   [Activity(Label = "LA Phil", MainLauncher = false, Icon = "@mipmap/icon", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
   public class ProgramNotesActivity : Activity
    { 
        private ProgramNotesAdapter _Adapter;
        private PerformerArtistsAdapter _PerformerAdapter;
        private Context mContext;
        private TextView lblTicketName, lblLength, lblDatetime , lblSeriesName;
        private ImageView imgThumbnail, btnBack;
        private RecyclerView concertRecyclerView,artistsRecyclerView;
        private TextView btnBuyTicket,btnAboutThePerformance;
        public Event selectedEvent;
        public List<Piece> piecesList;
        public List<Performer> performerList;
        public EventService eventService = ServiceContainer.Resolve<EventService>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ProgramNotesView);
            OverridePendingTransition(Resource.Animation.No_animation, Resource.Animation.No_animation);
            mContext = this;
            selectedEvent = UserModel.Instance.SelectedEvent;
            var lytCustomBottom = (LinearLayout)FindViewById(Resource.Id.lytCustomBottom);
            var tabBarView = new TabBarView(mContext);
            lytCustomBottom.AddView(tabBarView);

            InitView();

            //if (selectedEvent.Description != null && selectedEvent.Description != "")
            //{
            //    lblPerformanceName.Visibility = ViewStates.Visible;
            //}
            //else
            //{
            //    lblPerformanceName.Visibility = ViewStates.Gone;
            //}

            if (selectedEvent.Description != null && selectedEvent.Description != "")
            {
                btnAboutThePerformance.Visibility = ViewStates.Visible;
            }
            else
            {
                btnAboutThePerformance.Visibility = ViewStates.Gone;
            }


            var localTime = TimeZoneInfo.ConvertTimeFromUtc(selectedEvent.StartTime.DateTime, System.TimeZoneInfo.Local);
            var date = localTime.ToString($"ddd / MMM dd - {TimeFormat}");
            lblDatetime.Text = date;

            btnBack.Click += (sender, e) =>
            {
                Finish();
                OverridePendingTransition(Resource.Animation.Slide_in_left, Resource.Animation.Slide_out_right);
            };
            btnBuyTicket.Click += (sender, e) =>
            {
                if (selectedEvent.BuyUrl == null)
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
            };

            btnAboutThePerformance.Click += (sender, e) =>
            {
                Intent intent = new Intent(this, typeof(AboutThePerformanceActivity));
                intent.PutExtra("Name",selectedEvent.Program.Name);
                intent.PutExtra("Description",selectedEvent.Description);
                StartActivity(intent);
            };

            GetData();
            //LoadEventDetails();

        }
        void LoadEventDetails()
        {
            eventService
                .EventDetail(selectedEvent)
                .Subscribe((Event evt) => {
                    GetEventDetailData(evt);
                });
        }

        private void GetEventDetailData(Event objEvent)
        {
            if(objEvent.Performers.Length>0)
            {
                performerList = (objEvent.Performers).OfType<Performer>().ToList();
                _PerformerAdapter = new PerformerArtistsAdapter(performerList, mContext);
                artistsRecyclerView.SetAdapter(_PerformerAdapter);
              //  _PerformerAdapter.ArtistsItemClick += OnArtistsItemClick;
            }
            if (objEvent.Pieces.Length > 0)
            {
                RunOnUiThread(() =>
                {
                    piecesList = (objEvent.Pieces).OfType<Piece>().ToList();
                    _Adapter = new ProgramNotesAdapter(piecesList, mContext);
                    concertRecyclerView.SetAdapter(_Adapter);
                    _Adapter.ItemClick += OnItemClick;
                });
            }
           
        }
        private async void GetData()
        {
            HttpClient httpClient = new HttpClient();
            var uri = new Uri(string.Format(selectedEvent.Url, string.Empty));

            var response = await httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var evt = JsonConvert.DeserializeObject<Event>(content);
                GetEventDetailData(evt);
            }
        }
        void OnItemClick(object sender, int position)
        {
          
            Intent intent = new Intent(this, typeof(PieceDetailActivity));
            intent.PutExtra("Name", piecesList[position].Name);
            intent.PutExtra("Description", piecesList[position].Description);
            intent.PutExtra("ComposerName", piecesList[position].ComposerName);
            StartActivity(intent);

        }
        private void InitView()
        {
            concertRecyclerView = FindViewById<RecyclerView>(Resource.Id.rv_programs);
            artistsRecyclerView = FindViewById<RecyclerView>(Resource.Id.rv_artists);
            btnBack = FindViewById<ImageView>(Resource.Id.btnBack);
            imgThumbnail = FindViewById<ImageView>(Resource.Id.imgThumbnail);
            lblTicketName = FindViewById<TextView>(Resource.Id.lblTicketName);
            lblLength = FindViewById<TextView>(Resource.Id.lblLength);
            btnBuyTicket = FindViewById<TextView>(Resource.Id.btnBuyTicket);
            var header = FindViewById<TextView>(Resource.Id.header);
            lblDatetime = FindViewById<TextView>(Resource.Id.lblDatetime);
            btnAboutThePerformance = FindViewById<TextView>(Resource.Id.btnAboutThePerformance);
            var lblArtists = FindViewById<TextView>(Resource.Id.lblArtists);

            //lblPerformanceName = FindViewById<TextView>(Resource.Id.lblPerformance);
            //lblPerformanceDesc = FindViewById<TextView>(Resource.Id.lblPerformanceDesc);
            //lblSeriesName = FindViewById<TextView>(Resource.Id.lblSeriesName);
            //lblPerformanceName.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            //lblPerformanceDesc.SetTypeface(Utility.RegularTypeface(mContext), TypefaceStyle.Normal);

            lblArtists.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);


            btnAboutThePerformance.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            btnBuyTicket.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);

            lblTicketName.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            header.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            lblDatetime.SetTypeface(Utility.RegularTypeface(mContext), TypefaceStyle.Normal);
            lblLength.SetTypeface(Utility.RegularTypeface(mContext), TypefaceStyle.Normal);

            LinearLayoutManager layoutManager = new LinearLayoutManager(mContext);
            LinearLayoutManager layoutManagerArtists = new LinearLayoutManager(mContext);
            concertRecyclerView.SetLayoutManager(layoutManager);
            artistsRecyclerView.SetLayoutManager(layoutManagerArtists);
            try
            {
                if (((int)Build.VERSION.SdkInt) >= 24)
                {
                    lblTicketName.TextFormatted = Android.Text.Html.FromHtml(selectedEvent.Program.Name, Android.Text.FromHtmlOptions.ModeLegacy);
                }
                else
                {
                    lblTicketName.TextFormatted = Android.Text.Html.FromHtml(selectedEvent.Program.Name);
                }
                //lblTicketName.Text = selectedEvent.Program.Name;

                if (selectedEvent.Image3x2Url != null && selectedEvent.Image3x2Url != "")
                {
                    Glide.With(mContext).Load(selectedEvent.Image3x2Url).Thumbnail(0.1f).Into(imgThumbnail);
                }
                else
                {
                    Glide.With(mContext).Load(selectedEvent.ImageUrl).Thumbnail(0.1f).Into(imgThumbnail);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(""+e.Message);
            }
        }

		public override void OnBackPressed()
		{
            base.OnBackPressed();
            Finish();
            OverridePendingTransition(Resource.Animation.Slide_in_left, Resource.Animation.Slide_out_right);
		}

        //protected override void OnResume()
        //{
        //    base.OnResume();
        //    GC.Collect(0);
        //}
        public string TimeFormat
        {
            get
            {
                if (DateFormat.Is24HourFormat(mContext))
                    return "H:mm tt";

                return "h:mm tt";
            }
        }
	}
}
