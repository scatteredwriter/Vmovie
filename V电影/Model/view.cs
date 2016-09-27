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
        public string app_fu_title { get; set; }
        public string intro { get; set; }
        public int count_comment { get; set; }
        public bool is_album { get; set; }
        public bool is_collect { get; set; }
        public ObservableCollection<Model.view_content> content { get; set; }
        public string image { get; set; }
        public string rating { get; set; }
        public Int64 publish_time { get; set; }
        public int count_like { get; set; }
        public int count_share { get; set; }
        public string cate { get; set; }
        public string tags { get; set; }
        public string share_link_sweibo { get; set; }
        public string share_link_weixin { get; set; }
        public string share_link_qzone { get; set; }
        public string share_link_qq { get; set; }
        public string share_sub_title { get; set; }
        public string weibo_share_image { get; set; }
    }
}
