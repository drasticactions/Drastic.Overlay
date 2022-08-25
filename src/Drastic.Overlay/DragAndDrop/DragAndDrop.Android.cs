// <copyright file="DragAndDrop.Android.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using static Android.Views.View;

namespace Drastic.Overlay
{
    public partial class DragAndDrop
    {
        /// <inheritdoc/>
        public override bool Initialize()
        {
            if (this.dragAndDropOverlayPlatformElementsInitialized)
            {
                return true;
            }

            base.Initialize();

            if (this.GraphicsView == null)
            {
                return false;
            }

            this.GraphicsView.GenericMotion += this.GraphicsView_GenericMotion;
            this.GraphicsView.Drag += this.GraphicsView_Drag;
            return this.dragAndDropOverlayPlatformElementsInitialized = true;
        }

        private void GraphicsView_GenericMotion(object? sender, GenericMotionEventArgs e)
        {
            Console.WriteLine(e.Event);
        }

        private void GraphicsView_Drag(object? sender, Android.Views.View.DragEventArgs e)
        {
        }
    }
}
