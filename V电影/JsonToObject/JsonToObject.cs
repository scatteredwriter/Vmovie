using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace V电影.JsonToObject
{
    public class JsonToObject
    {
        public static ObservableCollection<Model.flipview> Convert_Flipview_Json(string json)
        {
            ObservableCollection<Model.flipview> flipview_lists = new ObservableCollection<Model.flipview>();
            Model.flipview flipview = new Model.flipview();

            try
            {
                JObject json_object = (JObject)JsonConvert.DeserializeObject(json);
                JArray data = (JArray)json_object["data"];
                for (int i = 0; i < data.Count; i++)
                {
                    flipview = new Model.flipview();
                    flipview.imageurl = data[i]["image"].ToString();
                    JObject extra_data = (JObject)data[i]["extra_data"];
                    flipview.app_banner_type = Convert.ToInt32(extra_data["app_banner_type"].ToString());
                    flipview.app_banner_param = extra_data["app_banner_param"].ToString();
                    flipview_lists.Add(flipview);
                }
                return flipview_lists;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public static ObservableCollection<Model.lastest_info> Convert_Lastest_Json(string json)
        {
            ObservableCollection<Model.lastest_info> lists = new ObservableCollection<Model.lastest_info>();
            List<Model.cate> cates_list = new List<Model.cate>();
            Model.lastest_info item = new Model.lastest_info();
            Model.cate cate = new Model.cate();

            try
            {
                JObject json_object = (JObject)JsonConvert.DeserializeObject(json);
                JArray data = (JArray)json_object["data"];
                for (int i = 0; i < data.Count; i++)
                {
                    item = new Model.lastest_info();
                    item.postid = Convert.ToInt32(data[i]["postid"].ToString());
                    item.title = data[i]["title"].ToString();
                    item.app_fu_title = data[i]["app_fu_title"].ToString();
                    item.recent_hot = Convert.ToBoolean(Convert.ToInt32(data[i]["recent_hot"].ToString()));
                    item.image = data[i]["image"].ToString();
                    item.is_album = Convert.ToBoolean(Convert.ToInt32(data[i]["is_album"].ToString()));
                    item.rating = data[i]["rating"].ToString();
                    item.duration = Convert.ToInt32(data[i]["duration"].ToString());
                    item.publish_time = Convert.ToInt64(data[i]["publish_time"].ToString());
                    item.like_num = Convert.ToInt32(data[i]["like_num"].ToString());
                    item.share_num = Convert.ToInt32(data[i]["share_num"].ToString());
                    item.request_url = data[i]["request_url"].ToString();
                    JArray cates = (JArray)data[i]["cates"];
                    for (int j = 0; j < cates.Count; j++)
                    {
                        cates_list = new List<Model.cate>();
                        cate = new Model.cate();
                        cate.cateid = Convert.ToInt32(cates[j]["cateid"].ToString());
                        cate.catename = cates[j]["catename"].ToString();
                        cates_list.Add(cate);
                    }
                    item.cates = cates_list;
                    lists.Add(item);
                }
                return lists;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static ObservableCollection<Model.cate> Convert_Cates_Json(string json)
        {
            ObservableCollection<Model.cate> lists = new ObservableCollection<Model.cate>();
            Model.cate item = new Model.cate();

            try
            {
                JObject json_object = (JObject)JsonConvert.DeserializeObject(json);
                JArray data = (JArray)json_object["data"];
                for (int i = 0; i < data.Count; i++)
                {
                    item = new Model.cate();
                    item.cate_type = Convert.ToBoolean(Convert.ToInt32(data[i]["cate_type"].ToString()));
                    if (item.cate_type)
                    {
                        item.tab = data[i]["tab"].ToString();
                    }
                    else
                    {
                        item.cateid = Convert.ToInt32(data[i]["cateid"].ToString());
                        item.orderid = Convert.ToInt32(data[i]["orderid"].ToString());
                    }
                    item.alias = data[i]["alias"].ToString();
                    item.cate_type = Convert.ToBoolean(Convert.ToInt32(data[i]["cate_type"].ToString()));
                    item.catename = data[i]["catename"].ToString();
                    item.icon = data[i]["icon"].ToString();
                    lists.Add(item);
                }
                return lists;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static ObservableCollection<Model.series> Convert_Series_Json(string json)
        {
            ObservableCollection<Model.series> lists = new ObservableCollection<Model.series>();
            Model.series item = new Model.series();
            try
            {
                JObject json_object = (JObject)JsonConvert.DeserializeObject(json);
                JArray data = (JArray)json_object["data"];
                for (int i = 0; i < data.Count; i++)
                {
                    item = new Model.series();
                    item.seriesid = Convert.ToInt32(data[i]["seriesid"].ToString());
                    item.title = data[i]["title"].ToString();
                    item.image = data[i]["image"].ToString();
                    item.weekly = data[i]["weekly"].ToString();
                    item.content = data[i]["content"].ToString();
                    item.app_image = data[i]["app_image"].ToString();
                    item.isfollow = Convert.ToBoolean(Convert.ToInt32(data[i]["isfollow"].ToString()));
                    item.is_end = Convert.ToBoolean(Convert.ToInt32(data[i]["is_end"].ToString()));
                    item.update_to = Convert.ToInt32(data[i]["update_to"].ToString());
                    item.follower_num = Convert.ToInt32(data[i]["follower_num"].ToString());
                    lists.Add(item);
                }
                return lists;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Model.series_video_info Convert_Series_Video_Info(string json)
        {
            Model.series_video_info item = new Model.series_video_info();
            try
            {
                JObject json_object = (JObject)JsonConvert.DeserializeObject(json);
                JObject data = (JObject)json_object["data"];
                item.title = data["title"].ToString();
                item.seriesid = Convert.ToInt32(data["seriesid"].ToString());
                item.series_postid = Convert.ToInt32(data["series_postid"].ToString());
                item.video_link = data["video_link"].ToString();
                item.episode = Convert.ToInt32(data["episode"].ToString());
                item.count_comment = Convert.ToInt32(data["count_comment"].ToString());
                item.thumbnail = data["thumbnail"].ToString();
                item.qiniu_url = data["qiniu_url"].ToString();
                item.share_sub_title = data["share_sub_title"].ToString();
                item.weibo_share_image = data["weibo_share_image"].ToString();
                item.count_share = Convert.ToInt32(data["count_share"].ToString());
                return item;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static ObservableCollection<Model.behind_data> Convert_Behind_Cates_Json(string json)
        {
            ObservableCollection<Model.behind_data> lists = new ObservableCollection<Model.behind_data>();
            Model.behind_data item = new Model.behind_data();
            try
            {
                JObject json_object = (JObject)JsonConvert.DeserializeObject(json);
                JArray data = (JArray)json_object["data"];
                for (int i = 0; i < data.Count; i++)
                {
                    item = new Model.behind_data();
                    item.cateid = Convert.ToInt32(data[i]["cateid"].ToString());
                    item.catename = data[i]["catename"].ToString();
                    lists.Add(item);
                }
                return lists;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static ObservableCollection<Model.behind_info> Convert_Behind_Info_Json(string json)
        {
            ObservableCollection<Model.behind_info> lists = new ObservableCollection<Model.behind_info>();
            Model.behind_info item = new Model.behind_info();
            try
            {
                JObject json_object = (JObject)JsonConvert.DeserializeObject(json);
                JArray data = (JArray)json_object["data"];
                for (int i = 0; i < data.Count; i++)
                {
                    item = new Model.behind_info();
                    item.app_fu_title = data[i]["app_fu_title"].ToString();
                    item.duration = Convert.ToInt32(data[i]["duration"].ToString());
                    item.image = data[i]["image"].ToString();
                    item.like_num = Convert.ToInt32(data[i]["like_num"].ToString());
                    item.postid = Convert.ToInt32(data[i]["postid"].ToString());
                    item.publish_time = Convert.ToInt32(data[i]["publish_time"].ToString());
                    item.rating = data[i]["rating"].ToString();
                    item.recent_hot = Convert.ToBoolean(Convert.ToInt32(data[i]["recent_hot"].ToString()));
                    item.request_url = data[i]["request_url"].ToString();
                    item.share_num = Convert.ToInt32(data[i]["share_num"].ToString());
                    item.title = data[i]["title"].ToString();
                    lists.Add(item);
                }
                return lists;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static ObservableCollection<Model.search> Convert_Search_Json(string json)
        {
            ObservableCollection<Model.search> lists = new ObservableCollection<Model.search>();
            Model.search item = new Model.search();
            try
            {
                JObject json_object = (JObject)JsonConvert.DeserializeObject(json);
                JArray data = (JArray)json_object["data"];
                for (int i = 0; i < data.Count; i++)
                {
                    item = new Model.search();
                    item.total = Convert.ToInt32(json_object["total"].ToString());
                    item.app_fu_title = data[i]["app_fu_title"].ToString();
                    item.discussion = Convert.ToInt32((data[i]["discussion"].ToString()));
                    item.duration = Convert.ToInt32(data[i]["duration"].ToString());
                    item.image = data[i]["image"].ToString();
                    item.like_num = Convert.ToInt32(data[i]["like_num"].ToString());
                    item.postid = Convert.ToInt32(data[i]["postid"].ToString());
                    item.publish_time = Convert.ToInt64(data[i]["publish_time"].ToString());
                    item.rating = data[i]["rating"].ToString();
                    if (Convert.ToDouble(item.rating) / 2.0 > 0.0)
                    {
                        double total_rating = Convert.ToDouble(item.rating);
                        int j = 0;
                        for (; j < (int)(total_rating / 2.0); j++)
                        {
                            item.star[j] = 0; //全黄
                        }
                        total_rating -= j * 2;
                        if (total_rating != 0.0)
                        {
                            item.star[j++] = 1; //半黄

                        }
                        for (; j < item.star.Count(); j++)
                        {
                            item.star[j] = 2; //全灰
                        }
                    }
                    item.rating = item.rating + "分";
                    item.recent_hot = Convert.ToBoolean(Convert.ToInt32(data[i]["recent_hot"].ToString()));
                    item.request_url = data[i]["request_url"].ToString();
                    item.share_num = Convert.ToInt32(data[i]["share_num"].ToString());
                    item.title = data[i]["title"].ToString();
                    if (data[i]["type"].ToString() == "1")
                    {
                        item.type = Windows.UI.Xaml.Visibility.Visible;
                    }
                    else
                    {
                        item.type = Windows.UI.Xaml.Visibility.Collapsed;
                    }
                    lists.Add(item);
                }
                return lists;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Model.user Convert_User_Json(string json)
        {
            Model.user result = new Model.user();
            try
            {
                JObject json_object = (JObject)JsonConvert.DeserializeObject(json);
                result.auth_key = json_object["auth_key"].ToString();
                JObject data = (JObject)json_object["data"];
                result.avatar = data["avatar"].ToString();
                result.email = data["email"].ToString();
                result.isadmin = Convert.ToBoolean(Convert.ToInt32(data["isadmin"].ToString()));
                result.uid = Convert.ToInt32(data["uid"].ToString());
                result.username = data["username"].ToString();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static ObservableCollection<Model.order> Convert_Order_Json(string json)
        {
            ObservableCollection<Model.order> lists = new ObservableCollection<Model.order>();
            try
            {
                JObject json_object = (JObject)JsonConvert.DeserializeObject(json);
                JArray data = (JArray)json_object["data"];
                for (int i = 0; i < data.Count; i++)
                {
                    Model.order item = new Model.order();
                    item.addtime = data[i]["addtime"].ToString();
                    item.hasnew = Convert.ToBoolean(Convert.ToInt32(data[i]["hasnew"].ToString()));
                    item.len = Convert.ToInt32(data[i]["len"].ToString());
                    item.seriesid = Convert.ToInt32(data[i]["seriesid"].ToString());
                    item.seriestitle = data[i]["seriestitle"].ToString();
                    item.series_postid = Convert.ToInt32(data[i]["series_postid"].ToString());
                    item.thumbnail = data[i]["thumbnail"].ToString();
                    item.title = data[i]["title"].ToString();
                    item.update_to = Convert.ToInt32(data[i]["update_to"].ToString());
                    lists.Add(item);
                }
                return lists;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Model.view Convert_View_Json(string json)
        {
            Model.view item = new Model.view();
            try
            {
                JObject json_object = (JObject)JsonConvert.DeserializeObject(json);
                JObject data = (JObject)json_object["data"];
                item.postid = Convert.ToInt32(data["postid"].ToString());
                item.title = data["title"].ToString();
                item.app_fu_title = data["app_fu_title"].ToString();
                item.intro = data["intro"].ToString();
                item.count_comment = Convert.ToInt32(data["count_comment"].ToString());
                item.is_album = Convert.ToBoolean(Convert.ToInt32(data["is_album"].ToString()));
                item.is_collect = Convert.ToBoolean(Convert.ToInt32(data["is_collect"].ToString()));
                JObject content = (JObject)data["content"];
                JArray video = (JArray)content["video"];
                item.content = new ObservableCollection<Model.view_content>();
                for (int i = 0; i < video.Count; i++)
                {
                    Model.view_content view_content = new Model.view_content();
                    try
                    {
                        view_content.image = video[i]["image"].ToString();
                        view_content.title = video[i]["title"].ToString();
                        view_content.duration = Convert.ToInt32(video[i]["duration"].ToString());
                        view_content.filesize = Convert.ToInt64(video[i]["filesize"].ToString());
                    }
                    catch (Exception)
                    {
                    }
                    view_content.source_link = video[i]["source_link"].ToString();
                    view_content.qiniu_url = video[i]["qiniu_url"].ToString();
                    item.content.Add(view_content);
                }
                item.image = data["image"].ToString();
                item.rating = data["rating"].ToString();
                try
                {
                    item.publish_time = Convert.ToInt64(data["publish_time"].ToString());
                }
                catch (Exception)
                {
                }
                item.count_like = Convert.ToInt32(data["count_like"].ToString());
                item.count_share = Convert.ToInt32(data["count_share"].ToString());
                item.cate = data["cate"][0].ToString();
                item.share_link_sweibo = data["share_link"]["sweibo"].ToString();
                item.share_link_weixin = data["share_link"]["weixin"].ToString();
                item.share_link_qzone = data["share_link"]["qzone"].ToString();
                item.share_link_qq = data["share_link"]["qq"].ToString();
                item.tags = data["tags"].ToString();
                item.share_sub_title = data["share_sub_title"].ToString();
                item.weibo_share_image = data["weibo_share_image"].ToString();
                return item;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Model.series_view Convert_Series_View_Json(string json)
        {
            try
            {
                JObject json_object = (JObject)JsonConvert.DeserializeObject(json);
                JObject data = (JObject)json_object["data"];
                Model.series_view item = new Model.series_view();
                item.seriesid = Convert.ToInt32(data["seriesid"].ToString());
                item.title = data["title"].ToString();
                item.image = data["image"].ToString();
                item.content = data["content"].ToString();
                item.weekly = data["weekly"].ToString();
                item.count_follow = Convert.ToInt32(data["count_follow"].ToString());
                item.isfollow = Convert.ToBoolean(Convert.ToInt32(data["isfollow"].ToString()));
                item.share_link = data["share_link"].ToString();
                item.is_end = Convert.ToBoolean(Convert.ToInt32(data["is_end"].ToString()));
                item.update_to = Convert.ToInt32(data["update_to"].ToString());
                item.tag_name = data["tag_name"].ToString();
                item.post_num_per_seg = Convert.ToInt32(data["post_num_per_seg"].ToString());
                ObservableCollection<Model.series_view_lists> view_lists = new ObservableCollection<Model.series_view_lists>();
                try
                {
                    JArray posts = (JArray)data["posts"];
                    for (int i = 0; i < posts.Count; i++)
                    {
                        Model.series_view_lists view_list = new Model.series_view_lists();
                        view_list.from_to = posts[i]["from_to"].ToString();
                        ObservableCollection<Model.series_view_item> view_items = new ObservableCollection<Model.series_view_item>();
                        try
                        {
                            JArray list = (JArray)posts[i]["list"];
                            for (int j = 0; j < list.Count; j++)
                            {
                                Model.series_view_item view_item = new Model.series_view_item();
                                view_item.series_postid = Convert.ToInt32(list[j]["series_postid"].ToString());
                                view_item.number = Convert.ToInt32(list[j]["number"].ToString());
                                view_item.title = list[j]["title"].ToString();
                                view_item.addtime = list[j]["addtime"].ToString();
                                view_item.duration = Convert.ToInt32(list[j]["duration"].ToString());
                                view_item.thumbnail = list[j]["thumbnail"].ToString();
                                view_item.source_link = list[j]["source_link"].ToString();
                                view_items.Add(view_item);
                            }
                            view_list.items = view_items;
                        }
                        catch (Exception)
                        {
                        }
                        view_lists.Add(view_list);
                    }
                }
                catch (Exception)
                {
                }
                item.posts = view_lists;
                return item;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static ObservableCollection<Model.comment_data> Convert_Comment_Data_Json(string json)
        {
            ObservableCollection<Model.comment_data> lists = new ObservableCollection<Model.comment_data>();
            try
            {
                JObject json_object = (JObject)JsonConvert.DeserializeObject(json);
                JArray data = (JArray)json_object["data"];
                for (int i = 0; i < data.Count; i++)
                {
                    Model.comment_data comment_data = new Model.comment_data();
                    comment_data.commentid = Convert.ToInt32(data[i]["commentid"].ToString());
                    comment_data.isrecommend = Convert.ToBoolean(Convert.ToInt32(data[i]["isrecommend"].ToString()));
                    comment_data.count_approve = Convert.ToInt32(data[i]["count_approve"].ToString());
                    comment_data.has_approve = Convert.ToBoolean(Convert.ToInt32(data[i]["has_approve"].ToString()));
                    comment_data.content = data[i]["content"].ToString();
                    comment_data.addtime = Convert.ToInt64(data[i]["addtime"].ToString());
                    JObject userinfo = (JObject)data[i]["userinfo"];
                    comment_data.userinfo = new Model.comment_user_info();
                    comment_data.userinfo.userid = Convert.ToInt32(userinfo["userid"].ToString());
                    comment_data.userinfo.username = userinfo["username"].ToString();
                    comment_data.userinfo.avatar = userinfo["avatar"].ToString();
                    comment_data.userinfo.isadmin = Convert.ToBoolean(Convert.ToInt32(userinfo["isadmin"].ToString()));
                    comment_data.userinfo.is_xpc_author = Convert.ToBoolean(Convert.ToInt32(userinfo["is_xpc_author"].ToString()));
                    if (!String.IsNullOrEmpty(data[i]["reply_username"].ToString()))
                    {
                        comment_data.reply_username = data[i]["reply_username"].ToString();
                        comment_data.reply_user_isadmin = Convert.ToBoolean(Convert.ToInt32(data[i]["reply_user_isadmin"].ToString()));
                        comment_data.reply_userinfo = new Model.comment_user_info();
                        comment_data.reply_userinfo.userid = Convert.ToInt32(userinfo["userid"].ToString());
                        comment_data.reply_userinfo.username = userinfo["username"].ToString();
                        comment_data.reply_userinfo.avatar = userinfo["avatar"].ToString();
                        comment_data.reply_userinfo.isadmin = Convert.ToBoolean(Convert.ToInt32(userinfo["isadmin"].ToString()));
                        comment_data.reply_userinfo.is_xpc_author = Convert.ToBoolean(Convert.ToInt32(userinfo["is_xpc_author"].ToString()));
                    }
                    JArray subcomment = (JArray)data[i]["subcomment"];
                    if (subcomment.Count != 0)
                    {
                        ObservableCollection<Model.comment_data> sub_lists = new ObservableCollection<Model.comment_data>();
                        try
                        {
                            for (int j = 0; j < subcomment.Count; j++)
                            {
                                Model.comment_data comment_data_j = new Model.comment_data();
                                comment_data_j.commentid = Convert.ToInt32(subcomment[j]["commentid"].ToString());
                                comment_data_j.isrecommend = Convert.ToBoolean(Convert.ToInt32(subcomment[j]["isrecommend"].ToString()));
                                comment_data_j.count_approve = Convert.ToInt32(subcomment[j]["count_approve"].ToString());
                                comment_data_j.has_approve = Convert.ToBoolean(Convert.ToInt32(subcomment[j]["has_approve"].ToString()));
                                comment_data_j.content = subcomment[j]["content"].ToString();
                                comment_data_j.addtime = Convert.ToInt64(subcomment[j]["addtime"].ToString());
                                JObject userinfo_j = (JObject)subcomment[j]["userinfo"];
                                comment_data_j.userinfo = new Model.comment_user_info();
                                comment_data_j.userinfo.userid = Convert.ToInt32(userinfo_j["userid"].ToString());
                                comment_data_j.userinfo.username = userinfo_j["username"].ToString();
                                comment_data_j.userinfo.avatar = userinfo_j["avatar"].ToString();
                                comment_data_j.userinfo.isadmin = Convert.ToBoolean(Convert.ToInt32(userinfo_j["isadmin"].ToString()));
                                comment_data_j.userinfo.is_xpc_author = Convert.ToBoolean(Convert.ToInt32(userinfo_j["is_xpc_author"].ToString()));
                                if (!String.IsNullOrEmpty(subcomment[j]["reply_username"].ToString()))
                                {
                                    comment_data_j.reply_username = subcomment[j]["reply_username"].ToString();
                                    comment_data_j.reply_user_isadmin = Convert.ToBoolean(Convert.ToInt32(subcomment[j]["reply_user_isadmin"].ToString()));
                                    comment_data_j.reply_userinfo = new Model.comment_user_info();
                                    comment_data_j.reply_userinfo.userid = Convert.ToInt32(userinfo_j["userid"].ToString());
                                    comment_data_j.reply_userinfo.username = userinfo_j["username"].ToString();
                                    comment_data_j.reply_userinfo.avatar = userinfo_j["avatar"].ToString();
                                    comment_data_j.reply_userinfo.isadmin = Convert.ToBoolean(Convert.ToInt32(userinfo_j["isadmin"].ToString()));
                                    comment_data_j.reply_userinfo.is_xpc_author = Convert.ToBoolean(Convert.ToInt32(userinfo_j["is_xpc_author"].ToString()));
                                }
                                comment_data_j.subcomment = null;
                                sub_lists.Add(comment_data_j);
                            }
                            comment_data.subcomment = sub_lists;
                        }
                        catch (Exception)
                        {
                            comment_data.subcomment = null;
                        }
                    }
                    else
                    {
                        comment_data.subcomment = null;
                    }
                    lists.Add(comment_data);
                }
                return lists;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static ObservableCollection<Model.notice> Convert_Message_Json(string json)
        {
            ObservableCollection<Model.notice> lists = new ObservableCollection<Model.notice>();
            try
            {
                JObject json_object = (JObject)JsonConvert.DeserializeObject(json);
                JObject data = (JObject)json_object["data"];
                JArray list = (JArray)data["list"];
                for (int i = 0; i < list.Count; i++)
                {
                    Model.notice notice = new Model.notice();
                    notice.noticeid = Convert.ToInt32(list[i]["noticeid"].ToString());
                    notice.isread = Convert.ToBoolean(Convert.ToInt32(list[i]["isread"].ToString()));
                    notice.addtime = Convert.ToInt64(list[i]["addtime"].ToString());
                    JObject m_object = (JObject)JsonConvert.DeserializeObject(list[i]["json"].ToString());
                    try
                    {
                        notice.message = new Model.message();

                        //user
                        notice.message.user = new Model.message_user();
                        JObject user = (JObject)m_object["user"];
                        notice.message.user.id = Convert.ToInt32(user["id"].ToString());
                        notice.message.user.username = user["username"].ToString();
                        notice.message.user.avatar = user["avatar"].ToString();

                        //reply
                        try
                        {
                            JObject reply = (JObject)m_object["reply"];
                            notice.message.reply = new Model.message_comment();
                            notice.message.reply.id = Convert.ToInt32(reply["id"].ToString());
                            notice.message.reply.content = reply["content"].ToString();
                            notice.message.reply.addtime = Convert.ToInt64(reply["addtime"].ToString());
                        }
                        catch (Exception)
                        {
                            notice.message.reply = null;
                        }

                        //comment
                        notice.message.comment = new Model.message_comment();
                        JObject comment = (JObject)m_object["comment"];
                        notice.message.comment.id = Convert.ToInt32(comment["id"].ToString());
                        notice.message.comment.content = comment["content"].ToString();
                        notice.message.comment.addtime = Convert.ToInt64(comment["addtime"].ToString());

                        //object
                        notice.message.m_object = new Model.message_object();
                        JObject m_o = (JObject)m_object["object"];
                        notice.message.m_object.id = Convert.ToInt32(m_o["id"].ToString());
                        notice.message.m_object.content = m_o["content"].ToString();
                        notice.message.m_object.image = m_o["image"].ToString();
                        try
                        {
                            JObject info = (JObject)m_o["info"];
                            notice.message.m_object.is_album = Convert.ToBoolean(Convert.ToInt32(info["is_album"].ToString()));
                            notice.message.m_object.app_fu_title = info["app_fu_title"].ToString();
                        }
                        catch (Exception)
                        {
                        }

                        //判断评论或点赞
                        if (notice.message.reply == null)
                            notice.type = "点赞";
                        else
                            notice.type = "评论";
                    }
                    catch (Exception)
                    {
                        notice.message = null;
                    }
                    lists.Add(notice);
                }
                return lists;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static ObservableCollection<Model.collect> Convert_Collect_Json(string json)
        {
            ObservableCollection<Model.collect> lists = new ObservableCollection<Model.collect>();
            try
            {
                JObject json_object = (JObject)JsonConvert.DeserializeObject(json);
                JArray data = (JArray)json_object["data"];
                for (int i = 0; i < data.Count; i++)
                {
                    Model.collect item = new Model.collect();
                    item.count_share = Convert.ToInt32(data[i]["count_share"].ToString());
                    item.count_like = Convert.ToInt32(data[i]["count_like"].ToString());
                    item.postid = Convert.ToInt32(data[i]["postid"].ToString());
                    item.title = data[i]["title"].ToString();
                    item.publish_time = Convert.ToInt64(data[i]["publish_time"].ToString());
                    item.duration = Convert.ToInt32(data[i]["duration"].ToString());
                    item.rating = data[i]["rating"].ToString();
                    if (Convert.ToDouble(item.rating) / 2.0 > 0.0)
                    {
                        double total_rating = Convert.ToDouble(item.rating);
                        int j = 0;
                        for (; j < (int)(total_rating / 2.0); j++)
                        {
                            item.star[j] = 0; //全黄
                        }
                        total_rating -= j * 2;
                        if (total_rating != 0.0)
                        {
                            item.star[j++] = 1; //半黄

                        }
                        for (; j < item.star.Count(); j++)
                        {
                            item.star[j] = 2; //全灰
                        }
                    }
                    item.rating = item.rating + "分";
                    item.image = data[i]["image"].ToString();
                    item.collect_time = Convert.ToInt64(data[i]["collect_time"].ToString());
                    item.app_fu_title = data[i]["app_fu_title"].ToString();
                    item.is_album = Convert.ToBoolean(Convert.ToInt32(data[i]["is_album"].ToString()));
                    lists.Add(item);
                }
                return lists;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
