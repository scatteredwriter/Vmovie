using Microsoft.Toolkit.Uwp.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace V电影.Pages.Share
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class BehindPage : Page
    {
        private int pivot_selectedindex = 0;

        private bool is_first_navigation = true;

        private UIElementCollection header_items;
        private UIElementCollection static_header_items;

        private Resource.APPTheme apptheme = new Resource.APPTheme();
        private ViewModel.BehindPageViewModel viewmodel = new ViewModel.BehindPageViewModel();

        public BehindPage()
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
                Get_Child(pivot, 0);
                Get_Child(pivot, 2);
                Dispatcher?.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    (((((Get_Header()[0] as PivotHeaderItem).ContentTemplateRoot) as Grid).Children[0] as Grid).Children[0] as TextBlock).Margin = new Thickness(0, 0, 0, 8);
                    ((((Get_Header()[0] as PivotHeaderItem).ContentTemplateRoot) as Grid).Children[0] as Grid).BorderThickness = new Thickness(0, 0, 0, 2);
                });
            }
        }

        private async Task First_Step()
        {
            string json = await HttpRequest.VmovieRequset.Behind_Cates_Request();
            viewmodel.Behind_Data = JsonToObject.JsonToObject.Convert_Behind_Cates_Json(json);
        }

        private UIElementCollection Get_Header()
        {
            if (header_items.Count != 0)
            {
                return header_items;
            }
            else
            {
                return static_header_items;
            }
        }

        private async void pivot_PivotItemLoaded(Pivot sender, PivotItemEventArgs args)
        {
            ImageCache imagecache = new ImageCache();
            await imagecache.InitializeAsync(await Params.Params.Get_ImageCacheFolder(), Params.Params.behind_floder);
            if (pivot_selectedindex < 0)
            {
                return;
            }
            if (viewmodel.Behind_Data[pivot_selectedindex].Is_Loaded && (args.Item.ContentTemplateRoot as ListView) != null)
            {
                return;
            }
            try
            {
                string json = await HttpRequest.VmovieRequset.Behind_Content_Request(viewmodel.Behind_Data[pivot_selectedindex].Behind_Info_P, viewmodel.Behind_Data[pivot_selectedindex].cateid);
                viewmodel.Behind_Data[pivot_selectedindex].Behind_Info = JsonToObject.JsonToObject.Convert_Behind_Info_Json(json);
                if (viewmodel.Behind_Data[pivot_selectedindex].Behind_Info != null)
                {
                    for (int i = 0; i < viewmodel.Behind_Data[pivot_selectedindex].Behind_Info.Count; i++)
                    {
                        try
                        {
                            if (viewmodel.Behind_Data[pivot_selectedindex].Behind_Info != null)
                            {
                                string uri = viewmodel.Behind_Data[pivot_selectedindex].Behind_Info[i].image;
                                ImageSource imagesource = await imagecache.GetFromCacheAsync(new Uri(uri), uri.Substring(uri.LastIndexOf('/') + 1));
                                viewmodel.Behind_Data[pivot_selectedindex].Behind_Info[i].image_source = imagesource;
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                    viewmodel.Behind_Data[pivot_selectedindex].Behind_Info_New_Count = viewmodel.Behind_Data[pivot_selectedindex].Behind_Info.Count;
                    viewmodel.Behind_Data[pivot_selectedindex].Is_Loaded = true;
                }
            }
            catch (Exception)
            {
            }
        }

        private void pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                (((((Get_Header()[pivot_selectedindex] as PivotHeaderItem).ContentTemplateRoot) as Grid).Children[0] as Grid).Children[0] as TextBlock).Margin = new Thickness(0, 0, 0, 10);
                ((((Get_Header()[pivot_selectedindex] as PivotHeaderItem).ContentTemplateRoot) as Grid).Children[0] as Grid).BorderThickness = new Thickness(0, 0, 0, 0);
                pivot_selectedindex = (sender as Pivot).SelectedIndex;
                (((((Get_Header()[pivot.SelectedIndex] as PivotHeaderItem).ContentTemplateRoot) as Grid).Children[0] as Grid).Children[0] as TextBlock).Margin = new Thickness(0, 0, 0, 8);
                ((((Get_Header()[pivot.SelectedIndex] as PivotHeaderItem).ContentTemplateRoot) as Grid).Children[0] as Grid).BorderThickness = new Thickness(0, 0, 0, 2);
            }
            catch (Exception)
            {
                return;
            }
        }

        private void ListView_Loaded(object sender, RoutedEventArgs e)
        {
            Get_Child((DependencyObject)sender, 1);
            viewmodel.Behind_Data[pivot_selectedindex].Listview_Scrollviewer.ViewChanging += Listview_Scrollviewer_ViewChanging;
        }

        private async void Listview_Scrollviewer_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            int index = 0;
            for (int i = 0; i < viewmodel.Behind_Data.Count; i++)
            {
                if (((sender as ScrollViewer).DataContext as Model.behind_data).cateid == viewmodel.Behind_Data[i].cateid)
                {
                    index = i;
                }
            }
            if (viewmodel.Behind_Data[index].Is_Behind_Loading)
            {
                return;
            }
            if (e.FinalView.VerticalOffset > (viewmodel.Behind_Data[index].Listview_Scrollviewer.ScrollableHeight * 2.0) / 3.0)
            {
                try
                {
                    viewmodel.Behind_Data[index].Is_Behind_Loading = true;
                    string json = await HttpRequest.VmovieRequset.Behind_Content_Request(++viewmodel.Behind_Data[index].Behind_Info_P, viewmodel.Behind_Data[index].cateid);
                    ObservableCollection<Model.behind_info> lists = JsonToObject.JsonToObject.Convert_Behind_Info_Json(json);
                    ObservableCollection<ImageSource> sources = new ObservableCollection<ImageSource>();
                    if (lists.Count == 0)
                    {
                        return;
                    }
                    for (int i = 0; i < lists.Count; i++)
                    {
                        if (viewmodel.Behind_Data[index].Behind_Info != null)
                            viewmodel.Behind_Data[index].Behind_Info.Add(lists[i]);
                    }
                    viewmodel.Behind_Data[index].Behind_Info_New_Count = lists.Count;
                    ImageCache imagecache = new ImageCache();
                    await imagecache.InitializeAsync(await Params.Params.Get_ImageCacheFolder(), Params.Params.behind_floder);
                    if (viewmodel.Behind_Data[pivot_selectedindex].Behind_Info != null)
                    {
                        for (int i = (viewmodel.Behind_Data[index].Behind_Info.Count - viewmodel.Behind_Data[index].Behind_Info_New_Count); i < viewmodel.Behind_Data[index].Behind_Info.Count; i++)
                        {
                            if (viewmodel.Behind_Data[pivot_selectedindex].Behind_Info != null)
                            {
                                string uri = viewmodel.Behind_Data[pivot_selectedindex].Behind_Info[i].image;
                                ImageSource imagesource = await imagecache.GetFromCacheAsync(new Uri(uri), uri.Substring(uri.LastIndexOf('/') + 1));
                                viewmodel.Behind_Data[pivot_selectedindex].Behind_Info[i].image_source = imagesource;
                            }
                        }
                        behind_info_new_border.Visibility = Visibility.Visible;
                        behind_new_count_tb.Text = "本次加载了" + viewmodel.Behind_Data[index].Behind_Info_New_Count.ToString() + "条新内容";
                        await Task.Delay(2000);
                        behind_info_new_border.Visibility = Visibility.Collapsed;
                        viewmodel.Behind_Data[index].Behind_Info_New_Count = viewmodel.Behind_Data[index].Behind_Info.Count;
                    }
                }
                catch (Exception)
                {
                }
            }
            viewmodel.Behind_Data[index].Is_Behind_Loading = false;
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
                            if (child is PivotPanel)
                            {
                                header_items = (((((child as PivotPanel).Children[0] as Grid).Children[1] as ContentControl).Content as Grid).Children[1] as PivotHeaderPanel).Children;
                            }
                            else
                            {
                                Get_Child(VisualTreeHelper.GetChild(o, count - 1), n);
                            }
                        }
                        else if (n == 1)
                        {
                            if (child is ScrollViewer)
                            {
                                viewmodel.Behind_Data[pivot_selectedindex].Listview_Scrollviewer = child as ScrollViewer;
                            }
                            else
                            {
                                Get_Child(VisualTreeHelper.GetChild(o, count - 1), n);
                            }
                        }
                        else if (n == 2)
                        {
                            if (child is PivotPanel)
                            {
                                static_header_items = (((((child as PivotPanel).Children[0] as Grid).Children[1] as ContentControl).Content as Grid).Children[0] as PivotHeaderPanel).Children;
                            }
                            else
                            {
                                Get_Child(VisualTreeHelper.GetChild(o, count - 1), n);
                            }
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

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (App.DeviceInfo.Device_type == Model.DeviceType.Mobile)
            {
                Pages.Mobile.MainPage.mainpage.View_Content((e.ClickedItem as Model.behind_info).postid.ToString());
            }
            else
            {
                MainPage.mainpage.View_Content((e.ClickedItem as Model.behind_info).postid.ToString());
            }
        }
    }
}
