using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

namespace V电影.Converter
{
    public class User_Name_Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (App.settings.Values.ContainsKey(Resource.APPTheme.user_name))
            {
                return App.settings.Values[Resource.APPTheme.user_name].ToString();
            }
            else
            {
                return "点击登录";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class Lastest_Second_Content_Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Model.lastest_info info = value as Model.lastest_info;
            string result = "";
            int duration_min = 0;
            int duration_sec = 0;
            duration_min = info.duration / 60;
            duration_sec = info.duration % 60;
            result = info.cates[0].catename + "  /  " + duration_min.ToString() + "\'" + duration_sec.ToString() + "\"";
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class Get_New_Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((int)value == 0)
            {
                return "没有新内容啦！";
            }
            else
            {
                return "本次加载了" + value.ToString() + "条新内容";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class Cate_CateName_Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return "#" + (value as Model.cate).catename + "#";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class Series_Info_Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return "已更新至第" + (value as Model.series).update_to.ToString() + "集  " + (value as Model.series).follower_num.ToString() + "人已订阅";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class Is_Attention_Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                bool is_follow = System.Convert.ToBoolean((value.ToString()));
                if (is_follow)
                {
                    return "ms-appx:///Assets/attention_finish.png";
                }
                else
                {
                    return "ms-appx:///Assets/attention.png";
                }
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class During_Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((int)value == 0.0)
            {
                return "";
            }
            string result = "";
            int duration_min = 0;
            int duration_sec = 0;
            duration_min = (int)value / 60;
            duration_sec = (int)value % 60;
            result = duration_min.ToString() + ":" + duration_sec.ToString();
            if (duration_min < 10)
            {
                result = result.Insert(0, "0");
            }
            if (duration_sec < 10)
            {
                result = result.Insert(result.IndexOf(':') + 1, "0");
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class Rating_Stars_Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            switch ((int)value)
            {
                case 0:
                    {
                        return new BitmapImage(new Uri("ms-appx:///Assets/star.png", UriKind.Absolute));
                    };
                case 1:
                    {
                        return new BitmapImage(new Uri("ms-appx:///Assets/star_2.png", UriKind.Absolute));
                    };
                case 2:
                    {
                        return new BitmapImage(new Uri("ms-appx:///Assets/star_1.png", UriKind.Absolute));
                    };
            }
            return new BitmapImage(new Uri("ms-appx:///Assets/star_1.png", UriKind.Absolute));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class Order_title_Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Model.order order = value as Model.order;
            return "第" + order.update_to.ToString() + "集：" + order.title;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class TimeStamp_Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Int64 timeStamp = System.Convert.ToInt64(value.ToString());
            DateTime dtStart = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Utc, TimeZoneInfo.Local);
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            DateTime resultTime = dtStart.Add(toNow);
            if (parameter != null && parameter.ToString() == "ymd")
            {
                if (DateTime.Now.Year == resultTime.Year)
                {
                    return resultTime.Month.ToString() + "." + resultTime.Day.ToString() + " " + resultTime.Hour.ToString() + ":" + resultTime.Minute.ToString();
                }
                else
                {
                    return resultTime.Year.ToString() + " " + resultTime.Month.ToString() + "." + resultTime.Day.ToString() + " " + resultTime.Hour.ToString() + ":" + resultTime.Minute.ToString();
                }
            }
            TimeSpan deltaTime = DateTime.Now - resultTime;
            if (deltaTime.TotalDays >= 365)
            {
                return System.Convert.ToInt32(deltaTime.TotalDays / 365).ToString() + "年前";
            }
            else if (deltaTime.TotalDays < 365 && deltaTime.TotalDays >= 30)
            {
                return System.Convert.ToInt32(deltaTime.TotalDays / 30).ToString() + "月前";
            }
            else if (deltaTime.TotalDays < 30 && deltaTime.TotalDays >= 1)
            {
                return System.Convert.ToInt32(deltaTime.TotalDays).ToString() + "天前";
            }
            else if (deltaTime.TotalHours > 1)
            {
                return System.Convert.ToInt32(deltaTime.TotalHours).ToString() + "小时前";
            }
            else if (deltaTime.TotalMinutes < 60 && deltaTime.TotalMinutes >= 1)
            {
                return System.Convert.ToInt32(deltaTime.TotalMinutes).ToString() + "分钟前";
            }
            else
            {
                return "刚刚";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class Is_Approve_Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                bool has_approve = (bool)value;
                if (has_approve)
                {
                    return "ms-appx:///Assets/comment_handle_.png";
                }
                else
                {
                    return "ms-appx:///Assets/comment_handle.png";
                }
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class Is_Unread_Backbround_Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                bool is_read = (bool)value;
                if (!is_read)
                    return (new SolidColorBrush(Windows.UI.Color.FromArgb(255, 240, 240, 240)));
                else
                    return (new SolidColorBrush(Windows.UI.Colors.Transparent));
            }
            else
                return (new SolidColorBrush(Windows.UI.Colors.Transparent));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class Is_Collect_Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                bool is_collect = (bool)value;
                if (is_collect)
                {
                    return "ms-appx:///Assets/details_like_finish.png";
                }
                else
                {
                    return "ms-appx:///Assets/details_like.png";
                }
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class Is_Display_Comment_Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (parameter.ToString() == "content_grid")
            {
                if (value != null && value.ToString() == "评论")
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
            else if (parameter.ToString() == "zan")
            {
                if (value != null && value.ToString() == "评论")
                {
                    return Visibility.Collapsed;
                }
                else if (value != null && value.ToString() == "点赞")
                {
                    return Visibility.Visible;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
