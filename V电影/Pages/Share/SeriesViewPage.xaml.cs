using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Display;
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
    public sealed partial class SeriesViewPage : Page, Attention_Notice
    {

        private int series_id;

        private MediaTransportControls transport;
        public static SeriesViewPage seriesviewpage;
        private DisplayRequest displayrequest = new DisplayRequest();
        private ViewModel.SeriesViewViewModel viewmodel = new ViewModel.SeriesViewViewModel();

        public SeriesViewPage()
        {
            this.InitializeComponent();
            transport = new MediaTransportControls();
            transport.IsCompact = true;
            transport.IsVolumeButtonVisible = true;
            transport.Style = MediaTransportStyle;
            CloseCommentView_sb.Completed += CloseCommentView_sb_Completed;
            this.DataContext = viewmodel;
            seriesviewpage = this;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.NavigationMode == NavigationMode.New || e.NavigationMode == NavigationMode.Back)
            {
                mediaelement.TransportControls = transport;

                Model.series_param param = e.Parameter as Model.series_param;
                series_id = param.series_id;
                if (param.number == -2)
                {
                    await Get_Video_Source(param.series_id);
                    await FirstStep(viewmodel.Current_Video_Info.seriesid);
                    if (viewmodel != null && viewmodel.Series_View != null)
                    {
                        int number = Get_Number(viewmodel.Current_Video_Info.series_postid);
                        if (number != -1)
                        {
                            Find_Current_Item(number);
                        }
                    }
                }
                else
                {
                    await FirstStep();
                    if (viewmodel != null && viewmodel.Series_View != null)
                    {
                        if (param.number == -1)
                        {
                            Find_Current_Item(viewmodel.Series_View.update_to);
                        }
                        else
                        {
                            Find_Current_Item(param.number);
                        }
                        await Get_Video_Source();
                    }
                }
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            try
            {
                displayrequest.RequestRelease();
            }
            catch (Exception)
            {
            }
            CloseCommentView_sb.Completed -= CloseCommentView_sb_Completed;
        }

        private async Task FirstStep(int series_id = -1)
        {
            string json = null;
            if (series_id != -1)
            {
                json = await HttpRequest.VmovieRequset.Series_View_Request(series_id);
            }
            else
            {
                json = await HttpRequest.VmovieRequset.Series_View_Request(this.series_id);
            }
            viewmodel.Series_View = JsonToObject.JsonToObject.Convert_Series_View_Json(json);
        }

        private int Get_Number(int series_postid)
        {
            for (int i = 0; i < viewmodel.Series_View.posts.Count; i++)
            {
                for (int j = 0; j < viewmodel.Series_View.posts[i].items.Count; j++)
                {
                    if (viewmodel.Series_View.posts[i].items[j].series_postid == series_postid)
                    {
                        return viewmodel.Series_View.posts[i].items[j].number;
                    }
                }
            }
            return -1;
        }

        private void Find_Current_Item(int number, int param = 1)
        {
            bool is_over = false;
            for (int i = 0; i < viewmodel.Series_View.posts.Count; i++)
            {
                for (int j = 0; j < viewmodel.Series_View.posts[i].items.Count; j++)
                {
                    if (viewmodel.Series_View.posts[i].items[j].number == number)
                    {
                        viewmodel.Current_Playing_Item = viewmodel.Series_View.posts[i].items[j];
                        if (param == 0)
                        {
                            viewmodel.Series_View.posts[i].items[j].is_playing = Visibility.Collapsed;
                        }
                        else
                        {
                            viewmodel.Series_View.posts[i].items[j].is_playing = Visibility.Visible;
                        }
                        is_over = true;
                        break;
                    }
                }
                if (is_over)
                {
                    break;
                }
            }
        }

        private async Task Get_Video_Source(int series_postid = -1)
        {
            string json = null;
            if (series_postid != -1)
            {
                json = await HttpRequest.VmovieRequset.Series_Video_Request(series_postid);
            }
            else
            {
                json = await HttpRequest.VmovieRequset.Series_Video_Request(viewmodel.Current_Playing_Item.series_postid);
            }
            viewmodel.Current_Video_Info = JsonToObject.JsonToObject.Convert_Series_Video_Info(json);
            ((comment_num.Content as StackPanel).Children[1] as TextBlock).Text = viewmodel.Current_Video_Info.count_comment.ToString();
        }

        private async void Change_Playing_Item(Model.series_view_item item)
        {
            Find_Current_Item(viewmodel.Current_Playing_Item.number, 0);
            Find_Current_Item(item.number);
            await Get_Video_Source();
        }

        private void more_content_but_Click(object sender, RoutedEventArgs e)
        {
            if (((more_content_but.Content as StackPanel).Children[0] as TextBlock).Text == "查看全部")
            {
                content_grid.MaxHeight = double.PositiveInfinity;
                ((more_content_but.Content as StackPanel).Children[0] as TextBlock).Text = "收起简介";
                ((more_content_but.Content as StackPanel).Children[1] as Image).Source = new BitmapImage(new Uri("ms-appx:///Assets/dropup.png", UriKind.Absolute));
            }
            else if (((more_content_but.Content as StackPanel).Children[0] as TextBlock).Text == "收起简介")
            {
                content_grid.MaxHeight = 40;
                ((more_content_but.Content as StackPanel).Children[0] as TextBlock).Text = "查看全部";
                ((more_content_but.Content as StackPanel).Children[1] as Image).Source = new BitmapImage(new Uri("ms-appx:///Assets/dropdown.png", UriKind.Absolute));
            }
        }

        private void item_listview_ItemClick(object sender, ItemClickEventArgs e)
        {
            Change_Playing_Item(e.ClickedItem as Model.series_view_item);
        }

        private async void attention_Click(object sender, RoutedEventArgs e)
        {
            Attention_Reuslt result = await Attention_Reuslt.Change_Attention(viewmodel.Series_View.seriesid, viewmodel.Series_View.isfollow);
            if (result != null)
            {
                viewmodel.Series_View.isfollow = result.isfollow;
                if (result.msg.ToString() == "追剧成功")
                {
                    try
                    {
                        Pages.Share.SeriesPage.seriespage.Reception(1, viewmodel.Series_View.seriesid);
                    }
                    catch (Exception)
                    {
                    }
                    ((sender as Button).Content as Image).Source = new BitmapImage(new Uri("ms-appx:///Assets/attention_finish.png", UriKind.Absolute));
                }
                else if (result.msg.ToString() == "已取消追剧")
                {
                    try
                    {
                        Pages.Share.SeriesPage.seriespage.Reception(0, viewmodel.Series_View.seriesid);
                    }
                    catch (Exception)
                    {
                    }
                    ((sender as Button).Content as Image).Source = new BitmapImage(new Uri("ms-appx:///Assets/attention.png", UriKind.Absolute));
                }
            }
        }

        public void Reception(int status, int seriesid)
        {
            if (seriesid != viewmodel.Series_View.seriesid)
            {
                return;
            }
            if (status == 1)
            {
                (attention.Content as Image).Source = new BitmapImage(new Uri("ms-appx:///Assets/attention_finish.png", UriKind.Absolute));
            }
            else if (status == 0)
            {
                (attention.Content as Image).Source = new BitmapImage(new Uri("ms-appx:///Assets/attention.png", UriKind.Absolute));
            }
            viewmodel.Series_View.isfollow = !(viewmodel.Series_View.isfollow);
        }

        private void ScrollViewer_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {

        }

        private void mediaelement_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            switch (mediaelement.CurrentState)
            {
                case MediaElementState.Playing:
                    {
                        displayrequest.RequestActive();
                    }; break;
                case MediaElementState.Closed:
                    {
                        displayrequest.RequestRelease();
                    }; break;
                case MediaElementState.Paused:
                    {
                        displayrequest.RequestRelease();
                    }; break;
                case MediaElementState.Stopped:
                    {
                        displayrequest.RequestRelease();
                    }; break;

            }
        }

        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FullWindowButton_Click(object sender, RoutedEventArgs e)
        {
            if (!mediaelement.IsFullWindow)
            {
                Windows.Graphics.Display.DisplayInformation.AutoRotationPreferences = Windows.Graphics.Display.DisplayOrientations.Landscape;
                mediaelement.SizeChanged += Mediaelement_SizeChanged;
            }
        }

        private void Mediaelement_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width <= e.PreviousSize.Width && !mediaelement.IsFullWindow)
            {
                Windows.Graphics.Display.DisplayInformation.AutoRotationPreferences = Windows.Graphics.Display.DisplayOrientations.Portrait;
                mediaelement.SizeChanged -= Mediaelement_SizeChanged;
            }
        }

        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {
            //await ((sender as Canvas).Parent as Grid).Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            //{
            //    ((sender as Canvas).Parent as Grid).Blur(10, 1, 0).Start();
            //});
        }

        private void comment_num_Click(object sender, RoutedEventArgs e)
        {
            comment_view_grid.Visibility = Visibility.Visible;
            (OpenCommentView_sb.Children[0] as Windows.UI.Xaml.Media.Animation.DoubleAnimation).From = comment_view_grid.ActualHeight;
            OpenCommentView_sb.Begin();
        }

        private void CloseCommentView_sb_Completed(object sender, object e)
        {
            comment_view_grid.Visibility = Visibility.Collapsed;
        }

        private void comment_view_CommentViewColsing(object sender, bool e)
        {
            (CloseCommentView_sb.Children[0] as Windows.UI.Xaml.Media.Animation.DoubleAnimation).To = comment_view_grid.ActualHeight;
            CloseCommentView_sb.Begin();
        }
    }
}
