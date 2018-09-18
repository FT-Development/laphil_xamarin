
using Android.App;
using Android.OS;
using Android.Widget;
using Android.Support.V7.App;
using Android.Content.PM;
using Android.Content;

namespace LAPhil.Droid
{
    [Activity(Label = "SignupSecondView", MainLauncher = false, Icon = "@mipmap/icon", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    class SignupSecondView : AppCompatActivity
    {
    private Context mContext;
    private ImageView btnBack, btnCross;
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
    }

}
}
