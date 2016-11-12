using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using V电影.Control;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace V电影.Pages.Share
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MessagePage : Page
    {
        private int p = 1;

        private bool is_message_loading;
        private bool is_inputing;

        private Model.message_comment reply_comment;
        private Model.message_object object_comment;
        private ScrollViewer message_listview_sv;
        private ViewModel.MessagePageViewModel viewmodel;

        public MessagePage()
        {
            this.InitializeComponent();
            viewmodel = new ViewModel.MessagePageViewModel();
            this.DataContext = viewmodel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.NavigationMode == NavigationMode.New)
            {
                add_comment_but.IsEnabled = false;
                FirstStep();
            }
        }

        private async void FirstStep()
        {
            string json = await HttpRequest.VmovieRequset.Get_Message_Request(p, "notice");
            viewmodel.Notice_Info = JsonToObject.JsonToObject.Convert_Message_Json(json);
            if (viewmodel.Notice_Info == null || (viewmodel.Notice_Info != null && viewmodel.Notice_Info.Count == 0))
            {
                has_null_grid.Visibility = Visibility.Visible;
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

        private async void content_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                if (((sender as TextBlock).DataContext as Model.notice).message.reply == null)
                    return;
                await (sender as TextBlock).Dispatcher?.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    if ((sender as TextBlock).ActualHeight > 55.0 && (((((sender as TextBlock).Parent as StackPanel).Children[1] as Button).Content as StackPanel).Children[0] as TextBlock).Text == "查看全部")
                    {
                        ((sender as TextBlock).DataContext as Model.notice).message.is_comment_overflow = true;
                        (sender as TextBlock).MaxLines = 2;
                        (((sender as TextBlock).Parent as StackPanel).Children[1] as Button).Visibility = Visibility.Visible;
                    }
                    else if ((sender as TextBlock).ActualHeight <= 55.0 && !(((sender as TextBlock).DataContext as Model.notice).message.is_comment_overflow) && (((sender as TextBlock).Parent as StackPanel).Children[1] as Button).Visibility == Visibility.Visible || (e.NewSize.Width != e.PreviousSize.Width && ((sender as TextBlock).DataContext as Model.notice).message.is_comment_overflow))
                    {
                        (sender as TextBlock).MaxLines = 0;
                        (((((sender as TextBlock).Parent as StackPanel).Children[1] as Button).Content as StackPanel).Children[0] as TextBlock).Text = "查看全部";
                        (((((sender as TextBlock).Parent as StackPanel).Children[1] as Button).Content as StackPanel).Children[1] as Image).Source = new BitmapImage(new Uri("ms-appx:///Assets/dropdown.png", UriKind.Absolute));
                        (((sender as TextBlock).Parent as StackPanel).Children[1] as Button).Visibility = Visibility.Collapsed;
                    }
                });
            }
            catch (Exception)
            {
            }
        }

        private void more_content_but_Click(object sender, RoutedEventArgs e)
        {
            if ((((sender as Button).Content as StackPanel).Children[0] as TextBlock).Text == "查看全部")
            {
                (((sender as Button).Content as StackPanel).Children[0] as TextBlock).Text = "收起简介";
                (((sender as Button).Parent as StackPanel).Children[0] as TextBlock).MaxLines = 0;
                (((sender as Button).Content as StackPanel).Children[1] as Image).Source = new BitmapImage(new Uri("ms-appx:///Assets/dropup.png", UriKind.Absolute));
            }
            else if ((((sender as Button).Content as StackPanel).Children[0] as TextBlock).Text == "收起简介")
            {
                (((sender as Button).Content as StackPanel).Children[0] as TextBlock).Text = "查看全部";
                (((sender as Button).Parent as StackPanel).Children[0] as TextBlock).MaxLines = 2;
                (((sender as Button).Content as StackPanel).Children[1] as Image).Source = new BitmapImage(new Uri("ms-appx:///Assets/dropdown.png", UriKind.Absolute));
            }
        }

        private void message_listview_Loaded(object sender, RoutedEventArgs e)
        {
            Get_Child(message_listview, 0);
            message_listview_sv.ViewChanging += Message_listview_sv_ViewChanging;
        }

        private async void Message_listview_sv_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            if (is_message_loading)
            {
                return;
            }
            if (e.FinalView.VerticalOffset > (message_listview_sv.ScrollableHeight * 2.0) / 3.0)
            {
                is_message_loading = true;
                string json = await HttpRequest.VmovieRequset.Get_Message_Request(++p, "notice");
                ObservableCollection<Model.notice> lists = JsonToObject.JsonToObject.Convert_Message_Json(json);
                if (lists.Count == 0)
                {
                    return;
                }
                for (int i = 0; i < lists.Count; i++)
                {
                    viewmodel.Notice_Info.Add(lists[i]);
                }
            }
            is_message_loading = false;
        }

        private void Get_Child(DependencyObject o, int n)
        {
            try
            {
                int count = VisualTreeHelper.GetChildrenCount(o);
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        var child = VisualTreeHelper.GetChild(o, count - 1);
                        if (n == 0)
                        {
                            if (child is ScrollViewer)
                            {
                                message_listview_sv = child as ScrollViewer;
                            }
                            else
                            {
                                Get_Child(VisualTreeHelper.GetChild(o, count - 1), n);
                            }
                        }
                        else if (n == 1)
                        {

                        }
                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        private void Readed_Message(int noticeid)
        {
            int index = -1;
            for (int i = 0; i < message_listview.Items.Count; i++)
            {
                if (noticeid == ((message_listview.Items[i] as Model.notice).noticeid))
                {
                    index = i;
                    break;
                }
            }
            ((((message_listview.ItemsPanelRoot as ItemsStackPanel).Children[index]) as ListViewItem).ContentTemplateRoot as Grid).Background = new SolidColorBrush(Windows.UI.Colors.Transparent);
        }

        private void message_listview_ItemClick(object sender, ItemClickEventArgs e) //回复评论
        {
            if (!(e.ClickedItem as Model.notice).isread)
            {
                Readed_Message((e.ClickedItem as Model.notice).noticeid);
            }
            if ((e.ClickedItem as Model.notice).type == "评论")
            {
                reply_comment = (e.ClickedItem as Model.notice).message.reply;
                object_comment = (e.ClickedItem as Model.notice).message.m_object;
                Reply_User_Changed((e.ClickedItem as Model.notice).message.user.username);
                add_comment_grid.Visibility = Visibility.Visible;
                is_inputing = true;
                comment_tb.Focus(FocusState.Pointer);
                Windows.UI.ViewManagement.InputPane.GetForCurrentView().TryShow();
            }
        }

        private void comment_content_but_Click(object sender, RoutedEventArgs e) //查看View内容
        {
            if (!((sender as Button).DataContext as Model.notice).isread)
            {
                Readed_Message(((sender as Button).DataContext as Model.notice).noticeid);
            }
            if (App.DeviceInfo.Device_type == Model.DeviceType.Mobile)
            {
                if (((sender as Button).DataContext as Model.notice).message.m_object.is_album)
                {
                    Pages.Mobile.MainPage.mainpage.View_Series_Content(((sender as Button).DataContext as Model.notice).message.m_object.id);
                }
                else
                {
                    Pages.Mobile.MainPage.mainpage.View_Content(((sender as Button).DataContext as Model.notice).message.m_object.id.ToString());
                }
            }
            else
            {
                if (((sender as Button).DataContext as Model.notice).message.m_object.is_album)
                {
                    MainPage.mainpage.View_Series_Content(((sender as Button).DataContext as Model.notice).message.m_object.id);
                }
                else
                {
                    MainPage.mainpage.View_Content(((sender as Button).DataContext as Model.notice).message.m_object.id.ToString());
                }
            }
        }

        private void Reply_User_Changed(string username)
        {
            comment_tb.PlaceholderText = "回复 " + username;
            comment_tb.Text = "";
        }

        private void comment_tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!(String.IsNullOrEmpty(comment_tb.Text)))
            {
                add_comment_but.IsEnabled = true;
            }
            else
            {
                add_comment_but.IsEnabled = false;
            }
        }

        private async void add_comment_but_Click(object sender, RoutedEventArgs e) //发送评论
        {
            string json = "";
            int postid = object_comment.id;
            int type = Convert.ToInt32(object_comment.is_album);
            int commentid = reply_comment.id;
            json = await HttpRequest.VmovieRequset.Add_Comment_Request(postid, type, comment_tb.Text, commentid);
            if (!String.IsNullOrEmpty(json))
            {
                string msg = "";
                JObject json_object = (JObject)JsonConvert.DeserializeObject(json);
                msg = json_object["msg"].ToString();
                if (msg == "评论成功")
                {
                    comment_tb.PlaceholderText = "我来说两句...";
                    comment_tb.Text = "";
                    add_comment_grid.Visibility = Visibility.Collapsed;
                    Windows.UI.ViewManagement.InputPane.GetForCurrentView().TryHide();
                }
                else
                {
                    Control.ShowMessage showmessage = new ShowMessage("评论", msg, "确定", "", 1);
                    showmessage._popup.IsOpen = true;
                }
            }
            else
            {
                Control.ShowMessage showmessage = new ShowMessage("评论", "操作失败", "重试", "", 1);
                showmessage._popup.IsOpen = true;
            }
        }

        private void add_comment_grid_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!is_inputing)
            {
                if (!(add_comment_but.FocusState == FocusState.Unfocused))
                    return;
                add_comment_grid.Visibility = Visibility.Collapsed;
                Windows.UI.ViewManagement.InputPane.GetForCurrentView().TryHide();
            }
            else
            {
                is_inputing = false;
                comment_tb.Focus(FocusState.Pointer);
            }
        }

        private void content_Loaded(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    await this.Dispatcher?.RunAsync(CoreDispatcherPriority.Normal, () =>
            //    {
            //        if (((sender as TextBlock).DataContext as Model.notice).message.reply == null)
            //            return;
            //        if ((sender as TextBlock).ActualHeight > 40.0 && (sender as TextBlock).MaxHeight == double.PositiveInfinity)
            //        {
            //            (sender as TextBlock).MaxHeight = 40.0;
            //            (((sender as TextBlock).Parent as StackPanel).Children[1] as Button).Visibility = Visibility.Visible;
            //            (sender as TextBlock).UpdateLayout();
            //        }
            //    });
            //}
            //catch (Exception)
            //{
            //}
        }
    }
}
