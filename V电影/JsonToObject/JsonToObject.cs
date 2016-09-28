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
        public static ObservableCollection<Model.flipview> Convert_Flipview_Json(string json) //Flipview Json转换
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

        public static ObservableCollection<Model.lastest_info> Convert_Lastest_Json(string json) //Lastest Json转换
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
                            return null;
                        }
                        view_lists.Add(view_list);
                    }
                }
                catch (Exception)
                {
                    return null;
                }
                item.posts = view_lists;
                return item;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
