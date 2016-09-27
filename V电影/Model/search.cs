using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace V电影.Model
{
    public class search : basemodel
    {
        public int total { get; set; }
        public int postid { get; set; }
        public string title { get; set; }
        public Visibility type { get; set; }
        public string app_fu_title { get; set; }
        public bool recent_hot { get; set; }
        public int discussion { get; set; }
        public string image { get; set; }
        public string rating { get; set; }
        public int[] star { get; set; } = new int[5];
        public int duration { get; set; }
        public Int64 publish_time { get; set; }
        public int like_num { get; set; }
        public int share_num { get; set; }
        public string request_url { get; set; }
    }
}
