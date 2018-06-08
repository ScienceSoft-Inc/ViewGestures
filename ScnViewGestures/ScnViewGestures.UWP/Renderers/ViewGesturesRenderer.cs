using ScnViewGestures.Plugin.Forms;
using ScnViewGestures.UWP.Renderers;
using System;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(ViewGestures), typeof(ViewGesturesRenderer))]
namespace ScnViewGestures.UWP.Renderers
{
    public class ViewGesturesRenderer : ViewRenderer<ViewGestures, Canvas>
	{
		private readonly double _deltaPercentage = .25;

		public static void Init() { }

		protected override void OnElementChanged(ElementChangedEventArgs<ViewGestures> e)
		{
			base.OnElementChanged(e);

			Canvas winControl;

			if (Control == null)
			{
				winControl = new Canvas();
				winControl.ManipulationMode = ManipulationModes.All;
				winControl.Background = new SolidColorBrush(Color.FromArgb(
				(byte)e.NewElement.BackgroundColor.A,
					(byte)e.NewElement.BackgroundColor.R,
					(byte)e.NewElement.BackgroundColor.G,
					(byte)e.NewElement.BackgroundColor.B));

				if (Children.Count > 0)
				{
					var child = Children[0];
					Children.Remove(child);
					winControl.Children.Add(child);
				}

				SetNativeControl(winControl);
			}
			else
			{
				winControl = Control;
			}

			if (e.NewElement == null)
			{
				winControl.PointerPressed -= WinControl_PointerPressed;
				winControl.ManipulationDelta -= WinControlOnManipulationDelta;
				winControl.ManipulationCompleted -= WinControlOnManipulationCompleted;
				winControl.Tapped -= WinControlOnTapped;
			}

			if (e.OldElement == null)
			{
				winControl.PointerPressed += WinControl_PointerPressed;
				winControl.ManipulationDelta += WinControlOnManipulationDelta;
				winControl.ManipulationCompleted += WinControlOnManipulationCompleted;
				winControl.Tapped += WinControlOnTapped;
			}
		}

		private void WinControl_PointerPressed(object sender, PointerRoutedEventArgs e)
		{
			var point = e.GetCurrentPoint(sender as Canvas);
			Element.OnTouchBegan(point.Position.X, point.Position.Y);
		}

		void WinControlOnManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
		{
			Element.OnTouchEnded();
		}

		void WinControlOnTapped(object sender, TappedRoutedEventArgs e)
		{
			var point = e.GetPosition(sender as Canvas);
			Element.OnTap(point.X, point.Y);
		}

		void WinControlOnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
		{
			// Left to Right:   +X
			// Right to Left:   -X
			// Top to Bottom:   +Y
			// Bottom to Top:   -Y

			// get the movement both directionally and absolute val
			var delX = e.Cumulative.Translation.X;
			var absX = Math.Abs(delX);
			var delY = e.Cumulative.Translation.Y;
			var absY = Math.Abs(delY);

			// calculate how much change will trigger an action based on the control's size
			var upDownMinDelta = ActualHeight * _deltaPercentage;
			var leftRightMinDelta = ActualWidth * _deltaPercentage;
			var isNeedComplete = false;

			if (absX > absY && absX > leftRightMinDelta)
			{
				isNeedComplete = delX < 0 ? Element.OnSwipeLeft() : Element.OnSwipeRight();
			}
			else if (absY > absX && absY > upDownMinDelta)
			{
				isNeedComplete = delY < 0 ? Element.OnSwipeUp() : Element.OnSwipeDown();
			}

			if(isNeedComplete) e.Complete();

			Element.OnDrag((float)e.Delta.Translation.X, (float)e.Delta.Translation.Y);
		}
	}
}