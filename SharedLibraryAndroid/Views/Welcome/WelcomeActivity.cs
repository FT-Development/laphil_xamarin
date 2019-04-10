using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Java.Lang;
using LAPhil.Droid;
namespace SharedLibraryAndroid
{
    [Activity(MainLauncher = false, Icon = "@mipmap/icon", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class WelcomeActivity : AppCompatActivity
    {
        private ViewPager viewPager;
        private LinearLayout dotsLayout;
        private TextView[] dots;
        public int[] layouts;
        public int[] iconList;
        private string[] contentDesc;
        private TextView btnNext;
        private ImageView lblContentLogo;
        private TextView lblContentDesc;
        private Context mContext;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //LayoutInflater.Factory = new FontUtils();

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.intro_layout);
            mContext = this;
           
          
            this.Window.AddFlags(WindowManagerFlags.Fullscreen);
            layouts = new int[]
              {
                          Resource.Drawable.on_board_slide01,
                          Resource.Drawable.on_board_slide02,
                          Resource.Drawable.on_board_slide03
              };
            iconList = new int[]
                {
                          Resource.Drawable.on_board_slide_icon,
                          Resource.Drawable.on_board_slide_icon01,
                          Resource.Drawable.on_board_slide_icon02
                };
            contentDesc = new string[]
            {
                        "Find Upcoming Shows",
                        "Access Your Tickets",
                        "Learn About The Music"

            };
            viewPager = FindViewById<ViewPager>(Resource.Id.pager);
            dotsLayout = FindViewById<LinearLayout>(Resource.Id.linearDot);
            btnNext = FindViewById<TextView>(Resource.Id.btnNext);
            lblContentDesc = FindViewById<TextView>(Resource.Id.lblContentDesc);
            btnNext.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            lblContentDesc.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);

            lblContentLogo = FindViewById<ImageView>(Resource.Id.lblContentLogo);
            AddDots(0);
         
   
            lblContentDesc.Text = contentDesc[0];
            lblContentLogo.SetImageResource(iconList[0]);
            ViewPagerAdapter adapter = new ViewPagerAdapter(layouts, contentDesc);
            viewPager.Adapter = adapter;

            viewPager.PageSelected += ViewPager_PageSelected;
            //viewPager.AddOnPageChangeListener(new ViewPager.IOnPageChangeListener());

            btnNext.Click += (sender, e) =>
            {
                int current = GetItem(+1);
                if (current < layouts.Length){
                    //move to next screen
                    viewPager.CurrentItem = current;
                }
                else
                {
                    Dialog dialog = new Dialog(mContext);
                    dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
                    dialog.SetContentView(Resource.Layout.WelcomPopup);
                    dialog.Window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                    dialog.SetCancelable(true);

                    TextView welcomePopupDescText = (TextView)dialog.FindViewById(Resource.Id.welcomePopupDescText);
                    TextView welcomePopupHeaderText = (TextView)dialog.FindViewById(Resource.Id.welcomePopupHeaderText);
                    welcomePopupDescText.SetTypeface(Utility.RegularTypeface(mContext), TypefaceStyle.Normal);
                    welcomePopupHeaderText.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
                    //welcomePopupHeaderText.SetTextSize(Android.Util.ComplexUnitType, 8.0);
                    //welcomePopupDescText.SetTextSize(Android.Util.ComplexUnitType, 8.0);

                    // popupWindow.OutsideTouchable = true;
                    TextView btnNoThanks = (TextView)dialog.FindViewById(Resource.Id.btnNoThanks);
                    TextView btnOk = (TextView)dialog.FindViewById(Resource.Id.btnOk);
                    btnNoThanks.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
                    btnOk.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
                    btnOk.Click += delegate {
                            dialog.Dismiss();
                        Utility.SetBooleanSharedPreference("isOldUser", true);
                        Intent intent = new Intent(this, typeof(ConcertActivity));
                        StartActivity(intent);
                        Finish();
                        };
                    btnNoThanks.Click += delegate {
                        dialog.Dismiss();
                        Dialog dialogNoti = new Dialog(mContext);
                        dialogNoti.RequestWindowFeature((int)WindowFeatures.NoTitle);
                        dialogNoti.SetContentView(Resource.Layout.NotificationPopup);
                        dialogNoti.Window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                        dialogNoti.SetCancelable(true);
                        dialogNoti.Show();

                        TextView popupHeaderText = (TextView)dialogNoti.FindViewById(Resource.Id.popupHeaderText);
                        TextView popupDescText = (TextView)dialogNoti.FindViewById(Resource.Id.popupDescText);

                        popupDescText.SetTypeface(Utility.RegularTypeface(mContext), TypefaceStyle.Normal);
                        popupHeaderText.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);

                        TextView btnOKNoti = (TextView)dialogNoti.FindViewById(Resource.Id.btn_ok);
                        btnOKNoti.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);

                        btnOKNoti.Click += delegate {
                            dialogNoti.Dismiss();
                            Utility.SetBooleanSharedPreference("isOldUser", true);
                            Intent intent1 = new Intent(this, typeof(ConcertActivity));
                            StartActivity(intent1);
                            Finish();
                        };
                    };

                    //dialog.Dismiss();
                    dialog.Show();
                    //launch main screen here
                }
            };
        }
        
        void ViewPager_PageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {
            AddDots(e.Position);
            lblContentDesc.Text = contentDesc[e.Position];
            lblContentLogo.SetImageResource(iconList[e.Position]);
            //changing the next button text
            // Next or Got it
            if (e.Position == layouts.Length - 1)
            {
                // if it is a last page. make button text to "Got it"
                //btnNext.Text = (GetString(Resource.String.start));
                btnNext.Text = "GET STARTED";
                //btnSkip.Visibility = ViewStates.Gone;
            }
            else
            {
                // if it is not a last page.
                //btnNext.Text = (GetString(Resource.String.next));
                btnNext.Text = "NEXT";
                // btnSkip.Visibility = ViewStates.Visible;
            }
        }
        private void AddDots(int currentPage)
        {
            dots = new TextView[layouts.Length];
            dotsLayout.RemoveAllViews();
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i] = new TextView(this);
                dots[i].Text = (Android.Text.Html.FromHtml("•")).ToString();
                dots[i].TextSize = 55;
                dots[i].SetTextColor(Color.ParseColor("#4A4A4A"));
                dotsLayout.AddView(dots[i]);
            }
            if (dots.Length > 0)
            {
                dots[currentPage].SetTextColor(Color.ParseColor("#ffffff"));
            }
        }
        int GetItem(int i)
        {
            return viewPager.CurrentItem + i;
        }
        public class ViewPagerAdapter : PagerAdapter
        {
            LayoutInflater layoutInflater;
            int[] _layout;
            string[] _contentDescription;
            public ViewPagerAdapter(int[] layout, string[] contentDescription)
            {
                _layout = layout;
                _contentDescription = contentDescription;
            }

            public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
            {
                layoutInflater = (LayoutInflater)Android.App.Application.Context.GetSystemService(Context.LayoutInflaterService);
                View view = layoutInflater.Inflate(Resource.Layout.intro_cell, container, false);
                ImageView image = (ImageView)view.FindViewById(Resource.Id.img);
              
                image.SetImageResource(_layout[position]);

                container.AddView(view);
                return view;
            }

            public override int Count
            {
                get
                {
                    return _layout.Length;
                }
            }
            public override bool IsViewFromObject(View view, Java.Lang.Object objectValue)
            {
                return view == objectValue;
            }
            public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object objectValue)
            {
                View view = (View)objectValue;
                container.RemoveView(view);
            }
        }
    }
}
