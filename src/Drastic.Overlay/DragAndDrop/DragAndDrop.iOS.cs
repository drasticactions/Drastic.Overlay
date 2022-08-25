// <copyright file="DragAndDrop.iOS.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using CoreGraphics;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace Drastic.Overlay
{
    public partial class DragAndDrop
    {
        private DragAndDropView dragAndDropView;

        /// <inheritdoc/>
        public override bool Initialize()
        {
            if (this.dragAndDropOverlayPlatformElementsInitialized)
            {
                return true;
            }

            base.Initialize();

            var nativeLayer = this.Window?.GetPlatform(true);
            if (nativeLayer is not UIWindow nativeWindow)
            {
                return false;
            }

            if (nativeWindow?.RootViewController?.View == null)
            {
                return false;
            }

            // We're going to create a new view.
            // This will handle the "drop" events, and nothing else.
            this.dragAndDropView = new DragAndDropView(this, nativeWindow.RootViewController.View.Frame);
            this.dragAndDropView.UserInteractionEnabled = true;
            nativeWindow?.RootViewController.View.AddSubview(this.dragAndDropView);
            nativeWindow?.RootViewController.View.BringSubviewToFront(this.dragAndDropView);
            return this.dragAndDropOverlayPlatformElementsInitialized = true;
        }

        /// <inheritdoc/>
        public override bool Deinitialize()
        {
            if (this.dragAndDropView != null)
            {
                this.dragAndDropView.RemoveFromSuperview();
                this.dragAndDropView.Dispose();
            }

            return base.Deinitialize();
        }

        private class DragAndDropView : UIView, IUIDropInteractionDelegate
        {
            private DragAndDrop overlay;

            public DragAndDropView(DragAndDrop overlay, CGRect frame)
                : base(frame)
            {
                this.overlay = overlay;
                this.AddInteraction(new UIDropInteraction(this));
            }

            // The following are all of the sessions we can handle.
            [Export("dropInteraction:canHandleSession:")]
            public bool CanHandleSession(UIDropInteraction interaction, IUIDropSession session)
            {
                Console.WriteLine($"CanHandleSession ({interaction}, {session})");

                return session.CanLoadObjects(new Class(typeof(UIImage)));
            }

            [Export("dropInteraction:sessionDidEnter:")]
            public void SessionDidEnter(UIDropInteraction interaction, IUIDropSession session)
            {
                Console.WriteLine($"SessionDidEnter ({interaction}, {session})");
                this.overlay.IsDragging = true;
            }

            [Export("dropInteraction:sessionDidExit:")]
            public void SessionDidExit(UIDropInteraction interaction, IUIDropSession session)
            {
                Console.WriteLine($"SessionDidExit ({interaction}, {session})");
                this.overlay.IsDragging = false;
            }

            [Export("dropInteraction:sessionDidUpdate:")]
            public UIDropProposal SessionDidUpdate(UIDropInteraction interaction, IUIDropSession session)
            {
                Console.WriteLine($"sessionDidUpdate ({interaction}, {session})");
                if (session.LocalDragSession == null && session.CanLoadObjects(typeof(UIImage)))
                {
                    return new UIDropProposal(UIDropOperation.Copy);
                }

                return new UIDropProposal(UIDropOperation.Cancel);
            }

            [Export("dropInteraction:performDrop:")]
            public void PerformDrop(UIDropInteraction interaction, IUIDropSession session)
            {
                Console.WriteLine($"performDrop ({interaction}, {session})");
                session.ProgressIndicatorStyle = UIDropSessionProgressIndicatorStyle.None;

                // TODO: Validate other object types.
                foreach (UIDragItem item in session.Items)
                {
                    if (item.ItemProvider.CanLoadObject(typeof(UIImage)))
                    {
                        item.ItemProvider.LoadObject<UIImage>((img, err) =>
                        {
                            using (NSData imageData = img.AsPNG())
                            {
                                byte[] barray = new byte[imageData.Length];
                                System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, barray, 0, Convert.ToInt32(imageData.Length));
                                this.overlay.Drop?.Invoke(this.overlay, new DragAndDropOverlayTappedEventArgs("test", barray));
                            }
                        });
                    }
                }
            }

            [Export("dropInteraction:concludeDrop:")]
            public void ConcludeDrop(UIDropInteraction interaction, IUIDropSession session)
            {
                Console.WriteLine($"concludeDrop ({interaction}, {session})");
                this.overlay.IsDragging = false;
            }

            public override bool PointInside(CGPoint point, UIEvent? uievent)
            {
                if (uievent != null && (long)uievent.Type == 9)
                {
                    return true;
                }

                return false;
            }
        }
    }
}
