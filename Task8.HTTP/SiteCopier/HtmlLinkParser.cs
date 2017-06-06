using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsQuery;
using NLog;
using SiteCopier.Interfaces;

namespace SiteCopier
{
    public class HtmlLinkParser : ILinkParser
    {
        private readonly ILogger _logger;

        public HtmlLinkParser(ILogger logger)
        {
            _logger = logger;
        }

        public IList<Uri> GetLinks(string text, IList<string> resourceExtensions)
        {
            var result = new List<Uri>();
            
            CQ cq = CQ.Create(text);
            
            result.AddRange(GetLinks("a", "href", cq, resourceExtensions));
            result.AddRange(GetLinks("link", "href", cq, resourceExtensions));
            result.AddRange(GetLinks("img", "src", cq, resourceExtensions));
            result.AddRange(GetLinks("script", "src", cq, resourceExtensions));

            return result;
        }

        private IEnumerable<Uri> GetLinks(string tag, string sourceAttribute, CQ csQuery, IList<string> resourceExtensions)
        {
            return csQuery.Find(tag)
                .Select(x => GetUri(x.GetAttribute(sourceAttribute, string.Empty)))
                .Where(uri => IsAcceptable(uri, resourceExtensions))
                .ToList();
        }

        private Uri GetUri(string uri)
        {
            if (string.IsNullOrWhiteSpace(uri)) return null;

            try
            {
                _logger.Info("Parsing uri {0}", uri);
                return new Uri(uri);
            }
            catch (UriFormatException e)
            {
                _logger.Warn(e.Message);
                return null;
            }
        }

        private bool IsAcceptable(Uri uri, IList<string> resourceExtensions)
        {
            if (uri == null) return false;

            if (resourceExtensions == null || !resourceExtensions.Any()) return true;

            var lastSegment = uri.Segments.Last();

            var extension = Path.GetExtension(lastSegment)?.TrimStart('.');

            if (string.IsNullOrWhiteSpace(extension)) return true;
            
            return resourceExtensions.Any(x => string.Equals(x, extension, StringComparison.OrdinalIgnoreCase));
        }
    }
}
