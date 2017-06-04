using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using SiteCopier.Interfaces;
using SiteCopier.Models;

namespace SiteCopier
{
    public class SiteDownloader : ISiteDownloader
    {
        private readonly ILinkParser _linkParser;
        private readonly IFileSaver _fileSaver;
        private readonly IDownloader _downloader;
        private readonly ILogger _logger;
        private readonly Uri _startUri;
        private readonly string _downloadPath;
        private readonly int _referenceDepth;
        private readonly IList<string> _extensions;
        private readonly DomainTransfer _transferType;

        public SiteDownloader(ILinkParser linkParser, IFileSaver fileSaver, IDownloader downloader, ILogger logger, DownloadOptions options)
        {
            _linkParser = linkParser;
            _fileSaver = fileSaver;
            _downloader = downloader;
            _logger = logger;
            _startUri = new Uri(options.StartUri);
            _downloadPath = options.DownloadPath;
            _referenceDepth = options.ReferenceDepth;
            _extensions = GetExtensionsFromString(options.ExtensionRestriction);
            _transferType = options.DomainTransfer;
        }

        public async void Load()
        {
            await Load(_startUri, 0);
            _logger.Info("Downloaded!");

        }

        private async Task Load(Uri uri, int currentDepth)
        {
            _logger.Info("Downloading content from {0}", uri.OriginalString);
            
            try
            {
                byte[] byteContent = await _downloader.Load(uri);
                string extension = Path.GetExtension(uri.AbsolutePath);

                string pathToSave = currentDepth == 0
                    ? _downloadPath
                    : Path.Combine(_downloadPath, $"level{currentDepth}");

                _fileSaver.Save(byteContent, pathToSave, string.IsNullOrWhiteSpace(extension) ? "html" : extension);

                if (currentDepth >= _referenceDepth) return;

                var content = Encoding.UTF8.GetString(byteContent);
                var links = _linkParser.GetLinks(content, _extensions);

                foreach (var link in FilterLinks(links, _transferType))
                {
                    await Load(link, currentDepth + 1);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
            }
        }

        private IList<string> GetExtensionsFromString(string extensions)
        {
            if (string.IsNullOrWhiteSpace(extensions)) return null;
            
            return extensions
                .Split(new [] {","}, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim().ToLowerInvariant())
                .ToList();
        }

        private IEnumerable<Uri> FilterLinks(IList<Uri> links, DomainTransfer transferType)
        {
            switch (transferType)
            {
                case DomainTransfer.InsideCurrentDomain:
                    return links.Where(x => x.Host.Equals(_startUri.Host, StringComparison.OrdinalIgnoreCase));
                case DomainTransfer.InsideCurrentPath:
                    return links.Where(x => GetUriPath(x).Equals(GetUriPath(_startUri), StringComparison.OrdinalIgnoreCase));
                default:
                    return links;
            }
        }

        private string GetUriPath(Uri uri)
        {
            var path = uri.AbsolutePath;
            var extension = Path.GetExtension(path);
            int extensionIndex = path.LastIndexOf(extension, StringComparison.OrdinalIgnoreCase);
            return string.Concat(uri.Host, path.Substring(0, extensionIndex));
        }
    }
}
