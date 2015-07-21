ScnViewGestures
======================
Xamarin.Forms gestures plugin for view controls (targeted at Android, iOS and Windows Phone)

Description of plugin
===========================================
You can add gestures for any Xamarin.Forms view control. For that need to initialise property "Content" in ViewGestures yours control. After that you can add event for necessary gestures.
Support gestures:
- TouchBegan;
- TouchEnded;
- Tap;
- SwipeLeft;
- SwipeRight;
- SwipeUp;
- SwipeDown.
Other gestures in development.

How to use this plugin in Xamarin.Forms app
===========================================
Look sample to know how right include plugin in your application.

If you want to use gestures plugin for view control then need to add initialize renderers for each platform.

In iOS project just use
```cs
Xamarin.Forms.Forms.Init ();
ViewGesturesRenderer.Init();
```
In Android project just use
```cs
Xamarin.Forms.Forms.Init (this, bundle);
ViewGesturesRenderer.Init();
```
In WinPhone project just use
```cs
Xamarin.Forms.Forms.Init ();
ViewGesturesRenderer.Init();
```