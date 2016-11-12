using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace V电影.Model
{
    public class series_view : basemodel
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

        private ObservableCollection<Model.series_view_lists> _posts;
        public ObservableCollection<Model.series_view_lists> posts
        {
            get
            {
                return _posts;
            }
            set
            {
                _posts = value;
                RaisePropertyChanged(nameof(posts));
            }
        }
    }

    public class series_view_lists : basemodel
    {
        public string from_to { get; set; }

        private ObservableCollection<Model.series_view_item> _items;
        public ObservableCollection<Model.series_view_item> items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
                RaisePropertyChanged(nameof(items));
            }
        }
    }

    public class series_view_item : basemodel
    {
        public int series_postid { get; set; }
        public int number { get; set; }
        public string title { get; set; }
        public string addtime { get; set; }
        public int duration { get; set; }
        public string thumbnail { get; set; }
        public string source_link { get; set; }

        private Visibility _is_playing = Visibility.Collapsed;
        public Visibility is_playing
        {
            get
            {
                return _is_playing;
            }
            set
            {
                _is_playing = value;
                RaisePropertyChanged(nameof(is_playing));
            }
        }
    }

    public class series_video_info : basemodel
    {
        public string title { get; set; }
        public int seriesid { get; set; }
        public int series_postid { get; set; }
        public string video_link { get; set; }
        public int episode { get; set; }
        public int count_comment { get; set; }
        public string thumbnail { get; set; }
        public string qiniu_url { get; set; }
        public string share_sub_title { get; set; }
        public string weibo_share_image { get; set; }
        public int count_share { get; set; }
    }
}
