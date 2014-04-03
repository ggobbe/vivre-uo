using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Targeting;
using Server.Commands;
using Server.Mobiles;

namespace Server.Commands
{
    public class ToutDeplacer
    {
        public static void Initialize()
        {
            CommandSystem.Register("ToutDeplacer", AccessLevel.Player, new CommandEventHandler(ToutDeplacer_OnCommand));
        }
        private static void ToutDeplacer_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            from.SendMessage("Pointez un item. Tous les items de même type seront déplacés.");
            from.BeginTarget(-1, true, TargetFlags.None, new TargetCallback(ChooseItem_OnTarget));
        }
        public static void ChooseItem_OnTarget(Mobile from, object targeted)
        {
            Item it = targeted as Item;

            if (it == null || !it.Movable || !(it.Parent is Container) || !(((Container)it.Parent).CheckItemUse(from, it)))
                from.SendMessage("Impossible de déplacer ceci!");
            else
            {
                from.SendMessage("Dans quel contenant voulez vous déposer le tout ?");
                from.BeginTarget(-1, true, TargetFlags.None, new TargetStateCallback(ChooseBag_OnTarget), it);
            }
        }
        public static void ChooseBag_OnTarget(Mobile from, object targeted, object state)
        {
            Item it = state as Item;
            Container c = null;

            if (targeted is Container)
            {
                c = targeted as Container;

            }
            else if (targeted is PackHorse || targeted is PackLlama)
            {
                BaseCreature bc = (BaseCreature)targeted;
                if (bc != null)
                {
                    if (bc.ControlMaster == from || from.AccessLevel > AccessLevel.Player)
                    {
                        c = bc.Backpack;
                    }
                    else
                    {
                        from.SendMessage("Cet animal ne vous appartient pas!");
                        return;
                    }
                }
            }
            else
            {
                from.SendMessage("Impossible de déplacer tous ces items ici!");
                return;
            }

            if (c != null)
            {
                Item[] items;

                if (it.Parent is Container)
                {
                    items = ((Container)it.Parent).FindItemsByType(it.GetType(), false);
                }
                else
                {
                    from.SendMessage("Impossible de déplacer un objet qui n'est pas dans un contenant!");
                    return;
                }

                foreach (Item item in items)
                {
                    // Scriptiz : on ne peut pas prendre les objets non movable
                    if (!item.Movable)
                        continue;

                    if (!c.TryDropItem(from, item, false))
                        from.SendMessage("Impossible de déplacer tous ces items ici!");
                }
            }
        }
    }
}