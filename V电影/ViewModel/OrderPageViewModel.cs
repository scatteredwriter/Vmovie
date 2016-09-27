using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V电影.ViewModel
{
    public class OrderPageViewModel : BasePageViewModel
    {
        private ObservableCollection<Model.order> _order_info;
        public ObservableCollection<Model.order> Order_Info
        {
            get
            {
                return _order_info;
            }
            set
            {
                _order_info = value;
                RaisePropertyChanged(nameof(Order_Info));
            }
        }
    }
}
