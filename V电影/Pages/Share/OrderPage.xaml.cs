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
    public sealed partial class OrderPage : Page
    {
        private int order_p;
        private bool is_order_loading;

        private ScrollViewer order_listview_sc;

        private ViewModel.OrderPageViewModel viewmodel = new ViewModel.OrderPageViewModel();

        public OrderPage()
        {
            this.InitializeComponent();
            order_p = 1;
            is_order_loading = false;
            this.DataContext = viewmodel;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.NavigationMode == NavigationMode.New)
            {
                await FirstStep();
            }
        }

        private async Task FirstStep()
        {
            string json = await HttpRequest.VmovieRequset.Order_Request(order_p);
            viewmodel.Order_Info = JsonToObject.JsonToObject.Convert_Order_Json(json);
            if (viewmodel.Order_Info.Count == 0)
            {
                has_null_grid.Visibility = Visibility.Visible;
            }
        }

        private void order_listview_Loaded(object sender, RoutedEventArgs e)
        {
            Get_Child(order_listview, 0);
            order_listview_sc.ViewChanging += Order_listview_sc_ViewChanging;
        }

        private async void Order_listview_sc_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            if (is_order_loading)
            {
                return;
            }
            if (e.FinalView.VerticalOffset > (order_listview_sc.ScrollableHeight * 2.0) / 3.0)
            {
                is_order_loading = true;
                string json = await HttpRequest.VmovieRequset.Order_Request(++order_p);
                ObservableCollection<Model.order> lists = JsonToObject.JsonToObject.Convert_Order_Json(json);
                if (lists.Count == 0)
                {
                    return;
                }
                for (int i = 0; i < lists.Count; i++)
                {
                    viewmodel.Order_Info.Add(lists[i]);
                }
            }
            is_order_loading = false;
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
                                order_listview_sc = child as ScrollViewer;
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

        private void order_listview_ItemClick(object sender, ItemClickEventArgs e)
        {
            Model.order item = e.ClickedItem as Model.order;
            if (App.DeviceInfo.Device_type == Model.DeviceType.Mobile)
            {
                Pages.Mobile.MainPage.mainpage.View_Series_Content(item.seriesid, item.update_to);
            }
            else
            {
                MainPage.mainpage.View_Series_Content(item.seriesid, item.update_to);
            }
        }
    }
}
