using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Web.Http;

namespace V电影.Cache
{
    public class ImageCache
    {
        private const string image_cache_folder_name = "ImageCache";
        private StorageFolder _localFolder;

        /// <summary>
        /// 得到需要的图片的位图集合
        /// </summary>
        /// <param name="ImageUrls"></param>
        /// <returns>图片位图的List集合</returns>
        public async Task<ObservableCollection<ImageSource>> Get_Image_Source(List<string> ImageUrls, string foldername)
        {
            await Create_FoldName(foldername);
            string filename = "";
            ObservableCollection<ImageSource> lists = new ObservableCollection<ImageSource>();
            for (int i = 0; i < ImageUrls.Count; i++)
            {
                SoftwareBitmapSource source = new SoftwareBitmapSource();
                SoftwareBitmap bitmap;
                filename = ImageUrls[i].Substring(ImageUrls[i].LastIndexOf('/') + 1);
                bool is_contain = await Is_Contain_File(filename);
                if (is_contain)
                {
                    bitmap = await ReadFromFile(filename);
                }
                else
                {
                    await WriteToFile(await DownloadImage(ImageUrls[i]), filename);
                    bitmap = await ReadFromFile(filename);
                }
                if (bitmap != null)
                {
                    await source.SetBitmapAsync(bitmap);
                    lists.Add(source);
                }
                else
                {
                    lists.Add(new BitmapImage(new Uri("ms-appx:///Assets/main_pic_shadow.png", UriKind.Absolute)));
                }
            }
            return lists;
        }

        private async Task Create_FoldName(string foldername)
        {
            _localFolder = await ApplicationData.Current.LocalCacheFolder.CreateFolderAsync(image_cache_folder_name, CreationCollisionOption.OpenIfExists);
            _localFolder = await _localFolder.CreateFolderAsync(foldername, CreationCollisionOption.OpenIfExists);
        }

        private async Task<bool> Is_Contain_File(string filename)
        {
            StorageFileQueryResult result = _localFolder.CreateFileQuery();
            IReadOnlyList<StorageFile> files = await result.GetFilesAsync();
            for (int i = 0; i < files.Count; i++)
            {
                if (files[i].Name == filename)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 从网络上下载图片
        /// </summary>
        /// <param name="url"></param>
        /// <returns>下载的图片的位图</returns>
        private async Task<SoftwareBitmap> DownloadImage(string url)
        {
            try
            {
                HttpClient hc = new HttpClient();
                HttpResponseMessage resp = await hc.GetAsync(new Uri(url));
                resp.EnsureSuccessStatusCode();
                IInputStream inputStream = await resp.Content.ReadAsInputStreamAsync();
                IRandomAccessStream memStream = new InMemoryRandomAccessStream();
                await RandomAccessStream.CopyAsync(inputStream, memStream);
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(memStream);
                SoftwareBitmap softBmp = await decoder.GetSoftwareBitmapAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
                return softBmp;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 将网络下载的图片存入成本地文件
        /// </summary>
        /// <param name="softwareBitmap"></param>
        /// <returns>返回图片的文件名</returns>
        private async Task WriteToFile(SoftwareBitmap softwareBitmap, string filename)
        {
            if (softwareBitmap != null)
            {
                // save image file to cache
                StorageFile file = await _localFolder.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists);
                using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
                    encoder.SetSoftwareBitmap(softwareBitmap);
                    await encoder.FlushAsync();
                }
            }
        }

        /// <summary>
        /// 读取缓存中的文件
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>缓存中的图片的位图</returns>
        private async Task<SoftwareBitmap> ReadFromFile(string filename)
        {
            StorageFile file = await _localFolder.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists);
            //var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri( filename));
            try
            {
                using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read))
                {
                    // Create the decoder from the stream
                    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                    // Get the SoftwareBitmap representation of the file
                    SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
                    return softwareBitmap;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }


    }
}
