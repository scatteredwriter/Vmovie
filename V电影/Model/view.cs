using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V电影.Model
{
    public class view
    {
        public int postid { get; set; }
        public string title { get; set; }
        public string app_title { get; set; }
        public string intro { get; set; }
        public int count_comment { get; set; }
        public bool isalbum { get; set; }
        public bool iscollection { get; set; }
        public string image { get; set; }
        public string rating { get; set; }
        public Int64 publish_time { get; set; }
        public int count_like { get; set; }
        public int count_share { get; set; }
        public string cate { get; set; }
        public string tags { get; set; }
        public string qiniu_url { get; set; }
        public string[] content_vids { get; set; }
    }
}
