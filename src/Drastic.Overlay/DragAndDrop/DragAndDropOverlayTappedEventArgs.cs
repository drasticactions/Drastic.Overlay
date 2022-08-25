// <copyright file="DragAndDropOverlayTappedEventArgs.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Drastic.Overlay
{
    public class DragAndDropOverlayTappedEventArgs : EventArgs
    {
        public DragAndDropOverlayTappedEventArgs(string filename, byte[] file)
        {
            this.Filename = filename;
            this.File = file;
        }

        public string Filename { get; set; }

        public byte[] File { get; }
    }
}
