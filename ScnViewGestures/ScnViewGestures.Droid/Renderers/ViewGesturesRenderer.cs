using Android.Content;
using Android.Views;
using ScnViewGestures.Plugin.Forms;
using ScnViewGestures.Plugin.Forms.Droid.Renderers;
using System;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ViewGestures), typeof(ViewGesturesRenderer))]

namespace ScnViewGestures.Plugin.Forms.Droid.Renderers
{
    public class ViewGesturesRenderer : ViewRenderer
    {
        public static void Init()
        {
        }

        private readonly GestureListener _listener;
        private readonly GestureDetector _detector;

        public ViewGesturesRenderer(Context context)
            : base(context)
        {
            _listener = new GestureListener();
            _detector = new GestureDetector(context, _listener);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.View> e)
        {
            base.OnElementChanged(e);

            var viewGestures = (ViewGestures) e.NewElement;

            if (e.NewElement == null)
            {
                GenericMotion -= HandleGenericMotion;
                Touch -= HandleTouch;

                _listener.OnTap = null;
                _listener.OnTouchBegan = null;
                _listener.OnTouchEnded = null;
                _listener.OnLongTap = null;

                _listener.OnSwipeLeft = null;
                _listener.OnSwipeRight = null;
                _listener.OnSwipeUp = null;
                _listener.OnSwipeDown = null;
            }

            if (e.OldElement == null)
            {
                GenericMotion += HandleGenericMotion;
                Touch += HandleTouch;

                _listener.OnTap = (x, y) => viewGestures.OnTap(x, y);
                _listener.OnLongTap = () => viewGestures.OnLongTap();

                _listener.OnTouchBegan = (x, y) => viewGestures.OnTouchBegan(x, y);
                _listener.OnTouchEnded = (x, y) => viewGestures.OnTouchEnded(x, y);

                _listener.OnSwipeLeft = () => viewGestures.OnSwipeLeft();
                _listener.OnSwipeRight = () => viewGestures.OnSwipeRight();
                _listener.OnSwipeUp = () => viewGestures.OnSwipeUp();
                _listener.OnSwipeDown = () => viewGestures.OnSwipeDown();

                _listener.OnDrag = (x, y) => viewGestures.OnDrag(x, y);
            }
        }

        private void HandleTouch(object sender, TouchEventArgs e)
        {
            _detector.OnTouchEvent(e.Event);
        }

        private void HandleGenericMotion(object sender, GenericMotionEventArgs e)
        {
            _detector.OnTouchEvent(e.Event);
        }

        public class GestureListener : GestureDetector.SimpleOnGestureListener
        {
            public GestureListener()
            {
                _tmrCallEnded = new Timer(1000)
                {
                    AutoReset = false
                };

                _tmrCallEnded.Elapsed += (s, e) => Device.BeginInvokeOnMainThread(() => OnTouchEnded?.Invoke(-1, -1));
            }

            //OnTouchEnded doesn't call if longtime press to control or use swipe in scroll
            private readonly Timer _tmrCallEnded;

            public Action<double, double> OnTap;
            public Action OnLongTap;

            public Action<double, double> OnTouchBegan;
            public Action<double, double> OnTouchEnded;

            public Action OnSwipeLeft;
            public Action OnSwipeRight;
            public Action OnSwipeUp;
            public Action OnSwipeDown;

            public Action<double, double> OnDrag;

            public override bool OnDown(MotionEvent e)
            {
#if DEBUG
                Console.WriteLine("OnDown");
#endif

                OnTouchBegan?.Invoke(e.GetX() / (3 - e.XPrecision), e.GetY() / (3 - e.YPrecision));

                return base.OnDown(e);
            }

            public override void OnShowPress(MotionEvent e)
            {
#if DEBUG
                Console.WriteLine("OnShowPress");
#endif

                OnTouchBegan?.Invoke(e.GetX() / (3 - e.XPrecision), e.GetY() / (3 - e.YPrecision));

                base.OnShowPress(e);
            }

            public override void OnLongPress(MotionEvent e)
            {
#if DEBUG
                Console.WriteLine("OnLongPress");
#endif

                OnLongTap?.Invoke();

                base.OnLongPress(e);
            }

            public override bool OnDoubleTap(MotionEvent e)
            {
#if DEBUG
                Console.WriteLine("OnDoubleTap");
#endif

                return base.OnDoubleTap(e);
            }

            public override bool OnDoubleTapEvent(MotionEvent e)
            {
#if DEBUG
                Console.WriteLine("OnDoubleTapEvent");
#endif

                return base.OnDoubleTapEvent(e);
            }

            public override bool OnSingleTapUp(MotionEvent e)
            {
#if DEBUG
                Console.WriteLine("OnSingleTapUp");
#endif

                OnTap?.Invoke(e.GetX() / (3 - e.XPrecision), e.GetY() / (3 - e.YPrecision));

                return base.OnSingleTapUp(e);
            }

            public override bool OnSingleTapConfirmed(MotionEvent e)
            {
#if DEBUG
                Console.WriteLine("OnSingleTapConfirmed");
#endif

                OnTouchEnded?.Invoke(e.GetX() / (3 - e.XPrecision), e.GetY() / (3 - e.YPrecision));

                return base.OnSingleTapConfirmed(e);
            }

            private const int SwipeThreshold = 100;
            private const int SwipeVelocityThreshold = 100;
            private const int TapThreshold = 20;

            public override bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
            {
#if DEBUG
                Console.WriteLine("OnFling");
#endif

                var diffY = e2.GetY() - e1.GetY();
                var diffX = e2.GetX() - e1.GetX();

                if (Math.Abs(diffX) < TapThreshold && Math.Abs(diffX) < TapThreshold)
                {
                    OnTap?.Invoke(e2.GetX() / (3 - e2.XPrecision), e2.GetY() / (3 - e2.YPrecision));
                }
                else if (Math.Abs(diffX) > Math.Abs(diffY))
                {
                    if (Math.Abs(diffX) > SwipeThreshold && Math.Abs(velocityX) > SwipeVelocityThreshold)
                    {
                        if (diffX > 0)
                        {
                            OnSwipeRight?.Invoke();
                        }
                        else
                        {
                            OnSwipeLeft?.Invoke();
                        }
                    }
                }
                else if (Math.Abs(diffY) > SwipeThreshold && Math.Abs(velocityY) > SwipeVelocityThreshold)
                {
                    if (diffY > 0)
                    {
                        OnSwipeDown?.Invoke();
                    }
                    else
                    {
                        OnSwipeUp?.Invoke();
                    }
                }

                OnTouchEnded?.Invoke(e2.GetX() / (3 - e2.XPrecision), e2.GetY() / (3 - e2.YPrecision));

                return true;
            }

            private const int ScrollThreshold = 2;

            public override bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
            {
#if DEBUG
                Console.WriteLine("OnScroll");
#endif

                if (OnDrag != null)
                {
                    var diffX = e2.GetX() - e1.GetX();
                    var diffY = e2.GetY() - e1.GetY();

                    //restart timer
                    _tmrCallEnded.Stop();
                    _tmrCallEnded.Start();

                    OnDrag(diffX / ScrollThreshold, diffY / ScrollThreshold);
                }
                else
                    OnTouchEnded?.Invoke(e2.GetX() / (3 - e2.XPrecision), e2.GetY() / (3 - e2.YPrecision));

                return base.OnScroll(e1, e2, distanceX, distanceY);
            }
        }
    }
}