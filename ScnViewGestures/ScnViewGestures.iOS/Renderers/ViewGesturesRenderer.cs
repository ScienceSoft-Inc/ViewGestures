using Foundation;
using ScnViewGestures.Plugin.Forms;
using ScnViewGestures.Plugin.Forms.iOS.Renderers;
using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ViewGestures), typeof(ViewGesturesRenderer))]

namespace ScnViewGestures.Plugin.Forms.iOS.Renderers
{
    public class ViewGesturesRenderer : ViewRenderer
    {
        // Used for registration with dependency service
        public static async void Init()
        {
            var temp = DateTime.Now;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            var viewGesture = (ViewGestures) e.NewElement;

            var tapGestureRecognizer = new TapGestureRecognizer
            {
                OnTouchesBegan = (x, y) => viewGesture.OnTouchBegan(x, y),
                OnTouchesEnded = () => viewGesture.OnTouchEnded(),
                OnTap = (x, y) => viewGesture.OnTap(x, y)
            };

            var longPressGestureRecognizer = new LongPressGestureRecognizer(() => viewGesture.OnLongTap())
            {
                OnTouchesBegan = (x, y) => viewGesture.OnTouchBegan(x, y),
                OnTouchesEnded = () => viewGesture.OnTouchEnded()
            };
            
            #region SwipeGestureRecognizer
            var swipeLeftGestureRecognizer = new SwipeGestureRecognizer(() => viewGesture.OnSwipeLeft())
            {
                Direction = UISwipeGestureRecognizerDirection.Left,

                OnTouchesBegan = (x, y) => viewGesture.OnTouchBegan(x, y),
                OnTouchesEnded = () => viewGesture.OnTouchEnded()
            };

            var swipeRightGestureRecognizer = new SwipeGestureRecognizer(() => viewGesture.OnSwipeRight())
            {
                Direction = UISwipeGestureRecognizerDirection.Right,

                OnTouchesBegan = (x, y) => viewGesture.OnTouchBegan(x, y),
                OnTouchesEnded = () => viewGesture.OnTouchEnded()
            };

            var swipeUpGestureRecognizer = new SwipeGestureRecognizer(() => viewGesture.OnSwipeUp())
            {
                Direction = UISwipeGestureRecognizerDirection.Up,

                OnTouchesBegan = (x, y) => viewGesture.OnTouchBegan(x, y),
                OnTouchesEnded = () => viewGesture.OnTouchEnded()
            };

            var swipeDownGestureRecognizer = new SwipeGestureRecognizer(() => viewGesture.OnSwipeDown())
            {
                Direction = UISwipeGestureRecognizerDirection.Down,

                OnTouchesBegan = (x, y) => viewGesture.OnTouchBegan(x, y),
                OnTouchesEnded = () => viewGesture.OnTouchEnded()
            };
            #endregion

            #region DragGestureRecognizer
            var dragGestureRecognizer = new DragGestureRecognizer
            {

                OnTouchesBegan = (x, y) => viewGesture.OnTouchBegan(x, y),
                OnTouchesEnded = () => viewGesture.OnTouchEnded(),
                OnDrag = (x, y) => viewGesture.OnDrag(x, y)
            };

            if (viewGesture != null)
            {
                //from iOS Developer Library (Gesture Recognizers)
                //...For your view to recognize both swipes and pans, you want the swipe gesture recognizer to analyze the touch event before the pan gesture recognizer does...
                if ((viewGesture.SupportGestures & ViewGestures.GestureType.gtSwipeLeft) != 0)
                    dragGestureRecognizer.RequireGestureRecognizerToFail(swipeLeftGestureRecognizer);
                if ((viewGesture.SupportGestures & ViewGestures.GestureType.gtSwipeRight) != 0)
                    dragGestureRecognizer.RequireGestureRecognizerToFail(swipeRightGestureRecognizer);
                if ((viewGesture.SupportGestures & ViewGestures.GestureType.gtSwipeUp) != 0)
                    dragGestureRecognizer.RequireGestureRecognizerToFail(swipeUpGestureRecognizer);
                if ((viewGesture.SupportGestures & ViewGestures.GestureType.gtSwipeDown) != 0)
                    dragGestureRecognizer.RequireGestureRecognizerToFail(swipeDownGestureRecognizer);
            }

            #endregion

            if (e.NewElement == null)
            {
                if (tapGestureRecognizer != null)
                    RemoveGestureRecognizer(tapGestureRecognizer);

                if (longPressGestureRecognizer != null)
                    RemoveGestureRecognizer(longPressGestureRecognizer);

                if (swipeLeftGestureRecognizer != null)
                    RemoveGestureRecognizer(swipeLeftGestureRecognizer);

                if (swipeRightGestureRecognizer != null)
                    RemoveGestureRecognizer(swipeRightGestureRecognizer);
                
                if (swipeUpGestureRecognizer != null)
                    RemoveGestureRecognizer(swipeUpGestureRecognizer);
                
                if (swipeDownGestureRecognizer != null)
                    RemoveGestureRecognizer(swipeDownGestureRecognizer);

                if (dragGestureRecognizer != null)
                    RemoveGestureRecognizer(dragGestureRecognizer);
            }

            if (e.OldElement == null)
            {
                AddGestureRecognizer(tapGestureRecognizer);
                AddGestureRecognizer(longPressGestureRecognizer);
                AddGestureRecognizer(swipeLeftGestureRecognizer);
                AddGestureRecognizer(swipeRightGestureRecognizer);
                AddGestureRecognizer(swipeUpGestureRecognizer);
                AddGestureRecognizer(swipeDownGestureRecognizer);
                AddGestureRecognizer(dragGestureRecognizer);
            }
        }

        class TapGestureRecognizer : UITapGestureRecognizer
        {
            public override void TouchesBegan(NSSet touches, UIEvent evt)
            {
                base.TouchesBegan(touches, evt);
                var touch = touches.AnyObject as UITouch;
                
                double positionX = -1;
                double positionY = -1;

                if (OnTap != null && touch != null)
                {
                    positionX = touch.PreviousLocationInView(View).X;
                    positionY = touch.PreviousLocationInView(View).Y;
                    OnTap(positionX, positionY);
                }

                if (OnTouchesBegan != null)
                    OnTouchesBegan(positionX, positionY);
            }

            public override void TouchesEnded(NSSet touches, UIEvent evt)
            {
                base.TouchesEnded(touches, evt);

                if (OnTouchesEnded != null)
                    OnTouchesEnded();
            }

            /*public override void TouchesMoved(NSSet touches, UIEvent evt)
            {
                base.TouchesMoved(touches, evt);

                if (OnTouchesEnded != null)
                    OnTouchesEnded();
            }*/

            public override void TouchesCancelled(NSSet touches, UIEvent evt)
            {
                base.TouchesCancelled(touches, evt);

                if (OnTouchesEnded != null)
                    OnTouchesEnded();
            }

            public Action<double, double> OnTap;
            public Action<double, double> OnTouchesBegan;
            public Action OnTouchesEnded;
        }

        class LongPressGestureRecognizer : UILongPressGestureRecognizer
        {
            public LongPressGestureRecognizer(Action action)
                : base(action)
            { }

            public override void TouchesBegan(NSSet touches, UIEvent evt)
            {
                base.TouchesBegan(touches, evt);

                var touch = touches.AnyObject as UITouch;

                double positionX = -1;
                double positionY = -1;

                if (touch != null)
                {
                    positionX = touch.PreviousLocationInView(View).X;
                    positionY = touch.PreviousLocationInView(View).Y;
                }

                if (OnTouchesBegan != null)
                    OnTouchesBegan(positionX, positionY);
            }

            public override void TouchesEnded(NSSet touches, UIEvent evt)
            {
                base.TouchesEnded(touches, evt);

                if (OnTouchesEnded != null)
                    OnTouchesEnded();
            }

            /*public override void TouchesMoved(NSSet touches, UIEvent evt)
            {
                base.TouchesMoved(touches, evt);

                if (OnTouchesEnded != null)
                    OnTouchesEnded();
            }*/

            public override void TouchesCancelled(NSSet touches, UIEvent evt)
            {
                base.TouchesCancelled(touches, evt);

                if (OnTouchesEnded != null)
                    OnTouchesEnded();
            }

            public Action<double, double> OnTouchesBegan;
            public Action OnTouchesEnded;
        }

        class SwipeGestureRecognizer : UISwipeGestureRecognizer
        {
            public SwipeGestureRecognizer(Action action)
                : base(action)
            { }

            public override void TouchesBegan(NSSet touches, UIEvent evt)
            {
                base.TouchesBegan(touches, evt);

                var touch = touches.AnyObject as UITouch;

                double positionX = -1;
                double positionY = -1;

                if (touch != null)
                {
                    positionX = touch.PreviousLocationInView(View).X;
                    positionY = touch.PreviousLocationInView(View).Y;
                }

                if (OnTouchesBegan != null)
                    OnTouchesBegan(positionX, positionY);
            }

            public override void TouchesEnded(NSSet touches, UIEvent evt)
            {
                base.TouchesEnded(touches, evt);

                if (OnTouchesEnded != null)
                    OnTouchesEnded();
            }

            public override void TouchesCancelled(NSSet touches, UIEvent evt)
            {
                base.TouchesCancelled(touches, evt);

                if (OnTouchesEnded != null)
                    OnTouchesEnded();
            }

            public Action<double, double> OnTouchesBegan;
            public Action OnTouchesEnded;
        }

        class DragGestureRecognizer : UIPanGestureRecognizer
        {
            public override void TouchesBegan(NSSet touches, UIEvent evt)
            {
                base.TouchesBegan(touches, evt);

                var touch = touches.AnyObject as UITouch;

                double positionX = -1;
                double positionY = -1;

                if (touch != null)
                {
                    positionX = touch.PreviousLocationInView(View).X;
                    positionY = touch.PreviousLocationInView(View).Y;
                }

                if (OnTouchesBegan != null)
                    OnTouchesBegan(positionX, positionY);
            }

            public override void TouchesEnded(NSSet touches, UIEvent evt)
            {
                base.TouchesEnded(touches, evt);

                if (OnTouchesEnded != null)
                    OnTouchesEnded();
            }

            public override void TouchesMoved(NSSet touches, UIEvent evt)
            {
                base.TouchesMoved(touches, evt);
                var touch = touches.AnyObject as UITouch;

                if (OnDrag != null && touch != null) 
                {
                    var offsetX = touch.PreviousLocationInView(View).X - (double)touch.LocationInView(View).X;
                    var offsetY = touch.PreviousLocationInView(View).Y - (double)touch.LocationInView(View).Y;
                    OnDrag (-offsetX, -offsetY);
                }
            }

            public override void TouchesCancelled(NSSet touches, UIEvent evt)
            {
                base.TouchesCancelled(touches, evt);

                if (OnTouchesEnded != null)
                    OnTouchesEnded();
            }

            public Action<double, double> OnTouchesBegan;
            public Action OnTouchesEnded;
            public Action<double, double> OnDrag;
        }
    }
}