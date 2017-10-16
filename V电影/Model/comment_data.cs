using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V电影.Model
{
    public class comment_data
    {
        public int commentid { get; set; }
        public bool isrecommend { get; set; }
        public int count_approve { get; set; }
        public bool has_approve { get; set; }
        public string content { get; set; }
        public Int64 addtime { get; set; }
        public comment_user_info userinfo { get; set; }
        public string reply_username { get; set; }
        public bool reply_user_isadmin { get; set; }
        public comment_user_info reply_userinfo { get; set; }
        public ObservableCollection<comment_data> subcomment { get; set; }
    }

    public class comment_user_info
    {
        public int useruid { get; set; }
        public string username { get; set; }
        public string avatar { get; set; }
        public bool isadmin { get; set; }
        public bool isediter { get; set; }
    }
}
