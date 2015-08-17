using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using ScnViewGestures.Plugin.Forms;

namespace SampleGesture.Views
{
    public class MainPage : ContentPage
    {
        public MainPage()
        {
            #region Gesture 1
            Label titleTap = new Label
            {
                Text = "Press label:",
            };

            Label pressLabel = new Label
            {
                Text = "Tap me",
                FontSize = 30,
            };

            var tapViewGestures = new ViewGestures
            {
                BackgroundColor = Color.Transparent,
                Content = pressLabel,
                DeformationValue = -5,
                HorizontalOptions = LayoutOptions.Center,
            };
            tapViewGestures.Tap += (s, e) => { this.DisplayAlert("Tap", "Gesture finished", "OK"); };
            #endregion

            #region Gesture 2
            Label titleSwipe = new Label
            {
                Text = "Swipe in box:",
            };
            
            BoxView boxSwipe = new BoxView
            {
                BackgroundColor = Color.Yellow,
                WidthRequest = 200,
                HeightRequest = 200,
            };

            var swipeViewGestures = new ViewGestures
            {
                BackgroundColor = Color.Transparent,
                Content = boxSwipe,
                HorizontalOptions = LayoutOptions.Center,
            };
            swipeViewGestures.SwipeUp += (s, e) => { this.DisplayAlert("Swipe", "UP", "OK"); };
            swipeViewGestures.SwipeDown += (s, e) => { this.DisplayAlert("Swipe", "DOWN", "OK"); };
            swipeViewGestures.SwipeLeft += (s, e) => { this.DisplayAlert("Swipe", "LEFT", "OK"); };
            swipeViewGestures.SwipeRight += (s, e) => { this.DisplayAlert("Swipe", "RIGHT", "OK"); };
            #endregion

            #region Gesture 3
            Label titleTouch = new Label
            {
                Text = "Touch box:",
            };

            BoxView boxTouch = new BoxView
            {
                BackgroundColor = Color.Green,
                WidthRequest = 200,
                HeightRequest = 200,
            };

            var touchViewGestures = new ViewGestures
            {
                BackgroundColor = Color.Transparent,
                Content = boxTouch,
                HorizontalOptions = LayoutOptions.Center,
            };
            touchViewGestures.TouchBegan += (s, e) => { boxTouch.BackgroundColor = Color.Red; };
            touchViewGestures.TouchEnded += (s, e) => { boxTouch.BackgroundColor = Color.Green; };
            touchViewGestures.LongTap += (s, e) => { boxTouch.BackgroundColor = Color.Green; };
            #endregion

            Content = new StackLayout
            {
                Padding = new Thickness(20),
                Spacing = 20,
                Children = 
                {
                    titleTap,
                    tapViewGestures,

                    titleSwipe,
                    swipeViewGestures,

                    titleTouch,
                    touchViewGestures
                }
            };
        }
    }
}
