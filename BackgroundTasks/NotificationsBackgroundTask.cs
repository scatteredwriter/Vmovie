using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using Windows.UI.Notifications;

namespace BackgroundTasks
{
    public sealed class NotificationsBackgroundTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
            ToastNotificationActionTriggerDetail detail = taskInstance.TriggerDetails as ToastNotificationActionTriggerDetail;
            string arguments = detail.Argument;
            string[] param = null;
            if (arguments.Contains('&'))
                param = arguments.Split('&');
            else
                param = new string[] { arguments };
            string action = null;
            string postid = null;
            if (param != null)
            {
                foreach (var item in param)
                {
                    if (item.Contains(Helper.Resource.action_string))
                        action = item.Substring(item.IndexOf('=') + 1);
                    else if (item.Contains(Helper.Resource.postid_string))
                        postid = item.Substring(item.IndexOf('=') + 1);
                }
            }
            if (action != null)
            {
                switch (action)
                {
                    case Helper.Resource.like_action: //喜欢操作
                        {
                            if (postid != null)
                            {
                                if (!ApplicationData.Current.LocalSettings.Values.ContainsKey(Helper.Resource.user_email)) //没有登录
                                {
                                    Helper.ToastHelper.PopToast("您还没有登录！", "请点击完成登录", "page=" + Helper.Resource.login_page);
                                    return;
                                }
                                DateTime startTime = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Utc, TimeZoneInfo.Local);
                                Int64 timeStamp = (Int64)(DateTime.Now - startTime).TotalSeconds;
                                string data = "[{\"postid\": \"{0}\",\"iscollect\": \"1\",\"addtime\": \"{1}\"}]";
                                data = data.Replace("{0}", postid);
                                data = data.Replace("{1}", timeStamp.ToString());
                                string result = Helper.HttpRequestHelper.Set_Collect_Request(data);
                            }
                        }; break;
                    case Helper.Resource.closepush_action: //关闭推送操作
                        {
                            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(Helper.Resource.is_open_daily_push) && (bool)ApplicationData.Current.LocalSettings.Values[Helper.Resource.is_open_daily_push])
                            {
                                ApplicationData.Current.LocalSettings.Values[Helper.Resource.is_open_daily_push] = false;
                                Helper.ToastHelper.PopToast("每日精选推送已关闭", "下次启动应用时生效");
                            }
                        }; break;
                }
            }

            deferral.Complete();
        }
    }
}
