using HarmonyLib;
using Bannerlord.ButterLib;
using MCM.Abstractions.Attributes.v1;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Attributes;
using MCM.Abstractions.Settings.Base.Global;
using MCM.Utils;
using System.Xml.Serialization;
using System.Runtime.CompilerServices;

namespace SoundTheAlarm
{
    //public class STASettings : BaseSettings
    public class STASettings : AttributeGlobalSettings<STASettings>
    {
        public const string InstanceID = "SoundTheAlarmSettings";
        public override string DisplayName => "Sound The Alarm";
        public override string FolderName => STAMain.ModuleFolderName;
        [XmlElement]
        public override string Id { get; } = InstanceID;


        //public static STASettings Instance = new STASettings();
        /*
        public static STASettings Instance
        {
            get
            {
                return (STASettings)SettingsDatabase.GetSettings<STASettings>();
            }
        }
       */


        [XmlElement]
        [SettingPropertyBool("Enable Village Popup", Order = 1, RequireRestart = false, HintText = "Enables popups when your villages are attacked.")]
        [SettingPropertyGroup("1. Fiefs")]
        public bool EnableVillagePopup { get; set; } = true;

        [XmlElement]
        [SettingPropertyBool("Enable Castle Popup", Order = 2, RequireRestart = false, HintText = "Enables popups when your castles are attacked.")]
        [SettingPropertyGroup("1. Fiefs")]
        public bool EnableCastlePopup { get; set; } = true;

        [XmlElement]
        [SettingPropertyBool("Enable Town Popup", Order = 3, RequireRestart = false, HintText = "Enables popups when your towns are attacked.")]
        [SettingPropertyGroup("1. Fiefs")]
        public bool EnableTownPopup { get; set; } = true;

        [XmlElement]
        [SettingPropertyBool("Enable Declaration of War Popup", Order = 4, RequireRestart = false, HintText = "Enables popups when factions declare war on one another.")]
        [SettingPropertyGroup("2. Declarations")]
        public bool EnableWarPopup { get; set; } = true;

        [XmlElement]
        [SettingPropertyBool("Enable Declaration of Peace Popup", Order = 5, RequireRestart = false, HintText = "Enables popups when factions declare peace with one another.")]
        [SettingPropertyGroup("2. Declarations")]
        public bool EnablePeacePopup { get; set; } = true;

        [XmlElement]
        [SettingPropertyBool("Enable Minor Faction Popups", Order = 6, RequireRestart = false, HintText = "Enables popups for declarations of war/peace for minor factions.")]
        [SettingPropertyGroup("2. Declarations")]
        public bool EnableMinorFactionPopup { get; set; } = true;

        [XmlElement]
        [SettingPropertyBool("Enable Pause Game on Popup", Order = 7, RequireRestart = false, HintText = "Enables the game to be paused when popups are shown.")]
        [SettingPropertyGroup("3. Miscellaneous")]
        public bool PauseGameOnPopup { get; set; } = true;

        [XmlElement]
        [SettingPropertyFloatingInteger("Time to Remove Village From List", 0.0f, 20.0f, Order = 8, HintText = "Sets the time to remove the village from the managed settlements list.")]
        [SettingPropertyGroup("3. Miscellaneous")]
        public float TimeToRemoveVillageFromList { get; set; } = 10.0f;

        [XmlElement]
        [SettingPropertyBool("Enable Debug Messages", Order = 7, RequireRestart = false, HintText = "Enables debug messages.")]
        [SettingPropertyGroup("4. Debug")]
        public bool EnableDebugMessages { get; set; } = false;
    }
}
