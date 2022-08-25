// <copyright file="BaseOverlay.Android.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Android.App;
using Android.Views;
using Microsoft.Maui.Handlers;

namespace Drastic.Overlay
{
    /// <summary>
    /// Base Overlay.
    /// </summary>
    internal abstract partial class BaseOverlay
    {
        // Yes, we're cheating here!
        // Normally, we hide these, but I'm gonna open them up
        // So I can poke them from the other classes.
        private Activity? platformActivity;
        private ViewGroup? platformLayer;

        /// <summary>
        /// Gets the platform activity.
        /// </summary>
        internal Activity? PlatformActivity => this.platformActivity;

        /// <summary>
        /// Gets the platform layer.
        /// This is the root layer of the application.
        /// </summary>
        internal ViewGroup? PlatformLayer => this.platformLayer;

        /// <inheritdoc/>
        public virtual bool Initialize()
        {
            return this.InternalInitialize();
        }

        /// <summary>
        /// Deinitialize the platform dependencies used to generate the overlay.
        /// </summary>
        public virtual void DeinitializePlatformDependencies()
        {
            if (this.platformActivity?.Window != null)
            {
                this.platformActivity.Window.DecorView.LayoutChange -= this.DecorViewLayoutChange;
            }

            this.IsPlatformViewInitialized = false;
        }

        private bool InternalInitialize()
        {
            if (this.IsPlatformViewInitialized)
            {
                return true;
            }

            if (this.Window == null)
            {
                return false;
            }

            var platformWindow = this.Window?.Content?.GetPlatform(true);
            if (platformWindow == null)
            {
                return false;
            }

            var handler = this.Window?.Handler as WindowHandler;
            if (handler?.MauiContext == null)
            {
                return false;
            }

            var rootManager = handler.MauiContext.GetNavigationRootManager();
            if (rootManager == null)
            {
                return false;
            }

            if (handler.PlatformView is not Activity activity)
            {
                return false;
            }

            this.platformActivity = activity;
            this.platformLayer = rootManager.RootView as ViewGroup;

            if (this.platformLayer?.Context == null)
            {
                return false;
            }

            if (this.platformActivity?.WindowManager?.DefaultDisplay == null)
            {
                return false;
            }

            var measuredHeight = this.platformLayer.MeasuredHeight;

            if (this.platformActivity.Window != null)
            {
                this.platformActivity.Window.DecorView.LayoutChange += this.DecorViewLayoutChange;
            }

            if (this.platformActivity?.Resources?.DisplayMetrics != null)
            {
                this.Density = this.platformActivity.Resources.DisplayMetrics.Density;
            }

            return this.IsPlatformViewInitialized = true;
        }

        private void DecorViewLayoutChange(object? sender, Android.Views.View.LayoutChangeEventArgs e)
        {
            this.HandleUIChange();
            this.Invalidate();
        }
    }
}
