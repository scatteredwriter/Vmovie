using Microsoft.WindowsAzure.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.PushNotifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace V电影.Pages.PC
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SplashPage : Page
    {
        private Windows.ApplicationModel.Activation.SplashScreen splashscreen;
        private Rect splashImageRect;

        public SplashPage()
        {
            this.InitializeComponent();
            Window.Current.SizeChanged += Current_SizeChanged;
            this.Loaded += SplashPage_Loaded;
        }

        public SplashPage(Windows.ApplicationModel.Activation.SplashScreen splashscreen) : this()
        {
            this.splashscreen = splashscreen;
            this.splashImageRect = splashscreen.ImageLocation;
        }

        private void PositionImage()
        {
            background.SetValue(Canvas.LeftProperty, splashImageRect.Left);
            background.SetValue(Canvas.TopProperty, splashImageRect.Top);
            background.Height = splashImageRect.Height;
            background.Width = splashImageRect.Width;
        }

        private async Task InitNotificationsAsync()
        {
            var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();

            var hub = new NotificationHub("VmovierNotificationHub", "Endpoint=sb://vmovierpush.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=lwvMUJDfBKjBB3CILySyCpXnnh7BOFZM/oXkQ3Bb2RA=");
            await hub.RegisterNativeAsync(channel.Uri);
        }

        private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            splashImageRect = splashscreen.ImageLocation;
            PositionImage();
        }

        private async void SplashPage_Loaded(object sender, RoutedEventArgs e)
        {
            PositionImage();
            await Task.Delay(1000);
            Title_SB.Begin();
            await InitNotificationsAsync();
        }

        private async void Title_SB_Completed(object sender, object e)
        {
            Frame rootFrame = new Frame();
            await Task.Delay(1000);
            rootFrame.Navigate(typeof(MainPage), new DrillInNavigationTransitionInfo());
            Window.Current.Content = rootFrame;
            Window.Current.SizeChanged -= Current_SizeChanged;
            this.Loaded -= SplashPage_Loaded;
        }
    }
}
