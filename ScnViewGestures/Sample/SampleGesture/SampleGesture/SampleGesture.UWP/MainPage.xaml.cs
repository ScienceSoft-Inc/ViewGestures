using ScnViewGestures.UWP.Renderers;

namespace SampleGesture.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
			ViewGesturesRenderer.Init();
			LoadApplication(new SampleGesture.App());
		}
    }
}