using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SoundTheAlarm
{
    public class STAAction
    {

        private Dictionary<string, float> managedSettlements;
        private Settlement settlementToTrack;

        private string GT_TRACK, GT_OK, GT_CLOSE;

        // Initialization method, called from STAMain - > OnGameLoaded
        public void Initialize()
        {
            managedSettlements = new Dictionary<string, float>();
            GT_TRACK = GameTexts.FindText("str_sta_ui_track", null).ToString();
            GT_OK = GameTexts.FindText("str_sta_ui_ok", null).ToString();
            GT_CLOSE = GameTexts.FindText("str_sta_ui_close", null).ToString();
        }

        // Action method fired once the VilageBeingRaided event fires
        public void DisplayVillageRaid(Village v)
        {
            RemoveExpiredFromManagedSettlements(v.Settlement);
            if (Hero.MainHero != null)
            {
                if (Hero.MainHero.IsAlive)
                {
                    if (ShouldAlertForSettlement(v.Settlement))
                    {
                        if (!managedSettlements.ContainsKey(v.Settlement.Name.ToString()))
                        {
                            managedSettlements.Add(v.Settlement.Name.ToString(), Campaign.CurrentTime);

                            string title = GetTitleFromVillage(v);
                            string display = GetDisplayFromVillage(v);

                            settlementToTrack = v.Settlement;

                            InformationManager.ShowInquiry(new InquiryData(title, display, true, true, GT_TRACK, GT_CLOSE, new Action(Track), null, ""), STASettings.Instance.PauseGameOnPopup);
                            if (STASettings.Instance.EnableDebugMessages)
                                InformationManager.DisplayMessage(new InformationMessage("STALibrary: " + display, new Color(1.0f, 0.0f, 0.0f)));
                        }
                    }
                }
            }
        }

        // Action method fired once the VillageBecomeNormal event fires
        public void FinalizeVillageRaid(Village v)
        {
            RemoveExpiredFromManagedSettlements(v.Settlement);
        }

        // Action method fired once the OnSiegeEventStartedEvent event fires
        public void DisplaySiege(SiegeEvent e)
        {
            if (Hero.MainHero != null)
            {
                if (Hero.MainHero.IsAlive)
                {
                    if (ShouldAlertForSettlement(e.BesiegedSettlement))
                    {
                        if (e.BesiegedSettlement.IsCastle && STASettings.Instance.EnableCastlePopup)
                        {
                            if (!managedSettlements.ContainsKey(e.BesiegedSettlement.Name.ToString()))
                            {
                                managedSettlements.Add(e.BesiegedSettlement.Name.ToString(), 0.0f);

                                string title = GetTitleFromSiege(e, true);
                                string display = GetDisplayFromSiege(e, true);

                                settlementToTrack = e.BesiegedSettlement;
                                InformationManager.ShowInquiry(new InquiryData(title, display, true, true, GT_TRACK, GT_CLOSE, new Action(Track), null, ""), STASettings.Instance.PauseGameOnPopup);
                                if (STASettings.Instance.EnableDebugMessages)
                                    InformationManager.DisplayMessage(new InformationMessage("STALibrary: " + display, new Color(1.0f, 0.0f, 0.0f)));
                            }
                        }
                        else if (e.BesiegedSettlement.IsTown && STASettings.Instance.EnableTownPopup)
                        {
                            managedSettlements.Add(e.BesiegedSettlement.Name.ToString(), 0.0f);

                            string title = GetTitleFromSiege(e, false);
                            string display = GetDisplayFromSiege(e, false);

                            settlementToTrack = e.BesiegedSettlement;
                            InformationManager.ShowInquiry(new InquiryData(title, display, true, true, GT_TRACK, GT_CLOSE, new Action(Track), null, ""), STASettings.Instance.PauseGameOnPopup);
                            if (STASettings.Instance.EnableDebugMessages)
                                InformationManager.DisplayMessage(new InformationMessage("STALibrary: " + display, new Color(1.0f, 0.0f, 0.0f)));
                        }
                    }
                }
            }
        }

        // Action method fired once the OnSiegeEventEndedEvent event fires
        public void FinalizeSiege(SiegeEvent e)
        {
            if (managedSettlements.ContainsKey(e.BesiegedSettlement.Name.ToString()))
            {
                managedSettlements.Remove(e.BesiegedSettlement.Name.ToString());
                if (STASettings.Instance.EnableDebugMessages)
                    InformationManager.DisplayMessage(new InformationMessage("STALibrary: Removed " + e.BesiegedSettlement.Name.ToString() + " from managed settlements list", new Color(1.0f, 0.0f, 0.0f)));
            }
        }

        // Action method fired once two empires declare war
        public void OnDeclareWar(IFaction faction1, IFaction faction2)
        {
            if (!faction1.IsKingdomFaction || !faction2.IsKingdomFaction)
                if (!STASettings.Instance.EnableMinorFactionPopup)
                    return;

            // Added to counter crash bug when kingdom is created while clean is at war (& faction leader is not set).
            if (faction1.Leader != null && faction2.Leader != null)
            {
                string title = GetDeclarationTitle(true);
                string display = GetDeclarationDisplay(faction1, faction2, true);

                InformationManager.ShowInquiry(new InquiryData(title, display, true, false, GT_OK, GT_CLOSE, null, null, ""), STASettings.Instance.PauseGameOnPopup);
                if (STASettings.Instance.EnableDebugMessages)
                    InformationManager.DisplayMessage(new InformationMessage("STALibrary: " + display, new Color(1.0f, 0.0f, 0.0f)));
            }
        }

        // Action method fired once two empires declare peace
        public void OnDeclarePeace(IFaction faction1, IFaction faction2)
        {
            if (!faction1.IsKingdomFaction || !faction2.IsKingdomFaction)
                if (!STASettings.Instance.EnableMinorFactionPopup)
                    return;

            string title = GetDeclarationTitle(false);
            string display = GetDeclarationDisplay(faction1, faction2, false);

            InformationManager.ShowInquiry(new InquiryData(title, display, true, false, GT_OK, GT_CLOSE, null, null, ""), STASettings.Instance.PauseGameOnPopup);
            if (STASettings.Instance.EnableDebugMessages)
                InformationManager.DisplayMessage(new InformationMessage("STALibrary: " + display, new Color(1.0f, 0.0f, 0.0f)));
        }

        // Action method fired once the user clicks 'Track' on the popup
        public void Track()
        {
            Campaign.Current.VisualTrackerManager.RegisterObject(settlementToTrack);
            if (STASettings.Instance.EnableDebugMessages)
                InformationManager.DisplayMessage(new InformationMessage("STALibrary: Tracking " + settlementToTrack.Name.ToString(), new Color(1.0f, 0.0f, 0.0f)));
        }

        // Check if the alert should fire (thanks to iPherian for submitting pull request that fixed alert not showing if you are not the king)
        private bool ShouldAlertForSettlement(Settlement settlement)
        {
            return settlement.MapFaction.Leader == Hero.MainHero || settlement.OwnerClan.Leader == Hero.MainHero;
        }

        // We ignore certain settlements when alerting for a period of time after user first alerted. This removes a settlement which has expired in that way from the list.
        private void RemoveExpiredFromManagedSettlements(Settlement settlement)
        {
            if (managedSettlements.ContainsKey(settlement.Name.ToString()))
            {
                if (!managedSettlements.TryGetValue(settlement.Name.ToString(), out float time))
                {
                    return;
                }
                if (Campaign.CurrentTime > time + STASettings.Instance.TimeToRemoveVillageFromList)
                {
                    managedSettlements.Remove(settlement.Name.ToString());
                    if (STASettings.Instance.EnableDebugMessages)
                        InformationManager.DisplayMessage(new InformationMessage("STALibrary: Removed " + settlement.Name.ToString() + " from managed settlements list", new Color(1.0f, 0.0f, 0.0f)));
                }

                if (STASettings.Instance.EnableDebugMessages)
                    InformationManager.DisplayMessage(new InformationMessage("STALibrary: " + settlement.Name.ToString() + " count is at " + ((time + STASettings.Instance.TimeToRemoveVillageFromList) - Campaign.CurrentTime), new Color(1.0f, 0.0f, 0.0f)));
            }
        }

        // Returns title data from village parameter, also applies icon to text header.
        private string GetTitleFromVillage(Village v)
        {
            TextObject header = GameTexts.FindText("str_sta_alarm_village_attack_title", null);
            header.SetTextVariable("ICON", "{=!}<img src=\"Icons\\Food@2x\">");

            return header.ToString();
        }

        // Returns display data from village parameter.
        private string GetDisplayFromVillage(Village v)
        {
            TextObject text = GameTexts.FindText("str_sta_alarm_village_attack_message", null);
            text.SetTextVariable("VILLAGE", v.Settlement.Name.ToString());
            TextObject attacker = new TextObject("", null);
            attacker.SetTextVariable("PARTY", v.Settlement.LastAttackerParty.Name);
            attacker.SetTextVariable("NAME", v.Settlement.LastAttackerParty.LeaderHero.Name);
            attacker.SetTextVariable("GENDER", v.Settlement.LastAttackerParty.LeaderHero.IsFemale ? 1 : 0);
            attacker.SetTextVariable("FACTION", v.Settlement.LastAttackerParty.LeaderHero.MapFaction.Name);
            text.SetTextVariable("ATTACKER", attacker);

            return text.ToString();
        }

        // Returns title data from siege parameter, combines castle and town into one method determined by isCastle parameter, also applies icon to text header.
        private string GetTitleFromSiege(SiegeEvent e, bool isCastle)
        {
            if (isCastle)
            {
                TextObject header = GameTexts.FindText("str_sta_alarm_castle_attack_title", null);
                header.SetTextVariable("ICON", "{=!}<img src=\"MapOverlay\\Settlement\\icon_wall\">");

                return header.ToString();
            }
            else
            {
                TextObject header = GameTexts.FindText("str_sta_alarm_town_attack_title", null);
                header.SetTextVariable("ICON", "{=!}<img src=\"MapOverlay\\Settlement\\icon_walls_lvl1\">");

                return header.ToString();
            }
        }

        // Returns display data from siege parameter, combines castle and town into one method determined by isCastle parameter.
        private string GetDisplayFromSiege(SiegeEvent e, bool isCastle)
        {
            if (isCastle)
            {
                TextObject text = GameTexts.FindText("str_sta_alarm_castle_attack_message", null);
                text.SetTextVariable("CASTLE", e.BesiegedSettlement.Name.ToString());
                TextObject attacker = new TextObject("", null);
                attacker.SetTextVariable("PARTY", e.BesiegedSettlement.LastAttackerParty.Name);
                attacker.SetTextVariable("NAME", e.BesiegedSettlement.LastAttackerParty.LeaderHero.Name);
                attacker.SetTextVariable("GENDER", e.BesiegedSettlement.LastAttackerParty.LeaderHero.IsFemale ? 1 : 0);
                attacker.SetTextVariable("FACTION", e.BesiegedSettlement.LastAttackerParty.LeaderHero.MapFaction.Name);
                text.SetTextVariable("ATTACKER", attacker);

                return text.ToString();
            }
            else
            {
                TextObject text = GameTexts.FindText("str_sta_alarm_town_attack_message", null);
                text.SetTextVariable("TOWN", e.BesiegedSettlement.Name.ToString());
                TextObject attacker = new TextObject("", null);
                attacker.SetTextVariable("PARTY", e.BesiegedSettlement.LastAttackerParty.Name);
                attacker.SetTextVariable("NAME", e.BesiegedSettlement.LastAttackerParty.LeaderHero.Name);
                attacker.SetTextVariable("GENDER", e.BesiegedSettlement.LastAttackerParty.LeaderHero.IsFemale ? 1 : 0);
                attacker.SetTextVariable("FACTION", e.BesiegedSettlement.LastAttackerParty.LeaderHero.MapFaction.Name);
                text.SetTextVariable("ATTACKER", attacker);

                return text.ToString();
            }
        }

        // Returns title data from for declaration of war/peace, combines both war/peace into one method determined by isWar parameter, also applies icon to text header.
        private string GetDeclarationTitle(bool isWar)
        {
            if (isWar)
            {
                TextObject header = GameTexts.FindText("str_sta_alarm_war_title", null);
                header.SetTextVariable("ICON", "{=!}<img src=\"Icons\\Party@2x\">");

                return header.ToString();
            }
            else
            {
                TextObject header = GameTexts.FindText("str_sta_alarm_peace_title", null);
                header.SetTextVariable("ICON", "{=!}<img src=\"Icons\\Morale@2x\">");

                return header.ToString();
            }
        }

        // Returns display data from for declaration of war/peace, combines both war/peace into one method determined by isWar parameter.
        private string GetDeclarationDisplay(IFaction faction1, IFaction faction2, bool isWar)
        {
            if (isWar)
            {
                TextObject text = GameTexts.FindText("str_sta_alarm_war_message", null);
                text.SetTextVariable("LEADER", faction1.Leader.Name);
                text.SetTextVariable("IS_FEMALE", faction1.Leader.IsFemale ? 1 : 0);
                text.SetTextVariable("FACTION1", faction1.Name.ToString());
                text.SetTextVariable("FACTION2", faction2.Name.ToString());

                return text.ToString();
            }
            else
            {
                TextObject text = GameTexts.FindText("str_sta_alarm_peace_message", null);
                text.SetTextVariable("LEADER", faction1.Leader.Name);
                text.SetTextVariable("IS_FEMALE", faction1.Leader.IsFemale ? 1 : 0);
                text.SetTextVariable("FACTION1", faction1.Name.ToString());
                text.SetTextVariable("FACTION2", faction2.Name.ToString());

                return text.ToString();
            }
        }

        private static STAAction _instance = null;

        public static STAAction Instance
        {
            get
            {
                if (STAAction._instance == null)
                    STAAction._instance = new STAAction();
                return STAAction._instance;
            }
        }
    }
}
