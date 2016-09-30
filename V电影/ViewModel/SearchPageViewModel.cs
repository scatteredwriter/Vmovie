using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace V电影.ViewModel
{
    public class SearchPageViewModel : BasePageViewModel
    {
        public bool Is_Go_Back { get; set; } = false;

        private ObservableCollection<string> _search_history;
        public ObservableCollection<string> Search_History
        {
            get
            {
                return _search_history;
            }
            set
            {
                _search_history = value;
                RaisePropertyChanged(nameof(Search_History));
            }
        }

        private ObservableCollection<Model.search> _search_result;
        public ObservableCollection<Model.search> Search_Result
        {
            get
            {
                return _search_result;
            }
            set
            {
                _search_result = value;
                RaisePropertyChanged(nameof(Search_Result));
            }
        }
    }
}
