using System;
using Server;
using Server.Network;
using Server.Engines.Craft;

namespace Server.Items
{
	public enum ToolQuality
	{
		Low,
		Regular,
		Exceptional
	}

	public abstract class BaseTool : Item, IUsesRemaining, ICraftable
	{
		private Mobile m_Crafter;
		private ToolQuality m_Quality;
		private int m_UsesRemaining;
        private CraftResource m_Resource;

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Crafter
		{
			get{ return m_Crafter; }
			set{ m_Crafter = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public ToolQuality Quality
		{
			get{ return m_Quality; }
			set{ UnscaleUses(); m_Quality = value; InvalidateProperties(); ScaleUses(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int UsesRemaining
		{
			get { return m_UsesRemaining; }
			set { m_UsesRemaining = value; InvalidateProperties(); }
		}

        public virtual bool BreakOnDepletion { get { return true; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public CraftResource Resource
        {
            get { return m_Resource; }
            set { m_Resource = value; Hue = CraftResources.GetHue(m_Resource); InvalidateProperties();}
        }

		public void ScaleUses()
		{
			m_UsesRemaining = (m_UsesRemaining * GetUsesScalar()) / 100;
			InvalidateProperties();
		}

		public void UnscaleUses()
		{
			m_UsesRemaining = (m_UsesRemaining * 100) / GetUsesScalar();
		}

		public int GetUsesScalar()
		{
			if ( m_Quality == ToolQuality.Exceptional )
				return 200;

			return 100;
		}

		public bool ShowUsesRemaining{ get{ return true; } set{} }

		public abstract CraftSystem CraftSystem{ get; }

		public BaseTool( int itemID ) : this( Utility.RandomMinMax( 25, 75 ), itemID )
		{
		}

		public BaseTool( int uses, int itemID ) : base( itemID )
		{
			m_UsesRemaining = uses;
			m_Quality = ToolQuality.Regular;
		}

		public BaseTool( Serial serial ) : base( serial )
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			// Makers mark not displayed on OSI
			//if ( m_Crafter != null )
			//	list.Add( 1050043, m_Crafter.Name ); // crafted by ~1_NAME~

			if ( m_Quality == ToolQuality.Exceptional )
				list.Add( 1060636 ); // exceptional

			list.Add( 1060584, m_UsesRemaining.ToString() ); // uses remaining: ~1_val~
		}

		public virtual void DisplayDurabilityTo( Mobile m )
		{
			LabelToAffix( m, 1017323, AffixType.Append, ": " + m_UsesRemaining.ToString() ); // Durability
		}

		public static bool CheckAccessible( Item tool, Mobile m )
		{
			return ( tool.IsChildOf( m ) || tool.Parent == m );
		}

		public static bool CheckTool( Item tool, Mobile m )
		{
			Item check = m.FindItemOnLayer( Layer.OneHanded );

			if ( check is BaseTool && check != tool && !(check is AncientSmithyHammer) )
				return false;

			check = m.FindItemOnLayer( Layer.TwoHanded );

			if ( check is BaseTool && check != tool && !(check is AncientSmithyHammer) )
				return false;

			return true;
		}

		public override void OnSingleClick( Mobile from )
		{
			DisplayDurabilityTo( from );

			base.OnSingleClick( from );
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( IsChildOf( from.Backpack ) || Parent == from )
			{
				CraftSystem system = this.CraftSystem;

				int num = system.CanCraft( from, this, null );

				if ( num > 0 && ( num != 1044267 || !Core.SE ) ) // Blacksmithing shows the gump regardless of proximity of an anvil and forge after SE
				{
					from.SendLocalizedMessage( num );
				}
				else
				{
					CraftContext context = system.GetContext( from );

					from.SendGump( new CraftGump( from, system, this, null ) );
				}
			}
			else
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 2 ); // version

            writer.Write((int)m_Resource);

			writer.Write( (Mobile) m_Crafter );
			writer.Write( (int) m_Quality );

			writer.Write( (int) m_UsesRemaining );
            
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
                case 2:
                {
                    m_Resource = (CraftResource)reader.ReadInt();
                    goto case 1;
                }
                case 1:
				{
					m_Crafter = reader.ReadMobile();
					m_Quality = (ToolQuality) reader.ReadInt();
					goto case 0;
				}
				case 0:
				{
					m_UsesRemaining = reader.ReadInt();
					break;
				}
			}
		}
		#region ICraftable Members

		public int OnCraft( int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool, CraftItem craftItem, int resHue )
		{
            Type resourceType = typeRes;

            if (resourceType == null)
                resourceType = craftItem.Resources.GetAt(0).ItemType;

            Resource = CraftResources.GetFromType(resourceType);

            if(Resource == CraftResource.MAgapite)
                UsesRemaining += 20;

			Quality = (ToolQuality)quality;

                if ( makersMark )
				Crafter = from;

			return quality;
		}

		#endregion
	}
}