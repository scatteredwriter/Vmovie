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
    public sealed partial class LikePage : Page
    {
        private int film_p = 1;
        private int behind_p = 1;
        private int pivot_selectedindex = 0;

        private bool is_film_listview_loading = false;
        private bool is_behind_listview_loading = false;

        private ScrollViewer film_listview_sc;
        private ScrollViewer behind_listview_sc;

        private ViewModel.LikePageViewModel viewmodel;

        public LikePage()
        {
            this.InitializeComponent();
            viewmodel = new ViewModel.LikePageViewModel();
            this.DataContext = viewmodel;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.NavigationMode == NavigationMode.New)
            {
                await First_Step();
            }
        }

        private async Task First_Step()
        {
            string json = await HttpRequest.VmovieRequset.Collect_Request(film_p, 1);
            viewmodel.Collect_Film = JsonToObject.JsonToObject.Convert_Collect_Json(json);
            json = await HttpRequest.VmovieRequset.Collect_Request(behind_p, 2);
            viewmodel.Collect_Behind = JsonToObject.JsonToObject.Convert_Collect_Json(json);
        }

        private void pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

        private void back_but_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }

        private void film_listview_Loaded(object sender, RoutedEventArgs e)
        {
            Get_Child(film_listview, 0);
            film_listview_sc.ViewChanging += Film_listview_sc_ViewChanging;
        }

        private async void Film_listview_sc_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            if (is_film_listview_loading)
            {
                return;
            }
            if (e.FinalView.VerticalOffset > (film_listview_sc.ScrollableHeight * 2.0) / 3.0)
            {
                is_film_listview_loading = true;
                string json = await HttpRequest.VmovieRequset.Collect_Request(++film_p, 1);
                ObservableCollection<Model.collect> lists = JsonToObject.JsonToObject.Convert_Collect_Json(json);
                if (lists.Count == 0)
                {
                    return;
                }
                for (int i = 0; i < lists.Count; i++)
                {
                    viewmodel.Collect_Film.Add(lists[i]);
                }
            }
            is_film_listview_loading = false;
        }

        private void behind_listview_Loaded(object sender, RoutedEventArgs e)
        {
            Get_Child(behind_listview, 1);
            behind_listview_sc.ViewChanging += Behind_listview_sc_ViewChanging;
        }

        private async void Behind_listview_sc_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            if (is_behind_listview_loading)
            {
                return;
            }
            if (e.FinalView.VerticalOffset > (film_listview_sc.ScrollableHeight * 2.0) / 3.0)
            {
                is_behind_listview_loading = true;
                string json = await HttpRequest.VmovieRequset.Collect_Request(++behind_p, 2);
                ObservableCollection<Model.collect> lists = JsonToObject.JsonToObject.Convert_Collect_Json(json);
                if (lists.Count == 0)
                {
                    return;
                }
                for (int i = 0; i < lists.Count; i++)
                {
                    viewmodel.Collect_Behind.Add(lists[i]);
                }
            }
            is_behind_listview_loading = false;
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
                                film_listview_sc = child as ScrollViewer;
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
                                behind_listview_sc = child as ScrollViewer;
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
            }
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (App.DeviceInfo.Device_type == Model.DeviceType.Mobile)
            {
                Pages.Mobile.MainPage.mainpage.View_Content((e.ClickedItem as Model.collect).postid.ToString());
            }
            else
            {
                MainPage.mainpage.View_Content((e.ClickedItem as Model.collect).postid.ToString());
            }
        }
    }
}
