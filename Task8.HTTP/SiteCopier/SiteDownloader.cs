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
        private readonly ISaver _saver;
        private readonly IDownloader _downloader;
        private readonly ILogger _logger;
        private readonly Uri _startUri;
        private readonly string _downloadPath;
        private readonly int _referenceDepth;
        private readonly IList<string> _extensions;
        private readonly DomainTransfer _transferType;
        private readonly IDictionary<string, string> _linksMapping;

        public SiteDownloader(ILinkParser linkParser, IFileSaver fileSaver, ISaver saver, IDownloader downloader, ILogger logger, DownloadOptions options)
        {
            _linkParser = linkParser;
            _fileSaver = fileSaver;
            _saver = saver;
            _downloader = downloader;
            _logger = logger;
            _startUri = new Uri(options.StartUri);
            _downloadPath = options.DownloadPath;
            _referenceDepth = options.ReferenceDepth;
            _extensions = GetExtensionsFromString(options.ExtensionRestriction);
            _transferType = options.DomainTransfer;
            _linksMapping = new Dictionary<string, string>();
        }

        public async void Load()
        {
            await Load(_startUri, 0);
            _logger.Info("Save mappings...");
            var mappings = _linksMapping.Select((x, i) => $"{i + 1}\t{x.Key}\t-\t{x.Value}");
            _saver.Save(mappings.ToArray(), _downloadPath, "mapping.txt");
            _logger.Info("Ready!");
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

                var fileName = _fileSaver.Save(byteContent, pathToSave, string.IsNullOrWhiteSpace(extension) ? "html" : extension);

                string fullPath = Path.Combine(pathToSave, fileName);
                _linksMapping.Add(fullPath, uri.OriginalString);

                if (currentDepth >= _referenceDepth) return;

                var content = Encoding.UTF8.GetString(byteContent);
                var links = _linkParser.GetLinks(content, _extensions);

                foreach (var link in _linkParser.FilterLinks(links, _transferType, _startUri))
                {
                    if (_linksMapping.Select(x => x.Value).FirstOrDefault(x => x.Equals(link.OriginalString)) != null)
                    {
                        _logger.Info($"Duplicate link {link.OriginalString}");
                        continue;
                    }

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
    }
}
