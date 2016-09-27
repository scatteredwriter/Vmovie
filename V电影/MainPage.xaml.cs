﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace V电影
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private bool is_tapped_close_but = false;

        private ViewModel.MainPageViewModel viewmodel = new ViewModel.MainPageViewModel();
        private Resource.APPTheme apptheme = new Resource.APPTheme();

        public static MainPage mainpage;

        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            this.DataContext = viewmodel;
            mainpage = this;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.New)
            {
                if (App.DeviceInfo.Device_type == Model.DeviceType.PC)
                {
                    CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
                    Window.Current.SetTitleBar(titlebar);
                    page_frame.Navigate(typeof(Pages.PC.HomePage));
                    second_frame.Navigate(typeof(Pages.PC.WelcomePage), second_frame.GetNavigationState(), new DrillInNavigationTransitionInfo());
                }
                else if (App.DeviceInfo.Device_type == Model.DeviceType.Mobile)
                {
                    page_frame.Navigate(typeof(Pages.PC.HomePage));
                }
            }
        }

        public Frame Get_Frame(int n)
        {
            if (n == 0)
            {
                return page_frame;
            }
            else if (n == 1)
            {
                return second_frame;
            }
            return null;
        }

        public async void Open_Pane()
        {
            splitview.IsPaneOpen = true;
            is_tapped_close_but = false;
            Pages_ListView_Open1.Begin();
            Pages_ListView_Open2.Begin();
            await Task.Delay(300);
            Close_Pane_But_Open1.Begin();
            Close_Pane_But_Open2.Begin();
        }

        private void Back_Button_Visible(int status)
        {
            if (status == 1)
            {
                back_but.Visibility = Visibility.Visible;
            }
            else
            {
                back_but.Visibility = Visibility.Collapsed;
            }
        }

        public void Navigate_To_SearchPage()
        {
            is_tapped_close_but = true;
            splitview.IsPaneOpen = false;
            (second_frame.ContentTransitions[0] as PaneThemeTransition).Edge = EdgeTransitionLocation.Bottom;
            Back_Button_Visible(1);
            second_frame.Navigate(typeof(Pages.Share.SearchPage), second_frame.GetNavigationState(), new DrillInNavigationTransitionInfo());
        }

        public void Navigate_To_LoginPage()
        {
            is_tapped_close_but = true;
            splitview.IsPaneOpen = false;
            (second_frame.ContentTransitions[0] as PaneThemeTransition).Edge = EdgeTransitionLocation.Right;
            Back_Button_Visible(1);
            second_frame.Navigate(typeof(Pages.Share.LoginPage), second_frame.GetNavigationState());
        }

        public void UpDate_User_Info()
        {
            viewmodel.User_Avatar = null;
            viewmodel.User_Name = null;
        }

        public void View_Content(string postid)
        {
            (second_frame.ContentTransitions[0] as PaneThemeTransition).Edge = EdgeTransitionLocation.Right;
            Back_Button_Visible(1);
            second_frame.Navigate(typeof(Pages.Share.ViewContentPage), postid, new DrillInNavigationTransitionInfo());
        }

        private void Pages_StackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            is_tapped_close_but = true;
            splitview.IsPaneOpen = false;
            string page_name = ((sender as StackPanel).Children[1] as TextBlock).Text;
            switch (page_name)
            {
                case "我的订阅":
                    {
                        page_frame.Navigate(typeof(Pages.Share.OrderPage));
                    }; break;
            }
        }

        private void ListView_Loaded(object sender, RoutedEventArgs e)
        {
            pages_listview.SelectedIndex = 0;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ImageBrush background_brush = new ImageBrush();
            background_brush.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/side_item_bg_.9.png", UriKind.Absolute));

            ImageBrush before_icon = new ImageBrush();
            ImageBrush after_icon = new ImageBrush();
            SolidColorBrush foreground_brush = new SolidColorBrush();

            if (App.listview_old_selectedindex < 0)
            {
                App.listview_old_selectedindex = 0;
            }
            try
            {
                switch (App.listview_old_selectedindex)
                {
                    case 0:
                        {
                            before_icon.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/side_home_.png", UriKind.Absolute));
                        }; break;
                    case 1:
                        {
                            before_icon.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/side_series_.png", UriKind.Absolute));
                        }; break;
                    case 2:
                        {
                            before_icon.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/side_behind_.png", UriKind.Absolute));
                        }; break;
                }

                switch (pages_listview.SelectedIndex)
                {
                    case 0:
                        {
                            after_icon.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/side_home.png", UriKind.Absolute));
                        }; break;
                    case 1:
                        {
                            after_icon.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/side_series.png", UriKind.Absolute));
                        }; break;
                    case 2:
                        {
                            after_icon.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/side_behind.png", UriKind.Absolute));
                        }; break;
                }

                (((((pages_listview.ItemsPanelRoot.Children[App.listview_old_selectedindex] as ListViewItem).Content as StackPanel).Children[0] as StackPanel).Children[0]) as Image).Source = before_icon.ImageSource;
                (((((pages_listview.ItemsPanelRoot.Children[App.listview_old_selectedindex] as ListViewItem).Content as StackPanel).Children[0] as StackPanel).Children[1]) as TextBlock).Foreground = apptheme.Gary_Color_Brush;
                (((pages_listview.ItemsPanelRoot.Children[App.listview_old_selectedindex] as ListViewItem).Content as StackPanel).Children[0] as StackPanel).Background = new SolidColorBrush();
                (((pages_listview.ItemsPanelRoot.Children[App.listview_old_selectedindex] as ListViewItem).Content as StackPanel).Children[1] as StackPanel).Visibility = Visibility.Collapsed;

                (((((pages_listview.ItemsPanelRoot.Children[pages_listview.SelectedIndex] as ListViewItem).Content as StackPanel).Children[0] as StackPanel).Children[0]) as Image).Source = after_icon.ImageSource;
                (((((pages_listview.ItemsPanelRoot.Children[pages_listview.SelectedIndex] as ListViewItem).Content as StackPanel).Children[0] as StackPanel).Children[1]) as TextBlock).Foreground = apptheme.Foreground_Color_Brush;
                (((pages_listview.ItemsPanelRoot.Children[pages_listview.SelectedIndex] as ListViewItem).Content as StackPanel).Children[0] as StackPanel).Background = background_brush;
                (((pages_listview.ItemsPanelRoot.Children[pages_listview.SelectedIndex] as ListViewItem).Content as StackPanel).Children[1] as StackPanel).Visibility = Visibility.Visible;

                App.listview_old_selectedindex = pages_listview.SelectedIndex;
            }
            catch (Exception)
            {
                return;
            }
        }

        private void close_pane_but_Tapped(object sender, TappedRoutedEventArgs e)
        {
            is_tapped_close_but = true;
            splitview.IsPaneOpen = false;
        }

        private void pages_listview_ItemClick(object sender, ItemClickEventArgs e)
        {
            is_tapped_close_but = true;
            splitview.IsPaneOpen = false;
            TextBlock tb = ((((e.ClickedItem as StackPanel).Children[0] as StackPanel).Children[1]) as TextBlock);
            switch (tb.Text)
            {
                case "首页":
                    {
                        page_frame.Navigate(typeof(Pages.PC.HomePage));
                    }; break;
                case "系列":
                    {
                        page_frame.Navigate(typeof(Pages.Share.SeriesPage));
                    }; break;
                case "幕后":
                    {
                        page_frame.Navigate(typeof(Pages.Share.BehindPage));
                    }; break;
            }
        }

        private void splitview_PaneClosing(SplitView sender, SplitViewPaneClosingEventArgs args)
        {
            if (is_tapped_close_but == false)
            {
                args.Cancel = true;
            }
            else
            {
                Close_Pane_But_Open1.Stop();
                Close_Pane_But_Open2.Stop();
            }
        }

        private void login_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (App.settings.Values.ContainsKey(Resource.APPTheme.user_auth_key))
            {
                Control.ShowMessage message = new Control.ShowMessage("账号注销", "您确定要注销已登录的账号么", "确定", "取消", 0);
                message._popup.IsOpen = true;
                message.OK_Click += (s, e1) =>
                {
                    App.settings.Values.Remove(Resource.APPTheme.user_auth_key);
                    App.settings.Values.Remove(Resource.APPTheme.user_avatar);
                    App.settings.Values.Remove(Resource.APPTheme.user_email);
                    App.settings.Values.Remove(Resource.APPTheme.user_name);
                    UpDate_User_Info();
                };
            }
            else
            {
                second_frame_title.Text = "登录";
                Navigate_To_LoginPage();
            }
        }

        public void Second_Frame_Go_Back(int param = 0)
        {
            if (second_frame.CanGoBack)
            {
                if (param == 1 || second_frame.SourcePageType == typeof(Pages.Share.LoginPage))
                {
                    second_frame_title.Text = "内容";
                }
                second_frame.GoBack();
            }
            if (!second_frame.CanGoBack)
            {
                Back_Button_Visible(0);
            }
        }

        private void back_but_Click(object sender, RoutedEventArgs e)
        {
            Second_Frame_Go_Back();
        }
    }
}