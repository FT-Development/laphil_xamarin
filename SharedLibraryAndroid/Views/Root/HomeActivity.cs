using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using LAPhil.Droid;
using Android.Content;
using Android.Support.Design.Widget;
using Android.Graphics;
using System.Collections.Generic;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using Android.Support.V4.View;


namespace SharedLibraryAndroid
{
    [Activity(Label = "LAPhil", MainLauncher = false, Icon = "@mipmap/icon", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class HomeActivity : AppCompatActivity
    {
        private Context mContext;
        private TabLayout tablayout;
        private Stack<String> fragmentHistory;
        private Stack<Fragment> fragmentsList;
        private readonly String[] Tabs = new String[] { "ConcertsView", "MyTicketsView", "WhenHereView", "MoreView" };
        private int lastSelectTab;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //LayoutInflater inflater = LayoutInflater;
            //LayoutInflaterCompat.SetFactory2(inflater,new FontUtils());
            SetContentView(Resource.Layout.Main);
            mContext = this;
            fragmentHistory = new Stack<String>();
            fragmentsList = new Stack<Fragment>();

            tablayout = FindViewById<TabLayout>(Resource.Id.tabs);
            SetupTabLayout();
            SetupTabIcons();
            ShowSectionView(0, null);
            tablayout.GetTabAt(0).Icon.SetColorFilter(Color.Black, PorterDuff.Mode.SrcIn);

            LinearLayout linearLayout = (LinearLayout)tablayout.GetChildAt(0);
            linearLayout.ShowDividers = ShowDividers.Middle;
            GradientDrawable drawable = new GradientDrawable();
            drawable.SetColor(Color.ParseColor("#dcdddf"));
            drawable.SetSize(1, 1);
            linearLayout.DividerPadding = 1;
            linearLayout.SetDividerDrawable(drawable);

            tablayout.TabSelected += (object sender, TabLayout.TabSelectedEventArgs e) =>
            {
              
                if (e.Tab.Position == 1 && !Utility.GetBooleanSharedPreference("isLogin"))
                {  
                    tablayout.GetTabAt(e.Tab.Position).Icon.SetColorFilter(Color.Black, PorterDuff.Mode.SrcIn);
                    Intent intent = new Intent(mContext, typeof(LoginActivity));
                    StartActivity(intent);
                    return;
                }
                else
                {
                    ShowSectionView(e.Tab.Position, null);
                    tablayout.GetTabAt(e.Tab.Position).Icon.SetColorFilter(Color.Black, PorterDuff.Mode.SrcIn);
                }
                lastSelectTab = e.Tab.Position;
            };
            tablayout.TabUnselected += (object sender, TabLayout.TabUnselectedEventArgs e) =>
            {
                tablayout.GetTabAt(e.Tab.Position).Icon.SetColorFilter(Color.White, PorterDuff.Mode.SrcIn);
            };
        }
        private void SetupTabLayout()
        {
            tablayout.AddTab(tablayout.NewTab().SetText("Concerts"));
            tablayout.AddTab(tablayout.NewTab().SetText("My Tickets"));
            tablayout.AddTab(tablayout.NewTab().SetText("When You'r Here"));
            tablayout.AddTab(tablayout.NewTab().SetText("More"));
        }
        private int[] tabIcons = {
            Resource.Drawable.tab_concerts,
            Resource.Drawable.tab_my_tickets,
            Resource.Drawable.tab_when,
            Resource.Drawable.tab_more

        };
        private void SetupTabIcons()
        {
            tablayout.GetTabAt(0).SetIcon(tabIcons[0]);
            tablayout.GetTabAt(1).SetIcon(tabIcons[1]);
            tablayout.GetTabAt(2).SetIcon(tabIcons[2]);
            tablayout.GetTabAt(3).SetIcon(tabIcons[3]);
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        public void ShowSectionView(int id, Bundle bundle = null)
        {
            Fragment fragment = null;
            switch (id)
            {
                case 0:
                    fragment = new ConcertsView();
                    break;
                case 1:
                    fragment = new MyTicketsView();
                    break;
                case 2:
                    fragment = new WhenHereView();
                    break;
                case 3:
                    fragment = new MoreView();
                    break;
                default:
                    return;
            }

            if (!fragmentHistory.Contains(Tabs[id]))
            {
                fragmentHistory.Push(Tabs[id]);
                fragmentsList.Push(fragment);
            }
            ShowSection(fragment: fragment, bundle: bundle);
        }

        public void ShowSection(Fragment fragment, Bundle bundle = null)
        {
           
             fragment.Arguments = bundle;
            var transaction = FragmentManager.BeginTransaction();
            transaction.Replace(Resource.Id.Main_FragmentContainer, fragment);
            //transaction.AddToBackStack(null);
            transaction.Commit();
          
        }

        public override void OnBackPressed()
        {
            try
            {
                if (fragmentHistory.Count <= 1)
                {
                    base.OnBackPressed();
                }
                else
                {
                    fragmentHistory.Pop();
                    fragmentsList.Pop();
                    if (fragmentsList.Count > 1)
                    {
                        FragmentManager.BeginTransaction().Replace(Resource.Id.Main_FragmentContainer, fragmentsList.Peek()).Commit();
                        for (int i = 0; i < Tabs.Length; i++)
                        {
                            if (string.Equals(fragmentHistory.Peek(), Tabs[i], StringComparison.OrdinalIgnoreCase))
                            {
                                UpdateTabSelection(i);
                            }
                        }
                    }
                    else
                    {
                        FragmentManager.BeginTransaction().Replace(Resource.Id.Main_FragmentContainer, new ConcertsView()).Commit();
                        UpdateTabSelection(0);
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error : " + e.Message);
            }
        }

        public void UpdateTabSelection(int currentTab)
        {
            for (int i = 0; i < Tabs.Length; i++)
            {
                if (currentTab == i)
                {
                    tablayout.GetTabAt(i).Icon.SetColorFilter(Color.Black, PorterDuff.Mode.SrcIn);
                    tablayout.GetTabAt(i).Select();

                }
                else
                {
                    tablayout.GetTabAt(i).Icon.SetColorFilter(Color.White, PorterDuff.Mode.SrcIn);
                }
            }
        }
        public void showFragment(int pos)
        {
         tablayout.GetTabAt(pos).Select();
        }
    }
}
