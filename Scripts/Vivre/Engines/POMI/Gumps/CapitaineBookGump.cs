using System; 
using System.Collections; 
using Server.Network; 
using Server.Prompts; 
using Server.Items; 
using Server.Gumps; 
using Server; 
using Server.Regions;
using Server.Accounting;
using Server.Mobiles;
using Server.HuePickers;

namespace Server.IPOMI
{
    public class CapitaineBookGump : Gump
    {
        private TownStone m_Town;
        private CapitaineBook m_book;

        public CapitaineBookGump(PlayerMobile from, TownStone town, CapitaineBook book)
            : base(20, 30)
        {
            m_Town = town;
            m_book = book;
            int i;

            AddPage(0);

            AddBackground(0, 0, 420, 430, 5054);
            AddBackground(10, 10, 400, 410, 3000);
            AddLabel(150, 10, 0, "Livre du Capitaine");
            AddLabel(130 + ((143 - (town.Nom.Length * 8)) / 2), 30, 0, town.Nom);

            if (m_Town.Gardes.Count < 10)
            {
                AddLabel(40, 75, 0, "Nommer un Garde");
                AddButton(20, 75, 2714, 2715, 1, GumpButtonType.Reply, 0);
            }
            if (m_Town.GardesPNJ.Count > 0)
            {
                AddLabel(250, 75, 0, "Rayon :");
                AddTextEntry(300, 75, 100, 20, 0x384, 1, String.Format("{0}", ((GuardSpawner)(m_Town.GardesPNJ[0])).RangeHome));
                AddButton(340, 75, 2714, 2715, 2, GumpButtonType.Reply, 0);
            }
            i = 0;
            foreach (PlayerMobile mobile in m_Town.Gardes)
            {
                AddLabel(40, 120 + i * 30, 0, mobile.Name);
                AddButton(15, 120 + i * 30, 0xA94, 0xA95, 100 + i, GumpButtonType.Reply, 0);

                try
                {
                    AddLabel(220, 120 + i * 30, 0, String.Format("{0} {1}", ((GuardSpawner)(m_Town.GardesPNJ[i])).Name, ((GuardSpawner)(m_Town.GardesPNJ[i])).Location));
                    AddButton(200, 120 + i * 30, 0xA94, 0xA95, 250 + i, GumpButtonType.Reply, 0);
                }
                catch
                {
                    AddLabel(220, 120 + i * 30, 0, "Ajouter Garde PNJ");
                    AddButton(200, 120 + i * 30, 2714, 2715, 200 + i, GumpButtonType.Reply, 0);
                }
                i++;
            }
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            PlayerMobile from = sender.Mobile as PlayerMobile;

            switch (info.ButtonID)
            {
                case 1:
                    {
                        from.Target = new GardeTarget(m_Town);
                        break;
                    }
                case 2:
                    {
                        try
                        {
                            int range = Int32.Parse(info.GetTextEntry(1).Text);
                            foreach (GuardSpawner guard in m_Town.GardesPNJ)
                            {
                                guard.RangeHome = range;
                                if (guard.SpawnedGuard != null)
                                    guard.SpawnedGuard.RangeHome = range;
                            }
                        }
                        catch
                        {
                            from.SendMessage("Entrez une valeur numerique");
                        }
                        break;
                    }
                default:
                    {
                        try
                        {
                            if (info.ButtonID >= 100 && info.ButtonID < 200)
                            {
                                try
                                {
                                    ((GuardSpawner)(m_Town.GardesPNJ[info.ButtonID - 100])).Delete();
                                    m_Town.GardesPNJ.RemoveAt(info.ButtonID - 100);
                                }
                                catch
                                { }
                                m_Town.Gardes.RemoveAt(info.ButtonID - 100);
                                ((PomiCloak)(m_Town.GardeCloak[info.ButtonID - 100])).Delete();
                                m_Town.GardeCloak.RemoveAt(info.ButtonID - 100);
                            }
                            if (info.ButtonID >= 200 && info.ButtonID < 250)
                            {
                                from.Target = new PNJTarget(m_Town);
                            }
                            if (info.ButtonID >= 250 && info.ButtonID < 300)
                            {
                                try
                                {
                                    ((GuardSpawner)(m_Town.GardesPNJ[info.ButtonID - 250])).Delete();
                                    m_Town.GardesPNJ.RemoveAt(info.ButtonID - 250);
                                }
                                catch { }
                            }
                        }
                        catch
                        {
                            from.SendMessage("Cette operation a deja ete faite par quelqu'un");
                        }
                        break;
                    }
            }
        }
    }
}