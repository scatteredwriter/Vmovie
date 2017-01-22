using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace BackgroundTasks.Helper
{
    public sealed class HttpRequestHelper
    {
        private static FormUrlEncodedContent Default_Params(int p, int size, KeyValuePair<string, string> addiction)
        {
            List<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>();
            param.Add(new KeyValuePair<string, string>("p", p.ToString()));
            param.Add(new KeyValuePair<string, string>("size", size.ToString()));
            if (addiction.Key != null)
            {
                param.Add(addiction);
            }
            FormUrlEncodedContent content = new FormUrlEncodedContent(param);
            return content;
        }

        private static HttpClient Default_HttpClient()
        {
            HttpClient httpclient = new HttpClient();
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(Helper.Resource.user_auth_key))
            {
                httpclient.DefaultRequestHeaders.Connection.Add("Keep-Alive");
                httpclient.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("gzip"));
                httpclient.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("VmovierApp", "5.0.9"));
                httpclient.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("Android", "6.0"));
                httpclient.DefaultRequestHeaders.Add("Auth-Key", ApplicationData.Current.LocalSettings.Values[Helper.Resource.user_auth_key].ToString());
            }
            return httpclient;
        }

        public static string Set_Collect_Request(string data) //收藏或取消收藏请求
        {
            HttpClient httpclient = Default_HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            List<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>();
            string result = "";
            try
            {
                param.Add(new KeyValuePair<string, string>("data", data));
                response = httpclient.PostAsync(Helper.Resource.set_collect_api, new FormUrlEncodedContent(param)).Result;
                result = response.Content.ReadAsStringAsync().Result;
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
