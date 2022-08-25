// <copyright file="BaseOverlay.iOS.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using CoreGraphics;
using UIKit;

namespace Drastic.Overlay
{
    /// <summary>
    /// Base Overlay.
    /// </summary>
    internal abstract partial class BaseOverlay
    {
        private PassthroughView? passthroughView;

        internal PassthroughView? PassthroughView => this.passthroughView;

        /// <inheritdoc/>
        public virtual bool Initialize()
        {
            if (this.IsPlatformViewInitialized)
            {
                return true;
            }

            var platformLayer = this.Window?.GetPlatform(true);
            if (platformLayer is not UIWindow platformWindow)
            {
                return false;
            }

            if (platformWindow?.RootViewController?.View == null)
            {
                return false;
            }

            // Create a passthrough view for holding the canvas and other diagnostics tools.
            this.passthroughView = new PassthroughView(this, platformWindow.RootViewController.View.Frame);
            this.IsPlatformViewInitialized = true;
            return this.IsPlatformViewInitialized;
        }

        public virtual void DeinitializePlatformDependencies()
        {
            this.passthroughView?.RemoveFromSuperview();
            this.passthroughView?.Dispose();
            this.IsPlatformViewInitialized = false;
        }

        private void FrameAction(Foundation.NSObservedChange obj)
        {
            this.HandleUIChange();
            this.Invalidate();
        }
    }

    internal class PassthroughView : UIView
    {
        private IWindowOverlay overlay;

        /// <summary>
        /// Initializes a new instance of the <see cref="PassthroughView"/> class.
        /// </summary>
        /// <param name="overlay">The Window Overlay.</param>
        /// <param name="frame">Base Frame.</param>
        public PassthroughView(IWindowOverlay windowOverlay, CGRect frame)
            : base(frame)
        {
            this.overlay = windowOverlay;
        }

        /// <summary>
        /// Event Handler for handling on touch events on the Passthrough View.
        /// </summary>
        public event EventHandler<CGPoint>? OnTouch;

        /// <inheritdoc/>
        public override bool PointInside(CGPoint point, UIEvent? uievent)
        {
            // If we don't have a UI event, return.
            if (uievent == null)
            {
                return false;
            }

            if (uievent.Type == UIEventType.Hover)
            {
                return false;
            }

            // If we are not pressing down, return.
            if (uievent.Type != UIEventType.Touches)
            {
                return false;
            }

            var disableTouchEvent = false;

            if (this.overlay.DisableUITouchEventPassthrough)
            {
                disableTouchEvent = true;
            }
            else if (this.overlay.EnableDrawableTouchHandling)
            {
                disableTouchEvent = this.overlay.WindowElements.Any(n => n.Contains(new Point(point.X, point.Y)));
            }

            this.OnTouch?.Invoke(this, point);
            return disableTouchEvent;
        }
    }
}
