using ModLib;
using ModLib.Attributes;
using System.Xml.Serialization;

namespace SoundTheAlarm {
    public class STASettings : SettingsBase {
        public const string InstanceID = "SoundTheAlarmSettings";
        public override string ModName => "Sound The Alarm";
        public override string ModuleFolderName => STAMain.ModuleFolderName;
        [XmlElement]
        public override string ID { get; set; } = InstanceID;

        public static STASettings Instance {
            get {
                return (STASettings)SettingsDatabase.GetSettings(InstanceID);
            }
        }

        [XmlElement]
        [SettingProperty("Enable Village Popup", "Enables popups when your villages are attacked.")]
        [SettingPropertyGroup("1. Fiefs")]
        public bool EnableVillagePopup { get; set; } = true;

        [XmlElement]
        [SettingProperty("Enable Castle Popup", "Enables popups when your castles are attacked.")]
        [SettingPropertyGroup("1. Fiefs")]
        public bool EnableCastlePopup { get; set; } = true;

        [XmlElement]
        [SettingProperty("Enable Town Popup", "Enables popups when your towns are attacked.")]
        [SettingPropertyGroup("1. Fiefs")]
        public bool EnableTownPopup { get; set; } = true;

        [XmlElement]
        [SettingProperty("Enable Declaration of War Popup", "Enables popups when factions declare war on one another.")]
        [SettingPropertyGroup("2. Declarations")]
        public bool EnableWarPopup { get; set; } = true;

        [XmlElement]
        [SettingProperty("Enable Declaration of Peace Popup", "Enables popups when factions declare peace with one another.")]
        [SettingPropertyGroup("2. Declarations")]
        public bool EnablePeacePopup { get; set; } = true;
        
        [XmlElement]
        [SettingProperty("Enable Minor Faction Popups", "Enables popups for declarations of war/peace for minor factions.")]
        [SettingPropertyGroup("2. Declarations")]
        public bool EnableMinorFactionPopup { get; set; } = true;

        [XmlElement]
        [SettingProperty("Enable Pause Game on Popup", "Enables the game to be paused when popups are shown.")]
        [SettingPropertyGroup("3. Miscellaneous")]
        public bool PauseGameOnPopup { get; set; } = true;

        [XmlElement]
        [SettingProperty("Time to Remove Village From List", 0.0f, 20.0f, "Sets the time to remove the village from the managed settlements list.")]
        [SettingPropertyGroup("3. Miscellaneous")]
        public float TimeToRemoveVillageFromList { get; set; } = 10.0f;

        [XmlElement]
        [SettingProperty("Enable Debug Messages", "Enables debug messages.")]
        [SettingPropertyGroup("4. Debug")]
        public bool EnableDebugMessages { get; set; } = false;
    }
}
