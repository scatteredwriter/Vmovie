using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace V电影.Pages.PC
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class HomePage : Page
    {
        private int pivot_selectedindex = 0;
        private int flipviewindex = 0;
        private int lastest_p = 1;

        private double delta_x = 0.0;

        private bool is_first_navigation = true;
        private bool is_lastest_loading = false;

        private DispatcherTimer date;
        private ScrollViewer lastest_listview_sc = null;
        private Resource.APPTheme apptheme = new Resource.APPTheme();

        private ViewModel.HomePageViewModel viewmodel = new ViewModel.HomePageViewModel();

        public HomePage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            date = new DispatcherTimer();
            date.Interval = new TimeSpan(0, 0, 5);
            date.Tick += Date_Tick;
            this.DataContext = viewmodel;
            this.SizeChanged += HomePage_SizeChanged;
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
                Dispatcher?.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    ((pivot.Items[0] as PivotItem).Header as Grid).Padding = new Thickness(0, 0, 0, 10);
                    for (int i = 0; i < viewmodel.Flipview_Lists.Count; i++)
                    {
                        Line line = new Line();
                        line.X1 = 0;
                        line.X2 = 25;
                        line.Y1 = 0;
                        line.Y2 = 0;
                        line.Margin = new Thickness(0, 0, 5, 0);
                        line.Opacity = 0.5;
                        line.StrokeThickness = 2.5;
                        flipview.SelectedIndex = 1;
                        flipview.SelectedIndex = 0;
                        if (i == 0)
                        {
                            line.Stroke = apptheme.Foreground_Color_Brush;
                        }
                        else
                        {
                            line.Stroke = apptheme.Gary_Color_Brush;
                        }
                        sp.Children.Add(line);
                    }
                    await Task.Delay(500);
                    lastest_listview_sc.ChangeView(null, 30, null);
                });
                date.Start();
            }
        }

        private async Task First_Step()
        {
            string json = await HttpRequest.VmovieRequset.Flipview_Requset();
            viewmodel.Flipview_Lists = JsonToObject.JsonToObject.Convert_Flipview_Json(json);
            Cache.ImageCache imagecache = new Cache.ImageCache();
            List<string> image_url = new List<string>();
            for (int i = 0; i < viewmodel.Flipview_Lists.Count; i++)
            {
                image_url.Add(viewmodel.Flipview_Lists[i].imageurl);
            }
            viewmodel.Flipview_Image_Sbs = await imagecache.Get_Image_Source(image_url, "FlipView");

            json = await HttpRequest.VmovieRequset.Lastest_Requset(lastest_p);
            viewmodel.Lastest_Info = JsonToObject.JsonToObject.Convert_Lastest_Json(json);
            imagecache = new Cache.ImageCache();
            image_url = new List<string>();
            for (int i = 0; i < viewmodel.Lastest_Info.Count; i++)
            {
                image_url.Add(viewmodel.Lastest_Info[i].image);
            }
            viewmodel.Lastest_Image_Sbs = await imagecache.Get_Image_Source(image_url, "Lastest");
            viewmodel.Lastest_New_Count = viewmodel.Lastest_Info.Count;

            json = await HttpRequest.VmovieRequset.Cates_Request();
            viewmodel.Cate_Lists = JsonToObject.JsonToObject.Convert_Cates_Json(json);
            Model.cate cate = new Model.cate();
            cate.alias = "Album";
            cate.cate_type = true;
            cate.catename = "专题";
            cate.icon = "http://cs.vmoiver.com/Uploads/Activity/2016-04-27/5720601258d7f.jpg";
            cate.tab = "album";
            viewmodel.Cate_Lists.Insert(0, cate);
            cate = new Model.cate();
            cate.alias = "Hot";
            cate.cate_type = true;
            cate.catename = "热门";
            cate.icon = "http://cs.vmoiver.com/Uploads/Activity/2016-04-26/571ed9b5d2e44.jpg";
            cate.tab = "hot";
            viewmodel.Cate_Lists.Insert(0, cate);
            imagecache = new Cache.ImageCache();
            image_url = new List<string>();
            for (int i = 0; i < viewmodel.Cate_Lists.Count; i++)
            {
                image_url.Add(viewmodel.Cate_Lists[i].icon);
            }
            viewmodel.Cate_Image_Sbs = await imagecache.Get_Image_Source(image_url, "Cates");
        }

        private void HomePage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                Changed_GridView_Item_Size();
            }
            catch (Exception)
            {
                return;
            }
        }

        private void Changed_GridView_Item_Size()
        {
            GridViewItem item = new GridViewItem();
            double height = 0.0;
            double width = this.ActualWidth;
            int items_count = cates_gridview.ItemsPanelRoot.Children.Count;
            int row_count = (int)width / 250;
            width /= row_count;
            height = width;
            for (int i = 0; i < items_count; i++)
            {
                item = new GridViewItem();
                item = cates_gridview.ItemsPanelRoot.Children[i] as GridViewItem;
                (item.ContentTemplateRoot as Grid).Height = (int)height;
                (item.ContentTemplateRoot as Grid).Width = (int)width;
            }
        }

        private async void Lastest_Refresh()
        {
            if (lastest_listview_sc.VerticalOffset == 0.0)
            {
                string json = await HttpRequest.VmovieRequset.Flipview_Requset();
                viewmodel.Flipview_Lists = JsonToObject.JsonToObject.Convert_Flipview_Json(json);
                Cache.ImageCache imagecache = new Cache.ImageCache();
                List<string> image_url = new List<string>();
                for (int i = 0; i < viewmodel.Flipview_Lists.Count; i++)
                {
                    image_url.Add(viewmodel.Flipview_Lists[i].imageurl);
                }
                viewmodel.Flipview_Image_Sbs = await imagecache.Get_Image_Source(image_url, "FlipView");

                json = await HttpRequest.VmovieRequset.Lastest_Requset(lastest_p = 1);
                viewmodel.Lastest_Info = JsonToObject.JsonToObject.Convert_Lastest_Json(json);
                imagecache = new Cache.ImageCache();
                image_url = new List<string>();
                for (int i = 0; i < viewmodel.Lastest_Info.Count; i++)
                {
                    image_url.Add(viewmodel.Lastest_Info[i].image);
                }
                viewmodel.Lastest_Image_Sbs = await imagecache.Get_Image_Source(image_url, "Lastest");
                viewmodel.Lastest_New_Count = viewmodel.Lastest_Info.Count;

                await Task.Delay(500);

                //移动回顶部
                for (double i = 0.0; i < (30 - lastest_listview_sc.VerticalOffset); i += 1)
                {
                    lastest_listview_sc.ChangeView(null, lastest_listview_sc.VerticalOffset + i, null);
                }
                lastest_listview_sc.ChangeView(null, 30, null);
            }
        }

        private void Date_Tick(object sender, object e)
        {
            try
            {
                if (flipviewindex < 0)
                {
                    flipviewindex = 0;
                }
                if (flipview.SelectedIndex == viewmodel.Flipview_Lists.Count - 1)
                {
                    flipview.SelectedIndex = 0;
                }
                else
                {
                    flipview.SelectedIndex++;
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ((((sender as Pivot).Items[pivot_selectedindex] as PivotItem).Header as Grid).Children[0] as TextBlock).Opacity = 0.5;
                (((sender as Pivot).Items[pivot_selectedindex] as PivotItem).Header as Grid).Padding = new Thickness(0, 0, 0, 14);
                (((sender as Pivot).Items[pivot_selectedindex] as PivotItem).Header as Grid).BorderThickness = new Thickness(0, 0, 0, 0);
                pivot_selectedindex = (sender as Pivot).SelectedIndex;
                ((((sender as Pivot).SelectedItem as PivotItem).Header as Grid).Children[0] as TextBlock).Opacity = 1;
                (((sender as Pivot).SelectedItem as PivotItem).Header as Grid).Padding = new Thickness(0, 0, 0, 10);
                (((sender as Pivot).SelectedItem as PivotItem).Header as Grid).BorderThickness = new Thickness(0, 0, 0, 2);
            }
            catch (Exception)
            {
                return;
            }
        }

        private void Grid_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            delta_x += e.Delta.Translation.X;
        }

        private void Grid_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (delta_x == 0)
            {
                return;
            }
            else if (delta_x >= 100)
            {
                try
                {
                    date.Stop();
                    Flipview_Last();
                    delta_x = 0;
                    date.Start();
                }
                catch (Exception)
                {
                    return;
                }
            }
            else if (delta_x <= -100)
            {
                try
                {
                    date.Stop();
                    Flipview_Next();
                    delta_x = 0;
                    date.Start();
                }
                catch (Exception)
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }

        private void Flipview_Next()
        {
            if (flipview.SelectedIndex == viewmodel.Flipview_Lists.Count - 1)
            {
                flipview.SelectedIndex = 0;
            }
            else
            {
                flipview.SelectedIndex++;
            }
        }

        private void Flipview_Last()
        {
            if (flipview.SelectedIndex == 0)
            {
                flipview.SelectedIndex = viewmodel.Flipview_Lists.Count - 1;
            }
            else
            {
                flipview.SelectedIndex--;
            }
        }

        private void flipview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (flipviewindex < 0)
                {
                    flipviewindex = 0;
                }
                (sp.Children[flipviewindex] as Line).Stroke = apptheme.Gary_Color_Brush;
                flipviewindex = flipview.SelectedIndex;
                (sp.Children[flipviewindex] as Line).Stroke = apptheme.Foreground_Color_Brush;
                if (flipview.SelectedIndex == 0)
                {
                    l_flipview.SelectedIndex = viewmodel.Flipview_Lists.Count - 1;
                    try
                    {
                        r_flipview.SelectedIndex = 1;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
                else if (flipview.SelectedIndex == 1)
                {
                    l_flipview.SelectedIndex = 0;
                    r_flipview.SelectedIndex = flipview.SelectedIndex + 1;
                }
                else if (flipview.SelectedIndex == viewmodel.Flipview_Lists.Count - 2)
                {
                    l_flipview.SelectedIndex = flipview.SelectedIndex - 1;
                    r_flipview.SelectedIndex = viewmodel.Flipview_Lists.Count - 1;
                }
                else if (flipview.SelectedIndex == viewmodel.Flipview_Lists.Count - 1)
                {
                    l_flipview.SelectedIndex = flipview.SelectedIndex - 1;
                    r_flipview.SelectedIndex = 0;
                }
                else
                {
                    l_flipview.SelectedIndex = flipview.SelectedIndex - 1;
                    r_flipview.SelectedIndex = flipview.SelectedIndex + 1;
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        private void r_flipview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (flipview.SelectedIndex == 0 && r_flipview.Items.Count == viewmodel.Flipview_Lists.Count)
            {
                r_flipview.SelectedIndex = 1;
            }
        }

        private void ListView_Loaded(object sender, RoutedEventArgs e)
        {
            Get_Child((DependencyObject)sender, 0);
            lastest_listview_sc.ViewChanging += Lastest_listview_sc_ViewChanging;
            lastest_listview_sc.ViewChanged += Lastest_listview_sc_ViewChanged;
        }

        private async void Lastest_listview_sc_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e) //下拉刷新
        {
            if (lastest_listview_sc.VerticalOffset == lastest_listview_sc.ScrollableHeight)
            {
                viewmodel.Lastest_New_Count = 0;
                lastest_new_border.Visibility = Visibility.Visible;
                await Task.Delay(2000);
                lastest_new_border.Visibility = Visibility.Collapsed;
                return;
            }
            if (!e.IsIntermediate)
            {
                Lastest_Refresh();
            }
        }

        private async void Lastest_listview_sc_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e) //下拉刷新判错 自动加载
        {
            if (App.DeviceInfo.Device_type != Model.DeviceType.PC)
            {
                if (lastest_listview_sc.VerticalOffset <= 10.0)
                {
                    if (e.IsInertial)
                    {
                        for (double i = 0.0; i < (30.0 - lastest_listview_sc.VerticalOffset); i += 1.0)
                        {
                            lastest_listview_sc.ChangeView(null, lastest_listview_sc.VerticalOffset + i, null);
                        }
                        lastest_listview_sc.ChangeView(null, 30.0, null);
                    }
                }
            }
            else
            {
                if (lastest_listview_sc.VerticalOffset <= 50.0 && lastest_listview_sc.VerticalOffset > 30.0)
                {
                    if (e.IsInertial)
                    {
                        for (double i = 0.0; i < (30.0 - lastest_listview_sc.VerticalOffset); i += 1.0)
                        {
                            lastest_listview_sc.ChangeView(null, lastest_listview_sc.VerticalOffset + i, null);
                        }
                        lastest_listview_sc.ChangeView(null, 30.0, null);
                    }
                }
            }
            if (is_lastest_loading)
            {
                return;
            }
            if (e.FinalView.VerticalOffset > (lastest_listview_sc.ScrollableHeight * 2.0) / 3.0)
            {
                is_lastest_loading = true;
                string json = await HttpRequest.VmovieRequset.Lastest_Requset(++lastest_p);
                ObservableCollection<Model.lastest_info> lists = JsonToObject.JsonToObject.Convert_Lastest_Json(json);
                ObservableCollection<ImageSource> sources = new ObservableCollection<ImageSource>();
                if (lists.Count == 0)
                {
                    return;
                }
                for (int i = 0; i < lists.Count; i++)
                {
                    viewmodel.Lastest_Info.Add(lists[i]);
                }
                viewmodel.Lastest_New_Count = viewmodel.Lastest_Info.Count - viewmodel.Lastest_New_Count;
                Cache.ImageCache imagecache = new Cache.ImageCache();
                List<string> image_url = new List<string>();
                for (int i = (viewmodel.Lastest_Info.Count - viewmodel.Lastest_New_Count); i < viewmodel.Lastest_Info.Count; i++)
                {
                    image_url.Add(viewmodel.Lastest_Info[i].image);
                }
                sources = await imagecache.Get_Image_Source(image_url, "Lastest");
                for (int i = 0; i < sources.Count; i++)
                {
                    viewmodel.Lastest_Image_Sbs.Add(sources[i]);
                }
                lastest_new_border.Visibility = Visibility.Visible;
                await Task.Delay(2000);
                lastest_new_border.Visibility = Visibility.Collapsed;
                viewmodel.Lastest_New_Count = viewmodel.Lastest_Info.Count;
            }
            is_lastest_loading = false;
        }

        private void PivotItem_LayoutUpdated(object sender, object e)
        {
            try
            {
                Changed_GridView_Item_Size();
            }
            catch (Exception)
            {
                return;
            }
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
                                lastest_listview_sc = child as ScrollViewer;
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
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
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

        private void View_Content_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MainPage.mainpage.View_Content(((sender as Image).DataContext as Model.flipview).app_banner_param);
        }

        private void lastest_listview_ItemClick(object sender, ItemClickEventArgs e)
        {
            MainPage.mainpage.View_Content((e.ClickedItem as Model.lastest_info).postid.ToString());
        }

        private void cates_gridview_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(Pages.Share.CatePage), (e.ClickedItem as Model.cate));
        }
    }
}
