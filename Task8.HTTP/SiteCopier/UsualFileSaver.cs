using System.IO;
using SiteCopier.Interfaces;

namespace SiteCopier
{
    public class UsualFileSaver : ISaver
    {
        public void Save(string[] text, string path, string fileName)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            
            string filePath = Path.Combine(path, fileName);

            using (var writer = new StreamWriter(filePath, false))
            {
                foreach (var str in text)
                {
                    writer.WriteLine(str);
                }

                writer.Close();
            }
        }
    }
}
