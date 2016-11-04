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
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace V电影.Pages.Share
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class CatePage : Page
    {
        private int cate_p = 1;
        private int cateid;
        private string tab;

        private bool is_cate_content_loading = false;

        private ScrollViewer cate_listview_sc = null;
        private ViewModel.CatePageViewModel viewmodel = new ViewModel.CatePageViewModel();

        public CatePage()
        {
            this.InitializeComponent();
            this.DataContext = viewmodel;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.NavigationMode == NavigationMode.New)
            {
                if ((e.Parameter != null) && e.Parameter.ToString() != "")
                {
                    Model.cate cate = e.Parameter as Model.cate;
                    cateid = (int)cate.cateid;
                    title.Text = cate.catename;
                    tab = cate.tab;
                    await First_Step();
                }
            }
        }

        private async Task First_Step()
        {
            string json = await HttpRequest.VmovieRequset.Cate_Content_Requset(cate_p, cateid, tab);
            viewmodel.Cate_Info = JsonToObject.JsonToObject.Convert_Lastest_Json(json);
            Cache.ImageCache imagecache = new Cache.ImageCache();
            viewmodel.Cate_Content_Image_Sbs = new ObservableCollection<ImageSource>();
            for (int i = 0; i < viewmodel.Cate_Info.Count; i++)
            {
                ImageSource imagesource = await imagecache.Get_Image_Source(viewmodel.Cate_Info[i].image, "Cate_Content");
                viewmodel.Cate_Content_Image_Sbs.Add(imagesource);
            }
            viewmodel.Cate_New_Count = viewmodel.Cate_Info.Count;

            await Task.Delay(500);
            cate_listview_sc.ChangeView(null, 30, null);
        }

        private async void Lastest_Refresh()
        {
            if (cate_listview_sc.VerticalOffset == 0.0)
            {
                string json = await HttpRequest.VmovieRequset.Cate_Content_Requset(cate_p = 1, cateid, tab);
                viewmodel.Cate_Info = JsonToObject.JsonToObject.Convert_Lastest_Json(json);
                Cache.ImageCache imagecache = new Cache.ImageCache();
                viewmodel.Cate_Content_Image_Sbs = new ObservableCollection<ImageSource>();
                for (int i = 0; i < viewmodel.Cate_Info.Count; i++)
                {
                    ImageSource imagesource = await imagecache.Get_Image_Source(viewmodel.Cate_Info[i].image, "Cate_Content");
                    viewmodel.Cate_Content_Image_Sbs.Add(imagesource);
                }
                viewmodel.Cate_New_Count = viewmodel.Cate_Info.Count;

                await Task.Delay(500);

                //移动回顶部
                for (double i = 0.0; i < (30 - cate_listview_sc.VerticalOffset); i += 1)
                {
                    cate_listview_sc.ChangeView(null, cate_listview_sc.VerticalOffset + i, null);
                }
                cate_listview_sc.ChangeView(null, 30, null);
            }
        }

        private void cate_listview_Loaded(object sender, RoutedEventArgs e)
        {
            Get_Child((DependencyObject)sender, 0);
            cate_listview_sc.ViewChanging += Lastest_listview_sc_ViewChanging;
            cate_listview_sc.ViewChanged += Lastest_listview_sc_ViewChanged;
        }

        private async void Lastest_listview_sc_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            if (App.DeviceInfo.Device_type != Model.DeviceType.PC)
            {
                if (cate_listview_sc.VerticalOffset <= 10.0)
                {
                    if (e.IsInertial)
                    {
                        for (double i = 0.0; i < (30.0 - cate_listview_sc.VerticalOffset); i += 1.0)
                        {
                            cate_listview_sc.ChangeView(null, cate_listview_sc.VerticalOffset + i, null);
                        }
                        cate_listview_sc.ChangeView(null, 30.0, null);
                    }
                }
            }
            else
            {
                if (cate_listview_sc.VerticalOffset <= 50.0 && cate_listview_sc.VerticalOffset > 30.0)
                {
                    if (e.IsInertial)
                    {
                        for (double i = 0.0; i < (30.0 - cate_listview_sc.VerticalOffset); i += 1.0)
                        {
                            cate_listview_sc.ChangeView(null, cate_listview_sc.VerticalOffset + i, null);
                        }
                        cate_listview_sc.ChangeView(null, 30.0, null);
                    }
                }
            }
            if (is_cate_content_loading)
            {
                return;
            }
            if (e.FinalView.VerticalOffset > (cate_listview_sc.ScrollableHeight * 2.0) / 3.0)
            {
                is_cate_content_loading = true;
                string json = await HttpRequest.VmovieRequset.Cate_Content_Requset(++cate_p, cateid, tab);
                ObservableCollection<Model.lastest_info> lists = JsonToObject.JsonToObject.Convert_Lastest_Json(json);
                ObservableCollection<ImageSource> sources = new ObservableCollection<ImageSource>();
                if (lists.Count == 0)
                {
                    return;
                }
                for (int i = 0; i < lists.Count; i++)
                {
                    viewmodel.Cate_Info.Add(lists[i]);
                }
                viewmodel.Cate_New_Count = viewmodel.Cate_Info.Count - viewmodel.Cate_New_Count;
                Cache.ImageCache imagecache = new Cache.ImageCache();
                for (int i = (viewmodel.Cate_Info.Count - viewmodel.Cate_New_Count); i < viewmodel.Cate_Info.Count; i++)
                {
                    ImageSource imagesource = await imagecache.Get_Image_Source(viewmodel.Cate_Info[i].image, "Cate_Content");
                    viewmodel.Cate_Content_Image_Sbs.Add(imagesource);
                }
                cate_new_border.Visibility = Visibility.Visible;
                await Task.Delay(2000);
                cate_new_border.Visibility = Visibility.Collapsed;
                viewmodel.Cate_New_Count = viewmodel.Cate_Info.Count;
            }
            is_cate_content_loading = false;
        }

        private async void Lastest_listview_sc_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (cate_listview_sc.VerticalOffset == cate_listview_sc.ScrollableHeight)
            {
                viewmodel.Cate_New_Count = 0;
                cate_new_border.Visibility = Visibility.Visible;
                await Task.Delay(2000);
                cate_new_border.Visibility = Visibility.Collapsed;
                return;
            }
            if (!e.IsIntermediate)
            {
                Lastest_Refresh();
            }
        }

        private void back_but_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }

        private void View_Content_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (App.DeviceInfo.Device_type == Model.DeviceType.Mobile)
            {
                Pages.Mobile.MainPage.mainpage.View_Content(((sender as RelativePanel).DataContext as Model.lastest_info).postid.ToString());
            }
            else
            {
                MainPage.mainpage.View_Content(((sender as RelativePanel).DataContext as Model.lastest_info).postid.ToString());
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
                                cate_listview_sc = child as ScrollViewer;
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
    }

    public class ListViewItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Album_DataTemplate { get; set; }
        public DataTemplate Normal_DataTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            Model.lastest_info model = item as Model.lastest_info;
            if (model.is_album)
            {
                return Album_DataTemplate;
            }
            else
            {
                return Normal_DataTemplate;
            }
        }
    }
}
