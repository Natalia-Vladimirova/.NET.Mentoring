namespace SiteCopier.Models
{
    public class DownloadOptions
    {
        public string StartUri { get; set; }

        public string DownloadPath { get; set; }

        public int ReferenceDepth { get; set; }

        public DomainTransfer DomainTransfer { get; set; }

        public string ExtensionRestriction { get; set; }
    }
}
