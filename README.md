ScnViewGestures
======================
Xamarin.Forms gestures plugin for view controls (targeted at Android, iOS and Windows Phone).

Description
===========================================
You can add gestures for any Xamarin.Forms view control. For that you need to initialize "Content" property in ViewGestures in your control. After that you can add events for required gestures.
Supported gestures:
- TouchBegan;
- TouchEnded;
- Tap;
- SwipeLeft;
- SwipeRight;
- SwipeUp;
- SwipeDown;
- Drag.

Other gestures are in development.

How to use this plugin in Xamarin.Forms app
===========================================

It's required that you initilize renderers for each platform separately.

iOS:
```cs
Xamarin.Forms.Forms.Init ();
ViewGesturesRenderer.Init();
```
Android:
```cs
Xamarin.Forms.Forms.Init (this, bundle);
ViewGesturesRenderer.Init();
```
WinPhone:
```cs
Xamarin.Forms.Forms.Init ();
ViewGesturesRenderer.Init();
```
