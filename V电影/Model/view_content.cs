using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V电影.Model
{
    public class view_content
    {
        public string image { get; set; }
        public string title { get; set; }
        public int duration { get; set; }
        public Int64 filesize { get; set; }
        public string source_link { get; set; }
        public string qiniu_url { get; set; }
    }
}
