using System;
using System.IO;
using System.Collections;
using Server;
using Server.Items;

namespace Server.IPOMI
{
	public class GuardSpawner : Item
	{
		private TownStone m_Town;
		private TimeSpan m_Delay;
		private PomiGuard m_SpawnedGuard;
		private int m_RangeHome;
		private SpawnTimer m_timer;
		private static bool m_Running;

		public GuardSpawner(Point3D location, TownStone town) : base( 0x1f13 )
		{
			m_Running = false;
			Location = location;
			Map = town.Map;
			m_SpawnedGuard = null;
			m_Town = town;
			m_Delay = TimeSpan.FromSeconds( 600.0 );
			Visible = false;
			Movable = false;
			PomiGuard guard = new PomiGuard(m_Town, this);
			m_SpawnedGuard = guard;
			if(m_Town.GardesPNJ.Count > 0 )
				m_RangeHome = ((GuardSpawner)(m_Town.GardesPNJ[0])).RangeHome;
			else
				m_RangeHome = 5;
			guard.Home = Location;
			guard.RangeHome = 5;
			Name = guard.Name;
		}
		
		public PomiGuard SpawnedGuard
		{
			get{ return m_SpawnedGuard;}
			set{ m_SpawnedGuard = value;}
		}
		
		public int RangeHome
		{
			get{ return m_RangeHome;}
			set{ m_RangeHome = value;}
		}
		
		public bool Running
		{
			get{ return m_Running;}
			set{ m_Running = value;}
		}
		
		public GuardSpawner( Serial serial ) : base( serial )
		{
		}
		
		public void Start()
		{
			m_timer = new SpawnTimer(m_Town, this, m_Delay);
			m_timer.Start();
			m_Running = true;
		}
		
		public override void OnDelete()
		{
			if(m_SpawnedGuard != null)
				m_SpawnedGuard.Delete();
			if(m_timer != null)
			{	
				m_timer.Stop();
			}
			m_Running = false;
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
			writer.Write( m_Delay);
			writer.Write( (TownStone)m_Town );
			writer.Write( (PomiGuard)m_SpawnedGuard);
			writer.Write( m_RangeHome );
			writer.Write( m_Running );
			
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			m_Delay = reader.ReadTimeSpan();
			m_Town = (TownStone)reader.ReadItem();
			m_SpawnedGuard = (PomiGuard)reader.ReadMobile();
			m_RangeHome = reader.ReadInt();
			m_Running = reader.ReadBool();
			if(m_Running)
			{
				m_timer =  new SpawnTimer(m_Town, this, m_Delay);
				m_timer.Start();
			}
			
		}
		
		private class SpawnTimer : Timer
		{
			private TownStone m_Town;
			private GuardSpawner m_spawner;
			public SpawnTimer(TownStone town, GuardSpawner spawner, TimeSpan delay) : base( delay )
			{
				m_Town = town;
				m_spawner = spawner;
			}
			protected override void OnTick()
			{
				PomiGuard guard = new PomiGuard(m_Town, m_spawner);
				m_spawner.SpawnedGuard = guard;
				guard.Home = m_spawner.Location;
				guard.RangeHome = 5;
				m_spawner.Name = guard.Name;
				m_spawner.Running = false;
			}
		}
	}
}
