using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace V电影.Pages.Share
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SeriesViewPage : Page
    {
        public SeriesViewPage()
        {
            this.InitializeComponent();
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
