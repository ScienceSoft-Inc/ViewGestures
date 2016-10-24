using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using ScnViewGestures.Plugin.Forms;
using ScnViewGestures.Plugin.Forms.WinRT.Renderers;
using Xamarin.Forms.Platform.WinRT;

[assembly: ExportRenderer(typeof(ViewGestures), typeof(ViewGesturesRenderer))]

namespace ScnViewGestures.Plugin.Forms.WinRT.Renderers
{
	public class ViewGesturesRenderer : ViewRenderer<ViewGestures, Canvas>
	{
		readonly double _deltaPercentage = .25;

		public static void Init() { }

		protected override void OnElementChanged(ElementChangedEventArgs<ViewGestures> e)
		{
			base.OnElementChanged(e);

			Canvas winControl;

			if (Control == null)
			{
				winControl = new Canvas
				{
					ManipulationMode = ManipulationModes.All,
					Background = new SolidColorBrush(Windows.UI.Color.FromArgb(
						(byte)e.NewElement.BackgroundColor.A,
						(byte)e.NewElement.BackgroundColor.R,
						(byte)e.NewElement.BackgroundColor.G,
						(byte)e.NewElement.BackgroundColor.B))
				};

				// the content view can have 1 child - move it under our Canvas element
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
				winControl.ManipulationDelta -= WinControl_ManipulationDelta;
				winControl.ManipulationCompleted -= WinControl_ManipulationCompleted;
				winControl.Tapped -= WinControl_Tapped;
			}

			if (e.OldElement == null)
			{
				// setup listeners
				winControl.PointerPressed += WinControl_PointerPressed;
				winControl.ManipulationDelta += WinControl_ManipulationDelta;
				winControl.ManipulationCompleted += WinControl_ManipulationCompleted;
				winControl.Tapped += WinControl_Tapped;
			}
		}

		private void WinControl_PointerPressed(object sender, PointerRoutedEventArgs e)
		{
			var point = e.GetCurrentPoint(sender as Canvas);
			Element.OnTouchBegan(point.Position.X, point.Position.Y);
		}

		private void WinControl_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
		{
			Element.OnTouchEnded();
		}

		private void WinControl_Tapped(object sender, TappedRoutedEventArgs e)
		{
			var point = e.GetPosition(sender as Canvas);
			Element.OnTap(point.X, point.Y);
		}

		//void winControl_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
		private void WinControl_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
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

			if (isNeedComplete) e.Complete();

			Element.OnDrag((float)e.Delta.Translation.X, (float)e.Delta.Translation.Y);
		}
	}
}