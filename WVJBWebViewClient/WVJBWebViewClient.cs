using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.UI.Popups;

namespace WVJBWebViewClient
{
    [AllowForWeb]
    public sealed class Bridge
    {

        public event EventHandler<bool> FpVideoFullScreenEvent;

        public async void showMessage(string msg)
        {
            await new MessageDialog(msg).ShowAsync();
        }

        public async void getMessage(string paramString, string param1, string param2)
        {
            await new MessageDialog(paramString).ShowAsync();
        }

        public void elementResize(string param, string height, string width)
        {

        }

        public void fpVideoFullScreen(string is_fullscreen)
        {
            if (is_fullscreen == "1")
            {
                FpVideoFullScreenEvent?.Invoke(this, true);
            }
            else
            {
                FpVideoFullScreenEvent?.Invoke(this, false);
            }
        }
    }

    public interface WVJBResponseCallback
    {
        void callback(object paramObject);
    }

    public interface WVJBHandler
    {
        void request(Object paramObject, WVJBWebViewClient.WVJBResponseCallback paramWVJBResponseCallback);
    }

    [AllowForWeb]
    public sealed class WVJBMessage
    {
        public string callbackId { get; set; }
        public object data { get; set; }
        public string handlerName { get; set; }
        public object responseData { get; set; }
        public string responseId { get; set; }
    }
}
