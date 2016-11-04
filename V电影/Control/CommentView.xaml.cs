using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Windows.UI.Xaml.Media.Imaging;
using System.Collections.ObjectModel;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace V电影.Control
{
    public sealed partial class CommentView : UserControl
    {
        private int p = 1;
        private bool is_comment_loading;

        private ScrollViewer comment_listview_sv;

        public int CommentNum
        {
            get
            {
                return (int)GetValue(CommentNumProperty);
            }
            set
            {
                SetValue(CommentNumProperty, value);
            }
        }
        public int PostId
        {
            get
            {
                return (int)GetValue(PostIdProperty);
            }
            set
            {
                SetValue(PostIdProperty, value);
            }
        }
        public int Type
        {
            get
            {
                return (int)GetValue(TypeProperty);
            }
            set
            {
                SetValue(TypeProperty, value);
            }
        }
        public Model.comment_data Reply_Info { get; set; }
        public int Comment_Type { get; set; } = 0;

        private ViewModel.CommentViewViewModel viewmodel = new ViewModel.CommentViewViewModel();

        private static readonly DependencyProperty CommentNumProperty = DependencyProperty.RegisterAttached("CommentNum", typeof(int), typeof(CommentView), new PropertyMetadata(-1, new PropertyChangedCallback(OnCommentNumPropertyChanged)));
        private static readonly DependencyProperty PostIdProperty = DependencyProperty.RegisterAttached("PostId", typeof(int), typeof(CommentView), new PropertyMetadata(0, OnPostIdPropertyChanged));
        private static readonly DependencyProperty TypeProperty = DependencyProperty.RegisterAttached("Type", typeof(int), typeof(CommentView), new PropertyMetadata(-1));

        public CommentView()
        {
            this.InitializeComponent();
            if (!App.settings.Values.ContainsKey(Resource.APPTheme.user_email)) //没有登录
            {
                comment_tb.IsEnabled = false;
            }
            add_comment_but.IsEnabled = false;
            is_comment_loading = false;
            comment_listview_grid.DataContext = viewmodel;
        }

        private async void FirstStep()
        {
            string json = await HttpRequest.VmovieRequset.Get_Comment_Request(PostId, p, Type);
            viewmodel.Comment_Data = JsonToObject.JsonToObject.Convert_Comment_Data_Json(json);
        }

        private static void OnCommentNumPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CommentView cv = d as CommentView;
            if (cv != null)
            {
                cv.comment_num.Text = cv.CommentNum.ToString() + "人评论";
            }
        }

        private static void OnPostIdPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CommentView cv = d as CommentView;
            if (cv.PostId != 0)
            {
                cv.FirstStep();
            }
        }

        private void close_but_Click(object sender, RoutedEventArgs e)
        {
            CommentViewColsing?.Invoke(this, true);
        }

        private async void Approve_Click(object sender, RoutedEventArgs e)
        {
            string json = await HttpRequest.VmovieRequset.Approve_Request(((sender as Button).DataContext as Model.comment_data).commentid, ((sender as Button).DataContext as Model.comment_data).has_approve);
            if (!String.IsNullOrEmpty(json))
            {
                JObject json_object = (JObject)JsonConvert.DeserializeObject(json);
                string msg = json_object["msg"].ToString();
                if (msg == "ok")
                {
                    for (int i = 0; i < viewmodel.Comment_Data.Count; i++)
                    {
                        if (viewmodel.Comment_Data[i].commentid == ((sender as Button).DataContext as Model.comment_data).commentid)
                        {
                            if (!((sender as Button).DataContext as Model.comment_data).has_approve) //已点赞
                            {
                                ((sender as Button).DataContext as Model.comment_data).has_approve = true;
                                (((sender as Button).Content as StackPanel).Children[1] as TextBlock).Text = (Convert.ToInt32((((sender as Button).Content as StackPanel).Children[1] as TextBlock).Text) + 1).ToString();
                                (((sender as Button).Content as StackPanel).Children[0] as Image).Source = new BitmapImage(new Uri("ms-appx:///Assets/comment_handle_.png", UriKind.Absolute));
                            }
                            else //已取消点赞
                            {
                                ((sender as Button).DataContext as Model.comment_data).has_approve = false;
                                (((sender as Button).Content as StackPanel).Children[1] as TextBlock).Text = (Convert.ToInt32((((sender as Button).Content as StackPanel).Children[1] as TextBlock).Text) - 1).ToString();
                                (((sender as Button).Content as StackPanel).Children[0] as Image).Source = new BitmapImage(new Uri("ms-appx:///Assets/comment_handle.png", UriKind.Absolute));
                            }
                        }
                        if (viewmodel.Comment_Data[i].subcomment != null)
                        {
                            for (int j = 0; j < viewmodel.Comment_Data[i].subcomment.Count; j++)
                            {
                                if (viewmodel.Comment_Data[i].subcomment[j].commentid == ((sender as Button).DataContext as Model.comment_data).commentid)
                                {
                                    if (!((sender as Button).DataContext as Model.comment_data).has_approve) //已点赞
                                    {
                                        ((sender as Button).DataContext as Model.comment_data).has_approve = true;
                                        (((sender as Button).Content as StackPanel).Children[1] as TextBlock).Text = (Convert.ToInt32((((sender as Button).Content as StackPanel).Children[1] as TextBlock).Text) + 1).ToString();
                                        (((sender as Button).Content as StackPanel).Children[0] as Image).Source = new BitmapImage(new Uri("ms-appx:///Assets/comment_handle_.png", UriKind.Absolute));
                                    }
                                    else //已取消点赞
                                    {
                                        ((sender as Button).DataContext as Model.comment_data).has_approve = false;
                                        (((sender as Button).Content as StackPanel).Children[1] as TextBlock).Text = (Convert.ToInt32((((sender as Button).Content as StackPanel).Children[1] as TextBlock).Text) - 1).ToString();
                                        (((sender as Button).Content as StackPanel).Children[0] as Image).Source = new BitmapImage(new Uri("ms-appx:///Assets/comment_handle.png", UriKind.Absolute));
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Control.ShowMessage showmessage = new ShowMessage("评论", msg, "确定", "", 1);
                    showmessage._popup.IsOpen = true;
                }
            }
            else
            {
                Control.ShowMessage showmessage = new ShowMessage("评论", "操作失败", "重试", "", 1);
                showmessage._popup.IsOpen = true;
            }
        }

        public event EventHandler<bool> CommentViewColsing;

        private async void add_comment_but_Click(object sender, RoutedEventArgs e)
        {
            string json = "";
            int commentid = -1;
            if (Comment_Type == 1) //回复他人的评论
            {
                commentid = Reply_Info.commentid;
                json = await HttpRequest.VmovieRequset.Add_Comment_Request(PostId, Type, comment_tb.Text, commentid);
            }
            else
            {
                json = await HttpRequest.VmovieRequset.Add_Comment_Request(PostId, Type, comment_tb.Text);
            }
            if (!String.IsNullOrEmpty(json))
            {
                string msg = "";
                JObject json_object = (JObject)JsonConvert.DeserializeObject(json);
                msg = json_object["msg"].ToString();
                if (msg == "评论成功")
                {
                    CommentNum++;
                    try
                    {
                        p = 1;
                        json = await HttpRequest.VmovieRequset.Get_Comment_Request(PostId, p, Type);
                        viewmodel.Comment_Data = JsonToObject.JsonToObject.Convert_Comment_Data_Json(json);
                        //double ver_offest = comment_listview_sv.VerticalOffset;
                        //ObservableCollection<Model.comment_data> lists = new ObservableCollection<Model.comment_data>();
                        //for (int i = 1; i <= p; i++)
                        //{
                        //    json = await HttpRequest.VmovieRequset.Get_Comment_Request(PostId, i, Type);
                        //    ObservableCollection<Model.comment_data> list = JsonToObject.JsonToObject.Convert_Comment_Data_Json(json);
                        //    if (list.Count == 0)
                        //    {
                        //        break;
                        //    }
                        //    for (int j = 0; j < list.Count; j++)
                        //    {
                        //        lists.Add(list[j]);
                        //    }
                        //}
                        //viewmodel.Comment_Data = lists;
                        //comment_listview_sv.ChangeView(null, ver_offest + 50, null, true);
                    }
                    catch (Exception)
                    {
                    }
                    comment_tb.PlaceholderText = "我来说两句...";
                    comment_tb.Text = "";
                }
                else
                {
                    Control.ShowMessage showmessage = new ShowMessage("评论", msg, "确定", "", 1);
                    showmessage._popup.IsOpen = true;
                }
            }
            else
            {
                Control.ShowMessage showmessage = new ShowMessage("评论", "操作失败", "重试", "", 1);
                showmessage._popup.IsOpen = true;
            }
        }

        private void Reply_User_Changed(Model.comment_data cd)
        {
            Model.comment_data comment_data = cd;
            if (Reply_Info == null || Reply_Info.userinfo.username != comment_data.userinfo.username)
            {
                Reply_Info = comment_data;
                comment_tb.PlaceholderText = "回复 " + Reply_Info.userinfo.username;
                comment_tb.Text = "";
            }
        }

        private void comment_lisview_ItemClick(object sender, ItemClickEventArgs e)
        {
            if(comment_tb.IsEnabled)
            {
                Comment_Type = 1;
                Reply_User_Changed(e.ClickedItem as Model.comment_data);
                comment_tb.Focus(FocusState.Programmatic);
            }
        }

        private void comment_tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!(String.IsNullOrEmpty(comment_tb.Text)))
            {
                add_comment_but.IsEnabled = true;
            }
            else
            {
                add_comment_but.IsEnabled = false;
            }
        }

        private void comment_lisview_LayoutUpdated(object sender, object e)
        {
            try
            {

                Get_Child(comment_lisview, 0);
                comment_listview_sv.ViewChanging += Comment_listview_sv_ViewChanging;
            }
            catch (Exception)
            {

            }
        }

        private async void Comment_listview_sv_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            if (is_comment_loading)
            {
                return;
            }
            if (e.FinalView.VerticalOffset > (comment_listview_sv.ScrollableHeight * 2.0) / 3.0)
            {
                is_comment_loading = true;
                string json = await HttpRequest.VmovieRequset.Get_Comment_Request(PostId, ++p, Type);
                ObservableCollection<Model.comment_data> lists = JsonToObject.JsonToObject.Convert_Comment_Data_Json(json);
                if (lists.Count == 0)
                {
                    is_comment_loading = false;
                    return;
                }
                for (int i = 0; i < lists.Count; i++)
                {
                    viewmodel.Comment_Data.Add(lists[i]);
                }
            }
            is_comment_loading = false;
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
                                comment_listview_sv = child as ScrollViewer;
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

    public sealed class CommentView_DataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate WithSubcommentDataTemplate { get; set; }
        public DataTemplate WithoutSubcommentDataTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            Model.comment_data comment_data = item as Model.comment_data;
            if (comment_data != null)
            {
                if (comment_data.subcomment == null)
                {
                    return WithoutSubcommentDataTemplate;
                }
                else
                {
                    return WithSubcommentDataTemplate;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
