using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace V电影.Model
{
    public class lastest_info : basemodel
    {
        public List<cate> cates { get; set; }
        public int duration { get; set; }
        public string image { get; set; }
        public bool is_album { get; set; }
        public int like_num { get; set; }
        public int postid { get; set; }
        public Int64 publish_time { get; set; }
        public string rating { get; set; }
        public bool recent_hot { get; set; }
        public string request_url { get; set; }
        public int share_num { get; set; }
        public string title { get; set; }
        public string app_fu_title { get; set; }
        public string wx_small_app_title { get; set; }

        private ImageSource _image_source = new BitmapImage(new Uri("ms-appx:///Assets/main_pic_shadow.png", UriKind.Absolute));
        public ImageSource image_source
        {
            get
            {
                return _image_source;
            }
            set
            {
                _image_source = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                _image_source = value;
                RaisePropertyChanged(nameof(image_source));
            }
        }
    }
}
