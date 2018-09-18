using UIKit;
using Foundation;
using CoreFoundation;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Disposables;
using LAPhil.Application;
using LAPhil.Events;
using LAPhil.Platform;
using Xamarin.Forms;
using System.Collections.Generic;


namespace LAPhil.iOS
{
    [Register("ProgramViewController")]
    public class ProgramViewController : UIViewController, IUICollectionViewDelegateFlowLayout, IUICollectionViewDataSource
    {
        EventService eventService = ServiceContainer.Resolve<EventService>();
        TimeService timeService = ServiceContainer.Resolve<TimeService>();
        public Event Event { get; set; }
        public Event FullEvent { get; private set; }
        const string pieceReuseIdentifier = "pieceReuseIdentifier";
        PieceCollectionViewCell sizingCell;
        CompositeDisposable DisposeBag;
        bool isDisplayAlert = false;

        [Outlet]
        protected NSLayoutConstraint collectionViewHeight { get; set; }

        [Outlet]
        protected UIImageView imgMain { get; set; }

        [Outlet]
        protected UILabel lblTitle { get; set; }

        [Outlet]
        protected UIButton btnBuyNow { get; set; }

        [Outlet]
        protected UICollectionView piecesCollection { get; set; }

        [Outlet]
        protected UIView BuyNowView { get; set; }

        [Outlet]
        protected UIView PiecesView { get; set; }

        [Outlet]
        protected UIStackView Components { get; set; }

        [Outlet]
        protected UIView ExtraMessageView { get; set; }

        [Outlet]
        protected UIButton ExtraMessageButton { get; set; }

        [Outlet]
        protected UIView ProducerNameView { get; set; }

        [Outlet]
        protected UILabel ProducerNameLabel { get; set; }

        [Outlet]
        protected UILabel DateAndTimeLabel { get; set; }

        [Outlet]
        protected UIView DateAndTimeView { get; set; }

        [Outlet]
        protected UIStackView stackViewArtist { get; set; }

        [Outlet]
        protected UILabel lblArtistName { get; set; }

        [Outlet]
        protected UIView viewArtist { get; set; }

        public ProgramViewController(IntPtr handle) : base(handle)
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.ConfigureDefaultBackButton();

            var pieceNib = UINib.FromName("PieceCollectionViewCell", bundleOrNil: null);
            sizingCell = (PieceCollectionViewCell)pieceNib.Instantiate(ownerOrNil: null, optionsOrNil: null)[0];

            piecesCollection.RegisterNibForCell(pieceNib, reuseIdentifier: pieceReuseIdentifier);
            piecesCollection.WeakDelegate = this;
            piecesCollection.WeakDataSource = this;

            var layout = (UICollectionViewFlowLayout) piecesCollection.CollectionViewLayout;
            layout.MinimumLineSpacing = 40.0f;

            // Available Font Names: 
            // ApercuPro-BoldItalic
            // ApercuPro-LightItalic
            // ApercuPro-BlackItalic
            // ApercuPro-Mono
            // ApercuPro-MonoBold
            // ApercuPro-MediumItalic
            // ApercuPro-Black
            // ApercuPro-Medium
            // ApercuPro-Regular
            // ApercuPro-Light
            // ApercuPro-Italic
            // ApercuPro-Bold

            ConfigureView();
            LoadData();
        }

        [Action("OnLearnMoreAboutTheSeries")]
        public void OnLearnMoreAboutTheSeries()
        {
            if (Event.Series != null && string.IsNullOrEmpty(Event.Series.WebUrl) == false)
            {
                Device.OpenUri(new Uri(Event.Series.WebUrl));
            }
        }

        protected virtual void ConfigureView()
        {
            ExtraMessageView.Hidden = true;
            ProducerNameView.Hidden = true;

            if (Event == null || Event.Program == null) {
                return;
            }

            if (Event.Program.Name != null) {
                lblTitle.AttributedText = ($"<b>{Event.Program.Name}</b>")
                .HtmlAttributedString(matchingLabel: lblTitle);    
            } else {
                lblTitle.Text = "";
            }

            if (Event.Program.ExtraMessageUrl != null && 
                Event.Program.ExtraMessageUrl != string.Empty &&
                Event.Program.ExtraMessage != null &&
                Event.Program.ExtraMessage != string.Empty)
            {
                var title = Event.Program.ExtraMessage
                 .HtmlAttributedString(
                     matchingButton: ExtraMessageButton,
                     controlState: UIControlState.Normal
                );
                
                //Components.InsertArrangedSubview(ExtraMessageView, afterView: lblTitle);
                ExtraMessageView.Hidden = false;
                ExtraMessageButton.SetAttributedTitle(title, UIControlState.Normal);
            } else {
                ExtraMessageView.Hidden = true;
                Components.RemoveArrangedSubview(ExtraMessageView);
            }

            if (Event.Program.ProducerName != null &&
                Event.Program.ProducerName != string.Empty
               )
            {
                var title = Event.Program.ProducerName
                 .HtmlAttributedString(
                     matchingLabel: ProducerNameLabel
                );

                //Components.InsertArrangedSubview(ProducerNameView, afterView: BuyNowView);
                ProducerNameView.Hidden = false;
                ProducerNameLabel.AttributedText = title;
            } else {
                ProducerNameView.Hidden = true;
                Components.RemoveArrangedSubview(ProducerNameView);
            }

            if (Event.StartTime != DateTimeOffset.MinValue)
            {
                var localTime = Event.StartTime.ToLocalTime();
                DateAndTimeLabel.Text = localTime.ToString($"ddd / MMM dd - {timeService.TimeFormat}");
            }
            else
            {
                DateAndTimeView.Hidden = true;
                Components.RemoveArrangedSubview(DateAndTimeView);
            }

        }

        protected void LoadData()
        {
            LoadEventDetails();
            LoadEventImage();
        }

        [Action("OnExtraMessage")]
        public void OnExtraMessage()
        {
            if (Event.Program.ExtraMessageUrl != null && Event.Program.ExtraMessageUrl != string.Empty)
            {
                Device.OpenUri(new Uri(Event.Program.ExtraMessageUrl));
            }
        }

        [Action("OnBuyNow")]
        public void OnBuyNow()
        {
            Console.WriteLine("ViewControllerAtIndex index : {0} ", Event.BuyUrl);

            if (Event.BuyUrl == null)
            {
                if (this.isDisplayAlert == false)
                {
                    this.isDisplayAlert = true;
                    //var okAlertController = UIAlertController.Create("LA Phil", "No tickets available.", UIAlertControllerStyle.Alert);
                    var okAlertController = UIAlertController.Create("No events available", "Please select another.", UIAlertControllerStyle.Alert);
                    //Add Actions
                    okAlertController.AddAction(UIAlertAction.Create("Okay", UIAlertActionStyle.Default, alert => {
                        this.isDisplayAlert = false;
                    }));
                    PresentViewController(okAlertController, true, null);
                }
            }
            else if (this.isDisplayAlert == false)
            {
                this.isDisplayAlert = true;
                UIApplication.SharedApplication.OpenUrl(NSUrl.FromString(Event.BuyUrl));

                Dictionary<object, object> parameters = new Dictionary<object, object>();
                parameters.Add("Program", Event.Program.Name);
                Firebase.Analytics.Analytics.LogEvent("BuyNow", parameters);
                
            }
            //return null;
        }

        void LoadEventDetails()
        {
            eventService
                .EventDetail(Event)
                .Subscribe((Event evt) => {

                    DispatchQueue.MainQueue.DispatchAsync(() => OnEventDetail(evt));
                });
        }

        void LoadEventImage()
        {
            var imgConcertRect = this.imgMain.Frame;
            var screenScale = UIScreen.MainScreen.Scale;

            Task.Run(async () =>
            {
                var size = new System.Drawing.Size(
                        width: (int)(imgConcertRect.Width * screenScale),
                        height: (int)(imgConcertRect.Height * screenScale)
                    );

                var bytes = await eventService.GetEventImageBytes(Event, size: size);

                if (bytes == null)
                {
                    DispatchQueue.MainQueue.DispatchAsync(() => {
                        imgMain.Hidden = true;
                    });

                    return;
                }

                var image = UIImage.LoadFromData(
                    NSData.FromArray(bytes)
                );

                DispatchQueue.MainQueue.DispatchAsync(() => {
                    UIView.Transition(
                        withView: imgMain,
                        duration: 0.75,
                        options: UIViewAnimationOptions.TransitionCrossDissolve,
                        animation: () => {
                            imgMain.Hidden = false;
                            imgMain.Image = image;
                            imgMain.ContentMode = UIViewContentMode.ScaleAspectFill;
                            imgMain.ClipsToBounds = true;
                        },
                        completion: null);
                });
            });
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            this.isDisplayAlert = false;
            //if (DisposeBag != null)
            //    DisposeBag.Dispose();
            
            DisposeBag = new CompositeDisposable();

            //if(FullEvent != null)
            //{
            //    var cells = FullEvent.Program.Pieces.Select((value, index) =>
            //    {
            //        return (PieceCollectionViewCell) piecesCollection.CellForItem(NSIndexPath.FromIndex((nuint)index));
            //    });

            //    foreach(var cell in cells)
            //    {
            //        var disposable = cell.Rx.ReadMore.Subscribe(OnReadMore);
            //        DisposeBag.Add(disposable);
            //    }
            //}
        }

        public override void ViewWillDisappear(bool animated)
        {
            //base.ViewWillDisappear(animated);
            //DisposeBag.Dispose();
            //DisposeBag = null;
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
        }

        void OnEventDetail(Event receivedEvent)
        {
            if (FullEvent != null && FullEvent.Equals(receivedEvent))
                return;

            DispatchQueue.MainQueue.DispatchAsync(() => {
                FullEvent = receivedEvent;

                if (FullEvent.Performers != null && FullEvent.Performers.Length > 0)
                {
                    var text = new NSMutableAttributedString(
                            str: "Artists \n",
                            font: UIFont.FromName("ApercuPro-Bold", 20),
                        foregroundColor: UIColor.Black
                        );

                    foreach (var tmpPerFormer in FullEvent.Performers)
                    {
                        if (tmpPerFormer.Name != null && tmpPerFormer.Role != null)
                        {
                            text.Append(new NSMutableAttributedString(
                                str: tmpPerFormer.Name ,
                                font: UIFont.FromName("ApercuPro-Bold", 18),
                                foregroundColor: UIColor.Black  //.FromRGB(red: 26, green: 150, blue: 212)
                                ));

                            text.Append(new NSMutableAttributedString(
                                str: ", " + tmpPerFormer.Role + "\n",
                                font: UIFont.FromName("ApercuPro-Italic", 18),
                                foregroundColor: UIColor.Gray

                                ));
                        }else if (tmpPerFormer.Name != null && tmpPerFormer.Role == null)
                        {
                            text.Append(new NSMutableAttributedString(
                                str: tmpPerFormer.Name + "\n",
                                font: UIFont.FromName("ApercuPro-Regular", 18),
                                foregroundColor: UIColor.Black  //FromRGB(red: 26, green: 150, blue: 212)
                                ));
                        }
                    }
                    lblArtistName.AttributedText = text;    //new NSAttributedString(text, attr, ref nsError);
                    stackViewArtist.AddArrangedSubview(viewArtist);
                }
                else
                {
                    stackViewArtist.Hidden = true;
                }

                var piecesCount = FullEvent?.Pieces.Length ?? 0;

                if(piecesCount == 0)
                {
                    Components.RemoveArrangedSubview(PiecesView);
                    PiecesView.Hidden = true;
                    return;
                }


                piecesCollection.ReloadData();


            });


        }

        protected virtual void ConfigureCell(PieceCollectionViewCell cell, NSIndexPath indexPath)
        {
            var model = FullEvent.Pieces[indexPath.Row];

            cell.Piece = model;
            cell.ResetViews();
            cell.lblPiece_name.Text = $"PIECE {indexPath.Row + 1}";

            cell.TitleValue = model.Name;
            cell.ComposerValue = model.ComposerName;
            cell.DurationValue = model.Duration;
            cell.DescriptionValue = model.Description;

            DisposeBag.Add(cell.Rx.ReadMore.Subscribe(OnReadMore));
            cell.Tag = indexPath.Row;
        }

        void OnReadMore(Piece piece)
        {
            var vc = (PieceDetailViewController)Storyboard.InstantiateViewController("PieceDetailViewController");
            vc.Piece = piece;
            NavigationController.PushViewController(vc, animated: true);
        }

        public nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return FullEvent?.Pieces.Length ?? 0;
        }

        public UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = (PieceCollectionViewCell) collectionView.DequeueReusableCell(
                reuseIdentifier: pieceReuseIdentifier, indexPath: indexPath
            );

            ConfigureCell(cell, indexPath: indexPath);

            collectionViewHeight.Constant = collectionView.CollectionViewLayout.CollectionViewContentSize.Height;
            View.SetNeedsLayout();
            return cell;
        }

        [Export("collectionView:layout:sizeForItemAtIndexPath:")]
        public CoreGraphics.CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
        {
            if (FullEvent == null)
                return CoreGraphics.CGSize.Empty;

            ConfigureCell(sizingCell, indexPath: indexPath);
            var size = sizingCell.ContentView.SystemLayoutSizeFittingSize(UIView.UILayoutFittingCompressedSize);
            return new CoreGraphics.CGSize(collectionView.Frame.Width, size.Height);
        }
    }
}
