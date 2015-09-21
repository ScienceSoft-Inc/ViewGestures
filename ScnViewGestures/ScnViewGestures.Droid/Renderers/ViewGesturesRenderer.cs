using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using ScnViewGestures.Plugin.Forms;
using ScnViewGestures.Plugin.Forms.Droid.Renderers;
using System.Timers;

[assembly: ExportRenderer(typeof(ViewGestures), typeof(ViewGesturesRenderer))]

namespace ScnViewGestures.Plugin.Forms.Droid.Renderers
{
    public class ViewGesturesRenderer : ViewRenderer
    {
        // Used for registration with dependency service
        public async static void Init()
        {
            var temp = DateTime.Now;
        }

        private readonly GestureListener _listener;
        private readonly GestureDetector _detector;

        public ViewGesturesRenderer()
        {
            _listener = new GestureListener();
            _detector = new GestureDetector(_listener);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.View> e)
        {
            base.OnElementChanged(e);

            var viewGestures = (ViewGestures)Element;

            if (e.NewElement == null)
            {
                this.GenericMotion -= HandleGenericMotion;
                this.Touch -= HandleTouch;

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
                this.GenericMotion += HandleGenericMotion;
                this.Touch += HandleTouch;

                _listener.OnTap = ((x, y) => viewGestures.OnTap(x,y));
                _listener.OnLongTap = (() => viewGestures.OnLongTap());

                _listener.OnTouchBegan = ((x, y) => viewGestures.OnTouchBegan(x, y));
                _listener.OnTouchEnded = (() => viewGestures.OnTouchEnded());

                _listener.OnSwipeLeft = (() => viewGestures.OnSwipeLeft());
                _listener.OnSwipeRight = (() => viewGestures.OnSwipeRight());
                _listener.OnSwipeUp = (() => viewGestures.OnSwipeUp());
                _listener.OnSwipeDown = (() => viewGestures.OnSwipeDown());

                _listener.OnDrag = ((x, y) => viewGestures.OnDrag(x, y));
            }
        }

        void HandleTouch(object sender, TouchEventArgs e)
        {
            _detector.OnTouchEvent(e.Event);
        }

        void HandleGenericMotion(object sender, GenericMotionEventArgs e)
        {
            _detector.OnTouchEvent(e.Event);
        }


        public class GestureListener : GestureDetector.SimpleOnGestureListener
        {
            public GestureListener()
            {
                tmrCallEnded = new Timer(1000);
                tmrCallEnded.AutoReset = false;
                tmrCallEnded.Elapsed += (s, e) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                        {
                            if (OnTouchEnded != null)
                                OnTouchEnded();
                        });
                };
            }
            //OnTouchEnded doesn't call if longtime press to control or use swipe in scroll
            private Timer tmrCallEnded;

            public Action<double, double> OnTap;
            public Action OnLongTap;

            public Action<double, double> OnTouchBegan;
            public Action OnTouchEnded;

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

                if (OnTouchBegan != null)
                    OnTouchBegan(e.GetX() / (3 - e.XPrecision), e.GetY() / (3 - e.YPrecision));

                return base.OnDown(e);
            }

            public override void OnShowPress(MotionEvent e)
            {
                #if DEBUG
                Console.WriteLine("OnShowPress");
                #endif

                if (OnTouchBegan != null)
                    OnTouchBegan(e.GetX() / (3 - e.XPrecision), e.GetY() / (3 - e.YPrecision));

                base.OnShowPress(e);
            }

            public override void OnLongPress(MotionEvent e)
            {
                #if DEBUG
                Console.WriteLine("OnLongPress");
                #endif

                if (OnLongTap != null)
                    OnLongTap();

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

                if (OnTap != null)
                    OnTap(e.GetX() / (3 - e.XPrecision), e.GetY() / (3 - e.YPrecision));

                return base.OnSingleTapUp(e);
            }

            public override bool OnSingleTapConfirmed(MotionEvent e)
            {
                #if DEBUG
                Console.WriteLine("OnSingleTapConfirmed");
                #endif

                if (OnTouchEnded != null)
                    OnTouchEnded();

                return base.OnSingleTapConfirmed(e);
            }

            private static int SWIPE_THRESHOLD = 100;
            private static int SWIPE_VELOCITY_THRESHOLD = 100;
            private static int TAP_THRESHOLD = 20;

            public override bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
            {
                #if DEBUG
                Console.WriteLine("OnFling");
                #endif

                float diffY = e2.GetY() - e1.GetY();
                float diffX = e2.GetX() - e1.GetX();

                if (Math.Abs(diffX) < TAP_THRESHOLD && Math.Abs(diffX) < TAP_THRESHOLD)
                {
                    if (OnTap != null)
                        OnTap(e2.GetX() / (3 - e2.XPrecision), e2.GetY() / (3 - e2.YPrecision));
                }
                else
                if (Math.Abs(diffX) > Math.Abs(diffY))
                {
                    if (Math.Abs(diffX) > SWIPE_THRESHOLD && Math.Abs(velocityX) > SWIPE_VELOCITY_THRESHOLD)
                    {
                        if (diffX > 0)
                        {
                            if (OnSwipeRight != null)
                                OnSwipeRight();
                        }
                        else
                        {
                            if (OnSwipeLeft != null)
                                OnSwipeLeft();
                        }
                    }
                }
                else if (Math.Abs(diffY) > SWIPE_THRESHOLD && Math.Abs(velocityY) > SWIPE_VELOCITY_THRESHOLD)
                {
                    if (diffY > 0)
                    {
                        if (OnSwipeDown != null)
                            OnSwipeDown();
                    }
                    else
                    {
                        if (OnSwipeUp != null)
                            OnSwipeUp();
                    }
                }

                if (OnTouchEnded != null)
                    OnTouchEnded();

                return true;
            }

            private static int SCROLL_THRESHOLD = 2;

            public override bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
            {
                #if DEBUG
                Console.WriteLine("OnScroll");
                #endif

                float diffX = e2.GetX() - e1.GetX();
                float diffY = e2.GetY() - e1.GetY();

                if (OnDrag != null)
                {
                    //restart timer
                    tmrCallEnded.Stop();
                    tmrCallEnded.Start();

                    OnDrag(diffX / SCROLL_THRESHOLD, diffY / SCROLL_THRESHOLD);
                }
                else
                    if (OnTouchEnded != null)
                        OnTouchEnded();

                return base.OnScroll(e1, e2, distanceX, distanceY);
            }
        }
     }
}