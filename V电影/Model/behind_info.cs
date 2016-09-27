using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace V电影.Model
{
    public class behind_info : basemodel
    {
        public int postid { get; set; }
        public string title { get; set; }
        public string app_fu_title { get; set; }
        public bool recent_hot { get; set; }
        public string image { get; set; }
        public string rating { get; set; }
        public int duration { get; set; }
        public int publish_time { get; set; }
        public int like_num { get; set; }
        public int share_num { get; set; }
        public string request_url { get; set; }

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
