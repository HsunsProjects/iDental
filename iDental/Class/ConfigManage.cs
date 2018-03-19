using System.Configuration;

namespace iDental.Class
{
    public class ConfigManage
    {
        /// <summary>
        /// 寫入或更改 Config Key 的 Value
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        public static void AddUpdateAppConfig(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException cex)
            {
                ErrorLog.ErrorMessageOutput(cex.ToString());
            }
        }

        /// <summary>
        /// 載入config值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string ReadAppConfig(string key)
        {
            try
            {
                if (IsConfigValue(key))
                {
                    return ConfigurationManager.AppSettings[key];
                }
            }
            catch (ConfigurationErrorsException cex)
            {
                ErrorLog.ErrorMessageOutput(cex.ToString());
            }
            return null;
        }

        /// <summary>
        /// 建立config key
        /// </summary>
        /// <param name="key">key name</param>
        public static void CreateConfig(string key)
        {
            try
            {
                if (!IsConfigKey(key))
                {
                    var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    var settings = configFile.AppSettings.Settings;
                    settings.Add(key, string.Empty);
                    configFile.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
                }
            }
            catch (ConfigurationErrorsException cex)
            {
                ErrorLog.ErrorMessageOutput(cex.ToString());
            }
        }

        /// <summary>
        /// 建立config key & value
        /// </summary>
        /// <param name="key">key name</param>
        /// <param name="value">value</param>
        public static void CreateConfig(string key, string value)
        {
            try
            {
                if (!IsConfigKey(key))
                {
                    var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    var settings = configFile.AppSettings.Settings;
                    settings.Add(key, value);
                    configFile.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
                }
            }
            catch (ConfigurationErrorsException cex)
            {
                ErrorLog.ErrorMessageOutput(cex.ToString());
            }
        }

        /// <summary>
        /// 判斷config有沒有key
        /// </summary>
        /// <param name="key">key name</param>
        /// <returns></returns>
        public static bool IsConfigKey(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                if (appSettings[key] != null)
                {
                    return true;
                }
                return false;
            }
            catch (ConfigurationErrorsException cex)
            {
                ErrorLog.ErrorMessageOutput(cex.ToString());
            }
            return false;
        }

        /// <summary>
        /// 判斷config key有沒有值(沒有key等於沒有值)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsConfigValue(string key)
        {
            try
            {
                if (IsConfigKey(key))
                {
                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[key]))
                        return true;
                }
            }
            catch (ConfigurationErrorsException cex)
            {
                ErrorLog.ErrorMessageOutput(cex.ToString());
            }
            return false;
        }
    }
}
