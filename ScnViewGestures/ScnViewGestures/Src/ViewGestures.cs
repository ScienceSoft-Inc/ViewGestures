using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ScnViewGestures.Plugin.Forms
{
    public class ViewGestures : ContentView
    {
        public ViewGestures()
        {
            TouchBegan += Gesture_PressBegan;
            TouchEnded += Gesture_PressEnded;
        }

        [Flags]
        public enum GestureType
        {
            gtNone = 0,
            gtTap = 1,
            gtLongType = 2,
            gtDrag = 4,
            gtSwipeLeft = 8,
            gtSwipeRight = 16,
            gtSwipeUp = 32,
            gtSwipeDown = 64,

            gtSwipeHorizontal = gtSwipeLeft | gtSwipeRight,
            gtSwipeVertical = gtSwipeUp | gtSwipeDown,
            gtSwipe = gtSwipeHorizontal | gtSwipeVertical,
        };

        #region Main gesture
        public event EventHandler TouchBegan;
        public void OnTouchBegan()
        {
            if (TouchBegan != null) 
                TouchBegan(Content, EventArgs.Empty);
        }

        public event EventHandler Touch;
        public void OnTouch(GestureType gestureType)
        {
            if (Touch != null) 
                Touch(Content, EventArgs.Empty);
        }

        public event EventHandler TouchEnded;
        public void OnTouchEnded()
        {
            if (TouchEnded != null) 
                TouchEnded(Content, EventArgs.Empty);
        }
        #endregion

        #region Tap gesture
        public event EventHandler Tap;
        public void OnTap()
        {
            if (Tap != null)
                Tap(Content, EventArgs.Empty);
            OnTouch(GestureType.gtTap);
        }
        #endregion

        #region LongTap gesture
        public event EventHandler LongTap;
        public void OnLongTap()
        {
            if (LongTap != null)
                LongTap(Content, EventArgs.Empty);
            OnTouch(GestureType.gtLongType);
        }
        #endregion

        #region Swipe gesture
        public event EventHandler SwipeLeft;
        public void OnSwipeLeft()
        {
            if (SwipeLeft != null)
            {
                SwipeLeft(Content, EventArgs.Empty);
                OnTouch(GestureType.gtSwipeLeft);
            }
            else
                OnTouch(GestureType.gtSwipe);
        }

        public event EventHandler SwipeRight;
        public void OnSwipeRight()
        {
            if (SwipeRight != null)
            {
                SwipeRight(Content, EventArgs.Empty);
                OnTouch(GestureType.gtSwipeRight);
            }
            else
                OnTouch(GestureType.gtSwipe);
        }

        public event EventHandler SwipeUp;
        public void OnSwipeUp()
        {
            if (SwipeUp != null)
            {
                SwipeUp(Content, EventArgs.Empty);
                OnTouch(GestureType.gtSwipeUp);
            }
            else
                OnTouch(GestureType.gtSwipe);
        }

        public event EventHandler SwipeDown;
        public void OnSwipeDown()
        {
            if (SwipeDown != null)
            {
                SwipeDown(Content, EventArgs.Empty);
                OnTouch(GestureType.gtSwipeDown);
            }
            else
                OnTouch(GestureType.gtSwipe);

        }
        #endregion

        private double deformationValue = 0.0;
        public double DeformationValue
        {
            get { return deformationValue; }
            set { deformationValue = value; }
        }

        async void Gesture_PressBegan(object sender, EventArgs e)
        {
            await this.ScaleTo(1 + (DeformationValue / 100), 100, Easing.CubicOut);
        }

        async void Gesture_PressEnded(object sender, EventArgs e)
        {
            await this.ScaleTo(1, 100, Easing.CubicOut);
        }

    }
}
