using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace V电影.Pages.Share
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SearchPage : Page
    {
        private int search_p = 1;
        private bool is_search_loading = false;

        public static SearchPage current;
        private ScrollViewer search_result_sc;

        private SQLite.Search_SQLite sqlite = new SQLite.Search_SQLite();
        public ViewModel.SearchPageViewModel viewmodel = new ViewModel.SearchPageViewModel();

        public SearchPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            this.DataContext = viewmodel;
            current = this;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.NavigationMode == NavigationMode.New)
            {
                viewmodel.Is_Go_Back = false;
                First_Step();
            }
        }

        private void First_Step()
        {
            viewmodel.Search_History = sqlite.Get_All_KeyWord();
            if (viewmodel.Search_History.Count == 0)
            {
                del_history_but.Visibility = Visibility.Collapsed;
            }
        }

        private async void search_tb_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                sqlite.Add_New_KeyWord(search_tb.Text);
                viewmodel.Search_History = sqlite.Get_All_KeyWord();
                await Search(search_tb.Text);
            }
        }

        private void search_tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (search_tb.Text == "")
            {
                search_grid.Visibility = Visibility.Collapsed;
                history_sp.Visibility = Visibility.Visible;
            }
        }

        private async void history_listview_ItemClick(object sender, ItemClickEventArgs e)
        {
            search_tb.Text = e.ClickedItem.ToString();
            await Search(search_tb.Text);
        }

        private void del_history_but_Tapped(object sender, TappedRoutedEventArgs e)
        {
            sqlite.Del_All_KeyWord();
            viewmodel.Search_History = sqlite.Get_All_KeyWord();
            del_history_but.Visibility = Visibility.Collapsed;
        }

        private async Task Search(string keyword)
        {
            history_sp.Visibility = Visibility.Collapsed;
            search_grid.Visibility = Visibility.Visible;
            del_history_but.Visibility = Visibility.Visible;
            string json = await HttpRequest.VmovieRequset.Search_Request(search_p, search_tb.Text);
            viewmodel.Search_Result = JsonToObject.JsonToObject.Convert_Search_Json(json);
            search_result_header.Text = "搜索到" + viewmodel.Search_Result[0].total.ToString() + "条关于" + "\"" + keyword + "\"" + "的内容";
        }

        private void search_result_listview_Loaded(object sender, RoutedEventArgs e)
        {
            Get_Child(search_result_listview, 0);
            search_result_sc.ViewChanging += Search_result_sc_ViewChanging;
        }

        private async void Search_result_sc_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            if (is_search_loading)
            {
                return;
            }
            if (e.FinalView.VerticalOffset > (search_result_sc.ScrollableHeight * 2.0) / 3.0)
            {
                is_search_loading = true;
                string json = await HttpRequest.VmovieRequset.Search_Request(++search_p, search_tb.Text);
                ObservableCollection<Model.search> lists = JsonToObject.JsonToObject.Convert_Search_Json(json);
                if (lists.Count == 0)
                {
                    return;
                }
                for (int i = 0; i < lists.Count; i++)
                {
                    viewmodel.Search_Result.Add(lists[i]);
                }
            }
            is_search_loading = false;
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
                                search_result_sc = child as ScrollViewer;
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

        private void Cancel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (App.DeviceInfo.Device_type == Model.DeviceType.Mobile)
            {
                Pages.Mobile.MainPage.mainpage.Second_Frame_Go_Back();
            }
            else
            {
                MainPage.mainpage.Second_Frame_Go_Back();
            }
        }

        private void search_result_listview_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (App.DeviceInfo.Device_type == Model.DeviceType.Mobile)
            {
                Pages.Mobile.MainPage.mainpage.View_Content((e.ClickedItem as Model.search).postid.ToString());
            }
            else
            {
                MainPage.mainpage.View_Content((e.ClickedItem as Model.search).postid.ToString());
            }
        }
    }
}
