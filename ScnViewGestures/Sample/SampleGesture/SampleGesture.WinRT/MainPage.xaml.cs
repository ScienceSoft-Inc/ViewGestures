using ScnViewGestures.Plugin.Forms.WinRT.Renderers;

namespace SampleGesture.WinRT
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