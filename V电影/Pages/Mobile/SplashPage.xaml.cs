using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.PushNotifications;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace V电影.Pages.Mobile
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SplashPage : Page
    {
        public SplashPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.NavigationMode == NavigationMode.New)
            {
                ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
                while (ApplicationView.GetForCurrentView().IsFullScreenMode) ;
                background_scale.CenterX = Window.Current.Bounds.Width / 2;
                background_scale.CenterY = Window.Current.Bounds.Height / 2;
                Background_Scale_SB.Begin();
            }
        }

        private async void Background_Scale_SB_Completed(object sender, object e)
        {
            await Task.Delay(200);
            ApplicationView.GetForCurrentView().ExitFullScreenMode();
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.ContentTransitions = new TransitionCollection();
            rootFrame.ContentTransitions.Add(new EdgeUIThemeTransition() { Edge = EdgeTransitionLocation.Right });
            rootFrame.Navigate(typeof(Pages.Mobile.MainPage));
        }
    }
}
