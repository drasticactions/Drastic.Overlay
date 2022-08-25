// <copyright file="DragAndDrop.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Drastic.Overlay
{
    public partial class DragAndDrop : WindowOverlay
    {
        private DropElementOverlay dropElement;
        private bool dragAndDropOverlayPlatformElementsInitialized;

        public DragAndDrop(IWindow window)
            : base(window)
        {
            this.dropElement = new DropElementOverlay();
            this.AddWindowElement(this.dropElement);
        }

        public event EventHandler<DragAndDropOverlayTappedEventArgs>? Drop;

        internal bool IsDragging
        {
            get => this.dropElement.IsDragging;
            set
            {
                this.dropElement.IsDragging = value;
                this.Invalidate();
            }
        }

        private class DropElementOverlay : IWindowOverlayElement
        {
            public bool IsDragging { get; set; }

            // We are not going to use Contains for this.
            // We're gonna set if it's invoked externally.
            public bool Contains(Microsoft.Maui.Graphics.Point point) => false;

            public void Draw(ICanvas canvas, RectF dirtyRect)
            {
                if (!this.IsDragging)
                {
                    return;
                }

                // We're going to fill the screen with a transparent
                // color to show the drag and drop is happening.
                canvas.FillColor = Microsoft.Maui.Graphics.Color.FromRgba(225, 0, 0, 100);
                canvas.FillRectangle(dirtyRect);
            }
        }
    }
}
