using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("Corps d'un mercenaire")]
    public class BaseMercenary : BaseCreature
    {
        public BaseMercenary()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 2, 0.1, 0.2)
        {
            BodyValue = 400;
            Hue = Utility.RandomSkinHue();
            
            Utility.AssignRandomHair(this);
            HairHue = Utility.RandomHairHue();

            if (Utility.Random(20) > 15)
            {
                Utility.AssignRandomFacialHair(this);
                FacialHairHue = HairHue;
            }

            SpeechHue = Utility.RandomDyedHue();

            Backpack bp = new Backpack();
            bp.Movable = false;
            AddItem(bp);
        }

        public override void OnDeath(Container c)
        {
            // No loot !
            foreach (Item i in c.Items)
            {
                i.Delete();
            }

            // fun?
            if (Utility.Random(25) == 10)
            {
                Item apple = new Apple();
                apple.Name = "Pomme qui tua Blanche-Neige";
                c.AddItem(apple);
            }

            base.OnDeath(c);
        }

        public BaseMercenary(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
