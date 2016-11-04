using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace V电影.ViewModel
{
    public class SeriesPageViewModel : BasePageViewModel
    {
        private ObservableCollection<Model.series> _series_info;
        public ObservableCollection<Model.series> Series_Info
        {
            get
            {
                if (_sis != null)
                {
                    int count;
                    if (_sis.Count <= _series_info.Count)
                    {
                        count = _sis.Count;
                    }
                    else
                    {
                        count = _series_info.Count;
                    }
                    for (int i = 0; i < count; i++)
                    {
                        _series_info[i].image_source = _sis[i];
                    }
                }
                return _series_info;
            }
            set
            {
                _series_info = value;
                RaisePropertyChanged(nameof(Series_Info));
            }
        }

        private ObservableCollection<ImageSource> _sis;
        public ObservableCollection<ImageSource> Series_Image_Sbs
        {
            get
            {
                return _sis;
            }
            set
            {
                _sis = value;
            }
        }

        private int _series_new_count = 0;
        public int Series_New_Count
        {
            get
            {
                return _series_new_count;
            }
            set
            {
                _series_new_count = value;
                RaisePropertyChanged("Series_New_Count");
            }
        }
    }
}
