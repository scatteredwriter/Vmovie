using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;

namespace WVJBWebViewClient
{
    [AllowForWeb]
    public sealed class Bridge
    {
        private IList<KeyValuePair<String, WVJBHandler>> messageHandlers = null;
        private IList<WVJBMessage> startupMessageQueue = null;
        public IList<WVJBMessage> StartUp_MessageQueue
        {
            get
            {
                return startupMessageQueue;
            }
        }

        public event EventHandler<int> MessageQueue_Changed;

        private void queueMessage(WVJBMessage paramWVJBMessage)
        {
            if (this.startupMessageQueue != null)
            {
                this.startupMessageQueue.Add(paramWVJBMessage);
                MessageQueue_Changed?.Invoke(this, StartUp_MessageQueue.Count);
                return;
            }
        }

        private void sendData(object paramObject, WVJBResponseCallback paramWVJBResponseCallback, string paramString)
        {
            if ((paramObject == null) && ((paramString == null) || (paramString.Length == 0)))
                return;
            WVJBMessage localWVJBMessage = new WVJBMessage();
            if (paramObject != null)
                localWVJBMessage.data = paramObject;
            if (paramWVJBResponseCallback != null)
            {
                StringBuilder localStringBuilder = new StringBuilder().Append("objc_cb_");
            }
            if (paramString != null)
                localWVJBMessage.handlerName = paramString;
            queueMessage(localWVJBMessage);
        }

        public void registerHandler(String paramString, WVJBHandler paramWVJBHandler)
        {
            if ((paramString == null) || (paramString.Length == 0) || (paramWVJBHandler == null))
                return;
            this.messageHandlers.Add(new KeyValuePair<string, WVJBHandler>(paramString, paramWVJBHandler));
        }

        public void callHandler(string paramString)
        {
            callHandler(paramString, null, null);
        }

        public void callHandler(string paramString, object paramObject)
        {
            callHandler(paramString, paramObject, null);
        }

        public void callHandler(string paramString, object paramObject, WVJBResponseCallback paramWVJBResponseCallback)
        {
            sendData(paramObject, paramWVJBResponseCallback, paramString);
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
}
