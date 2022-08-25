// <copyright file="BaseOverlay.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Drastic.Overlay
{
    /// <summary>
    /// This is a base implementation of IWindowOverlay.
    /// If you know what you're doing, you can always make your
    /// own IWindowOverlay and do what you want without the graphics layer.
    /// </summary>
    internal abstract partial class BaseOverlay : IWindowOverlay
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseOverlay"/> class.
        /// </summary>
        /// <param name="window"></param>
        public BaseOverlay(IWindow window)
        {
            this.Window = window;
        }

        /// <inheritdoc/>
        public event EventHandler<WindowOverlayTappedEventArgs> Tapped;

        /// <inheritdoc/>
        public bool DisableUITouchEventPassthrough { get; set; }

        /// <inheritdoc/>
        public bool EnableDrawableTouchHandling { get; set; }

        /// <inheritdoc/>
        public bool IsVisible { get; set; }

        /// <inheritdoc/>
        public IWindow Window { get; }

        /// <inheritdoc/>
        public float Density { get; set; }

        /// <inheritdoc/>
        public IReadOnlyCollection<IWindowOverlayElement> WindowElements => new List<IWindowOverlayElement>();

        /// <inheritdoc/>
        public bool IsPlatformViewInitialized { get; private set; }

        /// <inheritdoc/>
        public bool AddWindowElement(IWindowOverlayElement element)
        {
            return false;
        }

        /// <inheritdoc/>
        public bool Deinitialize()
        {
            this.DeinitializePlatformDependencies();
            return true;
        }

        /// <inheritdoc/>
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
        }

        /// <inheritdoc/>
        public void HandleUIChange()
        {
        }

        /// <inheritdoc/>
        public void Invalidate()
        {
        }

        /// <inheritdoc/>
        public bool RemoveWindowElement(IWindowOverlayElement element)
        {
            return false;
        }

        /// <inheritdoc/>
        public void RemoveWindowElements()
        {
        }

#if !ANDROID && !IOS && !WINDOWS && !MACCATALYST
        public virtual void DeinitializePlatformDependencies()
        {
        }

        /// <inheritdoc/>
        public bool Initialize()
        {
            return false;
        }
#endif
    }
}
