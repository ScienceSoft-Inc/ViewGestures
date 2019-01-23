using ScnViewGestures.Plugin.Forms;
using Xamarin.Forms;

namespace SampleGesture.Views
{
    public class MainPage : ContentPage
	{
		public MainPage()
		{
			#region Gesture 1

			var titleTap = new Label
			{
				Text = "Press label:"
			};

			var pressLabel = new Label
			{
				Text = "Tap me",
				FontSize = 30
			};

			var tapViewGestures = new ViewGestures
			{
				BackgroundColor = Color.Transparent,
				Content = pressLabel,
				AnimationEffect = ViewGestures.AnimationType.atScaling,
				AnimationScale = -5,
				HorizontalOptions = LayoutOptions.Center
			};
			tapViewGestures.Tap += (s, e) => DisplayAlert("Tap", "Gesture finished", "OK");
		    tapViewGestures.LongTap += (s, e) => DisplayAlert("Long Tap", "Gesture finished", "OK");

            #endregion

            #region Gesture 2

            var titleSwipe = new Label
			{
				Text = "Swipe or touch this box:"
			};

			var boxSwipe = new BoxView
			{
				BackgroundColor = Color.Yellow,
				WidthRequest = 150,
				HeightRequest = 150,
				InputTransparent = true
			};

			var swipeViewGestures = new ViewGestures
			{
				BackgroundColor = Color.Transparent,
				Content = boxSwipe,
				HorizontalOptions = LayoutOptions.Center,
				AnimationEffect = ViewGestures.AnimationType.atFlashingTap,
				AnimationColor = Color.FromRgba(192, 192, 192, 128),
				AnimationSpeed = 500
			};

			swipeViewGestures.SwipeUp += (s, e) => DisplayAlert("Swipe", "UP", "OK");
			swipeViewGestures.SwipeDown += (s, e) => DisplayAlert("Swipe", "DOWN", "OK");
			swipeViewGestures.SwipeLeft += (s, e) => DisplayAlert("Swipe", "LEFT", "OK");
			swipeViewGestures.SwipeRight += (s, e) => DisplayAlert("Swipe", "RIGHT", "OK");
		    swipeViewGestures.Tap += (s, e) => DisplayAlert("Tap", "Gesture finished", "OK");
		    swipeViewGestures.LongTap += (s, e) => DisplayAlert("Long Tap", "Gesture finished", "OK");

            #endregion

            #region Gesture 3

            var titleDrag = new Label
			{
				Text = "Drag box:"
			};

			var boxDrag = new BoxView
			{
				BackgroundColor = Color.Green,
				WidthRequest = 100,
				HeightRequest = 100,
				InputTransparent = true
			};

			var dragViewGestures = new ViewGestures
			{
				BackgroundColor = Color.Transparent,
				Content = boxDrag,
				HorizontalOptions = LayoutOptions.Center
			};

		    dragViewGestures.TouchBegan += (s, e) => boxDrag.BackgroundColor = Color.Red;
		    dragViewGestures.TouchEnded += (s, e) => boxDrag.BackgroundColor = Color.Green;
			dragViewGestures.Drag += (s, e) =>
			{
				dragViewGestures.TranslationX += e.DistanceX;
				dragViewGestures.TranslationY += e.DistanceY;
			};

            #endregion

            Content = new StackLayout
			{
				Padding = 20,
				Spacing = 20,
				Children =
				{
					titleTap,
					tapViewGestures,

					titleSwipe,
					swipeViewGestures,

					titleDrag,
					dragViewGestures
				}
			};
		}
	}
}