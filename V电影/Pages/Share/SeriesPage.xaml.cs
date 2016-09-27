using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace V电影.Pages.Share
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SeriesPage : Page
    {

        private int series_p = 1;

        private bool is_first_navigation = true;
        private bool is_series_loading = false;

        private ScrollViewer series_listview_sc = null;
        private Resource.APPTheme apptheme = new Resource.APPTheme();

        private ViewModel.SeriesPageViewModel viewmodel = new ViewModel.SeriesPageViewModel();

        public SeriesPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            this.DataContext = viewmodel;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.NavigationMode == NavigationMode.New)
            {
                if (is_first_navigation)
                    is_first_navigation = false;
                else
                    return;
                await First_Step();
                await Task.Delay(500);
                series_listview_sc.ChangeView(null, 30, null);
            }
        }

        private async Task First_Step()
        {
            string json = await HttpRequest.VmovieRequset.Series_Request(series_p);
            viewmodel.Series_Info = JsonToObject.JsonToObject.Convert_Series_Json(json);
            Cache.ImageCache imagecache = new Cache.ImageCache();
            List<string> image_url = new List<string>();
            for (int i = 0; i < viewmodel.Series_Info.Count; i++)
            {
                image_url.Add(viewmodel.Series_Info[i].app_image);
            }
            viewmodel.Series_Image_Sbs = await imagecache.Get_Image_Source(image_url, "Series");
            viewmodel.Series_New_Count = viewmodel.Series_Info.Count;
        }

        private async void Series_Refresh()
        {
            if (series_listview_sc.VerticalOffset == 0.0)
            {
                string json = await HttpRequest.VmovieRequset.Series_Request(series_p = 1);
                viewmodel.Series_Info = JsonToObject.JsonToObject.Convert_Series_Json(json);
                Cache.ImageCache imagecache = new Cache.ImageCache();
                List<string> image_url = new List<string>();
                for (int i = 0; i < viewmodel.Series_Info.Count; i++)
                {
                    image_url.Add(viewmodel.Series_Info[i].app_image);
                }
                viewmodel.Series_Image_Sbs = await imagecache.Get_Image_Source(image_url, "Series");
                viewmodel.Series_New_Count = viewmodel.Series_Info.Count;
                await Task.Delay(1000);

                //移动回顶部
                for (double i = 0.0; i < (30 - series_listview_sc.VerticalOffset); i += 1)
                {
                    series_listview_sc.ChangeView(null, series_listview_sc.VerticalOffset + i, null);
                }
                series_listview_sc.ChangeView(null, 30, null);
            }
        }

        private void series_listview_Loaded(object sender, RoutedEventArgs e)
        {
            Get_Child(series_listview, 0);
            series_listview_sc.ViewChanging += Series_listview_sc_ViewChanging;
            series_listview_sc.ViewChanged += Series_listview_sc_ViewChanged;
        }

        private async void Series_listview_sc_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (series_listview_sc.VerticalOffset == series_listview_sc.ScrollableHeight)
            {
                viewmodel.Series_New_Count = 0;
                series_new_border.Visibility = Visibility.Visible;
                await Task.Delay(2000);
                series_new_border.Visibility = Visibility.Collapsed;
                return;
            }
            if (!e.IsIntermediate)
            {
                Series_Refresh();
            }
        }

        private async void Series_listview_sc_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            if (App.DeviceInfo.Device_type != Model.DeviceType.PC)
            {
                if (series_listview_sc.VerticalOffset <= 10.0)
                {
                    if (e.IsInertial)
                    {
                        for (double i = 0.0; i < (30.0 - series_listview_sc.VerticalOffset); i += 1.0)
                        {
                            series_listview_sc.ChangeView(null, series_listview_sc.VerticalOffset + i, null);
                        }
                        series_listview_sc.ChangeView(null, 30.0, null);
                    }
                }
            }
            else
            {
                if (series_listview_sc.VerticalOffset <= 50.0 && series_listview_sc.VerticalOffset > 30.0)
                {
                    if (e.IsInertial)
                    {
                        for (double i = 0.0; i < (30.0 - series_listview_sc.VerticalOffset); i += 1.0)
                        {
                            series_listview_sc.ChangeView(null, series_listview_sc.VerticalOffset + i, null);
                        }
                        series_listview_sc.ChangeView(null, 30.0, null);
                    }
                }
            }
            if (is_series_loading)
            {
                return;
            }
            if (e.FinalView.VerticalOffset > (series_listview_sc.ScrollableHeight * 2.0) / 3.0)
            {
                is_series_loading = true;
                string json = await HttpRequest.VmovieRequset.Series_Request(++series_p);
                ObservableCollection<Model.series> lists = JsonToObject.JsonToObject.Convert_Series_Json(json);
                ObservableCollection<ImageSource> sources = new ObservableCollection<ImageSource>();
                if (lists.Count == 0)
                {
                    return;
                }
                for (int i = 0; i < lists.Count; i++)
                {
                    viewmodel.Series_Info.Add(lists[i]);
                }
                viewmodel.Series_New_Count = viewmodel.Series_Info.Count - viewmodel.Series_New_Count;
                Cache.ImageCache imagecache = new Cache.ImageCache();
                List<string> image_url = new List<string>();
                for (int i = (viewmodel.Series_Info.Count - viewmodel.Series_New_Count); i < viewmodel.Series_Info.Count; i++)
                {
                    image_url.Add(viewmodel.Series_Info[i].image);
                }
                sources = await imagecache.Get_Image_Source(image_url, "Series");
                for (int i = 0; i < sources.Count; i++)
                {
                    viewmodel.Series_Image_Sbs.Add(sources[i]);
                }
                series_new_border.Visibility = Visibility.Visible;
                await Task.Delay(2000);
                series_new_border.Visibility = Visibility.Collapsed;
                viewmodel.Series_New_Count = viewmodel.Series_Info.Count;
            }
            is_series_loading = false;
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
                                series_listview_sc = child as ScrollViewer;
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

        private void menu_but_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MainPage.mainpage.Open_Pane();
        }

        private void search_but_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MainPage.mainpage.Navigate_To_SearchPage();
        }

        private async void attention_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Model.series series = (sender as Image).DataContext as Model.series;
            string json = await HttpRequest.VmovieRequset.Series_Follow_Request(series.seriesid, Convert.ToInt32(!series.isfollow));
            JObject json_object = (JObject)JsonConvert.DeserializeObject(json);
            if (json_object["status"].ToString() == "0")
            {
                series.isfollow = !series.isfollow;
                if (json_object["msg"].ToString() == "追剧成功")
                {
                    (sender as Image).Source = new BitmapImage(new Uri("ms-appx:///Assets/attention_finish.png", UriKind.Absolute));
                }
                else if (json_object["msg"].ToString() == "已取消追剧")
                {
                    (sender as Image).Source = new BitmapImage(new Uri("ms-appx:///Assets/attention.png", UriKind.Absolute));
                }
            }
            Control.ShowMessage showmessage = new Control.ShowMessage("系列", json_object["msg"].ToString(), "确定", "", 1);
            showmessage._popup.IsOpen = true;
        }

        private void View_Content_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //MainPage.mainpage.View_Content(((sender as RelativePanel).DataContext as Model.series).seriesid.ToString());
        }
    }
}
