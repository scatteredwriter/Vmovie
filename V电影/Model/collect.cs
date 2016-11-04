using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V电影.Model
{
    public class collect
    {
        public int count_share { get; set; }
        public int count_like { get; set; }
        public int postid { get; set; }
        public string title { get; set; }
        public Int64 publish_time { get; set; }
        public int duration { get; set; }
        public string rating { get; set; }
        public int[] star { get; set; } = new int[5];
        public string image { get; set; }
        public Int64 collect_time { get; set; }
        public string app_fu_title { get; set; }
        public bool is_album { get; set; }
    }
}
