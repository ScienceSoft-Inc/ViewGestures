using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ScnViewGestures.Plugin.Forms
{
    public class ViewGestures : ContentView
    {
        public ViewGestures()
        {
            TouchBegan += PressBeganEffect;
            TouchBegan += PressEndedEffectWithDelay;

            TouchEnded += PressEndedEffect;
        }

        #region TagProperty
        public static readonly BindableProperty TagProperty =
            BindableProperty.Create<ViewGestures, string>(p => p.Tag, "");

        public string Tag
        {
            get { return (string)GetValue(TagProperty); }
            set { SetValue(TagProperty, value); }
        }
        #endregion

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
        static object tapLocker = new object();
        private bool _isTaped = false;

        public event EventHandler Tap;
        public void OnTap()
        {
            lock (tapLocker) 
                if (!_isTaped)
                {
                    _isTaped = true;

                    if (Tap != null)
                        Tap(Content, EventArgs.Empty);
                    OnTouch(GestureType.gtTap);
                }
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

        #region Drag
        public event EventHandler<DragEventArgs> Drag;
        public void OnDrag(float distanceX, float distanceY)
        {
            if (Drag != null)
                Drag(Content, new DragEventArgs(distanceX, distanceY));
            OnTouch(GestureType.gtDrag);
        }
        #endregion

        private double deformationValue = 0.0;
        public double DeformationValue
        {
            get { return deformationValue; }
            set { deformationValue = value; }
        }

        async void PressBeganEffect(object sender, EventArgs e)
        {
            lock (tapLocker)
                _isTaped = false;

            if (DeformationValue != 0)
                await this.ScaleTo(1 + (DeformationValue / 100), 100, Easing.CubicOut);
        }

        async void PressEndedEffect(object sender, EventArgs e)
        {
            await this.ScaleTo(1, 100, Easing.CubicOut);
        }

        async void PressEndedEffectWithDelay(object sender, EventArgs e)
        {
            await Task.Delay(2000);
            OnTouchEnded();
        }
    }

    public class DragEventArgs : EventArgs
    {
        public readonly float DistanceX;
        public readonly float DistanceY;

        public DragEventArgs(float distanceX, float distanceY)
        {
            DistanceX = distanceX;
            DistanceY = distanceY;
        }
    }
}
