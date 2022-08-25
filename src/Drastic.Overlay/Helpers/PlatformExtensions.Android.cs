﻿// <copyright file="PlatformExtensions.Android.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Android.App;
using Android.Content;
using Microsoft.Maui.Platform;
using AView = Android.Views.View;

namespace Drastic.Overlay
{
    internal static class PlatformExtensions
    {
        public static NavigationRootManager GetNavigationRootManager(this IMauiContext mauiContext) =>
            mauiContext.Services.GetRequiredService<NavigationRootManager>();

        internal static AView? GetPlatform(this IElement view, bool returnWrappedIfPresent)
        {
            if (view.Handler is IPlatformViewHandler nativeHandler && nativeHandler.PlatformView != null)
            {
                return nativeHandler.PlatformView;
            }

            return view.Handler?.PlatformView as AView;
        }

        internal static List<T> GetChildView<T>(this Android.Views.ViewGroup view)
        {
            var childCount = view.ChildCount;
            var list = new List<T>();
            for (var i = 0; i < childCount; i++)
            {
                var child = view.GetChildAt(i);
                if (child is T tChild)
                {
                    list.Add(tChild);
                }
            }

            return list;
        }
    }
}
