using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace V电影.ViewModel
{
    public class MainPageViewModel : BasePageViewModel
    {
        public string User_Avatar
        {
            get
            {
                if (App.settings.Values.ContainsKey(Resource.APPTheme.user_avatar))
                {
                    return App.settings.Values[Resource.APPTheme.user_avatar].ToString();
                }
                else
                {
                    return "Assets/default_avatar.png";
                }
            }
            set
            {
                RaisePropertyChanged(nameof(User_Avatar));
            }
        }

        public string User_Name
        {
            get
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
            set
            {
                RaisePropertyChanged(nameof(User_Name));
            }
        }

        public SolidColorBrush User_Name_Foreground
        {
            get
            {
                if (App.settings.Values.ContainsKey(Resource.APPTheme.user_avatar))
                {
                    return App.APPTheme.Foreground_Color_Brush;
                }
                else
                {
                    return App.APPTheme.Gary_Color_Brush;
                }
            }
            set
            {
                RaisePropertyChanged(nameof(User_Name_Foreground));
            }
        }
    }
}
