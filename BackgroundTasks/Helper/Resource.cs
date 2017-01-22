using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundTasks.Helper
{
    class Resource
    {
        internal const string action_string = "action";
        internal const string postid_string = "postid";
        internal const string like_action = "Like";
        internal const string closepush_action = "ClosePush";
        internal const string login_page = "Login";

        internal const string user_auth_key = "user_auth_key"; //用户验证key
        internal const string user_email = "user_email";
        internal const string is_open_daily_push = "is_open_daily_push"; //每日精选推送是否开启

        internal const string set_collect_api = "http://app.vmoiver.com/apiv3/user/setCollect"; //收藏或取消收藏API
    }
}
