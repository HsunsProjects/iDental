using System.Configuration;

namespace iDental.Class
{
    public class ConfigManage
    {
        /// <summary>
        /// 判斷 Config KEY 有無值
        /// </summary>
        /// <param name="key">Config Key</param>
        /// <returns>true/false</returns>
        public static bool ReadSetting(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                if (appSettings[key] != null)
                {
                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[key]))
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
        /// 寫入或更改 Config Key 的 Value
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        public static void AddUpdateAppCongig(string key, string value)
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
    }
}
