/***************************************************************************
 *                               NPChat.cs
 *                            -------------------
 *   begin                : May 14, 2010
 *   copyright            : (C) Scriptiz
 *   email                : maeliguul@hotmail.com
 *   version              : 2011-08-03
 *
 ***************************************************************************/

/***************************************************************************
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or
 *   (at your option) any later version.
 *
 ***************************************************************************/
using System;
using System.Collections.Generic;
using Server;
using Server.Commands;
using Server.Mobiles;
using Server.Network;
using Server.Misc;
using Server.Targeting;
using Server.Items;
using Server.Gumps;

namespace Server.Gumps
{
    public class NPChat : Gump
    {
        public static void Initialize()
        {
            CommandSystem.Register("npChat", AccessLevel.Counselor, new CommandEventHandler(NPChat_OnCommand));
            CommandSystem.Register("clearTmp", AccessLevel.GameMaster, new CommandEventHandler(ClearTmp_OnCommand));
        }

        [Usage("npChat")]
        [Description("Ouvre une fenêtre qui permet de faire parler un NPC.")]
        private static void NPChat_OnCommand(CommandEventArgs e)
        {
            e.Mobile.Target = new NPChatTarget();
            e.Mobile.SendMessage("Qui voulez-vous faire parler ?");
        }

        [Usage("clearTmp")]
        [Description("Supprime toutes les TmpPoint.")]
        private static void ClearTmp_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendMessage("Supression des TmpPoint en cours...");
            List<Item> tmpPoints = new List<Item>();
            foreach (Item i in World.Items.Values)
            {
                if (i != null && i is WayPoint && i.Name == "TmpPoint")
                {
                    tmpPoints.Add(i);
                }
            }

            int totalTmpPoint = tmpPoints.Count;

            for (int i = 0; i < tmpPoints.Count; i++)
            {
                tmpPoints[i].Delete();
            }

            e.Mobile.SendMessage("Il y avait " + totalTmpPoint + " TmpPoint qui ont été supprimés.");
        }

        public static void DeplacerMobile(Mobile m, Point3D p)
        {
            if (m == null) return;
            BaseCreature b = null;
            if(m is BaseCreature) b = (BaseCreature)m;
            if (b == null) return;

            if (b.AI == AIType.AI_None) b.AI = AIType.AI_Melee;

            if (b.CurrentWayPoint != null && b.CurrentWayPoint.Name == "TmpPoint")
                b.CurrentWayPoint.Delete();

            b.CurrentSpeed = 0.2;

            WayPoint point = new WayPoint();
            point.Name = "TmpPoint";
            point.MoveToWorld(p, b.Map);
            point.NextPoint = point;
            b.CurrentWayPoint = point;
        }

        Mobile m_Owner;
        Mobile m_Speaker;
        int x, y;

        public NPChat(Mobile owner, Mobile speaker)
            : this(owner, speaker, PropsConfig.GumpOffsetX, PropsConfig.GumpOffsetY)
        {
        }

        public NPChat(Mobile owner, Mobile speaker, int x, int y)
            : base(x, y)
        {
            this.m_Owner = owner;
            this.m_Speaker = speaker;
            int hue = this.m_Speaker.SpeechHue;

            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            AddPage(0);
            AddBackground(0, 0, 290, 90, 9400);
            AddLabel(13, 35, hue, this.m_Speaker.Name);   // @"[MobileName]"
            AddImage(8, 8, 1143);
            AddButton(215, 35, 247, 248, 1, GumpButtonType.Reply, 0);   // Send chat

            if (m_Speaker is BaseCreature)
            {
                AddButton(220, 63, 1209, 1210, 2, GumpButtonType.Reply, 0);   // Move button
                AddLabel(237, 60, hue, "Move");
            }

            AddButton(15, 63, 1209, 1210, 3, GumpButtonType.Reply, 0);
            AddLabel(32, 60, hue, "Goto");
            AddButton(120, 63, 1209, 1210, 4, GumpButtonType.Reply, 0);
            AddLabel(137, 60, hue, "Props");

            AddTextEntry(18, 10, 254, 20, hue, 0, @"");

            // On ajoute le mobile à la liste des mobiles écoutés
            NPListener.AddListenedMobile(m_Owner, m_Speaker);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if (m_Speaker == null || m_Speaker.Deleted)
            {
                from.SendMessage("Le Mobile que vous controlliez à disparu.");
                return;
            }

            switch (info.ButtonID)
            {
                case 0:
                    {
                        // Si le gump est fermé on retire le Mobile de la liste des mobiles écoutés
                        NPListener.RemoveListenedMobile(m_Owner, m_Speaker);
                        if (m_Speaker != null && m_Speaker is BaseCreature)
                        {
                            BaseCreature b = (BaseCreature)m_Speaker;
                            b.CurrentSpeed = b.PassiveSpeed;

                            if (b.CurrentWayPoint != null && b.CurrentWayPoint.Name == "TmpPoint")
                            {
                                b.CurrentWayPoint.Delete();
                                b.CurrentWayPoint = null;
                                b.AI = AIType.AI_Use_Default;
                            }
                        }
                        break;
                    }
                case 1:
                    {
                        TextRelay entry0 = info.GetTextEntry(0);
                        string text0 = (entry0 == null ? "" : entry0.Text.Trim());
                        if (m_Speaker != null && m_Speaker is Mobile && text0 != "")
                        {
                            this.m_Speaker.PublicOverheadMessage(MessageType.Regular, this.m_Speaker.SpeechHue, false, text0);

                            if(this.m_Owner.GetDistanceToSqrt(this.m_Speaker) >= 10)
                                this.m_Owner.SendMessage(this.m_Speaker.SpeechHue, this.m_Speaker.Name + " : " + text0);

                            from.SendGump(new NPChat(from, m_Speaker, this.X, this.Y));
                        }
                        break;
                    }
                case 2:
                    {
                        if (m_Speaker != null && m_Speaker is Mobile)
                        {
                            m_Owner.Target = new DeplacerTarget(m_Speaker);
                            from.SendGump(new NPChat(from, m_Speaker, this.X, this.Y));
                        }
                        break;
                    }
                case 3:
                    {
                        m_Owner.Hidden = true;
                        m_Owner.MoveToWorld(m_Speaker.Location, m_Speaker.Map);
                        from.SendGump(new NPChat(from, m_Speaker, this.X, this.Y));
                        break;
                    }
                case 4:
                    {
                        from.SendGump(new NPChat(from, m_Speaker, this.X, this.Y));
                        m_Owner.SendGump(new PropertiesGump(m_Owner, m_Speaker));
                        break;
                    }
            }
        }

        private class NPChatTarget : Target
        {
            public NPChatTarget()
                : base(15, false, TargetFlags.None)
            {
            }

            protected override void OnTarget(Mobile from, object targ)
            {
                if (targ is Mobile)
                {
                    Mobile m = (Mobile)targ;
                    from.SendGump(new NPChat(from, m));
                }
                else
                {
                    from.SendMessage("Veuillez cibler un Mobile.");
                }
            }
        }

        private class DeplacerTarget : Target
        {
            Mobile m_MovingMobile;

            public DeplacerTarget(Mobile movingMobile)
                : base(25, true, TargetFlags.None)
            {
                this.m_MovingMobile = movingMobile;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is LandTarget)
                {
                    LandTarget targ = (LandTarget)targeted;
                    DeplacerMobile(m_MovingMobile, new Point3D(targ.X, targ.Y, targ.Z));
                }
                else if (targeted is StaticTarget)
                {
                    StaticTarget targ = (StaticTarget)targeted;
                    DeplacerMobile(m_MovingMobile, new Point3D(targ.X, targ.Y, targ.Z));
                }
                else
                {
                    from.SendMessage("Vous ne pouvez déplacer le mobile sur " + targeted.GetType().ToString());
                }
            }
        }
    }
}