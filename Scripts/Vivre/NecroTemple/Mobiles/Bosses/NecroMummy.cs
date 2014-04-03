using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a necromummy corpse")]
    class NecroMummy : BaseCreature, NecroBoss
    {
        [Constructable]
		public NecroMummy() : base( AIType.AI_Melee, FightMode.Closest, 10, 2, 0.2, 0.4 )
		{
			Name = "a Necromummy";
			Body = 154;
			BaseSoundID = 471;
            Hue = 1271;

			SetStr( 350, 500 );
			SetDex(50);
			SetInt( 300, 500 );

			SetHits( 300, 450 );

			SetDamage( 20, 30 );

			SetDamageType( ResistanceType.Physical, 40 );
			SetDamageType( ResistanceType.Cold, 60 );

			SetResistance( ResistanceType.Physical, 70, 90 );
			SetResistance( ResistanceType.Fire, 50 );
			SetResistance( ResistanceType.Cold, 100 );
			SetResistance( ResistanceType.Poison, 50, 70 );
			SetResistance( ResistanceType.Energy, 50, 70 );

            SetSkill(SkillName.MagicResist, 30.1, 40.0);
            SetSkill(SkillName.Wrestling, 90.1, 110.0);
            SetSkill(SkillName.Tactics, 90.1, 110.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 82;

            // Loot de sorts
            int count = 0;
            if (Utility.Random(1, 10) >= 5)
            {
                PackItem(new EvilOmenScroll());
                count++;
            }

            if (Utility.Random(1, 10) >= 6)
            {
                PackItem(new BloodOathScroll());
                count++;
            }

            if (Utility.Random(1, 10) >= 7)
            {
                PackItem(new CorpseSkinScroll());
                count++;
            }

            if (Utility.Random(1, 10) >= 8)
            {
                PackItem(new WraithFormScroll());
                count++;
            }

            if (Utility.Random(1, 10) >= 9)
            {
                PackItem(new PainSpikeScroll());
                count++;
            }

            // Si zéro ou un seul sort, on met le scroll evil omen pour pas que ça soit vide...
            if (count <= 1)
                PackItem(new EvilOmenScroll());

            PackNecroReg(20, 40);
		}

		public override bool BleedImmune{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Greater; } }
        public override bool Unprovokable { get { return true; } }
        public override bool BardImmune { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return false; } }
        public override bool AlwaysMurderer { get { return true; } }

        public NecroMummy(Serial serial)
            : base(serial)
		{
		}

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.FeyAndUndead; }
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
    }
}
