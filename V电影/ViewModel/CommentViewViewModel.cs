using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V电影.ViewModel
{
    public class CommentViewViewModel:BasePageViewModel
    {
        private ObservableCollection<Model.comment_data> _comment_data;
        public ObservableCollection<Model.comment_data> Comment_Data
        {
            get
            {
                return _comment_data;
            }
            set
            {
                _comment_data = value;
                RaisePropertyChanged(nameof(Comment_Data));
            }
        }
    }
}
