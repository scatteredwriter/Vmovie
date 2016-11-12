using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V电影.Model
{
    public class notice
    {
        public int noticeid { get; set; }
        public bool isread { get; set; }
        public Int64 addtime { get; set; }
        public string type { get; set; }
        public message message { get; set; }
    }

    public class message
    {
        public message_user user { get; set; }
        public message_comment reply { get; set; }
        public message_comment comment { get; set; }
        public message_object m_object { get; set; }
        public bool is_comment_overflow { get; set; }
    }

    public class message_user
    {
        public int id { get; set; }
        public string username { get; set; }
        public string avatar { get; set; }
    }

    public class message_comment
    {
        public int id { get; set; }
        public string content { get; set; }
        public Int64 addtime { get; set; }
    }

    public class message_object
    {
        public int id { get; set; }
        public string content { get; set; }
        public string image { get; set; }
        public bool is_album { get; set; }
        public string app_fu_title { get; set; }
    }
}
