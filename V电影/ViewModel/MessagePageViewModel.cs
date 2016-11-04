using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V电影.ViewModel
{
    public class MessagePageViewModel : BasePageViewModel
    {
        private ObservableCollection<Model.notice> _notice_info;
        public ObservableCollection<Model.notice> Notice_Info
        {
            get
            {
                return _notice_info;
            }
            set
            {
                _notice_info = value;
                RaisePropertyChanged(nameof(Notice_Info));
            }
        }
    }
}
