using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace V电影.ViewModel
{
    public class CatePageViewModel : BasePageViewModel
    {
        private ObservableCollection<Model.lastest_info> _cate_info;
        public ObservableCollection<Model.lastest_info> Cate_Info
        {
            get
            {
                return _cate_info;
            }
            set
            {
                _cate_info = value;
                RaisePropertyChanged("Cate_Info");
            }
        }

        private int _cate_new_count = 0;
        public int Cate_New_Count
        {
            get
            {
                return _cate_new_count;
            }
            set
            {
                _cate_new_count = value;
                RaisePropertyChanged("Cate_New_Count");
            }
        }
    }
}
