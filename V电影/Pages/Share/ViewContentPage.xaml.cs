using Microsoft.Toolkit.Uwp.UI.Animations;
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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

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

        public static ViewContentPage current;

        private MediaTransportControls transport;
        private ViewModel.ViewContentPageViewModel viewmodel = new ViewModel.ViewContentPageViewModel();

        public ViewContentPage()
        {
            this.InitializeComponent();
            webview.Settings.IsJavaScriptEnabled = true;
            webview.NavigationStarting += Webview_NavigationStarting;
            webview.NavigationCompleted += Webview_NavigationCompleted;
            webview.ScriptNotify += Webview_ScriptNotify;
            bridge.MessageQueue_Changed += Bridge_MessageQueue_Changed;
            transport = new MediaTransportControls();
            transport.IsCompact = true;
            transport.IsVolumeButtonVisible = true;
            transport.Style = MediaTransportStyle;
            this.DataContext = viewmodel;
            current = this;
        }

        private void Webview_ScriptNotify(object sender, NotifyEventArgs e)
        {
            Debug.WriteLine("收到消息了");
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
                    switch (viewmodel.View_Info.content.Count)
                    {
                        case 0:
                            {
                                mediaelement_grid.Visibility = Visibility.Collapsed;
                                Grid.SetRow(webview_grid, 0);
                                Grid.SetRowSpan(webview_grid, 2);
                            }; break;
                        case 1:
                            {
                                mediaelement.Source = new Uri(viewmodel.View_Info.content[0].qiniu_url);

                            }; break;
                        default:
                            {
                                mediaelement.Source = new Uri(viewmodel.View_Info.content[0].qiniu_url);
                            }; break;
                    }
                }
                catch (Exception)
                {
                    mediaelement_grid.Visibility = Visibility.Collapsed;
                    Grid.SetRow(webview_grid, 0);
                    Grid.SetRowSpan(webview_grid, 2);
                    uri = new Uri(e.Parameter.ToString());
                }
            }
        }

        private async Task FirstStep()
        {
            string json = await HttpRequest.VmovieRequset.View_Content_Request(postid);
            viewmodel.View_Info = JsonToObject.JsonToObject.Convert_View_Json(json);
        }

        private void Webview_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            //await webview.InvokeScriptAsync("connectWebViewJavascriptBridge", null);
        }

        private void Bridge_MessageQueue_Changed(object sender, int e)
        {
            for (int i = 0; i < e; i++)
            {
                Debug.WriteLine(bridge.StartUp_MessageQueue[i].handlerName);
            }
        }

        private void Webview_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            webview.AddWebAllowedObject("WebViewJavascriptBridge", bridge);
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

        public void Accept_Uri(string uri, string title)
        {
            mediaelement.Source = new Uri(uri);
        }

        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void Canvas_Loaded(object sender, RoutedEventArgs e)
        {
            await ((sender as Canvas).Parent as Grid).Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ((sender as Canvas).Parent as Grid).Blur(10, 1, 0).Start();
            });
        }
    }
}
