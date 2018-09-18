
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

namespace SharedLibraryAndroid
{
  public class ProgramNotesView : Fragment
    {
        private ProgramNotesAdapter _Adapter;
        private Context mContext;
        private TextView lblTicketName, lblLength;
        private ImageView imgThumbnail,btnBack;
        private RecyclerView concertRecyclerView;
        private Button btnBuyTicket;
        public Event selectedEvent;
        public List<Piece> piecesList;
        public EventService eventService= ServiceContainer.Resolve<EventService>();
       
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.ProgramNotesView, container, false);

            mContext = this.Activity;
            InitView(view);

           
            btnBack.Click += (sender,e) => 
            {
                FragmentManager.PopBackStack();
            };
            btnBuyTicket.Click += (sender, e) => 
            {
                if (selectedEvent.BuyUrl == null)
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
                    string url = selectedEvent.BuyUrl;
                    string newUrl = url.Replace("https://", "https://lapatester:p@ssw0rd@");
                    var uri = Android.Net.Uri.Parse(newUrl);
                    var intent = new Intent(Intent.ActionView, uri);
                    StartActivity(intent);
                }
            };
            //btnAboutThePerformance.Click += (sender, e) =>
            //{
            //    AboutThePerformanceView fragment = new AboutThePerformanceView();
            //    Bundle args = new Bundle();
            //    args.PutString("Name", selectedEvent.Program.Name);
            //    args.PutString("Description", selectedEvent.Description);
            //    fragment.Arguments = args;
            //    var transaction = FragmentManager.BeginTransaction();
            //    transaction.Add(Resource.Id.Main_FragmentContainer, fragment);
            //    transaction.AddToBackStack(null);
            //    transaction.Commit();
            //};
         
                GetData();
            return view.RootView;
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
            if (objEvent.Pieces.Length > 0)
            {
                piecesList = (objEvent.Pieces).OfType<Piece>().ToList();
                _Adapter = new ProgramNotesAdapter(piecesList, mContext);
                concertRecyclerView.SetAdapter(_Adapter);
                _Adapter.ItemClick += OnItemClick;

            }
        }
        private async void GetData(){
            HttpClient httpClient = new HttpClient();
            var uri = new Uri(string.Format(selectedEvent.Url, string.Empty));

            var response = await httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var  evt = JsonConvert.DeserializeObject<Event>(content);
                GetEventDetailData(evt);
            }
        }
        void OnItemClick(object sender, int position)
        {
            PieceDetailView fragment = new PieceDetailView();
            Bundle args = new Bundle();
            args.PutString("Name", piecesList[position].Name);
            args.PutString("Description", piecesList[position].Description);
            args.PutString("ComposerName", piecesList[position].ComposerName);
            fragment.Arguments = args;
            var transaction = FragmentManager.BeginTransaction();
            transaction.Add(Resource.Id.Main_FragmentContainer, fragment);
            transaction.AddToBackStack(null);
            transaction.Commit();
        }
        private void InitView(View view)
        {
            concertRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.rv_programs);
            btnBack = view.FindViewById<ImageView>(Resource.Id.btnBack);
            imgThumbnail = view.FindViewById<ImageView>(Resource.Id.imgThumbnail);
            lblTicketName = view.FindViewById<TextView>(Resource.Id.lblTicketName);
            lblLength = view.FindViewById<TextView>(Resource.Id.lblLength);
            btnBuyTicket = view.FindViewById<Button>(Resource.Id.btnBuyTicket);
            var header = view.FindViewById<TextView>(Resource.Id.header);
          //  btnAboutThePerformance = view.FindViewById<Button>(Resource.Id.btnAboutThePerformance);

            lblTicketName.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            header.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            lblLength.SetTypeface(Utility.RegularTypeface(mContext), TypefaceStyle.Normal);
          
            LinearLayoutManager layoutManager = new LinearLayoutManager(mContext);
            concertRecyclerView.SetLayoutManager(layoutManager);
       

            try
            {
                lblTicketName.Text = selectedEvent.Program.Name;
                Glide.With(mContext).Load(selectedEvent.ImageUrl).Thumbnail(0.1f).Into(imgThumbnail);
            }
            catch (Exception)
            { }
        }
    }
}
