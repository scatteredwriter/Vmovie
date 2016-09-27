using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace V电影.HttpRequest
{
    public class VmovieRequset
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
            if (App.settings.Values.ContainsKey(Resource.APPTheme.user_auth_key))
            {
                httpclient.DefaultRequestHeaders.Connection.Add("Keep-Alive");
                httpclient.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("gzip"));
                httpclient.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("VmovierApp", "5.0.9"));
                httpclient.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("Android", "6.0"));
                httpclient.DefaultRequestHeaders.Add("Auth-Key", App.settings.Values[Resource.APPTheme.user_auth_key].ToString());
            }
            return httpclient;
        }

        public static async Task<string> Flipview_Requset() //幻灯片请求
        {
            HttpClient httpclient = Default_HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            Cache.JsonCache cache = new Cache.JsonCache();
            string result = "";
            try
            {
                response = await httpclient.GetAsync(API.Vmovies_API.flipview_api);
                result = await response.Content.ReadAsStringAsync();
                await cache.Cache(result, "FlipView");
                return result;
            }
            catch (Exception)
            {
                result = await cache.Get_Cache("FlipView");
                return result;
            }
        }

        public static async Task<string> Lastest_Requset(int p) //最新数据请求
        {
            HttpClient httpclient = Default_HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            Cache.JsonCache cache = new Cache.JsonCache();
            string result = "";
            string api = API.Vmovies_API.get_by_tab_api;
            try
            {
                response = await httpclient.PostAsync(api, Default_Params(p, Params.Params.lastest_size, new KeyValuePair<string, string>("tab", Params.Params.lastest_tab)));
                result = await response.Content.ReadAsStringAsync();
                await cache.Cache(result, "Lastest");
                return result;
            }
            catch (Exception)
            {
                result = await cache.Get_Cache("Lastest");
                return result;
            }
        }

        public static async Task<string> Cates_Request() //频道数据请求
        {
            HttpClient httpclient = Default_HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            Cache.JsonCache cache = new V电影.Cache.JsonCache();
            string result = "";
            try
            {
                response = await httpclient.GetAsync(API.Vmovies_API.cate_lists_api);
                result = await response.Content.ReadAsStringAsync();
                await cache.Cache(result, "Cates");
                return result;
            }
            catch (Exception)
            {
                result = await cache.Get_Cache("Cates");
                return result;
            }
        }

        public static async Task<string> Cate_Content_Requset(int p, int cateid, string tab) //频道列表内容请求
        {
            HttpClient httpclient = Default_HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            Cache.JsonCache cache = new Cache.JsonCache();
            string result = "";
            string api = "";
            try
            {
                if (tab != null)
                {
                    api = API.Vmovies_API.get_by_tab_api;
                    response = await httpclient.PostAsync(api, Default_Params(p, Params.Params.cate_size, new KeyValuePair<string, string>("tab", tab)));
                }
                else
                {
                    api = API.Vmovies_API.cate_content_api;
                    response = await httpclient.PostAsync(api, Default_Params(p, Params.Params.cate_size, new KeyValuePair<string, string>("cateid", cateid.ToString())));
                }
                result = await response.Content.ReadAsStringAsync();
                await cache.Cache(result, "Cate_Content");
                return result;
            }
            catch (Exception)
            {
                result = await cache.Get_Cache("Cate_Content");
                return result;
            }
        }

        public static async Task<string> Series_Request(int p) //系列数据请求
        {
            HttpClient httpclient = Default_HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            Cache.JsonCache cache = new Cache.JsonCache();
            string result = "";
            string api = API.Vmovies_API.series_api;
            try
            {
                response = await httpclient.PostAsync(api, Default_Params(p, Params.Params.series_size, new KeyValuePair<string, string>(null, null)));
                result = await response.Content.ReadAsStringAsync();
                await cache.Cache(result, "Series");
                return result;
            }
            catch (Exception)
            {
                result = await cache.Get_Cache("Series");
                return result;
            }
        }

        public static async Task<string> Series_Follow_Request(int seriesid, int is_follow) //系列追剧接口
        {
            HttpClient httpclient = Default_HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            List<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>();
            string result = "";
            try
            {
                param.Add(new KeyValuePair<string, string>("seriesid", seriesid.ToString()));
                param.Add(new KeyValuePair<string, string>("isfollow", is_follow.ToString()));
                response = await httpclient.PostAsync(API.Vmovies_API.series_follow_api, new FormUrlEncodedContent(param));
                result = await response.Content.ReadAsStringAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<string> Behind_Cates_Request() //幕后文章列表数据请求
        {
            HttpClient httpclient = Default_HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            Cache.JsonCache cache = new Cache.JsonCache();
            string api = API.Vmovies_API.backstage_lists_api;
            string result = "";
            try
            {
                response = await httpclient.GetAsync(api);
                result = await response.Content.ReadAsStringAsync();
                await cache.Cache(result, "Behind_Cates");
                return result;
            }
            catch (Exception)
            {
                result = await cache.Get_Cache("Behind_Cates");
                return result;
            }
        }

        public static async Task<string> Behind_Content_Request(int p, int cateid) //幕后文章内容数据请求
        {
            HttpClient httpclient = Default_HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            Cache.JsonCache cache = new Cache.JsonCache();
            string api = API.Vmovies_API.backstage_content_api;
            string result = "";
            try
            {
                response = await httpclient.PostAsync(api, Default_Params(p, Params.Params.behind_size, new KeyValuePair<string, string>("cateid", cateid.ToString())));
                result = await response.Content.ReadAsStringAsync();
                await cache.Cache(result, "Behind_Content");
                return result;
            }
            catch (Exception)
            {
                result = await cache.Get_Cache("Behind_Content");
                return result;
            }
        }

        public static async Task<string> Search_Request(int p, string keyword) //搜索数据请求
        {
            HttpClient httpclient = Default_HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            string api = API.Vmovies_API.search_api;
            string result = "";
            try
            {
                response = await httpclient.PostAsync(api, Default_Params(p, Params.Params.search_size, new KeyValuePair<string, string>("kw", keyword)));
                result = await response.Content.ReadAsStringAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<string> Login_Request(string email, string password) //登录请求
        {
            HttpClient httpclient = new HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            string api = API.Vmovies_API.login_api;
            string result = "";
            try
            {
                List<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>();
                param.Add(new KeyValuePair<string, string>("email", email));
                param.Add(new KeyValuePair<string, string>("password", password));
                response = await httpclient.PostAsync(api, new FormUrlEncodedContent(param));
                result = await response.Content.ReadAsStringAsync();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static async Task<string> Order_Request(int p) //我的订阅请求
        {
            HttpClient httpclient = Default_HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            Cache.JsonCache cache = new Cache.JsonCache();
            string api = API.Vmovies_API.order_api;
            string result = "";
            try
            {
                response = await httpclient.PostAsync(api, Default_Params(p, Params.Params.behind_size, new KeyValuePair<string, string>(null, null)));
                result = await response.Content.ReadAsStringAsync();
                await cache.Cache(result, "Order");
                return result;
            }
            catch (Exception)
            {
                result = await cache.Get_Cache("Order");
                return result;
            }
        }

        public static async Task<string> View_Content_Request(int postid) //文章内容请求
        {
            HttpClient httpclient = Default_HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            List<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>();
            string result = "";
            try
            {
                param.Add(new KeyValuePair<string, string>("postid", postid.ToString()));
                response = await httpclient.PostAsync(API.Vmovies_API.view_content_api, new FormUrlEncodedContent(param));
                result = await response.Content.ReadAsStringAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<string> Series_View_Request(int series_id)
        {
            HttpClient httpclient = Default_HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            List<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>();
            string result = "";
            try
            {
                param.Add(new KeyValuePair<string, string>("postid", series_id.ToString()));
                response = await httpclient.PostAsync(API.Vmovies_API.series_view_api, new FormUrlEncodedContent(param));
                result = await response.Content.ReadAsStringAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
