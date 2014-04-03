using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	public abstract class BaseIngot : Item, ICommodity
	{
		private CraftResource m_Resource;

		[CommandProperty( AccessLevel.GameMaster )]
		public CraftResource Resource
		{
			get{ return m_Resource; }
			set{ m_Resource = value; InvalidateProperties(); }
		}

		public override double DefaultWeight
		{
			get { return 0.1; }
		}

        int ICommodity.DescriptionNumber { get { return LabelNumber; } }


		bool ICommodity.IsDeedable { get { return true; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			writer.Write( (int) m_Resource );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
				{
					m_Resource = (CraftResource)reader.ReadInt();
					break;
				}
				case 0:
				{
					OreInfo info;

					switch ( reader.ReadInt() )
					{
						case 0: info = OreInfo.MIron; break;
						case 1: info = OreInfo.MBronze; break;
						case 2: info = OreInfo.MGold; break;
						case 3: info = OreInfo.MCopper; break;
						case 4: info = OreInfo.MOldcopper; break;
						case 5: info = OreInfo.MDullcopper; break;
						case 6: info = OreInfo.MSilver; break;
						case 7: info = OreInfo.MShadow; break;
						case 8: info = OreInfo.MBloodrock; break;
                        case 9: info = OreInfo.MBlackrock; break;
                        case 10: info = OreInfo.MMytheril; break;
                        case 11: info = OreInfo.MRose; break;
                        case 12: info = OreInfo.MVerite; break;
                        case 13: info = OreInfo.MAgapite; break;
                        case 14: info = OreInfo.MRusty; break;
                        case 15: info = OreInfo.MValorite; break;
                        case 16: info = OreInfo.MDragon; break;
                        case 17: info = OreInfo.MTitan; break;
                        case 18: info = OreInfo.MCrystaline; break;
                        case 19: info = OreInfo.MKrynite; break;
                        case 20: info = OreInfo.MVulcan; break;
                        case 21: info = OreInfo.MBloodcrest; break;
                        case 22: info = OreInfo.MElvin; break;
                        case 23: info = OreInfo.MAcid; break;
                        case 24: info = OreInfo.MAqua; break;
                        case 25: info = OreInfo.MEldar; break;
                        case 26: info = OreInfo.MGlowing; break;
                        case 27: info = OreInfo.MGorgan; break;
                        case 28: info = OreInfo.MSandrock; break;
                        case 29: info = OreInfo.MSteel; break;
                        default: info = null; break;
					}

					m_Resource = CraftResources.GetFromOreInfo( info );
					break;
				}
			}
		}

		public BaseIngot( CraftResource resource ) : this( resource, 1 )
		{
		}

		public BaseIngot( CraftResource resource, int amount ) : base( 0x1BF2 )
		{
			Stackable = true;
			Amount = amount;
			Hue = CraftResources.GetHue( resource );
			m_Resource = resource;
		}

		public BaseIngot( Serial serial ) : base( serial )
		{
		}

		public override void AddNameProperty( ObjectPropertyList list )
		{
			if ( Amount > 1 )
				list.Add( 1050039, "{0}\t#{1}", Amount, 1027154 ); // ~1_NUMBER~ ~2_ITEMNAME~
			else
				list.Add( 1027154 ); // ingots
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( !CraftResources.IsStandard( m_Resource ) )
			{
				int num = CraftResources.GetLocalizationNumber( m_Resource );

				if ( num > 0 )
					list.Add( num );
				else
					list.Add( CraftResources.GetName( m_Resource ) );
			}
		}

        /*public override int LabelNumber
        {
            get
            {
                if (m_Resource >= CraftResource.DullCopper && m_Resource <= CraftResource.Valorite)
                    return 1042684 + (int)(m_Resource - CraftResource.DullCopper);

                return 1042692;
            }
        }*/
                 
	}

	[FlipableAttribute( 0x1BF2, 0x1BEF )]
	public class IronIngot : BaseIngot
	{
		[Constructable]
		public IronIngot() : this( 1 )
		{
		}

		[Constructable]
		public IronIngot( int amount ) : base( CraftResource.MIron, amount )
		{
		}

		public IronIngot( Serial serial ) : base( serial )
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

	[FlipableAttribute( 0x1BF2, 0x1BEF )]
	public class BronzeIngot : BaseIngot
	{
		[Constructable]
		public BronzeIngot() : this( 1 )
		{
		}

		[Constructable]
		public BronzeIngot( int amount ) : base( CraftResource.MBronze, amount )
		{
		}

        public BronzeIngot(Serial serial)            : base(serial)
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

	[FlipableAttribute( 0x1BF2, 0x1BEF )]
    public class GoldIngot : BaseIngot
	{
		[Constructable]
		public GoldIngot() : this(1)
		{
		}

		[Constructable]
		public GoldIngot( int amount ) : base( CraftResource.MGold, amount )
		{
		}

		public GoldIngot( Serial serial ) : base( serial )
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

	[FlipableAttribute( 0x1BF2, 0x1BEF )]
	public class CopperIngot : BaseIngot
	{
		[Constructable]
		public CopperIngot() : this( 1 )
		{
		}

		[Constructable]
		public CopperIngot( int amount ) : base( CraftResource.MCopper, amount )
		{
		}

		public CopperIngot( Serial serial ) : base( serial )
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

	[FlipableAttribute( 0x1BF2, 0x1BEF )]
	public class OldcopperIngot : BaseIngot
	{
		[Constructable]
		public OldcopperIngot() : this( 1 )
		{
		}

		[Constructable]
        public OldcopperIngot(int amount)
            : base(CraftResource.MOldcopper, amount)
		{
		}

        public OldcopperIngot(Serial serial)
            : base(serial)
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

	[FlipableAttribute( 0x1BF2, 0x1BEF )]
	public class DullcopperIngot : BaseIngot
	{
		[Constructable]
		public DullcopperIngot() : this( 1 )
		{
		}

		[Constructable]
		public DullcopperIngot( int amount ) : base( CraftResource.MDullcopper, amount )
		{
		}

        public DullcopperIngot(Serial serial)
            : base(serial)
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
   

	[FlipableAttribute( 0x1BF2, 0x1BEF )]
	public class SilverIngot : BaseIngot
	{
		[Constructable]
		public SilverIngot() : this( 1 )
		{
		}

		[Constructable]
		public SilverIngot( int amount ) : base( CraftResource.MSilver, amount )
		{
		}

        public SilverIngot(Serial serial) : base(serial)
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

	[FlipableAttribute( 0x1BF2, 0x1BEF )]
	public class ShadowIngot : BaseIngot
	{
		[Constructable]
		public ShadowIngot() : this( 1 )
		{
		}

		[Constructable]
		public ShadowIngot( int amount ) : base( CraftResource.MShadow, amount )
		{
		}

        public ShadowIngot(Serial serial)
            : base(serial)
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

	[FlipableAttribute( 0x1BF2, 0x1BEF )]
	public class BloodrockIngot : BaseIngot
	{
		[Constructable]
		public BloodrockIngot() : this( 1 )
		{
		}

		[Constructable]
		public BloodrockIngot( int amount ) : base( CraftResource.MBloodrock, amount )
		{
		}

        public BloodrockIngot(Serial serial) : base(serial)
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

    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class BlackrockIngot : BaseIngot
    {
        [Constructable]
        public BlackrockIngot() : this(1)
        {
        }

        [Constructable]
        public BlackrockIngot(int amount)
            : base(CraftResource.MBlackrock, amount)
        {
        }

        public BlackrockIngot(Serial serial)
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

    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class MytherilIngot : BaseIngot
    {
        [Constructable]
        public MytherilIngot()
            : this(1)
        {
        }

        [Constructable]
        public MytherilIngot(int amount)
            : base(CraftResource.MMytheril, amount)
        {
        }

        public MytherilIngot(Serial serial)
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

    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class RoseIngot : BaseIngot
    {
        [Constructable]
        public RoseIngot()
            : this(1)
        {
        }

        [Constructable]
        public RoseIngot(int amount)
            : base(CraftResource.MRose, amount)
        {
        }

        public RoseIngot(Serial serial)
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

    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class VeriteIngot : BaseIngot
    {
        [Constructable]
        public VeriteIngot()
            : this(1)
        {
        }

        [Constructable]
        public VeriteIngot(int amount)
            : base(CraftResource.MVerite, amount)
        {
        }

        public VeriteIngot(Serial serial)
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

    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class AgapiteIngot : BaseIngot
    {
        [Constructable]
        public AgapiteIngot()
            : this(1)
        {
        }

        [Constructable]
        public AgapiteIngot(int amount)
            : base(CraftResource.MAgapite, amount)
        {
        }

        public AgapiteIngot(Serial serial)
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

    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class RustyIngot : BaseIngot
    {
        [Constructable]
        public RustyIngot()
            : this(1)
        {
        }

        [Constructable]
        public RustyIngot(int amount)
            : base(CraftResource.MRusty, amount)
        {
        }

        public RustyIngot(Serial serial)
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

    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class ValoriteIngot : BaseIngot
    {
        [Constructable]
        public ValoriteIngot()
            : this(1)
        {
        }

        [Constructable]
        public ValoriteIngot(int amount)
            : base(CraftResource.MValorite, amount)
        {
        }

        public ValoriteIngot(Serial serial)
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

    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class DragonIngot : BaseIngot
    {
        [Constructable]
        public DragonIngot()
            : this(1)
        {
        }

        [Constructable]
        public DragonIngot(int amount)
            : base(CraftResource.MDragon, amount)
        {
        }

        public DragonIngot(Serial serial)
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


    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class TitanIngot : BaseIngot
    {
        [Constructable]
        public TitanIngot()
            : this(1)
        {
        }

        [Constructable]
        public TitanIngot(int amount)
            : base(CraftResource.MTitan, amount)
        {
        }

        public TitanIngot(Serial serial)
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

    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class CrystalineIngot : BaseIngot
    {
        [Constructable]
        public CrystalineIngot()
            : this(1)
        {
        }

        [Constructable]
        public CrystalineIngot(int amount)
            : base(CraftResource.MCrystaline, amount)
        {
        }

        public CrystalineIngot(Serial serial)
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

    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class KryniteIngot : BaseIngot
    {
        [Constructable]
        public KryniteIngot()
            : this(1)
        {
        }

        [Constructable]
        public KryniteIngot(int amount)
            : base(CraftResource.MKrynite, amount)
        {
        }

        public KryniteIngot(Serial serial)
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

    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class VulcanIngot : BaseIngot
    {
        [Constructable]
        public VulcanIngot()
            : this(1)
        {
        }

        [Constructable]
        public VulcanIngot(int amount)
            : base(CraftResource.MVulcan, amount)
        {
        }

        public VulcanIngot(Serial serial)
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

    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class BloodcrestIngot : BaseIngot
    {
        [Constructable]
        public BloodcrestIngot()
            : this(1)
        {
        }

        [Constructable]
        public BloodcrestIngot(int amount)
            : base(CraftResource.MBloodcrest, amount)
        {
        }

        public BloodcrestIngot(Serial serial)
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

    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class ElvinIngot : BaseIngot
    {
        [Constructable]
        public ElvinIngot()
            : this(1)
        {
        }

        [Constructable]
        public ElvinIngot(int amount)
            : base(CraftResource.MElvin, amount)
        {
        }

        public ElvinIngot(Serial serial)
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

    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class AcidIngot : BaseIngot
    {
        [Constructable]
        public AcidIngot()
            : this(1)
        {
        }

        [Constructable]
        public AcidIngot(int amount)
            : base(CraftResource.MAcid, amount)
        {
        }

        public AcidIngot(Serial serial)
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

    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class AquaIngot : BaseIngot
    {
        [Constructable]
        public AquaIngot()
            : this(1)
        {
        }

        [Constructable]
        public AquaIngot(int amount)
            : base(CraftResource.MAqua, amount)
        {
        }

        public AquaIngot(Serial serial)
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


    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class EldarIngot : BaseIngot
    {
        [Constructable]
        public EldarIngot()
            : this(1)
        {
        }

        [Constructable]
        public EldarIngot(int amount)
            : base(CraftResource.MEldar, amount)
        {
        }

        public EldarIngot(Serial serial)
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

    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class GlowingIngot : BaseIngot
    {
        [Constructable]
        public GlowingIngot()
            : this(1)
        {
        }

        [Constructable]
        public GlowingIngot(int amount)
            : base(CraftResource.MGlowing, amount)
        {
        }

        public GlowingIngot(Serial serial)
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

    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class GorganIngot : BaseIngot
    {
        [Constructable]
        public GorganIngot()
            : this(1)
        {
        }

        [Constructable]
        public GorganIngot(int amount)
            : base(CraftResource.MGorgan, amount)
        {
        }

        public GorganIngot(Serial serial)
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

    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class SandrockIngot : BaseIngot
    {
        [Constructable]
        public SandrockIngot()
            : this(1)
        {
        }

        [Constructable]
        public SandrockIngot(int amount)
            : base(CraftResource.MSandrock, amount)
        {
        }

        public SandrockIngot(Serial serial)
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

    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class SteelIngot : BaseIngot
    {
        [Constructable]
        public SteelIngot()
            : this(1)
        {
        }

        [Constructable]
        public SteelIngot(int amount)
            : base(CraftResource.MSteel, amount)
        {
        }

        public SteelIngot(Serial serial)
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