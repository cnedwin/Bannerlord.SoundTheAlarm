using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Settings.Base.Global;
using System.Collections.Generic;
using TaleWorlds.Localization;

namespace SoundTheAlarm
{
    public class STASettings : AttributeGlobalSettings<STASettings>
    {
        public override string Id { get; } = "SoundTheAlarmSettings";
        public override string DisplayName => new TextObject("战争警报1.5.5.1 (cnedwin)", new Dictionary<string, TextObject>
    {
        { "VERSION", new TextObject(typeof(STASettings).Assembly.GetName().Version.ToString(3)) }
    }).ToString();
        public override string FolderName { get; } = "SoundTheAlarm";
        public override string FormatType { get; } = "json2";

        [SettingPropertyBool("启用村庄弹窗", Order = 1, RequireRestart = false, HintText = "在您的村庄受到攻击时启用弹出窗口.")]
        [SettingPropertyGroup("1. 封地")]
        public bool EnableVillagePopup { get; set; } = true;

        [SettingPropertyBool("启用城堡弹窗", Order = 2, RequireRestart = false, HintText = "敌人攻击城堡时启用弹出窗口.")]
        [SettingPropertyGroup("1. 封地")]
        public bool EnableCastlePopup { get; set; } = true;

        [SettingPropertyBool("启用城镇弹窗", Order = 3, RequireRestart = false, HintText = "在您的城镇受到攻击时启用弹出窗口.")]
        [SettingPropertyGroup("1. 封地")]
        public bool EnableTownPopup { get; set; } = true;

        [SettingPropertyBool("启用战争宣言弹窗", Order = 1, RequireRestart = false, HintText = "当各派相互宣战时启用弹出窗口.")]
        [SettingPropertyGroup("2. 宣战")]
        public bool EnableWarPopup { get; set; } = true;

        [SettingPropertyBool("启用停战宣言弹窗", Order = 2, RequireRestart = false, HintText = "当各派宣布彼此和平时启用弹出窗口.")]
        [SettingPropertyGroup("2. 宣战")]
        public bool EnablePeacePopup { get; set; } = true;

        [SettingPropertyBool("启用小势力弹窗", Order = 3, RequireRestart = false, HintText = "使弹出式窗口可以宣告小派系宣战/和平.")]
        [SettingPropertyGroup("2. 宣战")]
        public bool EnableMinorFactionPopup { get; set; } = true;

        [SettingPropertyBool("启用在弹出窗口时暂停游戏", Order = 1, RequireRestart = false, HintText = "在显示弹出窗口时暂停游戏.")]
        [SettingPropertyGroup("3. 杂项")]
        public bool PauseGameOnPopup { get; set; } = true;

        [SettingPropertyFloatingInteger("从列表中删除村庄的时间", 0.0f, 20.0f, "设置从管理定居点列表中删除村庄的时间.")]
        [SettingPropertyGroup("3. 杂项")]
        public float TimeToRemoveVillageFromList { get; set; } = 10.0f;

        [SettingPropertyBool("启用调试信息", Order = 3, RequireRestart = false, HintText = "启用调试信息.")]
        [SettingPropertyGroup("4. 调试")]
        public bool EnableDebugMessages { get; set; } = false;
    }
}
