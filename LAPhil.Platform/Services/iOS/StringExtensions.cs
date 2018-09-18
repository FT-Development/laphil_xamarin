#if __IOS__
using System;
using System.Text.RegularExpressions;
using UIKit;
using CoreGraphics;
using Foundation;


namespace LAPhil.Platform
{
    public static class NSAttributedStringExtensions
    {
        public static CGRect RectForView(this NSAttributedString source, UIView view)
        {
            var size = new CoreGraphics.CGSize(width: view.Frame.Width, height: nfloat.MaxValue);

            var rect = source.GetBoundingRect(
                size: size,
                options: NSStringDrawingOptions.UsesLineFragmentOrigin,
                context: null
            );

            var newRect = new CoreGraphics.CGRect(
                x: view.Frame.X,
                y: view.Frame.Y,
                width: rect.Width,
                height: Math.Ceiling(rect.Height) + 10
            );

            return newRect;
        }
    }
    public static class StringExtensions
    {

        public static void ConvertToFont(this NSMutableAttributedString source, UIFont font, UIColor color = null, NSParagraphStyle paragraphStyle = null)
        {
            var range = new NSRange(start: 0, len: 0);
            Func<NSRange, nint> MaxRange = (value) => value.Location + value.Length;

            while (MaxRange(range) < source.Length)
            {
                var attributes = source.GetAttributes(MaxRange(range), out range);
                if(attributes.TryGetValue(UIStringAttributeKey.Font, out NSObject value))
                {
                    var oldFont = (UIFont)value;
                    var descriptor = font.FontDescriptor;
                    var newDescriptor = descriptor.CreateWithTraits(oldFont.FontDescriptor.SymbolicTraits);
                    var newFont = UIFont.FromDescriptor(descriptor: newDescriptor, pointSize: font.PointSize);
                    source.AddAttribute(UIStringAttributeKey.Font, value: newFont, range: range);

                    if(color != null)
                    {
                        source.RemoveAttribute(UIStringAttributeKey.ForegroundColor, range: range);
                        source.AddAttribute(UIStringAttributeKey.ForegroundColor, value: color, range: range);
                    }
                }
            }

            if (paragraphStyle != null)
            {
                var fullRange = new NSRange(0, source.Length);
                source.RemoveAttribute(UIStringAttributeKey.ParagraphStyle, range: range);
                source.AddAttribute(UIStringAttributeKey.ParagraphStyle, value: paragraphStyle, range: fullRange);
            }
        }

        public static NSAttributedString HtmlAttributedString(this string source, UIButton matchingButton, UIControlState controlState)
        {
            var normalAttributedString = matchingButton.GetAttributedTitle(controlState);
            var paragraphStyle = (NSParagraphStyle)normalAttributedString.GetAttribute(UIStringAttributeKey.ParagraphStyle, 0, out NSRange effectiveRange);

            var attributedString = HtmlAttributedString(
                source,
                font: matchingButton.TitleLabel.Font,
                color: matchingButton.TitleLabel.TextColor,
                paragraphStyle: paragraphStyle
            );

            return attributedString;
        }
        public static NSAttributedString HtmlAttributedString(this string source, UILabel matchingLabel)
        {
            var paragraphStyle = (NSParagraphStyle) matchingLabel.AttributedText.GetAttribute(UIStringAttributeKey.ParagraphStyle, 0, out NSRange effectiveRange);
            var attributedString = HtmlAttributedString(
                source,
                font: matchingLabel.Font,
                color: matchingLabel.TextColor,
                paragraphStyle: paragraphStyle
            );


            var size = new CoreGraphics.CGSize(width: matchingLabel.Frame.Width, height: nfloat.MaxValue);

            var rect = attributedString.GetBoundingRect(
                size: size,
                options: NSStringDrawingOptions.UsesLineFragmentOrigin,
                context: null
            );

            var newRect = new CoreGraphics.CGRect(
                x: matchingLabel.Frame.X,
                y: matchingLabel.Frame.Y,
                width: rect.Width,
                height: Math.Ceiling(rect.Height)
            );

            // When the label is in a UIStakView it will not grow to fill 
            // the height unless we set this.  
            matchingLabel.PreferredMaxLayoutWidth = newRect.Width;

            return attributedString;
        }

        public static NSAttributedString HtmlAttributedString(this string source, UITextView matchingTextView)
        {
            var paragraphStyle = (NSParagraphStyle)matchingTextView.AttributedText.GetAttribute(UIStringAttributeKey.ParagraphStyle, 0, out NSRange effectiveRange);
            var attributedString = HtmlAttributedString(
                source,
                font: matchingTextView.Font,
                color: matchingTextView.TextColor,
                paragraphStyle: paragraphStyle
            );

            var size = new CoreGraphics.CGSize(width: matchingTextView.Frame.Width, height: nfloat.MaxValue);

            var rect = attributedString.GetBoundingRect(
                size: size,
                options: NSStringDrawingOptions.UsesLineFragmentOrigin,
                context: null
            );

            var newRect = new CoreGraphics.CGRect(
                x: matchingTextView.Frame.X,
                y: matchingTextView.Frame.Y,
                width: rect.Width,
                height: Math.Ceiling(rect.Height) + 10
            );

            // When the label is in a UIStakView it will not grow to fill 
            // the height unless we set this.
            matchingTextView.Frame = newRect;
            //matchingTextView.PreferredMaxLayoutWidth = newRect.Width;
            return attributedString;
        }

        public static NSAttributedString HtmlAttributedString(this string source, UIFont font, UIColor color = null, NSParagraphStyle paragraphStyle = null)
        {
            NSError error = null;
            var attributes = new NSAttributedStringDocumentAttributes
            {
                DocumentType = NSDocumentType.HTML,
                StringEncoding = NSStringEncoding.UTF8
            };

            var html = new NSAttributedString(
                data: NSData.FromString(source ?? "", NSStringEncoding.UTF8),
                documentAttributes: attributes,
                error: ref error
            );

            var mutableHtml = new NSMutableAttributedString(html);
            mutableHtml.ConvertToFont(font: font, color: color, paragraphStyle: paragraphStyle);

            return mutableHtml;
        }

        public static NSAttributedString HtmlAttributedString(this string source)
        {
            NSError error = null;
            var attributes = new NSAttributedStringDocumentAttributes
            {
                DocumentType = NSDocumentType.HTML,
                StringEncoding = NSStringEncoding.UTF8 
            };

            var html = new NSAttributedString(
                data: NSData.FromString(source, NSStringEncoding.UTF8), 
                documentAttributes: attributes, 
                error: ref error
            );

            return html;
        }
    }
}
#endif