using System;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
    public class TeintureBottle : Item
    {
        [Constructable]
        public TeintureBottle()
            : this(1)
        {
        }

        [Constructable]
        public TeintureBottle(int amount)
            : base(0xF0E)
        {
            Stackable = false;
            Weight = 1.0;
            Amount = amount;
        }

        public TeintureBottle(Serial serial)
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

        public override void OnDoubleClick(Mobile from)
        {
            from.BeginTarget(-1, false, TargetFlags.None, new TargetCallback(OnMixTeinture));
            from.SendMessage("Dans quoi souhaitez-vous verser cette bouteille?");
        }

        public void OnMixTeinture(Mobile from, object obj)
        {
            if (obj is TeintureBottle)
            {
                TeintureBottle tb = (TeintureBottle)obj;

                if(tb.Hue == this.Hue)
                {
                    from.SendMessage("Cela ne sert à rien de mélanger deux fois la même teinture");
                    return;
                }
                if (tb.Hue == 0x47E || this.Hue == 0x47E)
                {
                    if (tb.Hue == 0x484 || this.Hue == 0x484)
                    {
                        this.Hue = 0x48D;
                        tb.ReplaceWith(new Bottle());
                    }
                    else if (tb.Hue == 0x48D || this.Hue == 0x48D)
                    {
                        this.Hue = 0x47f;
                        tb.ReplaceWith(new Bottle());
                    }
                    else if (tb.Hue == 0x489 || this.Hue == 0x489)
                    {
                        this.Hue = 0x491;
                        tb.ReplaceWith(new Bottle());
                    }
                    else if (tb.Hue == 0x485 || this.Hue == 0x485)
                    {
                        this.Hue = 0x484;
                        tb.ReplaceWith(new Bottle());
                    }
                    else if (tb.Hue == 0x484 || this.Hue == 0x484)
                    {
                        this.Hue = 0x48E;
                        tb.ReplaceWith(new Bottle());
                    }
                    else if (tb.Hue == 0x48C || this.Hue == 0x48C)
                    {
                        this.Hue = 0x48F;
                        tb.ReplaceWith(new Bottle());
                    }
                    else if (tb.Hue == 0x48B || this.Hue == 0x48B)
                    {
                        this.Hue = 0x490;
                        tb.ReplaceWith(new Bottle());
                    }
                    else if (tb.Hue == 0x48A || this.Hue == 0x48A)
                    {
                        this.Hue = 0x495;
                        tb.ReplaceWith(new Bottle());
                    }
                    else
                    {
                        from.SendMessage("Il ne sert à rien de mélanger ces deux teintures");
                        tb.ReplaceWith(new Bottle());
                        this.ReplaceWith(new Bottle());
                        return;
                    }
                    this.Name = "Teinture mélangée";
                    from.SendMessage("Vous créez une teinture d'une autre coloration");
                    return;
                }
                else if (tb.Hue == 0x497 || this.Hue == 0x497)
                {
                    if (tb.Hue == 0x48C || this.Hue == 0x48C)
                    {
                        this.Hue = 0x483;
                        tb.ReplaceWith(new Bottle());
                    }
                    else if (tb.Hue == 0x48B || this.Hue == 0x48B)
                    {
                        this.Hue = 0x486;
                        tb.ReplaceWith(new Bottle());
                    }
                    else if (tb.Hue == 0x48A || this.Hue == 0x48A)
                    {
                        this.Hue = 0x488;
                        tb.ReplaceWith(new Bottle());
                    }
                    else
                    {
                        from.SendMessage("Il ne sert à rien de mélanger ces deux teintures");
                        tb.ReplaceWith(new Bottle());
                        this.ReplaceWith(new Bottle());
                        return;
                    }
                    this.Name = "Teinture mélangée";
                    from.SendMessage("Vous créez une teinture d'une autre coloration");
                    return;
                }
                else if (tb.Hue == 0x484 || this.Hue == 0x484)
                {
                    if (tb.Hue == 0x489 || this.Hue == 0x489)
                    {
                        this.Hue = 0x48C;
                        tb.ReplaceWith(new Bottle());
                    }
                    else if (tb.Hue == 0x485 || this.Hue == 0x485)
                    {
                        this.Hue = 0x48B;
                        tb.ReplaceWith(new Bottle());
                    }
                    else if (tb.Hue == 0x48C || this.Hue == 0x48C)
                    {
                        this.Hue = 0x48A;
                        tb.ReplaceWith(new Bottle());
                    }
                    else
                    {
                        from.SendMessage("Il ne sert à rien de mélanger ces deux teintures");
                        tb.ReplaceWith(new Bottle());
                        this.ReplaceWith(new Bottle());
                        return;
                    }
                    this.Name = "Teinture mélangée";
                    from.SendMessage("Vous créez une teinture d'une autre coloration");
                    return;
                }
                else
                {
                    from.SendMessage("Il ne sert à rien de mélanger ces deux teintures");
                    tb.ReplaceWith(new Bottle());
                    this.ReplaceWith(new Bottle());
                    return;
                }
            }
            else if(obj is SpecialDyeTub)
            {
                SpecialDyeTub dt = (SpecialDyeTub)obj;

                if(dt.Redyable)
                {
                    dt.DyedHue = this.Hue;
                    dt.Redyable = false;
                    dt.Charges = 10;
                    from.SendMessage("Vous préparez le bac de teinture");
                    return;
                }

                from.SendMessage("Ce bac doit d'abord être lavé avant d'être utilisé") ;
            }
            else
                from.SendMessage("Vous ne pouvez que colorer une autre teinture ou un bac spécial vide");
        }
    }
    public class WhiteTeinture : TeintureBottle
    {

        [Constructable]
        public WhiteTeinture() : base()
        {
            Name = "Teinture blanche";
            Hue = 0x47E;
            Movable = true;
            Stackable = false;
        }

        public WhiteTeinture(Serial serial)
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
         
        }
    }
    public class BlackTeinture : TeintureBottle
    {

        [Constructable]
        public BlackTeinture()
            : base()
        {
            Name = "Teinture noire";
            Hue = 0x497;
            Movable = true;
            Stackable = false;
        }

        public BlackTeinture(Serial serial)
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
          
        }
    }
    public class CyanTeinture : TeintureBottle
    {

        [Constructable]
        public CyanTeinture()
            : base()
        {
            Name = "Teinture cyan";
            Hue = 0x484;
            Movable = true;
            Stackable = false;
        }

        public CyanTeinture(Serial serial)
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
           
        }
    }
    public class YellowTeinture : TeintureBottle
    {

        [Constructable]
        public YellowTeinture()
            : base()
        {
            Name = "Teinture jaune";
            Hue = 0x489;
            Movable = true;
            Stackable = false;
        }

        public YellowTeinture(Serial serial)
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
         
        }
    }
    public class MagentaTeinture : TeintureBottle
    {

        [Constructable]
        public MagentaTeinture()
            : base()
        {
            Name = "Teinture magenta";
            Hue = 0x485;
            Movable = true;
            Stackable = false;
        }

        public MagentaTeinture(Serial serial)
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

        }
    }
}

