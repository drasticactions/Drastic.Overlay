// <copyright file="DragAndDrop.Windows.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Drastic.Overlay
{
    public partial class DragAndDrop
    {
        private Microsoft.UI.Xaml.Controls.Panel? panel;

        public override bool Initialize()
        {
            if (this.dragAndDropOverlayPlatformElementsInitialized)
            {
                return true;
            }

            base.Initialize();

            var nativeElement = this.Window.Content.GetPlatform(true);
            if (nativeElement == null)
            {
                return false;
            }

            var handler = this.Window.Handler as Microsoft.Maui.Handlers.WindowHandler;
            if (handler?.PlatformView is not Microsoft.UI.Xaml.Window window)
            {
                return false;
            }

            this.panel = window.Content as Microsoft.UI.Xaml.Controls.Panel;
            if (this.panel is null)
            {
                return false;
            }

            this.panel.AllowDrop = true;
            this.panel.DragOver += this.PanelDragOver;
            this.panel.Drop += this.PanelDrop;
            this.panel.DragLeave += this.PanelDragLeave;
            this.panel.DropCompleted += this.PanelDropCompleted;
            return this.dragAndDropOverlayPlatformElementsInitialized = true;
        }

        public override bool Deinitialize()
        {
            if (this.panel != null)
            {
                this.panel.AllowDrop = false;
                this.panel.DragOver -= this.PanelDragOver;
                this.panel.Drop -= this.PanelDrop;
                this.panel.DragLeave -= this.PanelDragLeave;
                this.panel.DropCompleted -= this.PanelDropCompleted;
            }

            return base.Deinitialize();
        }

        private void PanelDropCompleted(Microsoft.UI.Xaml.UIElement sender, Microsoft.UI.Xaml.DropCompletedEventArgs args)
        {
            this.IsDragging = false;
        }

        private void PanelDragLeave(object sender, Microsoft.UI.Xaml.DragEventArgs e)
        {
            this.IsDragging = false;
        }

        private async void PanelDrop(object sender, Microsoft.UI.Xaml.DragEventArgs e)
        {
            // We're gonna cheat and only take the first item dragged in by the user.
            // In the real world, you would probably want to handle multiple drops and figure
            // Out what to do for your app.
            if (e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                var items = await e.DataView.GetStorageItemsAsync();
                if (items.Any())
                {
                    var item = items.First() as StorageFile;
                    if (item != null)
                    {
                        // Take the random access stream and turn it into a byte array.
                        var bits = await item.OpenAsync(FileAccessMode.Read);
                        var reader = new DataReader(bits.GetInputStreamAt(0));
                        var bytes = new byte[bits.Size];
                        await reader.LoadAsync((uint)bits.Size);
                        reader.ReadBytes(bytes);
                        this.Drop?.Invoke(this, new DragAndDropOverlayTappedEventArgs(item.Name, bytes));
                    }
                }
            }

            this.IsDragging = false;
        }

        private void PanelDragOver(object sender, Microsoft.UI.Xaml.DragEventArgs e)
        {
            // For this, we're going to allow "copy"
            // As I want to drag an image into the panel.
            e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Copy;
            this.IsDragging = true;
        }
    }
}
