using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using LAPhil.Droid;

namespace SharedLibraryAndroid
{
    public class TabBarView : LinearLayout
    {
        private Context context;
        private LinearLayout concerts, tickets, whenyou, more;
        private ImageView imgConcerts, imgTickets, imgWhenYou, imgMore;
        private TextView lblConcerts, lblTickets, lblWhenYou;

        public TabBarView(Context context) : base(context)
        {
         
            this.context = context;
            View view = LayoutInflater.From(context).Inflate(Resource.Layout.Tab_Bar_View, null, false);
            this.AddView(view);
            concerts = view.FindViewById<LinearLayout>(Resource.Id.lytConcerts);
            tickets = view.FindViewById<LinearLayout>(Resource.Id.lytMyTickets);
            whenyou = view.FindViewById<LinearLayout>(Resource.Id.lytWhenYou);
            more = view.FindViewById<LinearLayout>(Resource.Id.lytMore);

            imgConcerts = view.FindViewById<ImageView>(Resource.Id.imgConcerts);
            imgTickets = view.FindViewById<ImageView>(Resource.Id.imgMyTickets);
            imgWhenYou = view.FindViewById<ImageView>(Resource.Id.imgWhenYou);
            imgMore = view.FindViewById<ImageView>(Resource.Id.imgMore);

            lblConcerts = view.FindViewById<TextView>(Resource.Id.lblConcerts);
            lblTickets = view.FindViewById<TextView>(Resource.Id.lblMyTickets);
            lblWhenYou = view.FindViewById<TextView>(Resource.Id.lblWhenYou);

            lblTickets.SetTypeface(Utility.RegularTypeface(context), TypefaceStyle.Normal);
            lblConcerts.SetTypeface(Utility.RegularTypeface(context), TypefaceStyle.Normal);
            lblWhenYou.SetTypeface(Utility.RegularTypeface(context), TypefaceStyle.Normal);

            updateBottomView(UserModel.Instance.SelectedTab);

            concerts.Click += delegate
            {
                UserModel.Instance.SelectedTab = "Concerts";
                updateBottomView("Concerts");
                context.StartActivity(new Intent(context, typeof(ConcertActivity)));
                ((Activity)context).Finish();
            };
            tickets.Click += delegate
            {
                UserModel.Instance.SelectedTab = "MyTickets";
                updateBottomView("MyTickets");
                if (Utility.GetBooleanSharedPreference("isLogin")){
                    context.StartActivity(new Intent(context, typeof(MyTicketsActivity)));
                  ((Activity)context).Finish();
                }else{
                    context.StartActivity(new Intent(context, typeof(LoginActivity)));
                    ((Activity)context).Finish();
                }
            };
            whenyou.Click += delegate
            {
                UserModel.Instance.SelectedTab = "WhenYou";
                updateBottomView("WhenYou");
                context.StartActivity(new Intent(context, typeof(WhenYouHereActivity)));
                ((Activity)context).Finish();
            };
            more.Click += delegate
            {
                UserModel.Instance.SelectedTab = "More";
                updateBottomView("More");
                context.StartActivity(new Intent(context, typeof(MoreActivity)));
                ((Activity)context).Finish();
            };
        }

        public void updateBottomView(String selectedTab)
        {
            if (selectedTab.Contains("Concerts"))
            {
                concerts.SetBackgroundResource(Resource.Color.tab_select_color);
                tickets.SetBackgroundResource(Resource.Color.app_color_Brown);
                whenyou.SetBackgroundResource(Resource.Color.app_color_Brown);
                more.SetBackgroundResource(Resource.Color.app_color_Brown);

                imgConcerts.SetColorFilter(Color.Black, PorterDuff.Mode.SrcIn);
                imgTickets.SetColorFilter(Color.White, PorterDuff.Mode.SrcIn);
                imgWhenYou.SetColorFilter(Color.White, PorterDuff.Mode.SrcIn);
                imgMore.SetColorFilter(Color.White, PorterDuff.Mode.SrcIn);


                lblConcerts.SetTextColor(Color.Black);
                lblTickets.SetTextColor(Color.White);
                lblWhenYou.SetTextColor(Color.White);

            }
            else if (selectedTab.Contains("MyTickets"))
            {

                concerts.SetBackgroundResource(Resource.Color.app_color_Brown);
                tickets.SetBackgroundResource(Resource.Color.tab_select_color);
                whenyou.SetBackgroundResource(Resource.Color.app_color_Brown);
                more.SetBackgroundResource(Resource.Color.app_color_Brown);

                imgConcerts.SetColorFilter(Color.White, PorterDuff.Mode.SrcIn);
                imgTickets.SetColorFilter(Color.Black, PorterDuff.Mode.SrcIn);
                imgWhenYou.SetColorFilter(Color.White, PorterDuff.Mode.SrcIn);
                imgMore.SetColorFilter(Color.White, PorterDuff.Mode.SrcIn);

                lblConcerts.SetTextColor(Color.White);
                lblTickets.SetTextColor(Color.Black);
                lblWhenYou.SetTextColor(Color.White);

            }
            else if (selectedTab.Contains("WhenYou"))
            {

                concerts.SetBackgroundResource(Resource.Color.app_color_Brown);
                tickets.SetBackgroundResource(Resource.Color.app_color_Brown);
                whenyou.SetBackgroundResource(Resource.Color.tab_select_color);
                more.SetBackgroundResource(Resource.Color.app_color_Brown);


                imgConcerts.SetColorFilter(Color.White, PorterDuff.Mode.SrcIn);
                imgTickets.SetColorFilter(Color.White, PorterDuff.Mode.SrcIn);
                imgWhenYou.SetColorFilter(Color.Black, PorterDuff.Mode.SrcIn);
                imgMore.SetColorFilter(Color.White, PorterDuff.Mode.SrcIn);

                lblConcerts.SetTextColor(Color.White);
                lblTickets.SetTextColor(Color.White);
                lblWhenYou.SetTextColor(Color.Black);

            }
            else
            {

                concerts.SetBackgroundResource(Resource.Color.app_color_Brown);
                tickets.SetBackgroundResource(Resource.Color.app_color_Brown);
                whenyou.SetBackgroundResource(Resource.Color.app_color_Brown);
                more.SetBackgroundResource(Resource.Color.tab_select_color);

                imgConcerts.SetColorFilter(Color.White, PorterDuff.Mode.SrcIn);
                imgTickets.SetColorFilter(Color.White, PorterDuff.Mode.SrcIn);
                imgWhenYou.SetColorFilter(Color.White, PorterDuff.Mode.SrcIn);
                imgMore.SetColorFilter(Color.Black, PorterDuff.Mode.SrcIn);

                lblConcerts.SetTextColor(Color.White);
                lblTickets.SetTextColor(Color.White);
                lblWhenYou.SetTextColor(Color.White);
            }
        }
    }
}
