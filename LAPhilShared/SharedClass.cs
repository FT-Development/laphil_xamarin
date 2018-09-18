using System;
using System.Collections.Generic;
using UIKit;

namespace LAPhilShared
{
    public class SharedClass
    {
        public SharedClass()
        {

        }

        //public static string urlsRegister       = "https://lapatester:p@ssw0rd@tickets-staging.laphil.com/account/register";
        //public static string GetUrlsRegister()
        //{
        //    var urlsRegister = "https://tickets-dev.laphil.com/account/register";
        //    return urlsRegister;
        //}

        public static List<string> GetFilterData()
        {
            //List<string> itemData = new List<string>() { "CHOOSE EVENT TYPE(S)", "CHOOSE YOUR DATE(S)" };
            List<string> itemData = new List<string>() { "Choose Event Type(s)", "Choose Your Date(s)" };
            return itemData;
        }

        public static List<string> GetEventTypeData()
        {
            List<string> itemData = new List<string>() { "GUSTAVO DUDAMEL", "LOS ANGELES PHILHARMONIC", "ORCHESTRAL", "CLASSICAL PRESENTATIONS","BROADWAY/STAGE", "FAMILY/SING-ALOG", "FILM", "JAZZ/BLUES", "POP/ROCK/URBAN", "LATIN/WORLD MUSIC", "IN/SIGHT", "LEASE EVENT", "EVENING", "MATINEE" };
            return itemData;
        }


        //public static List<string> GetEvetnTypeData()
        //{
        //    List<string> itemData = new List<string>() { "Orchestral", "Classical Presentations", "Jazz", "Broadway/Stage" , "Family/Sing-Along", "Film" };
        //    return itemData;
        //}

        public static List<string> GetGettingHereData()
        {
            List<string> itemData = new List<string>() { "PARKING", "METERO", "BUS" };
            return itemData;
        }

        public static List<string> GetWhereYourHereData()
        {
            List<string> itemData = new List<string>() { "NEARBY DINING", "SEATING CHART", "ACCESSIBILITY" };
            return itemData;
        }

        public static List<string> GetParkingData()
        {
            List<string> itemData = new List<string>() { "Public Parking" };
            return itemData;
        }



        public static void StartLoaderIndicator(UIImageView loadingIndeicator)
        {
            loadingIndeicator.AnimationImages = getGIFdata();
            loadingIndeicator.AnimationRepeatCount = 0;
            loadingIndeicator.AnimationDuration = 4.0;
            loadingIndeicator.StartAnimating();
        }

        public static void StopLoaderIndicator(UIImageView loadingIndeicator)
        {
            loadingIndeicator.StopAnimating();
        }

        public static UIImage[] getGIFdata()
        {
            var GIFdata = new UIImage[] {
                  UIImage.FromBundle ("GIF/010001")
                , UIImage.FromBundle ("GIF/010002")
                , UIImage.FromBundle ("GIF/010003")
                , UIImage.FromBundle ("GIF/010004")
                , UIImage.FromBundle ("GIF/010005")
                , UIImage.FromBundle ("GIF/010006")
                , UIImage.FromBundle ("GIF/010007")
                , UIImage.FromBundle ("GIF/010008")
                , UIImage.FromBundle ("GIF/010009")
                , UIImage.FromBundle ("GIF/010010")
                , UIImage.FromBundle ("GIF/010011")
                , UIImage.FromBundle ("GIF/010012")
                , UIImage.FromBundle ("GIF/010013")
                , UIImage.FromBundle ("GIF/010014")
                , UIImage.FromBundle ("GIF/010015")
                , UIImage.FromBundle ("GIF/010016")
                , UIImage.FromBundle ("GIF/010017")
                , UIImage.FromBundle ("GIF/010018")
                , UIImage.FromBundle ("GIF/010019")
                , UIImage.FromBundle ("GIF/010020")
                , UIImage.FromBundle ("GIF/010021")
                , UIImage.FromBundle ("GIF/010022")
                , UIImage.FromBundle ("GIF/010023")
                , UIImage.FromBundle ("GIF/010024")
                , UIImage.FromBundle ("GIF/010025")
                , UIImage.FromBundle ("GIF/010026")
                , UIImage.FromBundle ("GIF/010027")
                , UIImage.FromBundle ("GIF/010028")
                , UIImage.FromBundle ("GIF/010029")
                , UIImage.FromBundle ("GIF/010030")
                , UIImage.FromBundle ("GIF/010031")
                , UIImage.FromBundle ("GIF/010032")
                , UIImage.FromBundle ("GIF/010033")
                , UIImage.FromBundle ("GIF/010034")
                , UIImage.FromBundle ("GIF/010035")
                , UIImage.FromBundle ("GIF/010036")
                , UIImage.FromBundle ("GIF/010037")
                , UIImage.FromBundle ("GIF/010038")
                , UIImage.FromBundle ("GIF/010039")
                , UIImage.FromBundle ("GIF/010040")
                , UIImage.FromBundle ("GIF/010041")
                , UIImage.FromBundle ("GIF/010042")
                , UIImage.FromBundle ("GIF/010043")
                , UIImage.FromBundle ("GIF/010043")
                , UIImage.FromBundle ("GIF/010044")
                , UIImage.FromBundle ("GIF/010045")
                , UIImage.FromBundle ("GIF/010046")
                , UIImage.FromBundle ("GIF/010047")
                , UIImage.FromBundle ("GIF/010048")
                , UIImage.FromBundle ("GIF/010049")
                , UIImage.FromBundle ("GIF/010050")
                , UIImage.FromBundle ("GIF/010051")
                , UIImage.FromBundle ("GIF/010052")
                , UIImage.FromBundle ("GIF/010053")
                , UIImage.FromBundle ("GIF/010054")
                , UIImage.FromBundle ("GIF/010055")
                , UIImage.FromBundle ("GIF/010056")
                , UIImage.FromBundle ("GIF/010057")
                , UIImage.FromBundle ("GIF/010058")
                , UIImage.FromBundle ("GIF/010059")
                , UIImage.FromBundle ("GIF/010060")
                , UIImage.FromBundle ("GIF/010061")
                , UIImage.FromBundle ("GIF/010062")
                , UIImage.FromBundle ("GIF/010063")
                , UIImage.FromBundle ("GIF/010064")
                , UIImage.FromBundle ("GIF/010065")
                , UIImage.FromBundle ("GIF/010066")
                , UIImage.FromBundle ("GIF/010067")
                , UIImage.FromBundle ("GIF/010068")
                , UIImage.FromBundle ("GIF/010069")
                , UIImage.FromBundle ("GIF/010070")
                , UIImage.FromBundle ("GIF/010071")
                , UIImage.FromBundle ("GIF/010072")
                , UIImage.FromBundle ("GIF/010073")
                , UIImage.FromBundle ("GIF/010074")
                , UIImage.FromBundle ("GIF/010075")
                , UIImage.FromBundle ("GIF/010076")
                , UIImage.FromBundle ("GIF/010077")
                , UIImage.FromBundle ("GIF/010078")
                , UIImage.FromBundle ("GIF/010079")
                , UIImage.FromBundle ("GIF/010080")
                , UIImage.FromBundle ("GIF/010081")
                , UIImage.FromBundle ("GIF/010082")
                , UIImage.FromBundle ("GIF/010083")
                , UIImage.FromBundle ("GIF/010084")
                , UIImage.FromBundle ("GIF/010085")
                , UIImage.FromBundle ("GIF/010086")
                , UIImage.FromBundle ("GIF/010087")
                , UIImage.FromBundle ("GIF/010088")
                , UIImage.FromBundle ("GIF/010089")
                , UIImage.FromBundle ("GIF/010090")
                , UIImage.FromBundle ("GIF/010091")
                , UIImage.FromBundle ("GIF/010092")
                , UIImage.FromBundle ("GIF/010093")
                , UIImage.FromBundle ("GIF/010094")
                , UIImage.FromBundle ("GIF/010095")
                , UIImage.FromBundle ("GIF/010096")
                , UIImage.FromBundle ("GIF/010097")
                , UIImage.FromBundle ("GIF/010098")
                , UIImage.FromBundle ("GIF/010099")
                , UIImage.FromBundle ("GIF/010100")
                , UIImage.FromBundle ("GIF/010101")
                , UIImage.FromBundle ("GIF/010102")
                , UIImage.FromBundle ("GIF/010103")
                , UIImage.FromBundle ("GIF/010104")
                , UIImage.FromBundle ("GIF/010105")
                , UIImage.FromBundle ("GIF/010106")
                , UIImage.FromBundle ("GIF/010107")
                , UIImage.FromBundle ("GIF/010108")
                , UIImage.FromBundle ("GIF/010109")
                , UIImage.FromBundle ("GIF/010110")
                , UIImage.FromBundle ("GIF/010111")
                , UIImage.FromBundle ("GIF/010112")
                , UIImage.FromBundle ("GIF/010113")
                , UIImage.FromBundle ("GIF/010114")
                , UIImage.FromBundle ("GIF/010115")
                , UIImage.FromBundle ("GIF/010116")
                , UIImage.FromBundle ("GIF/010117")
                , UIImage.FromBundle ("GIF/010118")
                , UIImage.FromBundle ("GIF/010119")
                , UIImage.FromBundle ("GIF/010120")
                , UIImage.FromBundle ("GIF/010121")
                , UIImage.FromBundle ("GIF/010122")
                , UIImage.FromBundle ("GIF/010123")
                , UIImage.FromBundle ("GIF/010124")
                , UIImage.FromBundle ("GIF/010125")
                , UIImage.FromBundle ("GIF/010126")
                , UIImage.FromBundle ("GIF/010127")
                , UIImage.FromBundle ("GIF/010128")
                , UIImage.FromBundle ("GIF/010129")
                , UIImage.FromBundle ("GIF/010130")
                , UIImage.FromBundle ("GIF/010131")
                , UIImage.FromBundle ("GIF/010132")
                , UIImage.FromBundle ("GIF/010133")
                , UIImage.FromBundle ("GIF/010134")
                , UIImage.FromBundle ("GIF/010135")
                , UIImage.FromBundle ("GIF/010136")
                , UIImage.FromBundle ("GIF/010137")
                , UIImage.FromBundle ("GIF/010138")
                , UIImage.FromBundle ("GIF/010139")
                , UIImage.FromBundle ("GIF/010140")
                , UIImage.FromBundle ("GIF/010141")
                , UIImage.FromBundle ("GIF/010142")
                , UIImage.FromBundle ("GIF/010143")
                , UIImage.FromBundle ("GIF/010144")
                , UIImage.FromBundle ("GIF/010145")
                , UIImage.FromBundle ("GIF/010146")
                , UIImage.FromBundle ("GIF/010147")
                , UIImage.FromBundle ("GIF/010148")
                , UIImage.FromBundle ("GIF/010149")
                , UIImage.FromBundle ("GIF/010150")
                , UIImage.FromBundle ("GIF/010151")
                , UIImage.FromBundle ("GIF/010152")
                , UIImage.FromBundle ("GIF/010153")
                , UIImage.FromBundle ("GIF/010154")
                , UIImage.FromBundle ("GIF/010155")
                , UIImage.FromBundle ("GIF/010156")
                , UIImage.FromBundle ("GIF/010157")
                , UIImage.FromBundle ("GIF/010158")
                , UIImage.FromBundle ("GIF/010160")
                , UIImage.FromBundle ("GIF/010161")
                , UIImage.FromBundle ("GIF/010162")
                , UIImage.FromBundle ("GIF/010163")
                , UIImage.FromBundle ("GIF/010164")
                , UIImage.FromBundle ("GIF/010165")
                , UIImage.FromBundle ("GIF/010166")
                , UIImage.FromBundle ("GIF/010167")
                , UIImage.FromBundle ("GIF/010168")
                , UIImage.FromBundle ("GIF/010169")
                , UIImage.FromBundle ("GIF/010171")
                , UIImage.FromBundle ("GIF/010172")
                , UIImage.FromBundle ("GIF/010173")
                , UIImage.FromBundle ("GIF/010174")
                , UIImage.FromBundle ("GIF/010175")
                , UIImage.FromBundle ("GIF/010176")
                , UIImage.FromBundle ("GIF/010177")
                , UIImage.FromBundle ("GIF/010178")
                , UIImage.FromBundle ("GIF/010179")
                , UIImage.FromBundle ("GIF/010181")
                , UIImage.FromBundle ("GIF/010182")
                , UIImage.FromBundle ("GIF/010183")
                , UIImage.FromBundle ("GIF/010184")
                , UIImage.FromBundle ("GIF/010185")
                , UIImage.FromBundle ("GIF/010186")
                , UIImage.FromBundle ("GIF/010187")
                , UIImage.FromBundle ("GIF/010188")
                , UIImage.FromBundle ("GIF/010189")
                , UIImage.FromBundle ("GIF/010190")
                , UIImage.FromBundle ("GIF/010191")
                , UIImage.FromBundle ("GIF/010192")
                , UIImage.FromBundle ("GIF/010193")
                , UIImage.FromBundle ("GIF/010194")
                , UIImage.FromBundle ("GIF/010195")
                , UIImage.FromBundle ("GIF/010196")
                , UIImage.FromBundle ("GIF/010197")
                , UIImage.FromBundle ("GIF/010198")
                , UIImage.FromBundle ("GIF/010199")
                , UIImage.FromBundle ("GIF/010200")
                , UIImage.FromBundle ("GIF/010201")
                , UIImage.FromBundle ("GIF/010202")
                , UIImage.FromBundle ("GIF/010203")
                , UIImage.FromBundle ("GIF/010204")
                , UIImage.FromBundle ("GIF/010205")
                , UIImage.FromBundle ("GIF/010206")
                , UIImage.FromBundle ("GIF/010207")
                , UIImage.FromBundle ("GIF/010208")
                , UIImage.FromBundle ("GIF/010209")
                , UIImage.FromBundle ("GIF/010210")
                , UIImage.FromBundle ("GIF/010211")
                , UIImage.FromBundle ("GIF/010212")
                , UIImage.FromBundle ("GIF/010213")
                , UIImage.FromBundle ("GIF/010214")
                , UIImage.FromBundle ("GIF/010215")
                , UIImage.FromBundle ("GIF/010216")
                , UIImage.FromBundle ("GIF/010217")
                , UIImage.FromBundle ("GIF/010218")
                , UIImage.FromBundle ("GIF/010219")
                , UIImage.FromBundle ("GIF/010220")
                , UIImage.FromBundle ("GIF/010221")
                , UIImage.FromBundle ("GIF/010222")
                , UIImage.FromBundle ("GIF/010223")
                , UIImage.FromBundle ("GIF/010224")
                , UIImage.FromBundle ("GIF/010225")
                , UIImage.FromBundle ("GIF/010226")
                , UIImage.FromBundle ("GIF/010227")
                , UIImage.FromBundle ("GIF/010228")
                , UIImage.FromBundle ("GIF/010229")
                , UIImage.FromBundle ("GIF/010230")
                , UIImage.FromBundle ("GIF/010231")
                , UIImage.FromBundle ("GIF/010232")
                , UIImage.FromBundle ("GIF/010233")
                , UIImage.FromBundle ("GIF/010234")
                , UIImage.FromBundle ("GIF/010235")
                , UIImage.FromBundle ("GIF/010236")
                , UIImage.FromBundle ("GIF/010237")
                , UIImage.FromBundle ("GIF/010238")
            };

            return GIFdata;

        }

    }
}
