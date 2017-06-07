using System;
using System.Collections.Generic;
using SiteCopier.Models;

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

        /// <summary>
        /// Filter links according to domain tranfser type.
        /// </summary>
        /// <param name="links">
        /// Links to be filtered.
        /// </param>
        /// <param name="transferType">
        /// Transfer type of acceptable links.
        /// </param>
        /// <param name="startUri">
        /// Uri of the start page.
        /// </param>
        /// <returns>
        /// A filtered list of links.
        /// </returns>
        IEnumerable<Uri> FilterLinks(IEnumerable<Uri> links, DomainTransfer transferType, Uri startUri);
    }
}
