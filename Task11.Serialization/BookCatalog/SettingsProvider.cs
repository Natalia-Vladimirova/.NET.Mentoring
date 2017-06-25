using System.Configuration;
using BookCatalog.Interfaces;

namespace BookCatalog
{
    public class SettingsProvider : ISettingsProvider
    {
        public string GetSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
