// <copyright file="BaseOverlay.Windows.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Microsoft.Maui.Handlers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Drastic.Overlay
{
    /// <summary>
    /// Base Overlay.
    /// </summary>
    internal abstract partial class BaseOverlay
    {
        private Microsoft.UI.Xaml.Controls.Frame? frame;
        private Panel? panel;
        private FrameworkElement? platformElement;

        internal Microsoft.UI.Xaml.Controls.Frame? Frame => frame;

        internal Microsoft.UI.Xaml.Controls.Panel? Panel => panel;

        /// <inheritdoc/>
        public virtual bool Initialize()
        {
            return this.InternalInitialize();
        }

        private bool InternalInitialize()
        {
            if (this.IsPlatformViewInitialized)
            {
                return true;
            }

            if (this.Window?.Content == null)
            {
                return false;
            }

            this.platformElement = this.Window.Content.GetPlatform(true);
            if (this.platformElement == null)
            {
                return false;
            }

            var handler = this.Window.Handler as WindowHandler;
            if (handler?.PlatformView is not Microsoft.UI.Xaml.Window _window)
            {
                return false;
            }

            this.panel = _window.Content as Panel;

            this.IsPlatformViewInitialized = true;
            return this.IsPlatformViewInitialized;
        }

        public virtual void DeinitializePlatformDependencies()
        {
            this.IsPlatformViewInitialized = false;
        }
    }
}
