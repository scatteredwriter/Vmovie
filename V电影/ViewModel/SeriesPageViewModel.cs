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
                return _series_info;
            }
            set
            {
                _series_info = value;
                RaisePropertyChanged(nameof(Series_Info));
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
