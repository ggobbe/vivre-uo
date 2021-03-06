using System;
using System.Collections;
using System.Collections.Generic;
using Server;

namespace Server.Mobiles
{
	public class Druid : BaseVendor
	{
        private List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos { get { return m_SBInfos; } }

        public override NpcGuild NpcGuild { get { return NpcGuild.MagesGuild; } }


		[Constructable]
		public Druid() : base( "the druid initiate" )
		{
			SetSkill( SkillName.AnimalLore, 65.0, 88.0 );
			SetSkill( SkillName.Tactics, 36.0, 68.0 );
			SetSkill( SkillName.Macing, 45.0, 68.0 );
			SetSkill( SkillName.MagicResist, 65.0, 88.0 );
			SetSkill( SkillName.Herding, 56.0, 68.0 );
			this.Race = Race.Elf;
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add( new SBDruid() );
		}

		


		public override void InitOutfit()
		{
			AddItem( new Server.Items.MonksRobe( 0xB0 ) );
		        AddItem( new Server.Items.WildStaff() );
                  	AddItem( new Server.Items.Sandals() );
            }

		public Druid( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}