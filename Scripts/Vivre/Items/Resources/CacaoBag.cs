using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class CacaoBag : Bag
    {
        [Constructable]
		public CacaoBag() : this(Utility.Random(3,8))
		{
		}

		[Constructable]
        public CacaoBag(int amount)
		{
            CacaoSeed treasure = new CacaoSeed();
            treasure.Amount = amount;
            DropItem(treasure);
            Hue = 47;
            Name = "Sac en tissu d'orient";
		}

        public CacaoBag(Serial serial)
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