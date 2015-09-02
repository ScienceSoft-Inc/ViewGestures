using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using ScnViewGestures.Plugin.Forms;
using ScnViewGestures.Plugin.Forms.iOS.Renderers;
using CoreGraphics;

[assembly: ExportRenderer(typeof(ViewGestures), typeof(ViewGesturesRenderer))]

namespace ScnViewGestures.Plugin.Forms.iOS.Renderers
{
    public class ViewGesturesRenderer : ViewRenderer
    {
        // Used for registration with dependency service
        public async static void Init()
        {
            var temp = DateTime.Now;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.View> e)
        {
            base.OnElementChanged(e);
            var viewGesture = (ViewGestures)Element;

            var tapGestureRecognizer = new TapGestureRecognizer(() => viewGesture.OnTap())
            {
                OnTouchesBegan = (() => viewGesture.OnTouchBegan()),
                OnTouchesEnded = (() => viewGesture.OnTouchEnded()),
            };

            var longPressGestureRecognizer = new LongPressGestureRecognizer(() => viewGesture.OnLongTap())
            {
                OnTouchesBegan = (() => viewGesture.OnTouchBegan()),
                OnTouchesEnded = (() => viewGesture.OnTouchEnded()),
            };

            var swipeLeftGestureRecognizer = new SwipeGestureRecognizer(() => viewGesture.OnSwipeLeft())
            {
                Direction = UISwipeGestureRecognizerDirection.Left,

                OnTouchesBegan = (() => viewGesture.OnTouchBegan()),
                OnTouchesEnded = (() => viewGesture.OnTouchEnded()),
            };

            var swipeRightGestureRecognizer = new SwipeGestureRecognizer(() => viewGesture.OnSwipeRight())
            {
                Direction = UISwipeGestureRecognizerDirection.Right,

                OnTouchesBegan = (() => viewGesture.OnTouchBegan()),
                OnTouchesEnded = (() => viewGesture.OnTouchEnded()),
            };

            var swipeUpGestureRecognizer = new SwipeGestureRecognizer(() => viewGesture.OnSwipeUp())
            {
                Direction = UISwipeGestureRecognizerDirection.Up,

                OnTouchesBegan = (() => viewGesture.OnTouchBegan()),
                OnTouchesEnded = (() => viewGesture.OnTouchEnded()),
            };

            var swipeDownGestureRecognizer = new SwipeGestureRecognizer(() => viewGesture.OnSwipeDown())
            {
                Direction = UISwipeGestureRecognizerDirection.Down,

                OnTouchesBegan = (() => viewGesture.OnTouchBegan()),
                OnTouchesEnded = (() => viewGesture.OnTouchEnded()),
            };

            var dragGestureRecognizer = new DragGestureRecognizer()
            {
             
                OnTouchesBegan = (() => viewGesture.OnTouchBegan()),
                OnTouchesEnded = (() => viewGesture.OnTouchEnded()),
                OnDrag = ((x, y) => viewGesture.OnDrag(x, y)),
            };

            if (e.NewElement == null)
            {
                if (tapGestureRecognizer != null)
                    this.RemoveGestureRecognizer(tapGestureRecognizer);

                if (longPressGestureRecognizer != null)
                    this.RemoveGestureRecognizer(longPressGestureRecognizer);

                if (swipeLeftGestureRecognizer != null)
                    this.RemoveGestureRecognizer(swipeLeftGestureRecognizer);

                if (swipeRightGestureRecognizer != null)
                    this.RemoveGestureRecognizer(swipeRightGestureRecognizer);
                
                if (swipeUpGestureRecognizer != null)
                    this.RemoveGestureRecognizer(swipeUpGestureRecognizer);
                
                if (swipeDownGestureRecognizer != null)
                    this.RemoveGestureRecognizer(swipeDownGestureRecognizer);

                if (dragGestureRecognizer != null)
                    this.RemoveGestureRecognizer(dragGestureRecognizer);
            }

            if (e.OldElement == null)
            {
                this.AddGestureRecognizer(tapGestureRecognizer);
                this.AddGestureRecognizer(longPressGestureRecognizer);
                this.AddGestureRecognizer(swipeLeftGestureRecognizer);
                this.AddGestureRecognizer(swipeRightGestureRecognizer);
                this.AddGestureRecognizer(swipeUpGestureRecognizer);
                this.AddGestureRecognizer(swipeDownGestureRecognizer);
                this.AddGestureRecognizer(dragGestureRecognizer);
            }
        }

        class TapGestureRecognizer : UITapGestureRecognizer
        {
            public TapGestureRecognizer(Action action)
                : base(action)
            { }

            public override void TouchesBegan(NSSet touches, UIEvent evt)
            {
                base.TouchesBegan(touches, evt);

                if (OnTouchesBegan != null)
                    OnTouchesBegan();
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

                if (OnTouchesEnded != null)
                    OnTouchesEnded();
            }

            public override void TouchesCancelled(NSSet touches, UIEvent evt)
            {
                base.TouchesCancelled(touches, evt);

                if (OnTouchesEnded != null)
                    OnTouchesEnded();
            }

            public Action OnTouchesBegan;
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

                if (OnTouchesBegan != null)
                    OnTouchesBegan();
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

                if (OnTouchesEnded != null)
                    OnTouchesEnded();
            }

            public override void TouchesCancelled(NSSet touches, UIEvent evt)
            {
                base.TouchesCancelled(touches, evt);

                if (OnTouchesEnded != null)
                    OnTouchesEnded();
            }

            public Action OnTouchesBegan;
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

                if (OnTouchesBegan != null)
                    OnTouchesBegan();
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

            public Action OnTouchesBegan;
            public Action OnTouchesEnded;
        }

        class DragGestureRecognizer : UIPanGestureRecognizer
        {
            public override void TouchesBegan(NSSet touches, UIEvent evt)
            {
                base.TouchesBegan(touches, evt);

                if (OnTouchesBegan != null)
                    OnTouchesBegan();
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
                UITouch touch = touches.AnyObject as UITouch;

                if (OnDrag != null && touch != null) 
                {
                    float offsetX = (float)touch.PreviousLocationInView (View).X - (float)touch.LocationInView (View).X;
                    float offsetY = (float)touch.PreviousLocationInView (View).Y - (float)touch.LocationInView (View).Y;
                    OnDrag (-offsetX, -offsetY);
                }
            }

            public override void TouchesCancelled(NSSet touches, UIEvent evt)
            {
                base.TouchesCancelled(touches, evt);

                if (OnTouchesEnded != null)
                    OnTouchesEnded();
            }

            public Action OnTouchesBegan;
            public Action OnTouchesEnded;
            public Action<float, float> OnDrag;
        }
    }
}