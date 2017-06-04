using System;
using System.Collections.Generic;

namespace SiteCopier.Interfaces
{
    public interface ILinkParser
    {
        /// <summary>
        /// Parse some text with links.
        /// </summary>
        /// <param name="text">
        /// Text to parse.
        /// </param>
        /// <param name="resourceExtensions">
        /// Extensions of files to be found. If the list is null or empty all links will be found.
        /// </param>
        /// <returns>
        /// A list of links found in the text with specified extensions.
        /// </returns>
        IList<Uri> GetLinks(string text, IList<string> resourceExtensions);
    }
}
