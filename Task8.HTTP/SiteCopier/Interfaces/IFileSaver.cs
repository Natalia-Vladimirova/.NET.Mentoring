namespace SiteCopier.Interfaces
{
    public interface IFileSaver
    {
        /// <summary>
        /// Save some text to a file.
        /// </summary>
        /// <param name="text">
        /// Text to save.
        /// </param>
        /// <param name="path">
        /// Path to save the file.
        /// </param>
        /// <param name="extension">
        /// Extension of the file the text is saved to.
        /// </param>
        /// <returns>
        /// File name with extension.
        /// </returns>
        string Save(byte[] text, string path, string extension);
    }
}
