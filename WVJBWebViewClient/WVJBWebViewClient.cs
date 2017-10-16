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
        private const string NewView = "NewView"; //新链接跳转
        private const string NewUrl = "NewUrl"; //网页跳转
        private const string PlayVideo = "PlayVideo"; //视频播放
        private const string DownLoadVideo = "DownLoadVideo"; //视频下载
        private const string FPComment = "FPComment"; //翻页评论

        public event EventHandler<bool> FpVideoFullScreenRequest;
        public event EventHandler<string> ChangedPlayVideo;
        public event EventHandler<int> NewViewRequest;
        public event EventHandler<string> OpenUrlRequest;
        public event EventHandler<int> DownloadVideoRequest;
        public event EventHandler<int> FPCommentRequest;

        public async void showMessage(string msg)
        {
            await new MessageDialog(msg).ShowAsync();
        }

        public void getMessage(string paramString, string param1, string param2)
        {
            switch (paramString)
            {
                case NewView:
                    {
                        NewViewRequest?.Invoke(this, Convert.ToInt32(param1));
                    }; break;
                case NewUrl:
                    {
                        OpenUrlRequest?.Invoke(this, param1.ToString());
                    }; break;
                case PlayVideo:
                    {
                        ChangedPlayVideo?.Invoke(this, param1);
                    }; break;
                case DownLoadVideo:
                    {
                        DownloadVideoRequest?.Invoke(this, Convert.ToInt32(param1));
                    }; break;
                case FPComment:
                    {
                        FPCommentRequest?.Invoke(this, Convert.ToInt32(param1));
                    };break;
            }
        }

        public void fpVideoFullScreen(string is_fullscreen)
        {
            if (is_fullscreen == "1")
            {
                FpVideoFullScreenRequest?.Invoke(this, true);
            }
            else
            {
                FpVideoFullScreenRequest?.Invoke(this, false);
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
