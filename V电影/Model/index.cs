using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace V电影.Model
{

    public class Index
    {
        public string status { get; set; }
        public string msg { get; set; }
        public Data data { get; set; }
        public class Data
        {
            public Banner banner { get; set; }
            public Classic classic { get; set; }
            public Hot hot { get; set; }
            public Album album { get; set; }
            public Today today { get; set; }
            public Posts posts { get; set; }

            public class Cate
            {
                public string catename { get; set; }
                public string cateid { get; set; }
            }

            public class Banner
            {
                public string selection_title { get; set; }
                public string total { get; set; }
                public List<List> list { get; set; }
                public string resource_type { get; set; }

                public class List : basemodel
                {
                    public string showtype { get; set; }
                    public string bannerid { get; set; }
                    public string image { get; set; }
                    public string title { get; set; }
                    public Extra_Data extra_data { get; set; }

                    private ImageSource _image_source = new BitmapImage(new Uri("ms-appx:///Assets/main_pic_shadow.png", UriKind.Absolute));
                    public ImageSource image_source
                    {
                        get
                        {
                            return _image_source;
                        }
                        set
                        {
                            _image_source = value;
                            RaisePropertyChanged(nameof(image_source));
                        }
                    }
                    public class Extra_Data
                    {
                        public string app_banner_type { get; set; }
                        public string app_banner_param { get; set; }
                        public string app_show_type { get; set; }
                        public string is_album { get; set; }
                    }
                }
            }

            public class Classic
            {
                public string total { get; set; }
                public string selection_title { get; set; }
                public List<List> list { get; set; }
                public string resource_type { get; set; }

                public class List : basemodel
                {
                    public string id { get; set; }
                    public string postid { get; set; }
                    public string title { get; set; }
                    public string description { get; set; }
                    public string image1 { get; set; }
                    public string image2 { get; set; }
                    public string userid { get; set; }
                    public string status { get; set; }
                    public string day_unixtime { get; set; }
                    public string count_view { get; set; }
                    public string uptime { get; set; }
                    public string addtime { get; set; }
                    public string image { get; set; }
                    public string showtype { get; set; }
                    public List<Cate> cate { get; set; }

                    private ImageSource _image_source = new BitmapImage(new Uri("ms-appx:///Assets/main_pic_shadow.png", UriKind.Absolute));
                    public ImageSource image_source
                    {
                        get
                        {
                            return _image_source;
                        }
                        set
                        {
                            _image_source = value;
                            RaisePropertyChanged(nameof(image_source));
                        }
                    }

                }
            }

            public class Hot
            {
                public List<List> list { get; set; }
                public string selection_title { get; set; }
                public string scheme { get; set; }
                public string total { get; set; }
                public string resource_type { get; set; }

                public class List : basemodel
                {
                    public string postid { get; set; }
                    public string title { get; set; }
                    public string wx_small_app_title { get; set; }
                    public string pid { get; set; }
                    public string app_fu_title { get; set; }
                    public string is_xpc { get; set; }
                    public string is_promote { get; set; }
                    public string is_xpc_zp { get; set; }
                    public string is_xpc_cp { get; set; }
                    public string is_xpc_fx { get; set; }
                    public string is_album { get; set; }
                    public string tags { get; set; }
                    public string recent_hot { get; set; }
                    public string discussion { get; set; }
                    public string image { get; set; }
                    public string rating { get; set; }
                    public string duration { get; set; }
                    public string publish_time { get; set; }
                    public string like_num { get; set; }
                    public string share_num { get; set; }
                    public List<Cate> cates { get; set; }
                    public string request_url { get; set; }
                    public string ispromote { get; set; }
                    public string isalbum { get; set; }
                    public string showtype { get; set; }
                    public List<Cate> cate { get; set; }

                    private ImageSource _image_source = new BitmapImage(new Uri("ms-appx:///Assets/main_pic_shadow.png", UriKind.Absolute));
                    public ImageSource image_source
                    {
                        get
                        {
                            return _image_source;
                        }
                        set
                        {
                            _image_source = value;
                            RaisePropertyChanged(nameof(image_source));
                        }
                    }

                }
            }

            public class Album
            {
                public string total { get; set; }
                public string selection_title { get; set; }
                public string scheme { get; set; }
                public string resource_type { get; set; }
                public List<List> list { get; set; }
                public class List : basemodel
                {
                    public string postid { get; set; }
                    public string title { get; set; }
                    public string wx_small_app_title { get; set; }
                    public string pid { get; set; }
                    public string app_fu_title { get; set; }
                    public string is_xpc { get; set; }
                    public string is_xpc_zp { get; set; }
                    public string is_xpc_cp { get; set; }
                    public string is_xpc_fx { get; set; }
                    public string tags { get; set; }
                    public string recent_hot { get; set; }
                    public string discussion { get; set; }
                    public string image { get; set; }
                    public string rating { get; set; }
                    public string duration { get; set; }
                    public string publish_time { get; set; }
                    public string like_num { get; set; }
                    public string share_num { get; set; }
                    public List<Cate> cates { get; set; }
                    public string request_url { get; set; }
                    public string showtype { get; set; }
                    public string ispromote { get; set; }
                    public string isalbum { get; set; }
                    public List<Cate> cate { get; set; }

                    private ImageSource _image_source = new BitmapImage(new Uri("ms-appx:///Assets/main_pic_shadow.png", UriKind.Absolute));
                    public ImageSource image_source
                    {
                        get
                        {
                            return _image_source;
                        }
                        set
                        {
                            _image_source = value;
                            RaisePropertyChanged(nameof(image_source));
                        }
                    }

                }
            }

            public class Today
            {
                public string total { get; set; }
                public string selection_title { get; set; }
                public List<List> list { get; set; }
                public string lastid { get; set; }
                public string resource_type { get; set; }
                public class List : basemodel
                {
                    public string postid { get; set; }
                    public string title { get; set; }
                    public string wx_small_app_title { get; set; }
                    public string pid { get; set; }
                    public string app_fu_title { get; set; }
                    public string is_xpc { get; set; }
                    public string is_xpc_zp { get; set; }
                    public string is_xpc_cp { get; set; }
                    public string is_xpc_fx { get; set; }
                    public string tags { get; set; }
                    public string recent_hot { get; set; }
                    public string discussion { get; set; }
                    public string image { get; set; }
                    public string rating { get; set; }
                    public string duration { get; set; }
                    public string publish_time { get; set; }
                    public string like_num { get; set; }
                    public string share_num { get; set; }
                    public string request_url { get; set; }
                    public string showtype { get; set; }
                    public string ispromote { get; set; }
                    public string isalbum { get; set; }
                    public List<Cate> cate { get; set; }

                    private ImageSource _image_source = new BitmapImage(new Uri("ms-appx:///Assets/main_pic_shadow.png", UriKind.Absolute));
                    public ImageSource image_source
                    {
                        get
                        {
                            return _image_source;
                        }
                        set
                        {
                            _image_source = value;
                            RaisePropertyChanged(nameof(image_source));
                        }
                    }

                }
            }

            public class Posts
            {
                public string total { get; set; }
                public string selection_title { get; set; }
                public string next_page_url { get; set; }
                public string next_page_url_full { get; set; }
                public List<List> list { get; set; }
                public string lastid { get; set; }
                public string resource_type { get; set; }
                public class List : basemodel
                {
                    public string postid { get; set; }
                    public string title { get; set; }
                    public string wx_small_app_title { get; set; }
                    public string pid { get; set; }
                    public string app_fu_title { get; set; }
                    public string is_xpc { get; set; }
                    public string is_xpc_zp { get; set; }
                    public string is_xpc_cp { get; set; }
                    public string is_xpc_fx { get; set; }
                    public string tags { get; set; }
                    public string recent_hot { get; set; }
                    public string discussion { get; set; }
                    public string image { get; set; }
                    public string rating { get; set; }
                    public string duration { get; set; }
                    public string publish_time { get; set; }
                    public string like_num { get; set; }
                    public string share_num { get; set; }
                    public string request_url { get; set; }
                    public string showtype { get; set; }
                    public string ispromote { get; set; }
                    public string isalbum { get; set; }
                    public List<Cate> cate { get; set; }

                    private ImageSource _image_source = new BitmapImage(new Uri("ms-appx:///Assets/main_pic_shadow.png", UriKind.Absolute));
                    public ImageSource image_source
                    {
                        get
                        {
                            return _image_source;
                        }
                        set
                        {
                            _image_source = value;
                            RaisePropertyChanged(nameof(image_source));
                        }
                    }

                }
            }
        }
    }

}
