using System;
using System.Threading.Tasks;

namespace SiteCopier.Interfaces
{
    public interface IDownloader
    {
        Task<byte[]> Load(Uri uri);
    }
}
