// <copyright file="PlatformExtensions.iOS.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;

namespace Drastic.Overlay
{
    internal static class PlatformExtensions
    {
        internal static UIView? GetPlatform(this IElement view, bool returnWrappedIfPresent)
        {
            if (view.Handler is IPlatformViewHandler nativeHandler && nativeHandler.PlatformView != null)
            {
                return nativeHandler.PlatformView;
            }

            return view.Handler?.PlatformView as UIView;
        }
    }
}
