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
        private ObservableCollection<Model.flipview> _flipview_lists;
        public ObservableCollection<Model.flipview> Flipview_Lists
        {
            get
            {
                if (_fis != null)
                {
                    for (int i = 0; i < _fis.Count; i++)
                    {
                        _flipview_lists[i].image_source = _fis[i];
                    }
                }
                return _flipview_lists;
            }
            set
            {
                _flipview_lists = value;
                RaisePropertyChanged("Flipview_Lists");
            }
        }

        private ObservableCollection<ImageSource> _fis;
        public ObservableCollection<ImageSource> Flipview_Image_Sbs
        {
            get
            {
                return _fis;
            }
            set
            {
                _fis = value;
            }
        }

        private ObservableCollection<Model.lastest_info> _lastest_info;
        public ObservableCollection<Model.lastest_info> Lastest_Info
        {
            get
            {
                if (_lis != null)
                {
                    for (int i = 0; i < _lis.Count; i++)
                    {
                        _lastest_info[i].image_source = _lis[i];
                    }
                }
                return _lastest_info;
            }
            set
            {
                _lastest_info = value;
                RaisePropertyChanged("Lastest_Info");
            }
        }

        private ObservableCollection<ImageSource> _lis;
        public ObservableCollection<ImageSource> Lastest_Image_Sbs
        {
            get
            {
                return _lis;
            }
            set
            {
                _lis = value;
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
                if (_cis != null)
                {
                    for (int i = 0; i < _cis.Count; i++)
                    {
                        _cate_lists[i].image_source = _cis[i];
                    }
                }
                return _cate_lists;
            }
            set
            {
                _cate_lists = value;
                RaisePropertyChanged("Cate_Lists");
            }
        }

        private ObservableCollection<ImageSource> _cis;
        public ObservableCollection<ImageSource> Cate_Image_Sbs
        {
            get
            {
                return _cis;
            }
            set
            {
                _cis = value;
            }
        }
    }
}
