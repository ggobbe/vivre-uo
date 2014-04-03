/***************************************************************************
 *                               KeysCommands.cs
 *                            -------------------
 *   begin                : August 21, 2010
 *   copyright            : (C) Scriptiz
 *   email                : maeliguul@hotmail.com
 *   version              : 2011-08-21
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
using Server.Mobiles;
using Server.Items;
using Server.Commands;
using Server.Targeting;

namespace Server.Commands
{
    public class KeysCommands
    {
        public static void Initialize()
        {
            CommandSystem.Register("CreateKeys", AccessLevel.GameMaster, new CommandEventHandler(CreateKeys_OnCommand));
            CommandSystem.Register("BindDoor", AccessLevel.GameMaster, new CommandEventHandler(BindDoor_OnCommand));
        }

        [Usage("CreateKeys")]
        [Description("Crée une clef pour la porte ciblée.")]
        public static void CreateKeys_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            int quantity = 1;

            if (e.Arguments.Length == 1)
            {
                try
                {
                    quantity = Int32.Parse(e.Arguments[0]);
                }
                catch
                {
                    from.SendMessage("Vous devez entrer un nombre comme argument !");
                    return;
                }
            }

            if (quantity > 100)
            {
                from.SendMessage("Seul un fou voudrait créer plus de 100 clefs...");
                quantity = 100;
            }

            from.SendMessage("Ciblez la porte pour laquelle vous voulez créer "+quantity+" clef(s)");
            from.Target = new CreateKeysTarget(quantity);
        }

        // Classe interne pour cibler la porte pour laquelle les clefs seront générées
        private class CreateKeysTarget : Target
        {
            private int m_Quantity;

            public CreateKeysTarget(int quantity)
                : base(10, false, TargetFlags.None)
            {
                m_Quantity = quantity;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is BaseHouseDoor)
                {
                    from.SendMessage("Ce n'est pas conseillé de faire ceci sur une porte de maison...");
                    return;
                }

                BaseDoor door = targeted as BaseDoor;

                if (door == null)
                {
                    from.SendMessage("Vous devez cibler une porte et non un " + targeted.GetType().Name);
                    return;
                }

                if (door.KeyValue != 0)
                {
                    from.SendMessage("La porte possède déjà un KeyValue !");
                    return;
                }

                door.KeyValue = (uint)door.Serial.Value;

                Bag keysBag = new Bag();
                keysBag.Hue = Utility.RandomDyedHue();
                for (int i = 1; i <= m_Quantity; i++)
                {
                    Key k = new Key(door.KeyValue);
                    keysBag.DropItem(k);
                }

                if (keysBag.Items.Count > 0)
                    from.Backpack.AddItem(keysBag);
                else
                    keysBag.Delete();
            }
        }

        [Usage("BindDoor")]
        [Description("Met le KeyValue de la première porte sur la deuxième")]
        public static void BindDoor_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            from.SendMessage("Ciblez une clef déjà générée à laquelle vous souhaitez lier une nouvelle porte.");
            from.BeginTarget(10, false, TargetFlags.None, BindDoor_KeyTarget);
        }

        // Méthode pour cibler la clef qui servira a ouvrir une nouvelle porte
        private static void BindDoor_KeyTarget(Mobile from, object targeted)
        {
            Key k = targeted as Key;

            if (k == null)
            {
                from.SendMessage("Veuillez cibler une clef.");
                return;
            }

            if (k.KeyValue == 0)
            {
                from.SendMessage("Cette clef n'a pas encore été liée à une porte donc n'a pas de KeyValue !");
                return;
            }

            from.SendMessage("Maintenant ciblez la porte que vous souhaitez pouvoir ouvrir à l'aide de cette clef.");
            from.Target = new BindDoorTarget(k);
        }

        // Classe interne pour cibler la porte que la clef ouvrira (donc mettre le KeyValue de la clef sur la porte)
        private class BindDoorTarget : Target
        {
            private Key m_Key;

            public BindDoorTarget(Key k)
                : base(10, false, TargetFlags.None)
            {
                m_Key = k;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is BaseHouseDoor)
                {
                    from.SendMessage("Mieux vaut ne pas faire ceci sur une porte de maison...");
                    return;
                }

                BaseDoor door = targeted as BaseDoor;

                if (door == null)
                {
                    from.SendMessage("Il fallait cibler une porte.");
                    return;
                }

                if (door.KeyValue != 0)
                {
                    from.SendMessage("Cette porte est déjà liée à une clef. Sa KeyValue n'est pas nulle.");
                    return;
                }

                door.KeyValue = m_Key.KeyValue;
                from.SendMessage("Cette porte sera désormais une porte de plus que cette clef ouvrira.");
            }
        }
    }
}