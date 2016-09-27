using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Search;

namespace V电影.Cache
{
    public class JsonCache
    {
        private const string foldername = "JsonCache";
        private StorageFolder _localFolder;

        public async Task Cache(string json, string json_type)
        {
            _localFolder = await ApplicationData.Current.LocalCacheFolder.CreateFolderAsync(foldername, CreationCollisionOption.OpenIfExists);
            StorageFile file = await _localFolder.CreateFileAsync(json_type + ".txt", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, json, Windows.Storage.Streams.UnicodeEncoding.Utf8);
        }

        public async Task<string> Get_Cache(string json_type)
        {
            string json = "";
            _localFolder = await ApplicationData.Current.LocalCacheFolder.CreateFolderAsync(foldername, CreationCollisionOption.OpenIfExists);
            StorageFileQueryResult result = _localFolder.CreateFileQuery();
            IReadOnlyList<StorageFile> files = await result.GetFilesAsync();
            for (int i = 0; i < files.Count; i++)
            {
                if (files[i].Name == json_type + ".txt")
                {
                    json = await FileIO.ReadTextAsync(files[i], Windows.Storage.Streams.UnicodeEncoding.Utf8);
                    return json;
                }
            }
            return null;
        }
    }
}
