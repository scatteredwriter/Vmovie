using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace V电影.Model
{
    public class cate : basemodel
    {
        public bool cate_type { get; set; }
        public int orderid { get; set; }
        public int cateid { get; set; }
        public string catename { get; set; }
        public string alias { get; set; }
        public string tab { get; set; }
        public string icon { get; set; }

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
