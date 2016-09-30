using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using V电影.Model;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
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
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
            Composition.Animation.Drop_Shadow(login_but_shadow, login_rectangle);
        }

        private void Got_Focus(object sender, RoutedEventArgs e)
        {
            Windows.UI.ViewManagement.InputPane.GetForCurrentView().TryShow();
        }

        private void email_tb_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                password_pb.Focus(FocusState.Keyboard);
            }
        }

        private void password_pb_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                Login();
            }
        }

        private void login_but_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Login();
        }

        public async void Login()
        {
            if (email_tb.Text == "" || password_pb.Password == "")
            {
                Control.ShowMessage message = new Control.ShowMessage("登录失败", "用户名或密码不能为空", "重试", "取消", 0);
                message._popup.IsOpen = true;
                return;
            }
            string json = await HttpRequest.VmovieRequset.Login_Request(email_tb.Text, password_pb.Password);
            if (json.Contains("\"status\":\"-1\"")) //登录失败
            {
                Control.ShowMessage message = new Control.ShowMessage("登录失败", "用户名或密码错误", "重试", "取消", 0);
                message._popup.IsOpen = true;
            }
            else
            {
                Model.user user = JsonToObject.JsonToObject.Convert_User_Json(json);
                App.settings.Values[Resource.APPTheme.user_auth_key] = user.auth_key;
                App.settings.Values[Resource.APPTheme.user_avatar] = user.avatar;
                App.settings.Values[Resource.APPTheme.user_email] = user.email;
                App.settings.Values[Resource.APPTheme.user_name] = user.username;
                Windows.UI.ViewManagement.InputPane.GetForCurrentView().TryHide();
                Control.ShowMessage message = new Control.ShowMessage("登录成功", "欢迎回来，" + user.username + "！\r\n您可以开始使用V电影服务了", "确定", "", 1);
                message._popup.IsOpen = true;
                if (App.DeviceInfo.Device_type == DeviceType.Mobile)
                {
                    Pages.Mobile.MainPage.mainpage.UpDate_User_Info();
                    Pages.Mobile.MainPage.mainpage.Second_Frame_Go_Back(1);
                }
                else
                {
                    MainPage.mainpage.UpDate_User_Info();
                    MainPage.mainpage.Second_Frame_Go_Back(1);
                }
            }
        }

        private async void sign_in_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("http://www.vmovier.com/user/register"));
        }
    }
}
