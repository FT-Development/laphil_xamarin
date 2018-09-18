using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using LAPhil.Droid;

namespace SharedLibraryAndroid
{
    [Activity(Theme = "@style/MyTheme.Splash", Icon = "@mipmap/icon", MainLauncher = true, NoHistory = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashActivity : AppCompatActivity
    {
        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
        }
        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() => { SimulateStartup(); });
            startupWork.Start();
        }
        async void SimulateStartup()
        {
            await Task.Delay(1000); // Simulate a bit of startup work.
            if(Utility.GetBooleanSharedPreference("isOldUser"))
            {
                UserModel.Instance.SelectedTab = "Concerts";
                StartActivity(new Intent(Application.ApplicationContext, typeof(ConcertActivity))); 
            }
            else
            { 
                StartActivity(new Intent(Application.ApplicationContext, typeof(WelcomeActivity)));
            }
           
        }

    }
}
