using Microsoft.WindowsAzure.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.PushNotifications;
using Windows.UI.Core;
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
        private Model.ToastParam toastparam;
        private Model.ProtocolParam protocolparam;

        public SplashPage()
        {
            this.InitializeComponent();
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

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.NavigationMode == NavigationMode.New)
            {
                ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
                while (ApplicationView.GetForCurrentView().IsFullScreenMode) ;
                background_scale.CenterX = Window.Current.Bounds.Width / 2;
                background_scale.CenterY = Window.Current.Bounds.Height / 2;
                Background_Scale_SB.Begin();
                if (e.Parameter != null && e.Parameter is Model.ToastParam)
                    this.toastparam = e.Parameter as Model.ToastParam;
                else if (e.Parameter != null && e.Parameter is Model.ProtocolParam)
                    this.protocolparam = e.Parameter as Model.ProtocolParam;
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
        }

        private async void Background_Scale_SB_Completed(object sender, object e)
        {
            await Task.Delay(200);
            ApplicationView.GetForCurrentView().ExitFullScreenMode();
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.ContentTransitions = new TransitionCollection();
            rootFrame.ContentTransitions.Add(new EdgeUIThemeTransition() { Edge = EdgeTransitionLocation.Right });
            rootFrame.Navigate(typeof(Pages.Mobile.MainPage));
            if (toastparam != null)
            {
                switch (toastparam.page)
                {
                    case Resource.APPTheme.view_content_page:
                        {
                            Pages.Mobile.MainPage.mainpage.View_Content(toastparam.postid);
                        }; break;
                    case Resource.APPTheme.login_page:
                        {
                            Pages.Mobile.MainPage.mainpage.Navigate_To_LoginPage();
                        }; break;
                }
            }
            else if (protocolparam != null && protocolparam.postid != null)
                Pages.Mobile.MainPage.mainpage.View_Content(protocolparam.postid);
        }
    }
}
