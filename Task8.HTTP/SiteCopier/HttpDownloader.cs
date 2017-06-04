using System;
using System.Net.Http;
using System.Threading.Tasks;
using SiteCopier.Interfaces;

namespace SiteCopier
{
    public class HttpDownloader : IDownloader
    {
        public async Task<byte[]> Load(Uri uri)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(uri);
            return await response.Content.ReadAsByteArrayAsync();
        }
    }
}
