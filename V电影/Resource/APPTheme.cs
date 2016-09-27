using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace V电影.Resource
{
    public class APPTheme : DependencyObject
    {
        public const string user_name = "user_name"; //用户名
        public const string user_avatar = "user_avatar"; //用户头像
        public const string user_auth_key = "user_auth_key"; //用户验证key
        public const string user_email = "user_email"; //用户邮箱

        //APP主题颜色
        public SolidColorBrush APP_Color_Brush
        {
            get
            {
                return (SolidColorBrush)GetValue(APP_Color_Brush_DP);
            }
            set
            {
                SetValue(APP_Color_Brush_DP, value);
            }
        }

        public Color APP_Color
        {
            get
            {
                SolidColorBrush temp_brush = (SolidColorBrush)GetValue(APP_Color_Brush_DP);
                _app_color = temp_brush.Color;
                return _app_color;
            }
        }

        //灰色
        public SolidColorBrush Gary_Color_Brush
        {
            get
            {
                return (SolidColorBrush)GetValue(Gary_Color_Brush_DP);
            }
            set
            {
                SetValue(Gary_Color_Brush_DP, value);
            }
        }

        //蓝色
        public SolidColorBrush Blue_Color_Brush
        {
            get
            {
                return (SolidColorBrush)GetValue(Blue_Color_Brush_DP);
            }
            set
            {
                SetValue(Blue_Color_Brush_DP, value);
            }
        }

        //浅白色
        public SolidColorBrush Second_White_Color_Brush
        {
            get
            {
                return (SolidColorBrush)GetValue(Second_White_Color_Brush_DP);
            }
            set
            {
                SetValue(Second_White_Color_Brush_DP, value);
            }
        }

        //边界浅白色
        public SolidColorBrush Border_White_Color_Brush
        {
            get
            {
                return (SolidColorBrush)GetValue(Border_White_Color_Brush_DP);
            }
            set
            {
                SetValue(Border_White_Color_Brush_DP, value);
            }
        }

        //APP前景色
        public SolidColorBrush Foreground_Color_Brush
        {
            get
            {
                return (SolidColorBrush)GetValue(Foreground_Color_Brush_DP);
            }
            set
            {
                SetValue(Foreground_Color_Brush_DP, value);
            }
        }

        //Header字体尺寸
        public int Header_Size
        {
            get
            {
                return (int)GetValue(Header_Size_DP);
            }
            set
            {
                SetValue(Header_Size_DP, value);
            }
        }

        //内容文字尺寸
        public int Content_Size
        {
            get
            {
                return (int)GetValue(Content_Size_DP);
            }
            set
            {
                SetValue(Content_Size_DP, value);
            }
        }

        //二级内容文字尺寸
        public int Second_Content_Size
        {
            get
            {
                return (int)GetValue(Second_Content_Size_DP);
            }
            set
            {
                SetValue(Second_Content_Size_DP, value);
            }
        }

        public static readonly DependencyProperty APP_Color_Brush_DP = DependencyProperty.Register("APP_Color_Brush", typeof(SolidColorBrush), typeof(APPTheme), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 1, 1, 1))));

        public static readonly DependencyProperty Gary_Color_Brush_DP = DependencyProperty.Register("Gary_Color_Brush", typeof(SolidColorBrush), typeof(APPTheme), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 125, 125, 125))));

        public static readonly DependencyProperty Blue_Color_Brush_DP = DependencyProperty.Register("Blue_Color_Brush", typeof(SolidColorBrush), typeof(APPTheme), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 49, 175, 252))));

        public static readonly DependencyProperty Second_White_Color_Brush_DP = DependencyProperty.Register("Second_White_Color_Brush", typeof(SolidColorBrush), typeof(APPTheme), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 245, 245, 245))));

        public static readonly DependencyProperty Border_White_Color_Brush_DP = DependencyProperty.Register("Border_White_Color_Brush", typeof(SolidColorBrush), typeof(APPTheme), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 221, 221, 225))));

        public static readonly DependencyProperty Foreground_Color_Brush_DP = DependencyProperty.Register("Foreground_Color_Brush", typeof(SolidColorBrush), typeof(APPTheme), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 255, 255, 255))));

        public static readonly DependencyProperty Header_Size_DP = DependencyProperty.Register("Header_Size", typeof(int), typeof(APPTheme), new PropertyMetadata(18));

        public static readonly DependencyProperty Content_Size_DP = DependencyProperty.Register("Content_Size", typeof(int), typeof(APPTheme), new PropertyMetadata(20));

        public static readonly DependencyProperty Second_Content_Size_DP = DependencyProperty.Register("Second_Content_Size", typeof(int), typeof(APPTheme), new PropertyMetadata(15));

        private Color _app_color;

    }
}
