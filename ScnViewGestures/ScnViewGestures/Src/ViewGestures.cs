using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ScnViewGestures.Plugin.Forms
{
    public class ViewGestures : ContentView
    {
		private View _mainContent;
		private Color _animationColor = Color.Transparent;
		private BoxView _animateFlashBox;
		private static readonly object TapLocker = new object();
		private bool _isTaped;
		private EventHandler<PositionEventArgs> _tap;
		private EventHandler<DragEventArgs> _drag;
		private EventHandler _longTap;
		private EventHandler _swipeLeft;
		private EventHandler _swipeRight;
		private EventHandler _swipeUp;
		private EventHandler _swipeDown;

		public ViewGestures()
        {
            TouchBegan += PressBeganEffect;
            TouchBegan += PressEndedEffectWithDelay;

            TouchEnded += PressEndedEffect;

            BackgroundColor = Color.Transparent;
        }

        public ViewGestures(View content) : this()
        {
            Content = content;
        }

		[Flags]
        public enum GestureType
        {
            gtNone = 0,
            gtTap = 1,
            gtLongTap = 2,
            gtDrag = 4,
            gtSwipeLeft = 8,
            gtSwipeRight = 16,
            gtSwipeUp = 32,
            gtSwipeDown = 64,

            gtSwipeHorizontal = gtSwipeLeft | gtSwipeRight,
            gtSwipeVertical = gtSwipeUp | gtSwipeDown,
            gtSwipe = gtSwipeHorizontal | gtSwipeVertical
        };

        public GestureType SupportGestures;

		public new View Content
        {
            get { return base.Content; }
            set 
            {
                _mainContent = value;
                
                var contentLayout = new AbsoluteLayout();

                AbsoluteLayout.SetLayoutFlags(value, AbsoluteLayoutFlags.All);
                AbsoluteLayout.SetLayoutBounds(value, new Rectangle(0f, 0f, 1f, 1f));
                contentLayout.Children.Add(value);
                
                _animateFlashBox = new BoxView 
                { 
                    BackgroundColor = _animationColor,
					InputTransparent = true
                };

                AbsoluteLayout.SetLayoutFlags(_animateFlashBox, AbsoluteLayoutFlags.HeightProportional | AbsoluteLayoutFlags.XProportional);
                _animateFlashBox.WidthRequest = 1;
                AbsoluteLayout.SetLayoutBounds(_animateFlashBox, new Rectangle(0.5f, 0f, AbsoluteLayout.AutoSize, 1f));
                contentLayout.Children.Add(_animateFlashBox);
                _animateFlashBox.IsVisible = false;

                //this box absorbs gesture action for elements are included in ViewGesture
                var boxAbsorbent = new BoxView
                {
	                BackgroundColor = Color.Transparent,
					InputTransparent = true
				};

                AbsoluteLayout.SetLayoutFlags(boxAbsorbent, AbsoluteLayoutFlags.All);
                AbsoluteLayout.SetLayoutBounds(boxAbsorbent, new Rectangle(0f, 0f, 1f, 1f));
                contentLayout.Children.Add(boxAbsorbent);

                base.Content = contentLayout;
            }
        }

        #region Animation

        public enum AnimationType
        {
            atNone = 0,
            atScaling = 1,
            atFlashing = 2,
            atFlashingTap = 3,
            atFading = 4
        }

        public AnimationType AnimationEffect = AnimationType.atNone;

        public Color AnimationColor 
        { 
            get { return _animationColor; }
            set
            {
                _animationColor = value;
                if (_animateFlashBox != null)
                    _animateFlashBox.BackgroundColor = _animationColor;
            } 
        }

	    public double AnimationScale { get; set; } = 0.0;

        public uint AnimationSpeed { get; set; } = 100;

	    #endregion

        #region TagProperty

	    public static readonly BindableProperty TagProperty = BindableProperty.Create("Tag", typeof(string), typeof(ViewGestures));

		public string Tag
        {
            get { return (string)GetValue(TagProperty); }
            set { SetValue(TagProperty, value); }
        }

        public static string GetTagByChild(object sender)
        {
	        if (!(sender is Element))
	        {
		        return null;
	        }

	        var parentElement = ((Element) sender).Parent;

	        while (parentElement != null && !(parentElement is ViewGestures))
	        {
		        parentElement = parentElement.Parent;
	        }

	        return (parentElement as ViewGestures)?.Tag;
        }

        #endregion

        #region Main gesture

        public event EventHandler<PositionEventArgs> TouchBegan;

        public void OnTouchBegan(double positionX, double positionY)
        {
	        TouchBegan?.Invoke(Content, new PositionEventArgs(positionX, positionY));
        }

        public event EventHandler Touch;

        public void OnTouch(GestureType gestureType)
        {
			Touch?.Invoke(Content, EventArgs.Empty);
		}

        public event EventHandler<PositionEventArgs> TouchEnded;

        public void OnTouchEnded(double positionX, double positionY)
        {
			TouchEnded?.Invoke(Content, new PositionEventArgs(positionX, positionY));
        }

        #endregion

        #region Tap gesture

		public event EventHandler<PositionEventArgs> Tap
        {
            add
            {
                _tap += value;
                SupportGestures |= GestureType.gtTap;
            }
            remove
            {
                _tap -= value;
                if (_tap == null)
                    SupportGestures ^= GestureType.gtTap;
            }
        }

        public void OnTap(double positionX, double positionY)
        {
            lock (TapLocker) 
                if (!_isTaped)
                {
                    _isTaped = true;

	                _tap?.Invoke(Content, new PositionEventArgs(positionX, positionY));
	                OnTouch(GestureType.gtTap);
                }
        }

        #endregion

        #region LongTap gesture

        public event EventHandler LongTap
        {
            add
            {
                _longTap += value;
                SupportGestures |= GestureType.gtLongTap;
            }
            remove
            {
                _longTap -= value;
                if (_longTap == null)
                    SupportGestures ^= GestureType.gtLongTap;
            }
        }

        public void OnLongTap()
        {
	        _longTap?.Invoke(Content, EventArgs.Empty);
	        OnTouch(GestureType.gtLongTap);
        }

        #endregion

        #region Swipe gesture

        public event EventHandler SwipeLeft
		{
            add
            {
                _swipeLeft += value;
                SupportGestures |= GestureType.gtSwipeLeft;
            }
            remove
            {
                _swipeLeft -= value;
                if (_swipeLeft == null)
                    SupportGestures ^= GestureType.gtSwipeLeft;
            }
        }

	    public bool OnSwipeLeft()
	    {
		    if (_swipeLeft != null)
		    {
			    _swipeLeft(Content, EventArgs.Empty);
			    OnTouch(GestureType.gtSwipeLeft);
			    return true;
		    }

		    OnTouch(GestureType.gtSwipe);
		    return false;
	    }

        public event EventHandler SwipeRight
        {
            add
            {
                _swipeRight += value;
                SupportGestures |= GestureType.gtSwipeRight;
            }
            remove
            {
                _swipeRight -= value;
                if (_swipeRight == null)
                    SupportGestures ^= GestureType.gtSwipeRight;
            }
        }

	    public bool OnSwipeRight()
	    {
		    if (_swipeRight != null)
		    {
			    _swipeRight(Content, EventArgs.Empty);
			    OnTouch(GestureType.gtSwipeRight);
			    return true;
		    }

		    OnTouch(GestureType.gtSwipe);
		    return false;
	    }

        public event EventHandler SwipeUp
        {
            add
            {
                _swipeUp += value;
                SupportGestures |= GestureType.gtSwipeUp;
            }
            remove
            {
                _swipeUp -= value;
                if (_swipeUp == null)
                    SupportGestures ^= GestureType.gtSwipeUp;
            }
        }

	    public bool OnSwipeUp()
	    {
		    if (_swipeUp != null)
		    {
			    _swipeUp(Content, EventArgs.Empty);
			    OnTouch(GestureType.gtSwipeUp);
			    return true;
		    }

		    OnTouch(GestureType.gtSwipe);
		    return false;
	    }

        public event EventHandler SwipeDown
        {
            add
            {
                _swipeDown += value;
                SupportGestures |= GestureType.gtSwipeDown;
            }
            remove
            {
                _swipeDown -= value;
                if (_swipeDown == null)
                    SupportGestures ^= GestureType.gtSwipeDown;
            }
        }

	    public bool OnSwipeDown()
	    {
		    if (_swipeDown != null)
		    {
			    _swipeDown(Content, EventArgs.Empty);
			    OnTouch(GestureType.gtSwipeDown);
			    return true;
		    }

		    OnTouch(GestureType.gtSwipe);
		    return false;
	    }

	    #endregion

        #region Drag

        public event EventHandler<DragEventArgs> Drag
        {
            add
            {
                _drag += value;
                SupportGestures |= GestureType.gtDrag;
            }
            remove
            {
                _drag -= value;
                if (_drag == null)
                    SupportGestures ^= GestureType.gtDrag;
            }
        }

        public void OnDrag(double distanceX, double distanceY)
        {
	        _drag?.Invoke(Content, new DragEventArgs(distanceX, distanceY));

	        OnTouch(GestureType.gtDrag);
        }

        #endregion

	    private async void PressBeganEffect(object sender, PositionEventArgs e)
	    {
		    lock (TapLocker)
			    _isTaped = false;

		    if (AnimationEffect == AnimationType.atScaling)
		    {
			    await this.ScaleTo(1 + (AnimationScale/100), AnimationSpeed, Easing.CubicOut);
		    }
		    else if (AnimationEffect == AnimationType.atFlashingTap && !_animateFlashBox.IsVisible)
		    {
			    _animateFlashBox.TranslationX = e.PositionX - _mainContent.Width/2;
			    _animateFlashBox.IsVisible = true;
			    _animateFlashBox.Animate(
				    "AnimateFlashBox",
				    t => t*_mainContent.Width,
				    x =>
				    {
					    _animateFlashBox.WidthRequest = x;

					    var delta = _mainContent.Width/2 - Math.Abs(_animateFlashBox.TranslationX) - x/2;
					    if (delta < 0)
					    {
						    if (_animateFlashBox.TranslationX < 0)
							    _animateFlashBox.TranslationX -= delta;
						    else
							    _animateFlashBox.TranslationX += delta;
					    }
				    },
				    16, AnimationSpeed, Easing.SinOut,
				    (x, y) =>
				    {
					    _animateFlashBox.WidthRequest = 1;
					    _animateFlashBox.IsVisible = false;
				    });
		    }
		    else if (AnimationEffect == AnimationType.atFlashing && !_animateFlashBox.IsVisible)
		    {
			    _animateFlashBox.TranslationX = 0;
			    _animateFlashBox.IsVisible = true;
			    _animateFlashBox.Animate(
				    "AnimateFlashBox",
				    t => t*_mainContent.Width,
				    x => _animateFlashBox.WidthRequest = x,
				    16, AnimationSpeed, Easing.SinOut,
				    (x, y) =>
				    {
					    _animateFlashBox.WidthRequest = 1;
					    _animateFlashBox.IsVisible = false;
				    });
		    }
	    }

	    private async void PressEndedEffect(object sender, EventArgs e)
        {
            if (AnimationEffect == AnimationType.atScaling)
                await this.ScaleTo(1, AnimationSpeed, Easing.CubicOut);
        }

	    private async void PressEndedEffectWithDelay(object sender, EventArgs e)
        {
            if (_drag != null)
                return;

            //OnTouchEnded isn't called if view is included in scroll panel
            await Task.Delay(2000);
            OnTouchEnded(-1, -1);
        }
    }

    public class DragEventArgs : EventArgs
    {
        public readonly double DistanceX;
        public readonly double DistanceY;

        public DragEventArgs(double distanceX, double distanceY)
        {
            DistanceX = distanceX;
            DistanceY = distanceY;
        }
    }

    public class PositionEventArgs : EventArgs
    {
        public readonly double PositionX;
        public readonly double PositionY;

        public PositionEventArgs(double positionX, double positionY)
        {
            PositionX = positionX;
            PositionY = positionY;
        }
    }
}