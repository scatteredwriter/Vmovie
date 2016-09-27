using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V电影.Model
{
    public class user
    {
        public string auth_key { get; set; }
        public int uid { get; set; }
        public string username { get; set; }
        public bool isadmin { get; set; }
        public string email { get; set; }
        public string avatar { get; set; }
    }
}
