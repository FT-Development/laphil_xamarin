using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Login.Forms
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
	{
        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

		async void OnLogoutButtonClicked (object sender, EventArgs e)
		{
			// App.IsUserLoggedIn = false;
			Navigation.InsertPageBefore (new LoginPage (), this);
			await Navigation.PopAsync ();
		}
	}
}
