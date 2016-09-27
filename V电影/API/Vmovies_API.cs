using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V电影.API
{
    public class Vmovies_API
    {
        public const string cate_lists_api = "http://app.vmoiver.com/apiv3/cate/getList"; //频道列表API
        public const string cate_content_api = "http://app.vmoiver.com/apiv3/post/getPostInCate"; //频道内容API
        public const string flipview_api = "http://app.vmoiver.com/apiv3/index/getBanner"; //幻灯片API
        public const string get_by_tab_api = "http://app.vmoiver.com/apiv3/post/getPostByTab"; //通过标签得到信息API
        public const string series_api = "http://app.vmoiver.com/apiv3/series/getList"; //系列API
        public const string series_follow_api = "http://app.vmoiver.com/apiv3/series/follow"; //系列追剧API
        public const string series_view_api = "http://app.vmoiver.com/apiv3/series/view"; //系列内容接口
        public const string series_video_api = "http://app.vmoiver.com/apiv3/series/getVideo"; //系列视频接口
        public const string backstage_lists_api = "http://app.vmoiver.com/apiv3/backstage/getCate"; //幕后文章列表API
        public const string backstage_content_api = "http://app.vmoiver.com/apiv3/backstage/getPostByCate"; //幕后文章内容API
        public const string search_api = "http://app.vmoiver.com/apiv3/search/index"; //搜索API
        public const string notice_api = "http://app.vmoiver.com/apiv3/notice/getListByTab"; //用户消息API
        public const string order_api = "http://app.vmoiver.com/apiv3/series/getSubscriptionByUser"; //用户订阅API
        public const string collections_api = "http://app.vmoiver.com/apiv3/user/getCollect"; //用户收藏API
        public const string login_api = "http://app.vmoiver.com/apiv3/user/login"; //用户登录API
        public const string view_content_api = "http://app.vmoiver.com/apiv3/post/view"; //文章内容API
        public const string request_url_api = "http://app.vmoiver.com/{0}?qingapp=app_new"; //URL请求API
    }
}
