using Drastic.Overlay;
using static DragAndDropSample.App;

namespace DragAndDropSample;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ((TestWindow)this.GetParentWindow()).dragAndDropOverlay.Drop += DragAndDropOverlay_Drop;
    }

    private void DragAndDropOverlay_Drop(object sender, DragAndDropOverlayTappedEventArgs e)
    {
        if (e == null)
            return;

        this.Dispatcher.Dispatch(() => {
            this.DropFilename.Text = e.Filename;
            this.DropImage.Source = ImageSource.FromStream(() => new MemoryStream(e.File));
        });
    }
}

