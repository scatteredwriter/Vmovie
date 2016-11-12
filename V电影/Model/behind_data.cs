using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace V电影.Model
{
    public class behind_data : basemodel
    {
        public int cateid { get; set; }
        public string catename { get; set; }

        public int Behind_Info_P { get; set; } = 1;
        public bool Is_Loaded { get; set; } = false;

        public ScrollViewer Listview_Scrollviewer { get; set; }

        public bool Is_Behind_Loading { get; set; } = false;

        private int _behind_info_new_count = 0;
        public int Behind_Info_New_Count
        {
            get
            {
                return _behind_info_new_count;
            }
            set
            {
                _behind_info_new_count = value;
                RaisePropertyChanged(nameof(Behind_Info_New_Count));
            }
        }

        private ObservableCollection<behind_info> _behind_info;
        public ObservableCollection<behind_info> Behind_Info
        {
            get
            {
                return _behind_info;
            }
            set
            {
                _behind_info = value;
                RaisePropertyChanged(nameof(Behind_Info));
            }
        }
    }
}
