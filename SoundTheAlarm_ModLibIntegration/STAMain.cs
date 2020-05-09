using System;
using System.Windows.Forms;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;

namespace SoundTheAlarm {
    public class STAMain : MBSubModuleBase {

        public static readonly string ModuleFolderName = "SoundTheAlarm";

        // Method run during the initial movie on game startup
        protected override void OnBeforeInitialModuleScreenSetAsRoot() {
            base.OnBeforeInitialModuleScreenSetAsRoot();
            if (STASettings.Instance.EnableDebugMessages)
                InformationManager.DisplayMessage(new InformationMessage(
                    "STALibrary:\n" +
                    "Villages(" + STASettings.Instance.EnableVillagePopup + ")\n" +
                    "Castles(" + STASettings.Instance.EnableCastlePopup + ")\n" +
                    "Towns(" + STASettings.Instance.EnableTownPopup + ")\n" +
                    "Wars(" + STASettings.Instance.EnableWarPopup + ")\n" +
                    "Peace(" + STASettings.Instance.EnablePeacePopup + ")\n" +
                    "PauseOnPopup(" + STASettings.Instance.PauseGameOnPopup + ")\n" +
                    "TimeToRemove(" + STASettings.Instance.TimeToRemoveVillageFromList + ")\n",
                    new Color(1.0f, 0.0f, 0.0f)));
        }

        // Initialize Sound The Alarm once the save has been loaded
        public override void OnGameLoaded(Game game, object initializerObject) {
            base.OnGameLoaded(game, initializerObject);
            try {
                game.GameTextManager.LoadGameTexts(BasePath.Name + $"Modules/SoundTheAlarm/ModuleData/module_strings.xml");
                STAAction.Instance.Initialize();
                if(STASettings.Instance.EnableVillagePopup) {
                    CampaignEvents.VillageBeingRaided.AddNonSerializedListener(this, new Action<Village>(STAAction.Instance.DisplayVillageRaid));
                    CampaignEvents.VillageBecomeNormal.AddNonSerializedListener(this, new Action<Village>(STAAction.Instance.FinalizeVillageRaid));
                }
                if (STASettings.Instance.EnableCastlePopup || STASettings.Instance.EnableTownPopup) {
                    CampaignEvents.OnSiegeEventStartedEvent.AddNonSerializedListener(this, new Action<SiegeEvent>(STAAction.Instance.DisplaySiege));
                    CampaignEvents.OnSiegeEventEndedEvent.AddNonSerializedListener(this, new Action<SiegeEvent>(STAAction.Instance.FinalizeSiege));
                }
                if (STASettings.Instance.EnableWarPopup) {
                    CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction>(STAAction.Instance.OnDeclareWar));
                }
                if (STASettings.Instance.EnablePeacePopup) {
                    CampaignEvents.MakePeace.AddNonSerializedListener(this, new Action<IFaction, IFaction>(STAAction.Instance.OnDeclarePeace));
                }
            } catch (Exception ex) {
                MessageBox.Show("An error has occurred whilst initialising Sound The Alarm:\n\n" + ex.Message + "\n\n" + ex.StackTrace);
            }
        }
    }
}
