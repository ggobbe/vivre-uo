using System;
using Server;
using Server.Items;
using Server.Targeting;

namespace Server.Commands
{
    public class ItemInfo
    {
        public static void Initialize()
        {
            CommandSystem.Register("ItemInfo", AccessLevel.Player, new CommandEventHandler(ItemInfo_OnCommand));
        }

        [Usage("ItemInfo")]
        [Description("Renvoie Type et ItemID")]
        public static void ItemInfo_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            from.Target = new InternalTarget();
        }

        private class InternalTarget : Target
        {
            public InternalTarget()
                : base(3, false, TargetFlags.None)
            {
            }

            protected override void OnTarget(Mobile from, object o)
            {
                try
                {
                    if (o is Item)
                    {
                        Item item = (Item)o as Item;

                        if (item != null)
                        {
                            string type = item.GetType().ToString();
                            string id = "0x" + item.ItemID.ToString("X");
                            from.SendMessage(type + " " + id);
                        }
                    }
                    else if (o is StaticTarget)
                    {
                        StaticTarget targ = (StaticTarget)o;
                        from.SendMessage("static " + "  " + "0x" + targ.ItemID.ToString("X"));
                    }

                }
                catch
                {
                    from.SendMessage("Ceci n'est pas un objet");
                }

            }
        }
    }
}