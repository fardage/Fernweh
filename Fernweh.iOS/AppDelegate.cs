﻿using Foundation;
using Rg.Plugins.Popup;
using UIKit;
using Xamarin;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Fernweh.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
#if ENABLE_TEST_CLOUD
            Calabash.Start();
#endif
            Forms.SetFlags("SwipeView_Experimental");

            Popup.Init();
            Forms.Init();

            App.ScreenWidth = UIScreen.MainScreen.Bounds.Width;
            App.ScreenHeight = UIScreen.MainScreen.Bounds.Height;

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}