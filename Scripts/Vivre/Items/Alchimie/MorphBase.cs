using System;
using Server.Targeting;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
    public class MorphBase : Item
    {

        [Constructable]
        public MorphBase()
            : base(0x182B)
        {
            Name = "Base de métamorphose";
            Movable = true; 
            Stackable = true;
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendMessage("Y ajouter un élément contraire à vous pourrait bien avoir un effet surprenant!");
            from.BeginTarget(2, false, TargetFlags.None, new TargetCallback(ChangelingDropTarget));
            base.OnDoubleClick(from);
        }

        public void ChangelingDropTarget(Mobile from, object obj)
        {
            if (!(obj is HairStrand))
            {
                from.SendMessage("Cela ne servirait à rien de verser le liquide ici");
                return;
            }

            HairStrand targ = (HairStrand)obj;

            if (from.Skills[SkillName.Alchemy].Value < 60)
            {
                from.SendMessage("Vos maigres talents d'alchimistes ne permettraient pas une pareille mixture...");
                return;
            }

            if (!from.CheckTargetSkill(SkillName.Alchemy, targ, 55, 95))
            {
                from.SendMessage("Vous mettez trop de cheveux à l'intérieur, ce qui gâche le mélange...");
                targ.Delete();
                return;
            }

            from.PrivateOverheadMessage(MessageType.Regular, 0x3B2, false, "Vous mélangez doucement le tout et terminez le mélange...", from.NetState);
            GenderPotion potion = new GenderPotion();
            potion.Female = targ.HairOwner.Female;
            from.AddToBackpack(potion);
            this.Delete();
            targ.Delete();
            return;
        }

        
        public MorphBase(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            if (!Stackable)
                Stackable = true;
        }
    }

   
}