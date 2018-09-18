#if __IOS__
using System;
using System.Linq;
using UIKit;

namespace LAPhil.Platform
{
    public static class UIStackViewExtensions
    {
        public static nuint ArrangedViewIndex(this UIStackView source, UIView view)
        {
            var subviews = source.ArrangedSubviews;
           
            for (var i = 0; i < subviews.Length; i++)
            {
                if(subviews[i] == view){
                    if (i == subviews.Length - 1)
                        return (nuint) i;

                    return (nuint) i + 1;
                }
            }

            return 0;
        }

        public static void InsertArrangedSubview(this UIStackView source, UIView view, UIView afterView)
        {
            var index = source.ArrangedViewIndex(afterView);
            source.InsertArrangedSubview(view, stackIndex: index);
        }
    }
}

#endif