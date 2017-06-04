using System;
using System.IO;
using NLog;
using SiteCopier.Interfaces;

namespace SiteCopier
{
    public class FileSaver : IFileSaver
    {
        private readonly ILogger _logger;

        public FileSaver(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Save some text to a file with a generated name.
        /// </summary>
        /// <param name="text">
        /// Text to save.
        /// </param>
        /// <param name="path">
        /// Path to the folder the file is saved to.
        /// </param>
        /// <param name="extension">
        /// Extension of the file the text is saved to.
        /// </param>
        /// <returns>
        /// The generated file name with extension.
        /// </returns>
        public string Save(byte[] text, string path, string extension)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fileName = string.IsNullOrWhiteSpace(extension) 
                ? Guid.NewGuid().ToString() 
                : $"{Guid.NewGuid()}.{extension}";

            string filePath = Path.Combine(path, fileName);

            using (var writer = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                _logger.Info("{0}: save the file with the name {1}", path, fileName);
                writer.Write(text, 0, text.Length);
                writer.Close();
            }

            return fileName;
        }
    }
}
