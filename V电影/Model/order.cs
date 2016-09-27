using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V电影.Model
{
    public class order
    {
        public int series_postid { get; set; }
        public int seriesid { get; set; }
        public string title { get; set; }
        public int update_to { get; set; }
        public int len { get; set; }
        public string thumbnail { get; set; }
        public string addtime { get; set; }
        public string seriestitle { get; set; }
        public bool hasnew { get; set; }
    }
}
