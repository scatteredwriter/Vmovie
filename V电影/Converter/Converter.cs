using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
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
            if ((value as Model.series).isfollow)
            {
                return "ms-appx:///Assets/attention_finish.png";
            }
            else
            {
                return "ms-appx:///Assets/attention.png";
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

    public class Search_Rating_Stars_Converter : IValueConverter
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
}
