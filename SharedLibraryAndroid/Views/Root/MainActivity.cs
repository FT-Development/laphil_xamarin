using System;
using Android.App;
using Android.Widget;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;

using System.Reactive.Linq;
using LAPhil.Logging;
using LAPhil.Application;
using LAPhil.Droid;
using Android.Content;
using Android.Graphics.Drawables;

namespace SharedLibraryAndroid
{

    [Activity(Label = "LAPhil", MainLauncher = false, Icon = "@mipmap/icon", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity
    {
        TabBar TabBarView { get; set; }
        FrameLayout Region { get; set; }
        IDisposable TabBarSubscription;
        Context mContext;
        ILog Log = ServiceContainer.Resolve<LoggingService>().GetLogger<MainActivity>();
        public int CurrentSection { get; internal set; } = Resource.Id.TabNavigation1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            mContext = this;
            TabBarView = FindViewById<TabBar>(Resource.Id.Main_TabBar);
            TabBarSubscription = TabBarView.Rx.Click.Subscribe(OnTabBarClick);
            ShowSection(CurrentSection,null);
        }

        protected override void OnDestroy()
        {
            TabBarSubscription.Dispose();
            base.OnDestroy();
        }

        void OnTabBarClick(TabBarButton button)
        {

            Drawable image = mContext.GetDrawable(Resource.Drawable.back_icon);
      
            TabBarButton tab= FindViewById<TabBarButton>(CurrentSection);
            tab.SetBackgroundColor(Android.Graphics.Color.ParseColor("#231f20"));
            tab.SetCompoundDrawablesWithIntrinsicBounds(null, image, null, null);
            ShowSection(button.Id,null);
            if (button.IsSelected) {
                button.SetBackgroundColor(Android.Graphics.Color.ParseColor("#FFFFFF"));
            }

        }

        public void ShowSection(int id, Bundle bundle = null)
        {
            Fragment fragment = null;
            switch(id)
            {
                case Resource.Id.TabNavigation1:
                   fragment = new ConcertsView();
                    break;
                case Resource.Id.TabNavigation2:
                    fragment = new MyTicketsView();
                    break;
                case Resource.Id.TabNavigation4:
                    fragment = new WhenHereView();
                    break;
                case Resource.Id.TabNavigation5:
                    fragment = new MoreView();
                    break;
                default:
                    return;
            }

            CurrentSection = id;
            ShowSection(fragment: fragment, bundle: bundle);
        }

        public void ShowSection(Fragment fragment, Bundle bundle = null)
        {
            fragment.Arguments = bundle;
            var transaction = FragmentManager.BeginTransaction();
            transaction.Replace(Resource.Id.Main_FragmentContainer, fragment);
            transaction.AddToBackStack(null);
            transaction.Commit();
        }
    }
}

