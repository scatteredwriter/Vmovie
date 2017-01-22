using Microsoft.WindowsAzure.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
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

        private Model.ToastParam toastparam;
        private Model.ProtocolParam protocolparam;

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

        public SplashPage(Windows.ApplicationModel.Activation.SplashScreen splashscreen, Model.ToastParam toastparam) : this(splashscreen)
        {
            this.toastparam = toastparam;
        }

        public SplashPage(Windows.ApplicationModel.Activation.SplashScreen splashscreen, Model.ProtocolParam protocolparam) : this(splashscreen)
        {
            this.protocolparam = protocolparam;
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
            try
            {
                PushNotificationChannel channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync(Windows.ApplicationModel.Core.CoreApplication.Id);

                NotificationHub hub1 = new NotificationHub("VmovierForWindowsMessagePush", "Endpoint=sb://vmoviermessagepush.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=D2mi37/sK6PMrFc32bmekKw3oT8AJNLkJ+u+rJh0iBA=");
                await hub1.RegisterNativeAsync(channel.Uri);

                if (App.settings.Values.ContainsKey(Resource.APPTheme.is_open_daily_push) && (bool)App.settings.Values[Resource.APPTheme.is_open_daily_push] == true || !App.settings.Values.ContainsKey(Resource.APPTheme.is_open_daily_push))
                {
                    if (!App.settings.Values.ContainsKey(Resource.APPTheme.is_open_daily_push))
                        App.settings.Values[Resource.APPTheme.is_open_daily_push] = true;
                    NotificationHub hub2 = new NotificationHub("VmovierForWindowsDailyPush", "Endpoint=sb://vmoviermessagepush.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=AAZmQxwICokOozFoZ+XpC3l2FYvWWpOY11SdhQFKzgk=");
                    await hub2.RegisterNativeAsync(channel.Uri);
                }
                else if (App.settings.Values.ContainsKey(Resource.APPTheme.is_open_daily_push) && (bool)App.settings.Values[Resource.APPTheme.is_open_daily_push] == false)
                {
                    try
                    {
                        NotificationHub hub = new NotificationHub("VmovierForWindowsDailyPush", "Endpoint=sb://vmoviermessagepush.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=AAZmQxwICokOozFoZ+XpC3l2FYvWWpOY11SdhQFKzgk=");
                        await hub.UnregisterNativeAsync();
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private async Task RegToastBackgroundTask()
        {
            // 判断一下是否允许访问后台任务
            var res = await BackgroundExecutionManager.RequestAccessAsync();
            if (res == BackgroundAccessStatus.Denied || res == BackgroundAccessStatus.Unspecified)
            {
                return;
            }

            Type taskType = typeof(BackgroundTasks.NotificationsBackgroundTask);
            var task = BackgroundTaskRegistration.AllTasks.Values.FirstOrDefault(t => t.Name == taskType.Name);
            if (task == null)
            {
                // 注册后台任务
                BackgroundTaskBuilder bd = new BackgroundTaskBuilder();
                bd.Name = taskType.Name;
                bd.TaskEntryPoint = taskType.FullName;
                // 声明触发器
                ToastNotificationActionTrigger trigger = new ToastNotificationActionTrigger();
                bd.SetTrigger(trigger);
                task = bd.Register();
            }
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
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    await InitNotificationsAsync();
                    await RegToastBackgroundTask();
                }
                catch (Exception)
                {
                    await Task.Delay(10000);
                }
            }
        }

        private async void Title_SB_Completed(object sender, object e)
        {
            Frame rootFrame = new Frame();
            await Task.Delay(1000);
            rootFrame.Navigate(typeof(MainPage), new DrillInNavigationTransitionInfo());
            Window.Current.Content = rootFrame;
            if (toastparam != null)
                switch (toastparam.page)
                {
                    case Resource.APPTheme.view_content_page:
                        {
                            MainPage.mainpage.View_Content(toastparam.postid);
                        }; break;
                    case Resource.APPTheme.login_page:
                        {
                            MainPage.mainpage.Navigate_To_LoginPage();
                        }; break;
                }
            else if (protocolparam != null && protocolparam.postid != null)
                MainPage.mainpage.View_Content(protocolparam.postid);
            Window.Current.SizeChanged -= Current_SizeChanged;
            this.Loaded -= SplashPage_Loaded;
        }
    }
}
