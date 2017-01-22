using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace V电影.Params
{
    public class Params
    {
        public static StorageFolder image_folder = null;

        public static async Task<StorageFolder> Get_ImageCacheFolder()
        {
            image_folder = await ApplicationData.Current.LocalCacheFolder.CreateFolderAsync("ImageCache", CreationCollisionOption.OpenIfExists);
            return image_folder;
        }

        #region FlipView请求参数
        public const string flipview_floder = "FlipView";
        #endregion

        #region Lastest请求参数
        public const int lastest_size = 20;
        public const string lastest_tab = "lastest";
        public const string lastest_floder = "Cate_Content";
        #endregion

        #region Cate参数请求
        public const int cate_size = 10;
        public const string cate_floder = "Cates";
        public const string cate_content_floder = "Cate_Content";
        #endregion

        #region Series请求参数
        public const int series_size = 20;
        public const string series_floder = "Series";
        #endregion

        #region Behind请求参数
        public const int behind_size = 10;
        public const string behind_floder = "Behind";
        #endregion

        #region Search请求参数
        public const int search_size = 10;
        #endregion

        #region Order请求参数
        public const int order_size = 10;
        #endregion

        #region 评论请求
        public const int comment_size = 10;
        #endregion

        #region 消息请求
        public const int message_size = 10;
        #endregion

        #region 喜欢的集合请求
        public const int collect_size = 10;
        #endregion

    }
}