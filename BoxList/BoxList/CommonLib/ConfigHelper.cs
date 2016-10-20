using System;
using System.Configuration;
using System.IO;

namespace BoxList.CommonLib
{
    public class ConfigHelper
    {
        private string baseDir = string.Empty;

        public static readonly ConfigHelper Instance = new ConfigHelper();

        private ConfigHelper()
        {
            baseDir = AppDomain.CurrentDomain.BaseDirectory;
        }

        public string GetAppSettingsConfigValue(string configFile, string key)
        {
            try
            {
                ExeConfigurationFileMap map = new ExeConfigurationFileMap();
                map.ExeConfigFilename = Path.Combine(baseDir, configFile);
                Configuration config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
                return config.AppSettings.Settings[key].Value;
            }
            catch
            {
                return string.Empty;
            }
        }

        public void SetAppSettingsConfigValue(string configFile, string key, string value, string section)
        {
            ExeConfigurationFileMap map = new ExeConfigurationFileMap();
            map.ExeConfigFilename = Path.Combine(baseDir, configFile);
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            config.AppSettings.Settings[key].Value = value;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(section);
        }

        public string GetConnectionStringsConfigValue(string configFile, string key)
        {
            try
            {
                return GetConnectionStringsSettings(configFile, key).ConnectionString;
            }
            catch
            {
                return string.Empty;
            }
        }

        public ConnectionStringSettings GetConnectionStringsSettings(string configFile, string key)
        {
            ExeConfigurationFileMap map = new ExeConfigurationFileMap();
            map.ExeConfigFilename = Path.Combine(baseDir, configFile);
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            return config.ConnectionStrings.ConnectionStrings[key];
        }

        public void SetConfigFile(string configFileName)
        {
            string configFilePath = Path.Combine(baseDir, configFileName);
            AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", configFilePath);
        }

        public void SetConfigFile(string configFileName, string configFileDirectory)
        {
            string configFilePath = Path.Combine(configFileDirectory, configFileName);
            AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", configFilePath);
        }
    }
}
