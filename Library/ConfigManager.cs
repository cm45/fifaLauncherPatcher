using Newtonsoft.Json;
using System;
using System.IO;

namespace Library
{
    public static class ConfigManager
    {
        public static AppConfig AppConfig
        {
            get
            {
                if (!File.Exists(Paths.CONFIG_PATH)) return Reset();
                try
                {
                    return JsonConvert.DeserializeObject<AppConfig>(File.ReadAllText(Paths.CONFIG_PATH));
                }
                catch
                {
                    //MessageBox.Show("Couldn't read configurations... Resetting config file...");
                    return Reset();
                }
            }
            set
            {
                try
                {
                    File.WriteAllText(Paths.CONFIG_PATH, JsonConvert.SerializeObject(value, Formatting.Indented));
                }
                catch
                {
                    throw new NotImplementedException();
                    //MessageBox.Show("Couldn't save configurations");
                }
            }
        }

        private static AppConfig Reset()
        {
            AppConfig = new AppConfig(Paths.DEFAULT_GAMEPATH);
            return AppConfig;
        }
    }
}
