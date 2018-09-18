using System;
namespace LAPhil.Urls
{
   public class LAPhilUrlService
    {
        public string WebDining { get; private set; }
        public string WebChart { get; private set; }
        public string WebAccessibility { get; private set; }
        public string WebMetroBus { get; private set; }
        public string WebRegister { get; private set; }
        public string WebTickets { get; private set; }
        public string WebSupportUs { get; private set; }
        public string WebGettingHere { get; private set; }
        public string WebFoodWine { get; private set; }
        public string WebFAQ { get; private set; }
        public string WebCentennial { get; private set; }
        public string WebMakeAGift { get; private set; }
        public string WebCorporateSponsorship { get; private set; }
        public string WebOtherWaysToSupport { get; private set; }
        public string WebMyAccountDetails { get; private set; }
        public string WebSeatingChart { get; private set; }
        public string WebForgotPassword { get; private set; }
        public string AppStore { get; private set; }
        public string AppTitle { get; private set; }
        public string LAStore { get; private set; }
        public string BowlStore { get; private set; }
        public string MobileOrdering { get; private set; }

        public LAPhilUrlService(
            string webDining,
            string webChart,
            string webAccessibility,
            string webMetroBus,
            string webRegister,
            string webTickets,
            string webSupportUs,
            string webGettingHere,
            string appStore,
            string appTitle,
            string webFoodWine,
            string faq,
            string centennial,
            string makeAGift,
            string corporateSponsorship,
            string otherWaysToSupport,
            string myAccountDetails,
            string webSeatingChart,
            string webForgotPassword,
            string laStore,
            string bowlStore,
            string mobileOrdering
        )
        {
            WebDining = webDining;
            WebChart = webChart;
            WebAccessibility = webAccessibility;
            WebMetroBus = webMetroBus;
            WebRegister = webRegister;
            WebTickets = webTickets;
            WebSupportUs = webSupportUs;
            WebGettingHere = webGettingHere;
            AppStore = appStore;
            AppTitle = appTitle;
            WebFoodWine = webFoodWine;
            WebFAQ = faq;
            WebCentennial = centennial;
            WebMakeAGift = makeAGift;
            WebCorporateSponsorship = corporateSponsorship;
            WebOtherWaysToSupport = otherWaysToSupport;
            WebMyAccountDetails = myAccountDetails;
            WebSeatingChart = webSeatingChart;
            WebForgotPassword = webForgotPassword;
            LAStore = laStore;
            BowlStore = bowlStore;
            MobileOrdering = mobileOrdering;
        }
    }
}
