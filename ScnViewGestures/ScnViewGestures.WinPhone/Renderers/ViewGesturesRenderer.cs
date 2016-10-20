using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ScnViewGestures.Plugin.Forms;
using ScnViewGestures.Plugin.Forms.WinPhone.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;
using Color = System.Windows.Media.Color;

[assembly: ExportRenderer(typeof(ViewGestures), typeof(ViewGesturesRenderer))]

namespace ScnViewGestures.Plugin.Forms.WinPhone.Renderers
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
					// note that a background is needed for the gestures to fire
					Background = new SolidColorBrush(Color.FromArgb(
						(byte) e.NewElement.BackgroundColor.A,
						(byte) e.NewElement.BackgroundColor.R,
						(byte) e.NewElement.BackgroundColor.G,
						(byte) e.NewElement.BackgroundColor.B))
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
				winControl.ManipulationStarted -= winControl_ManipulationStarted;
				winControl.ManipulationDelta -= winControl_ManipulationDelta;
				winControl.ManipulationCompleted -= winControl_ManipulationCompleted;
				winControl.Tap -= winControl_Tap;
			}

			if (e.OldElement == null)
			{
				// setup listeners
				winControl.ManipulationStarted += winControl_ManipulationStarted;
				winControl.ManipulationDelta += winControl_ManipulationDelta;
				winControl.ManipulationCompleted += winControl_ManipulationCompleted;
				winControl.Tap += winControl_Tap;
			}
		}

		void winControl_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
		{
			Element.OnTouchBegan(e.ManipulationOrigin.X, e.ManipulationOrigin.Y);
		}

		void winControl_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
		{
			Element.OnTouchEnded();
		}

		void winControl_Tap(object sender, GestureEventArgs e)
		{
			var point = e.GetPosition(sender as Canvas);
			Element.OnTap(point.X, point.Y);
		}

		void winControl_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
		{
			// Left to Right:   +X
			// Right to Left:   -X
			// Top to Bottom:   +Y
			// Bottom to Top:   -Y

			// get the movement both directionally and absolute val
			var delX = e.CumulativeManipulation.Translation.X;
			var absX = Math.Abs(delX);
			var delY = e.CumulativeManipulation.Translation.Y;
			var absY = Math.Abs(delY);

			// calculate how much change will trigger an action based on the control's size
			var upDownMinDelta = ActualHeight*_deltaPercentage;
			var leftRightMinDelta = ActualWidth*_deltaPercentage;
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

			Element.OnDrag((float) e.DeltaManipulation.Translation.X, (float) e.DeltaManipulation.Translation.Y);
		}
	}
}