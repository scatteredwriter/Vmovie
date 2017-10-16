using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V电影.API
{
    public class Vmovies_API
    {
        public const string cate_lists_api = "https://app.vmovier.com/apiv3/cate/getList"; //频道列表API
        public const string cate_content_api = "http://app.vmoiver.com/apiv3/post/getPostInCate"; //频道内容API
        public const string flipview_api = "http://app.vmoiver.com/apiv3/index/getBanner"; //幻灯片API
        public const string index_api = "https://app.vmovier.com/apiv3/index/index"; //Index信息API
        public const string get_by_tab_api = "http://app.vmoiver.com/apiv3/post/getPostByTab"; //通过Tab得到信息API
        public const string series_api = "http://app.vmoiver.com/apiv3/series/getList"; //系列API
        public const string series_follow_api = "http://app.vmoiver.com/apiv3/series/follow"; //系列追剧API
        public const string series_view_api = "http://app.vmoiver.com/apiv3/series/view"; //系列内容接口
        public const string series_video_api = "http://app.vmoiver.com/apiv3/series/getVideo"; //系列视频接口
        public const string backstage_lists_api = "http://app.vmoiver.com/apiv3/backstage/getCate"; //幕后文章列表API
        public const string backstage_content_api = "http://app.vmoiver.com/apiv3/backstage/getPostByCate"; //幕后文章内容API
        public const string search_api = "http://app.vmoiver.com/apiv3/search/index"; //搜索API
        public const string order_api = "http://app.vmoiver.com/apiv3/series/getSubscriptionByUser"; //用户订阅API
        public const string collections_api = "http://app.vmoiver.com/apiv3/user/getCollect"; //用户收藏API
        public const string login_api = "http://app.vmoiver.com/apiv3/user/login"; //用户登录API
        public const string view_content_api = "http://www.vmovier.com/post/getViewData?id={0}"; //文章内容API
        public const string request_url_api = "http://app.vmoiver.com/{0}?qingapp=app_new"; //URL请求API
        public const string get_comment_api = "http://app.vmoiver.com/apiv3/comment/getLists"; //获取评论API
        public const string add_comment_api = "http://app.vmoiver.com/apiv3/comment/add"; //添加评论API
        public const string approve_api = "http://app.vmoiver.com/apiv3/comment/approve"; //评论点赞API
        public const string unapprove_api = "http://app.vmoiver.com/apiv3/comment/unApprove"; //评论取消点赞API
        public const string set_collect_api = "http://app.vmoiver.com/apiv3/user/setCollect"; //收藏或取消收藏API
        public const string get_list_by_tab = "http://app.vmoiver.com/apiv3/notice/getListByTab"; //通过Tab得到列表API
        public const string unreadnum_api = "http://app.vmoiver.com/apiv3/notice/getUnreadNumForApp"; //未读消息数量API
        public const string config_api = "http://app.vmoiver.com/apiv3/config/config"; //Config API
    }
}
