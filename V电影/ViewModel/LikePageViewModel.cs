using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V电影.ViewModel
{
    public class LikePageViewModel : BasePageViewModel
    {
        private ObservableCollection<Model.collect> _collect_film;
        public ObservableCollection<Model.collect> Collect_Film
        {
            get
            {
                return _collect_film;
            }
            set
            {
                _collect_film = value;
                RaisePropertyChanged(nameof(Collect_Film));
            }
        }

        private ObservableCollection<Model.collect> _collect_behind;
        public ObservableCollection<Model.collect> Collect_Behind
        {
            get
            {
                return _collect_behind;
            }
            set
            {
                _collect_behind = value;
                RaisePropertyChanged(nameof(Collect_Behind));
            }
        }
    }
}
