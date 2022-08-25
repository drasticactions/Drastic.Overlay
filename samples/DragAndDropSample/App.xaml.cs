using Drastic.Overlay;

namespace DragAndDropSample;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    // You do not need to create a new window to use the overlays,
    // VisualDiagnostics is created on all Windows by default.
    // You can also add them to the generic Window by using "AddOverlay"
    protected override Window CreateWindow(IActivationState activationState) => new TestWindow() { Page = new AppShell() };

    public class TestWindow : Window, IVisualTreeElement
    {
        internal DragAndDrop dragAndDropOverlay;

        public TestWindow()
        {
            this.dragAndDropOverlay = new DragAndDrop(this);
        }

        protected override void OnCreated()
        {
            this.AddOverlay(this.dragAndDropOverlay);
            base.OnCreated();
        }
    }
}
