using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace V电影.Model
{
    public class series : basemodel
    {
        public int seriesid { get; set; }
        public string title { get; set; }
        public string image { get; set; }
        public string weekly { get; set; }
        public string content { get; set; }
        public string app_image { get; set; }
        public bool isfollow { get; set; }
        public bool is_end { get; set; }
        public int update_to { get; set; }
        public int follower_num { get; set; }

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
