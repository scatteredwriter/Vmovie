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
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace V电影.Pages.Share
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SettingPage : Page
    {
        public SettingPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.NavigationMode == NavigationMode.New)
            {
                First_Step();
                daily_push.Toggled += Switch_Toggled;
            }
        }

        private void First_Step()
        {
            //每日推送开关初始化
            if (App.settings.Values.ContainsKey(Resource.APPTheme.is_open_daily_push))
                daily_push.IsOn = (bool)App.settings.Values[Resource.APPTheme.is_open_daily_push];
            else
                App.settings.Values[Resource.APPTheme.is_open_daily_push] = daily_push.IsOn = true;
        }

        private async void Switch_Toggled(object sender, RoutedEventArgs e)
        {
            switch ((sender as ToggleSwitch).Name)
            {
                case "daily_push":
                    {
                        NotificationHub hub = new NotificationHub("VmovierForWindowsDailyPush", "Endpoint=sb://vmoviermessagepush.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=AAZmQxwICokOozFoZ+XpC3l2FYvWWpOY11SdhQFKzgk=");
                        App.settings.Values[Resource.APPTheme.is_open_daily_push] = (sender as ToggleSwitch).IsOn;
                        if (App.settings.Values.ContainsKey(Resource.APPTheme.is_open_daily_push) && (bool)App.settings.Values[Resource.APPTheme.is_open_daily_push] == false)
                        {
                            try
                            {
                                await hub.UnregisterNativeAsync();
                            }
                            catch (Exception)
                            {
                                App.settings.Values[Resource.APPTheme.is_open_daily_push] = (sender as ToggleSwitch).IsOn = !((sender as ToggleSwitch).IsOn);
                            }
                        }
                        else if (App.settings.Values.ContainsKey(Resource.APPTheme.is_open_daily_push) && (bool)App.settings.Values[Resource.APPTheme.is_open_daily_push] == true)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                try
                                {
                                    PushNotificationChannel channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync(Windows.ApplicationModel.Core.CoreApplication.Id);

                                    if (!App.settings.Values.ContainsKey(Resource.APPTheme.is_open_daily_push))
                                        App.settings.Values[Resource.APPTheme.is_open_daily_push] = true;
                                    await hub.RegisterNativeAsync(channel.Uri);
                                }
                                catch (Exception)
                                {
                                    await Task.Delay(10000);
                                }
                            }
                        }
                    }; break;
            }
        }

        private void back_but_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
                if (App.DeviceInfo.Device_type == Model.DeviceType.Mobile)
                {
                    Pages.Mobile.MainPage.mainpage.Open_Pane();
                }
                else
                {
                    MainPage.mainpage.Open_Pane();
                }
            }
        }
    }
}
