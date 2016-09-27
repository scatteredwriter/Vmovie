using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V电影.Model
{
    public class series_view
    {
        public int seriesid { get; set; }
        public string title { get; set; }
        public string image { get; set; }
        public string content { get; set; }
        public string weekly { get; set; }
        public int count_follow { get; set; }
        public bool isfollow { get; set; }
        public string share_link { get; set; }
        public bool is_end { get; set; }
        public int update_to { get; set; }
        public string tag_name { get; set; }
        public int post_num_per_seg { get; set; }
        public ObservableCollection<Model.series_view_lists> posts { get; set; }
    }

    public class series_view_lists
    {
        public string from_to { get; set; }
        public ObservableCollection<Model.series_view_item> items { get; set; }
    }

    public class series_view_item
    {
        public int series_postid { get; set; }
        public int number { get; set; }
        public string title { get; set; }
        public string addtime { get; set; }
        public int duration { get; set; }
        public string thumbnail { get; set; }
        public string source_link { get; set; }
    }
}
