using Microsoft.Toolkit.Uwp.UI;
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
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace V电影.Pages.Mobile
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class HomePage : Page
    {
        private int pivot_selectedindex = 0;
        private int flipviewindex = 0;
        private int lastest_p = 1;

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
                date.Start();
            }
        }

        private async Task First_Step()
        {
            ((pivot.Items[0] as PivotItem).Header as Grid).Padding = new Thickness(0, 0, 0, 13);

            string json = await HttpRequest.VmovieRequset.Flipview_Requset();
            viewmodel.Flipview_Lists = JsonToObject.JsonToObject.Convert_Flipview_Json(json);
            ImageCache imagecache = new ImageCache();
            await imagecache.InitializeAsync(await Params.Params.Get_ImageCacheFolder(), Params.Params.flipview_floder);
            for (int i = 0; i < viewmodel.Flipview_Lists.Count; i++)
            {
                string uri = viewmodel.Flipview_Lists[i].imageurl;
                ImageSource imagesource = await imagecache.GetFromCacheAsync(new Uri(uri), uri.Substring(uri.LastIndexOf('/') + 1));
                viewmodel.Flipview_Lists[i].image_source = imagesource;
            }
            Add_FlipView_Lines();

            json = await HttpRequest.VmovieRequset.Lastest_Requset(lastest_p);
            viewmodel.Lastest_Info = JsonToObject.JsonToObject.Convert_Lastest_Json(json);
            imagecache = new ImageCache();
            await imagecache.InitializeAsync(await Params.Params.Get_ImageCacheFolder(), Params.Params.lastest_floder);
            for (int i = 0; i < viewmodel.Lastest_Info.Count; i++)
            {
                string uri = viewmodel.Lastest_Info[i].image;
                ImageSource imagesource = await imagecache.GetFromCacheAsync(new Uri(uri), uri.Substring(uri.LastIndexOf('/') + 1));
                viewmodel.Lastest_Info[i].image_source = imagesource;
            }
            viewmodel.Lastest_New_Count = viewmodel.Lastest_Info.Count;

            await Task.Delay(500);
            lastest_listview_sc.ChangeView(null, 30, null);

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
            imagecache = new ImageCache();
            await imagecache.InitializeAsync(await Params.Params.Get_ImageCacheFolder(), Params.Params.cate_floder);
            for (int i = 0; i < viewmodel.Cate_Lists.Count; i++)
            {
                string uri = viewmodel.Cate_Lists[i].icon;
                ImageSource imagesource = await imagecache.GetFromCacheAsync(new Uri(uri), uri.Substring(uri.LastIndexOf('/') + 1));
                viewmodel.Cate_Lists[i].image_source = imagesource;
            }
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
            if (cates_gridview.Items == null)
                return;
            GridViewItem item = null;
            double height = 0.0;
            double width = this.ActualWidth;
            int items_count = cates_gridview.ItemsPanelRoot.Children.Count;
            width /= 2;
            height = width;
            for (int i = 0; i < items_count; i++)
            {
                item = cates_gridview.ItemsPanelRoot.Children[i] as GridViewItem;
                (item.ContentTemplateRoot as Grid).Height = height;
                (item.ContentTemplateRoot as Grid).Width = width;
            }
        }

        private void Add_FlipView_Lines()
        {
            Dispatcher?.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                for (int i = 0; i < viewmodel.Flipview_Lists.Count; i++)
                {
                    Line line = new Line();
                    line.X1 = 0;
                    line.X2 = 20;
                    line.Y1 = 0;
                    line.Y2 = 0;
                    line.Margin = new Thickness(0, 0, 5, 0);
                    line.Opacity = 0.5;
                    line.StrokeThickness = 2.0;
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
                if (flipview.ItemsPanelRoot.Children != null)
                    viewmodel.Max_Height = (flipview.ItemsPanelRoot.Children[0] as FlipViewItem).ActualHeight;
            });
        }

        private async void Lastest_Refresh()
        {
            if (lastest_listview_sc.VerticalOffset == 0.0)
            {
                int temp_flipview_count = viewmodel.Flipview_Lists.Count;

                string json = await HttpRequest.VmovieRequset.Flipview_Requset();
                viewmodel.Flipview_Lists = JsonToObject.JsonToObject.Convert_Flipview_Json(json);
                ImageCache imagecache = new ImageCache();
                await imagecache.InitializeAsync(await Params.Params.Get_ImageCacheFolder(), Params.Params.flipview_floder);
                for (int i = 0; i < viewmodel.Flipview_Lists.Count; i++)
                {
                    string uri = viewmodel.Flipview_Lists[i].imageurl;
                    ImageSource imagesource = await imagecache.GetFromCacheAsync(new Uri(uri), uri.Substring(uri.LastIndexOf('/') + 1));
                    viewmodel.Flipview_Lists[i].image_source = imagesource;
                }

                if (temp_flipview_count != viewmodel.Flipview_Lists.Count)
                {
                    sp.Children.Clear();
                    Add_FlipView_Lines();
                }

                json = await HttpRequest.VmovieRequset.Lastest_Requset(lastest_p = 1);
                viewmodel.Lastest_Info = JsonToObject.JsonToObject.Convert_Lastest_Json(json);
                imagecache = new ImageCache();
                await imagecache.InitializeAsync(await Params.Params.Get_ImageCacheFolder(), Params.Params.lastest_floder);
                for (int i = 0; i < viewmodel.Lastest_Info.Count; i++)
                {
                    string uri = viewmodel.Lastest_Info[i].image;
                    ImageSource imagesource = await imagecache.GetFromCacheAsync(new Uri(uri), uri.Substring(uri.LastIndexOf('/') + 1));
                    viewmodel.Lastest_Info[i].image_source = imagesource;
                }
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
                (((sender as Pivot).SelectedItem as PivotItem).Header as Grid).Padding = new Thickness(0, 0, 0, 12);
                (((sender as Pivot).SelectedItem as PivotItem).Header as Grid).BorderThickness = new Thickness(0, 0, 0, 2);
            }
            catch (Exception)
            {
                return;
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
                //if (flipview.SelectedIndex == 0)
                //{
                //    l_flipview.SelectedIndex = viewmodel.Flipview_Lists.Count - 1;
                //    try
                //    {
                //        r_flipview.SelectedIndex = 1;
                //    }
                //    catch (Exception ex)
                //    {
                //        Debug.WriteLine(ex.Message);
                //    }
                //}
                //else if (flipview.SelectedIndex == 1)
                //{
                //    l_flipview.SelectedIndex = 0;
                //    r_flipview.SelectedIndex = flipview.SelectedIndex + 1;
                //}
                //else if (flipview.SelectedIndex == viewmodel.Flipview_Lists.Count - 2)
                //{
                //    l_flipview.SelectedIndex = flipview.SelectedIndex - 1;
                //    r_flipview.SelectedIndex = viewmodel.Flipview_Lists.Count - 1;
                //}
                //else if (flipview.SelectedIndex == viewmodel.Flipview_Lists.Count - 1)
                //{
                //    l_flipview.SelectedIndex = flipview.SelectedIndex - 1;
                //    r_flipview.SelectedIndex = 0;
                //}
                //else
                //{
                //    l_flipview.SelectedIndex = flipview.SelectedIndex - 1;
                //    r_flipview.SelectedIndex = flipview.SelectedIndex + 1;
                //}
            }
            catch (Exception)
            {
                return;
            }
        }

        private void cates_gridview_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Changed_GridView_Item_Size();
        }

        private void ListView_Loaded(object sender, RoutedEventArgs e)
        {
            Get_Child((DependencyObject)sender, 0);
            lastest_listview_sc.ViewChanging += Lastest_listview_sc_ViewChanging;
            lastest_listview_sc.ViewChanged += Lastest_listview_sc_ViewChanged;
        }

        private void Lastest_listview_sc_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e) //下拉刷新
        {
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
                ImageCache imagecache = new ImageCache();
                await imagecache.InitializeAsync(await Params.Params.Get_ImageCacheFolder(), Params.Params.lastest_floder);
                for (int i = (viewmodel.Lastest_Info.Count - viewmodel.Lastest_New_Count); i < viewmodel.Lastest_Info.Count; i++)
                {
                    string uri = viewmodel.Lastest_Info[i].image;
                    ImageSource imagesource = await imagecache.GetFromCacheAsync(new Uri(uri), uri.Substring(uri.LastIndexOf('/') + 1));
                    viewmodel.Lastest_Info[i].image_source = imagesource;
                }
                lastest_new_border.Visibility = Visibility.Visible;
                await Task.Delay(2000);
                lastest_new_border.Visibility = Visibility.Collapsed;
                viewmodel.Lastest_New_Count = viewmodel.Lastest_Info.Count;
            }
            is_lastest_loading = false;
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
            catch (Exception)
            {
            }
        }

        private void menu_but_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (App.DeviceInfo.Device_type == Model.DeviceType.Mobile)
            {
                Pages.Mobile.MainPage.mainpage.Open_Pane();
            }
            else
            {
                MainPage.mainpage.Open_Pane();
            }
        }

        private void search_but_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (App.DeviceInfo.Device_type == Model.DeviceType.Mobile)
            {
                Pages.Mobile.MainPage.mainpage.Navigate_To_SearchPage();
            }
            else
            {
                MainPage.mainpage.Navigate_To_SearchPage();
            }
        }

        private void View_Content_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (App.DeviceInfo.Device_type == Model.DeviceType.Mobile)
            {
                Pages.Mobile.MainPage.mainpage.View_Content(((sender as Image).DataContext as Model.flipview).app_banner_param);
            }
            else
            {
                MainPage.mainpage.View_Content(((sender as Image).DataContext as Model.flipview).app_banner_param);
            }
        }

        private void lastest_listview_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (App.DeviceInfo.Device_type == Model.DeviceType.Mobile)
            {
                Pages.Mobile.MainPage.mainpage.View_Content((e.ClickedItem as Model.lastest_info).postid.ToString());
            }
            else
            {
                MainPage.mainpage.View_Content((e.ClickedItem as Model.lastest_info).postid.ToString());
            }
        }

        private void cates_gridview_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(Pages.Share.CatePage), (e.ClickedItem as Model.cate));
        }
    }
}