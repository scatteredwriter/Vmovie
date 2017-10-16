using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;

namespace V电影.ViewModel
{
    public class HomePageViewModel : BasePageViewModel
    {
        private Model.Index _index;
        public Model.Index Index
        {
            get
            {
                return _index;
            }
            set
            {
                _index = value;
                RaisePropertyChanged(nameof(Index));
            }
        }

        private ObservableCollection<Model.flipview> _flipview_lists;
        public ObservableCollection<Model.flipview> Flipview_Lists
        {
            get
            {
                return _flipview_lists;
            }
            set
            {
                _flipview_lists = value;
                RaisePropertyChanged("Flipview_Lists");
            }
        }

        private ObservableCollection<Model.lastest_info> _lastest_info;
        public ObservableCollection<Model.lastest_info> Lastest_Info
        {
            get
            {
                return _lastest_info;
            }
            set
            {
                _lastest_info = value;
                RaisePropertyChanged("Lastest_Info");
            }
        }

        private int _lastest_new_count = 0;
        public int Lastest_New_Count
        {
            get
            {
                return _lastest_new_count;
            }
            set
            {
                _lastest_new_count = value;
                RaisePropertyChanged("Lastest_New_Count");
            }
        }

        private ObservableCollection<Model.cate> _cate_lists;
        public ObservableCollection<Model.cate> Cate_Lists
        {
            get
            {
                return _cate_lists;
            }
            set
            {
                _cate_lists = value;
                RaisePropertyChanged("Cate_Lists");
            }
        }

        private double _max_height;
        public double Max_Height
        {
            get
            {
                return _max_height;
            }
            set
            {
                _max_height = value;
                RaisePropertyChanged(nameof(Max_Height));
            }
        }
    }
}
