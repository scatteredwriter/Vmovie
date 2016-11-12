using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V电影.ViewModel
{
    public class BehindPageViewModel : BasePageViewModel
    {
        private ObservableCollection<Model.behind_data> _behind_data;
        public ObservableCollection<Model.behind_data> Behind_Data
        {
            get
            {
                return _behind_data;
            }
            set
            {
                _behind_data = value;
                RaisePropertyChanged(nameof(Behind_Data));
            }
        }
    }
}
