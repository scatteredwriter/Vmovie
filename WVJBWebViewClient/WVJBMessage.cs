using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVJBWebViewClient
{
    public sealed class WVJBMessage
    {
        public string callbackId { get; set; }
        public object data { get; set; }
        public string handlerName { get; set; }
        public object responseData { get; set; }
        public string responseId { get; set; }
    }
}
