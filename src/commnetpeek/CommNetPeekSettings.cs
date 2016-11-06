using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace commnetpeek
{
    public class CommNetPeekSettings
    {
        private static Settings privateInstance;
        public static Settings Instance
        {
            get
            {
                if (privateInstance is Settings && privateInstance.settingsLoaded)
                    return privateInstance;
                else
                    return privateInstance = Settings.Load();
            }
        }
    }

    public class Settings
    {
        public bool settingsLoaded = false;

        //Global settings to be read from the setting cfg
        //-----
        [Persistent] public int MajorVersion;
        [Persistent] public int MinorVersion;
        [Persistent] public bool EnableSignalDelay;
        [Persistent] public float SpeedOfLight;
        //-----

        public static String DefaultSettingFile
        {
            get
            {
                return KSPUtil.ApplicationRootPath + "/GameData/CommNetPeek/commnetpeek_settings.cfg";
            }
        }

        public static Settings Load()
        {
            Settings settings = new Settings();
            ConfigNode settingNode = ConfigNode.Load(Settings.DefaultSettingFile);

            if(settingNode == null)
            {
                CNPLog.Error("The CommNetPeek setting file '{0}' is not found!", Settings.DefaultSettingFile);
                return null;
            }

            settingNode = settingNode.GetNode("CommNetPeekSettings");
            bool success = ConfigNode.LoadObjectFromConfig(settings, settingNode);
            CNPLog.Verbose("Loading the CommNetPeek settings with {0}: LOADED {1}", settingNode, success ? "OK" : "FAIL");
            settings.settingsLoaded = success;

            return settings;
        }
    }
}
