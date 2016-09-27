using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V电影.ViewModel
{
    public class ViewContentPageViewModel : BasePageViewModel
    {
        private Model.view _view_info;
        public Model.view View_Info
        {
            get
            {
                return _view_info;
            }
            set
            {
                _view_info = value;
                RaisePropertyChanged(nameof(View_Info));
            }
        }
    }
}
