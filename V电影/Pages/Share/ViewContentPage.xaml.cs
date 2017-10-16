using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Phone.UI.Input;
using Windows.System.Display;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace V电影.Pages.Share
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ViewContentPage : Page
    {
        Uri uri;
        private int postid;
        private WVJBWebViewClient.Bridge bridge = new WVJBWebViewClient.Bridge();
        private DisplayRequest displayrequest = new DisplayRequest();

        public static ViewContentPage current;

        private MediaTransportControls transport;
        public ViewModel.ViewContentPageViewModel viewmodel = new ViewModel.ViewContentPageViewModel();

        public ViewContentPage()
        {
            this.InitializeComponent();
            webview.Settings.IsJavaScriptEnabled = true;
            transport = new MediaTransportControls();
            transport.IsCompact = true;
            transport.IsVolumeButtonVisible = true;
            transport.Style = MediaTransportStyle;
            bridge.ChangedPlayVideo += Bridge_ChangedPlayVideo;
            bridge.DownloadVideoRequest += Bridge_DownloadVideoRequest;
            bridge.FPCommentRequest += Bridge_FPCommentRequest;
            bridge.FpVideoFullScreenRequest += Bridge_FpVideoFullScreen;
            bridge.NewViewRequest += Bridge_NewViewRequest;
            bridge.OpenUrlRequest += Bridge_OpenUrlRequest;
            CloseCommentView_sb.Completed += CloseCommentView_sb_Completed;
            this.DataContext = viewmodel;
            current = this;
        }

        private void Bridge_OpenUrlRequest(object sender, string e)
        {
            this.Dispatcher?.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                try
                {
                    if (App.DeviceInfo.Device_type == Model.DeviceType.Mobile)
                        Pages.Mobile.MainPage.mainpage.View_Content(e.ToString());
                    else
                        MainPage.mainpage.View_Content(e.ToString());
                }
                catch (Exception)
                {
                }
            });
        }

        private void Bridge_NewViewRequest(object sender, int e)
        {
            this.Dispatcher?.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                try
                {
                    if (App.DeviceInfo.Device_type == Model.DeviceType.Mobile)
                        Pages.Mobile.MainPage.mainpage.View_Content(e.ToString());
                    else
                        MainPage.mainpage.View_Content(e.ToString());
                }
                catch (Exception)
                {
                }
            });
        }

        private void Bridge_FpVideoFullScreen(object sender, bool e)
        {
            this.Dispatcher?.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (e)
                {
                    if (App.DeviceInfo.Device_type == Model.DeviceType.Mobile)
                    {
                        Windows.Graphics.Display.DisplayInformation.AutoRotationPreferences = Windows.Graphics.Display.DisplayOrientations.Landscape;
                    }
                    else
                    {
                        MainPage.mainpage.EnterFullScreenMode(1);
                    }

                }
                else
                {
                    if (App.DeviceInfo.Device_type == Model.DeviceType.Mobile)
                    {
                        Windows.Graphics.Display.DisplayInformation.AutoRotationPreferences = Windows.Graphics.Display.DisplayOrientations.Portrait;
                    }
                    else
                    {
                        MainPage.mainpage.EnterFullScreenMode(0);
                    }
                }
            });
        }

        private void Bridge_FPCommentRequest(object sender, int e)
        {
            this.Dispatcher?.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                try
                {
                    if (App.DeviceInfo.Device_type == Model.DeviceType.Mobile)
                        Pages.Mobile.MainPage.mainpage.View_Content(e.ToString());
                    else
                        MainPage.mainpage.View_Content(e.ToString());
                }
                catch (Exception)
                {
                }
            });
        }

        private void Bridge_DownloadVideoRequest(object sender, int e)
        {
            this.Dispatcher?.RunAsync(CoreDispatcherPriority.Normal, () =>
            {

            });
        }

        private void Bridge_ChangedPlayVideo(object sender, string e)
        {
            this.Dispatcher?.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (viewmodel.View_Info.content_vids.Count() > 1)
                {
                    if (!e.Contains("http:"))
                        mediaelement.Source = new Uri("http:" + new Uri(e));
                    else
                        mediaelement.Source = new Uri(e);
                }
            });
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.NavigationMode == NavigationMode.New || e.NavigationMode == NavigationMode.Back)
            {
                mediaelement.TransportControls = transport;
                try
                {
                    postid = Convert.ToInt32(e.Parameter.ToString());
                    await FirstStep();
                    try
                    {
                        if (viewmodel.View_Info.qiniu_url != null)
                        {
                            if (!viewmodel.View_Info.qiniu_url.Contains("http:"))
                                mediaelement.Source = new Uri("http:" + viewmodel.View_Info.qiniu_url);
                            else
                                mediaelement.Source = new Uri(viewmodel.View_Info.qiniu_url);
                        }
                    }
                    catch (Exception)
                    {
                        mediaelement_grid.Visibility = Visibility.Collapsed;
                        Grid.SetRow(webview_grid, 0);
                        Grid.SetRowSpan(webview_grid, 2);
                    }
                }
                catch (Exception)
                {
                    mediaelement_grid.Visibility = Visibility.Collapsed;
                    command_grid.Visibility = Visibility.Collapsed;
                    Grid.SetRow(webview_grid, 0);
                    Grid.SetRowSpan(webview_grid, 2);
                    uri = new Uri(e.Parameter.ToString());
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
            bridge.FpVideoFullScreenRequest -= Bridge_FpVideoFullScreen;
            CloseCommentView_sb.Completed -= CloseCommentView_sb_Completed;
        }

        private async Task FirstStep()
        {
            string json = await HttpRequest.VmovieRequset.View_Content_Request(postid);
            viewmodel.View_Info = JsonToObject.JsonToObject.Convert_View_Json(json);
        }

        private async void webview_DOMContentLoaded(WebView sender, WebViewDOMContentLoadedEventArgs args)
        {
            string js = "";
            js = "var obj = document.createElement(\"script\");";
            js += "obj.type = \"text/javascript\"; ";
            js += "obj.src = \"ms-appx-web:///Html/JavaScript1.js\";";
            js += "document.body.appendChild(obj);";
            await webview.InvokeScriptAsync("eval", new string[] { js });
        }

        private void webview_Loaded(object sender, RoutedEventArgs e)
        {
            if (postid != 0)
            {
                string api = API.Vmovies_API.request_url_api;
                api = api.Replace("{0}", postid.ToString());
                uri = new Uri(api);
            }
            HttpRequestMessage request = Get_Default_Header(uri);
            webview.NavigateWithHttpRequestMessage(request);
        }

        private void Webview_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            webview.AddWebAllowedObject("WebViewJavascriptBridge", bridge);
        }

        private void webview_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (App.DeviceInfo.Device_type != Model.DeviceType.Mobile)
            {
                if (ApplicationView.GetForCurrentView().IsFullScreenMode)
                {
                    MainPage.mainpage.EnterFullScreenMode(1);
                }
                else
                {
                    MainPage.mainpage.EnterFullScreenMode(0);
                }
            }
        }

        private HttpRequestMessage Get_Default_Header(Uri uri)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Headers.Connection.Add(new Windows.Web.Http.Headers.HttpConnectionOptionHeaderValue("Keep-Alive"));
            request.Headers.AcceptEncoding.Add(new Windows.Web.Http.Headers.HttpContentCodingWithQualityHeaderValue("gzip"));
            request.Headers.UserAgent.Add(new Windows.Web.Http.Headers.HttpProductInfoHeaderValue("VmovierApp", "5.0.9"));
            request.Headers.UserAgent.Add(new Windows.Web.Http.Headers.HttpProductInfoHeaderValue("Android", "6.0"));
            if (App.settings.Values.ContainsKey(Resource.APPTheme.user_auth_key))
            {
                request.Headers.Append("Auth-Key", App.settings.Values[Resource.APPTheme.user_auth_key].ToString());
            }
            return request;
        }

        public void Accept_Uri(string uri, string title)
        {
            mediaelement.Source = new Uri(uri);
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

        private void comment_view_CommentViewColsing(object sender, bool e)
        {
            (CloseCommentView_sb.Children[0] as Windows.UI.Xaml.Media.Animation.DoubleAnimation).To = comment_view_grid.ActualHeight;
            CloseCommentView_sb.Begin();
        }

        private void CloseCommentView_sb_Completed(object sender, object e)
        {
            comment_view_grid.Visibility = Visibility.Collapsed;
        }

        private async void like_num_Click(object sender, RoutedEventArgs e)
        {
            DateTime startTime = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Utc, TimeZoneInfo.Local);
            Int64 timeStamp = (Int64)(DateTime.Now - startTime).TotalSeconds;
            string data = "[{\"postid\": \"{0}\",\"iscollect\": \"{1}\",\"addtime\": \"{2}\"}]";
            data = data.Replace("{0}", viewmodel.View_Info.postid.ToString());
            data = data.Replace("{1}", Convert.ToInt32(!viewmodel.View_Info.iscollection).ToString());
            data = data.Replace("{2}", timeStamp.ToString());
            string json = await HttpRequest.VmovieRequset.Set_Collect_Request(data);
            if (!String.IsNullOrEmpty(json))
            {
                try
                {
                    JObject json_jobject = (JObject)JsonConvert.DeserializeObject(json);
                    string msg = json_jobject["msg"].ToString();
                    if (msg == "ok")
                    {
                        viewmodel.View_Info.iscollection = !viewmodel.View_Info.iscollection;
                        if (viewmodel.View_Info.iscollection)
                        {
                            ((like_num.Content as StackPanel).Children[0] as Image).Source = new BitmapImage(new Uri("ms-appx:///Assets/details_like_finish.png", UriKind.Absolute));
                            ((like_num.Content as StackPanel).Children[1] as TextBlock).Text = (Convert.ToInt32(((like_num.Content as StackPanel).Children[1] as TextBlock).Text) + 1).ToString();
                        }
                        else
                        {
                            ((like_num.Content as StackPanel).Children[0] as Image).Source = new BitmapImage(new Uri("ms-appx:///Assets/details_like.png", UriKind.Absolute));
                            ((like_num.Content as StackPanel).Children[1] as TextBlock).Text = (Convert.ToInt32(((like_num.Content as StackPanel).Children[1] as TextBlock).Text) - 1).ToString();
                        }
                    }
                    else
                    {
                        Control.ShowMessage showmessage = new Control.ShowMessage("收藏", msg, "重试", "", 1);
                        showmessage._popup.IsOpen = true;
                    }
                }
                catch (Exception)
                {
                    Control.ShowMessage showmessage = new Control.ShowMessage("收藏", "操作失败", "重试", "", 1);
                    showmessage._popup.IsOpen = true;
                }
            }
            else
            {
                Control.ShowMessage showmessage = new Control.ShowMessage("收藏", "操作失败", "重试", "", 1);
                showmessage._popup.IsOpen = true;
            }
        }
    }
}
