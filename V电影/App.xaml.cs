﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using V电影.Model;
using V电影.Resource;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace V电影
{
    /// <summary>
    /// 提供特定于应用程序的行为，以补充默认的应用程序类。
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// 初始化单一实例应用程序对象。这是执行的创作代码的第一行，
        /// 已执行，逻辑上等同于 main() 或 WinMain()。
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        public static DeviceInfo DeviceInfo = new DeviceInfo();
        public static APPTheme APPTheme = new APPTheme();
        public static int listview_old_selectedindex = 0;

        public static ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;

        private void Changed_TitleBar_Or_StatusBar()
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar")) //手机状态栏
            {
                StatusBar statusBar = StatusBar.GetForCurrentView();
                statusBar.BackgroundOpacity = 1;
                statusBar.BackgroundColor = APPTheme.APP_Color;//标题栏背景色
                statusBar.ForegroundColor = Colors.White;//标题栏前景色
            }
            else //PC状态栏
            {
                var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                titleBar.BackgroundColor = APPTheme.APP_Color;//标题栏背景色
                titleBar.ForegroundColor = Colors.White;//标题栏前景色
                titleBar.ButtonHoverBackgroundColor = Color.FromArgb(255, 44, 42, 43);//鼠标悬浮在三键上的背景色
                titleBar.ButtonHoverForegroundColor = Colors.White;//鼠标悬浮在三键上的前景色
                titleBar.ButtonBackgroundColor = APPTheme.APP_Color;//按钮背景色
                titleBar.ButtonForegroundColor = Color.FromArgb(255, 255, 255, 255);//按钮前景色
                titleBar.ButtonPressedBackgroundColor = Color.FromArgb(255, 35, 33, 34);//按钮按下背景色
                titleBar.ButtonPressedForegroundColor = Color.FromArgb(255, 92, 90, 91);//按钮按下前景色
                titleBar.ButtonInactiveBackgroundColor = APPTheme.APP_Color;//按钮处于非活跃状态时的背景色
                titleBar.ButtonInactiveForegroundColor = Color.FromArgb(255, 146, 146, 146);//按钮处于非活跃状态时的前景色
                titleBar.InactiveBackgroundColor = APPTheme.APP_Color;//应用处于非活跃状态时的背景色
                titleBar.InactiveForegroundColor = Colors.White;//应用处于非活跃状态时的前景色
            }
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            base.OnActivated(args);

            #region Toast启动
            if (args.Kind == ActivationKind.ToastNotification)
            {
                ToastNotificationActivatedEventArgs toast_args = args as ToastNotificationActivatedEventArgs;
                string arguments = toast_args.Argument;
                string[] param = null;
                string page = null;
                string postid = null;
                if (arguments.Contains('&'))
                    param = arguments.Split('&');
                else
                    param = new string[] { arguments };
                if (param != null)
                {
                    foreach (var item in param)
                    {
                        if (item.Contains(Resource.APPTheme.page_param) || item.Contains(Resource.APPTheme.login_page))
                            page = item.Substring(item.IndexOf('=') + 1);
                        else if (item.Contains(Resource.APPTheme.postid_param))
                            postid = item.Substring(item.IndexOf('=') + 1);
                    }
                }
                Frame rootFrame = Window.Current.Content as Frame;
                Changed_TitleBar_Or_StatusBar();
                if (DeviceInfo.Device_type == DeviceType.Mobile)
                {
                    if (Pages.Mobile.MainPage.mainpage != null && page != null && page == Resource.APPTheme.view_content_page && postid != null)
                    {
                        Pages.Mobile.MainPage.mainpage.View_Content(postid);
                        return;
                    }
                    else if (Pages.Mobile.MainPage.mainpage != null && page != null && page == Resource.APPTheme.login_page)
                    {
                        Pages.Mobile.MainPage.mainpage.Navigate_To_LoginPage();
                        return;
                    }
                    else
                    {
                        Window.Current.Activate();
                    }
                }
                else
                {
                    if (MainPage.mainpage != null && page != null && page == Resource.APPTheme.view_content_page && postid != null)
                    {
                        MainPage.mainpage.View_Content(postid);
                        return;
                    }
                    else if (Pages.Mobile.MainPage.mainpage != null && page != null && page == Resource.APPTheme.login_page)
                    {
                        MainPage.mainpage.Navigate_To_LoginPage();
                        return;
                    }
                    else
                    {
                        Window.Current.Activate();
                    }
                }
                if (rootFrame == null)
                {
                    rootFrame = new Frame();
                    Window.Current.Content = rootFrame;
                }
                if (rootFrame.Content == null)
                {
                    if (DeviceInfo.Device_type == DeviceType.Mobile)
                    {
                        rootFrame.ContentTransitions = new TransitionCollection();
                        rootFrame.ContentTransitions.Add(new AddDeleteThemeTransition());
                        if (page != null && page == Resource.APPTheme.view_content_page && postid != null)
                            rootFrame.Navigate(typeof(Pages.Mobile.SplashPage), new Model.ToastParam { page = page, postid = postid });
                        else if (page != null && page == Resource.APPTheme.login_page)
                            rootFrame.Navigate(typeof(Pages.Mobile.SplashPage), new Model.ToastParam { page = page });
                        else
                            rootFrame.Navigate(typeof(Pages.Mobile.SplashPage));
                    }
                    else
                    {
                        if (page != null && page == Resource.APPTheme.view_content_page && postid != null)
                        {
                            Pages.PC.SplashPage splashpage = new Pages.PC.SplashPage(toast_args.SplashScreen, new Model.ToastParam { page = page, postid = postid });
                            Window.Current.Content = splashpage;
                        }
                        else if (page != null && page == Resource.APPTheme.login_page)
                        {
                            Pages.PC.SplashPage splashpage = new Pages.PC.SplashPage(toast_args.SplashScreen, new Model.ToastParam { page = page });
                            Window.Current.Content = splashpage;
                        }
                        else
                        {
                            Pages.PC.SplashPage splashpage = new Pages.PC.SplashPage(toast_args.SplashScreen);
                            Window.Current.Content = splashpage;
                        }
                    }
                    Window.Current.Activate();
                }
            }
            #endregion

            #region 协议启动
            else if (args.Kind == ActivationKind.Protocol)
            {
                ProtocolActivatedEventArgs protocolargs = args as ProtocolActivatedEventArgs;
                string original_uri = protocolargs.Uri.OriginalString;
                string postid = null;
                string uri = null;
                string title = null;
                if (original_uri.Contains("vmovier://video/"))
                {
                    postid = original_uri.Substring(original_uri.LastIndexOf('/') + 1);
                }
                else if (original_uri.Contains("vmovier://play"))
                {
                    uri = original_uri.Substring(original_uri.IndexOf("qiniu_url=") + 10, original_uri.LastIndexOf("&") - original_uri.IndexOf("qiniu_url=") - "qiniu_url=".Length);
                    title = original_uri.Substring(original_uri.IndexOf("title=") + "title=".Length);
                }
                Frame rootFrame = Window.Current.Content as Frame;
                Changed_TitleBar_Or_StatusBar();
                if (DeviceInfo.Device_type == DeviceType.Mobile)
                {
                    if (Pages.Mobile.MainPage.mainpage != null && postid != null)
                    {
                        Pages.Mobile.MainPage.mainpage.View_Content(postid);
                        return;
                    }
                    else if (Pages.Mobile.MainPage.mainpage != null && Pages.Share.ViewContentPage.current != null && uri != null && title != null)
                    {
                        Pages.Share.ViewContentPage.current.Accept_Uri(uri, title);
                        return;
                    }
                    else
                    {
                        Window.Current.Activate();
                    }
                }
                else
                {
                    if (MainPage.mainpage != null && postid != null)
                    {
                        MainPage.mainpage.View_Content(postid);
                        return;
                    }
                    else if (MainPage.mainpage != null && Pages.Share.ViewContentPage.current != null && uri != null && title != null)
                    {
                        Pages.Share.ViewContentPage.current.Accept_Uri(uri, title);
                        return;
                    }
                    else
                    {
                        Window.Current.Activate();
                    }
                }
                if (rootFrame == null)
                {
                    rootFrame = new Frame();
                    Window.Current.Content = rootFrame;
                }
                if (rootFrame.Content == null)
                {
                    if (DeviceInfo.Device_type == DeviceType.Mobile)
                    {
                        rootFrame.ContentTransitions = new TransitionCollection();
                        rootFrame.ContentTransitions.Add(new AddDeleteThemeTransition());
                        if (postid != null)
                        {
                            rootFrame.Navigate(typeof(Pages.Mobile.SplashPage), new Model.ProtocolParam { postid = postid });
                        }
                        else
                            rootFrame.Navigate(typeof(Pages.Mobile.SplashPage));
                    }
                    else
                    {
                        if (postid != null)
                        {
                            Pages.PC.SplashPage splashpage = new Pages.PC.SplashPage(protocolargs.SplashScreen, new Model.ProtocolParam { postid = postid });
                            Window.Current.Content = splashpage;
                        }
                        else
                        {
                            Pages.PC.SplashPage splashpage = new Pages.PC.SplashPage(protocolargs.SplashScreen);
                            Window.Current.Content = splashpage;
                        }
                    }
                    Window.Current.Activate();
                }
            }
            #endregion
        }

        /// <summary>
        /// 在应用程序由最终用户正常启动时进行调用。
        /// 将在启动应用程序以打开特定文件等情况下使用。
        /// </summary>
        /// <param name="e">有关启动请求和过程的详细信息。</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            Changed_TitleBar_Or_StatusBar();
            Frame rootFrame = Window.Current.Content as Frame;

            // 不要在窗口已包含内容时重复应用程序初始化，
            // 只需确保窗口处于活动状态
            if (rootFrame == null)
            {
                // 创建要充当导航上下文的框架，并导航到第一页
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: 从之前挂起的应用程序加载状态
                }

            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // 当导航堆栈尚未还原时，导航到第一页，
                    // 并通过将所需信息作为导航参数传入来配置
                    // 参数
                    if (DeviceInfo.Device_type == DeviceType.Mobile)
                    {
                        // 将框架放在当前窗口中
                        Window.Current.Content = rootFrame;
                        rootFrame.ContentTransitions = new TransitionCollection();
                        rootFrame.ContentTransitions.Add(new AddDeleteThemeTransition());
                        rootFrame.Navigate(typeof(Pages.Mobile.SplashPage));
                    }
                    else
                    {
                        Pages.PC.SplashPage splashpage = new Pages.PC.SplashPage(e.SplashScreen);
                        Window.Current.Content = splashpage;
                    }
                }
                // 确保当前窗口处于活动状态
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// 导航到特定页失败时调用
        /// </summary>
        ///<param name="sender">导航失败的框架</param>
        ///<param name="e">有关导航失败的详细信息</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// 在将要挂起应用程序执行时调用。  在不知道应用程序
        /// 无需知道应用程序会被终止还是会恢复，
        /// 并让内存内容保持不变。
        /// </summary>
        /// <param name="sender">挂起的请求的源。</param>
        /// <param name="e">有关挂起请求的详细信息。</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: 保存应用程序状态并停止任何后台活动
            deferral.Complete();
        }
    }
}
