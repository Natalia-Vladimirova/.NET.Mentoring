namespace SiteCopier.Interfaces
{
    public interface ISaver
    {
        /// <summary>
        /// Save some text to a file.
        /// </summary>
        /// <param name="text">
        /// Text to save.
        /// </param>
        /// <param name="path">
        /// Path to the folder the file is saved to.
        /// </param>
        /// <param name="fileName">
        /// The name of the file which the file is saved with.
        /// </param>
        void Save(string[] text, string path, string fileName);
    }
}
