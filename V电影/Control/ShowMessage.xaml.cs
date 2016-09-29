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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace V电影.Control
{
    public sealed partial class ShowMessage : UserControl
    {
        public Popup _popup;
        private string _tilte;
        private string _second_content;
        private string _but1;
        private string _but2;

        public ShowMessage()
        {
            this.InitializeComponent();

            Composition.Animation.Drop_Shadow(root_shadow, root_rectangle, 0f);
            _popup = new Popup();
            _popup.ChildTransitions = new Windows.UI.Xaml.Media.Animation.TransitionCollection();
            Windows.UI.Xaml.Media.Animation.EdgeUIThemeTransition edge = new Windows.UI.Xaml.Media.Animation.EdgeUIThemeTransition();
            edge.Edge = EdgeTransitionLocation.Bottom;
            _popup.ChildTransitions.Add(new Windows.UI.Xaml.Media.Animation.EdgeUIThemeTransition());
            this.Width = Window.Current.Bounds.Width;
            this.Height = Window.Current.Bounds.Height;
            _popup.Child = this;
            this.Loaded += ShowMessage_Loaded;
            this.Unloaded += ShowMessage_Unloaded;
        }

        public ShowMessage(string title, string second_content, string but1, string but2, int type) : this()
        {
            _tilte = title;
            _second_content = second_content;
            _but1 = but1;
            _but2 = but2;
            if (type == 1)
            {
                cancel.Visibility = Visibility.Collapsed;
                ok.Margin = new Thickness(20, 10, 20, 10);
                Grid.SetColumnSpan(ok, 2);
            }
        }

        private void ShowMessage_Loaded(object sender, RoutedEventArgs e)
        {
            title.Text = _tilte;
            second_content.Text = _second_content;
            ok.Content = _but1;
            cancel.Content = _but2;
            Window.Current.SizeChanged += ShowMessage_SizeChanged;
        }

        private void ShowMessage_Unloaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged -= ShowMessage_SizeChanged;
        }

        private void ShowMessage_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            this.Width = e.Size.Width;
            this.Height = e.Size.Height;
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            _popup.IsOpen = false;
            OK_Click?.Invoke(this, e);
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            _popup.IsOpen = false;
            Cancel_Click?.Invoke(this, e);
        }

        public event EventHandler<RoutedEventArgs> OK_Click;
        public event EventHandler<RoutedEventArgs> Cancel_Click;
    }
}
