/***************************************************************************
 *                             SerializeDuper.cs
 *                            -------------------
 * Implements a duplication command based on the Serialize() and
 *	Deserialize() methods by utilizing a new Writer/Reader combination.
 * 
 * What can be duplicated?
 *	A GM may duplicate any item or mobile without properties
 *		above his AccessLevel
 *	
 * What cannot be duplicated?
 *	PlayerMobiles cannot be duplicated.
 *	Items/mobiles cannot be duplicated if their references exceed a
 *		certain depth.
 *		(e.g. a bag in a bag, in a bag, in a bag, in a backpack of a mobile)
 *	Items/mobiles cannot be duplicated if the duplicated items exceed
 *		a certain sanity amount.
 * 
 * You can find the most recent version of this script under the
 *	following link:
 *	
 *
 * Created by Lichtblitz ( GM Aldor )
 ***************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using Server.Guilds;
using Server.Mobiles;
using Server.Commands;
using Server.Commands.Generic;
using Server.Targeting;
using CPA = Server.CommandPropertyAttribute;
using Server.Items;

namespace Server.Commands
{
	public class DupeCommand
	{
		/// <summary>
		/// Maximum depth of recursions
		/// </summary>
		public const int MAX_RECURSIONS = 5;

		/// <summary>
		/// Maximum amount of duplicated entities
		/// </summary>
		public const int MAX_ENTITIES = 500;

		public static void Initialize()
		{
			CommandSystem.Register( "dupe", AccessLevel.GameMaster, new CommandEventHandler( Dupe_OnCommand ) );
		}

		[Usage( "dupe" )]
		[Description( "Duplicates any Item or Mobile and any Item or Mobile referenced by them." )]
		public static void Dupe_OnCommand( CommandEventArgs e )
		{
			e.Mobile.Target = new DupeTarget();
		}
	}

	/// <summary>
	/// Target cursor including duplication methods.
	/// </summary>
	public class DupeTarget : Target
	{
		public DupeTarget() : base( -1, false, TargetFlags.None ) { }

		protected override void OnTarget( Mobile from, object targeted )
		{
			if ( from == null || targeted == null || (targeted is Item && ((Item)targeted).Deleted) || (targeted is Mobile && ((Mobile)targeted).Deleted) )
			{
				return;
			}

			if ( targeted is PlayerMobile )
			{
				from.SendMessage( "That's not the way to increase the number of players!" );
				return;
			}

			List<Item> itemList = new List<Item>();
			List<Mobile> mobileList = new List<Mobile>();
			Dictionary<int, int> serialMapping = new Dictionary<int, int>();

			try
			{
				Dupe( from, targeted, DupeCommand.MAX_RECURSIONS, itemList, mobileList, serialMapping );

				// Duping messed up the parents; we need to fix this now
				foreach ( Item item in itemList.ToArray() )
				{
					// Items need to be copied to avoid EnumerationExceptions
					foreach ( object child in item.Items.ToArray() )
					{
						item.AddItem( (Item)child );
					}
				}

				foreach ( Mobile mobile in mobileList.ToArray() )
				{
					// Items need to be copied to avoid EnumerationExceptions
					foreach ( object child in mobile.Items.ToArray() )
					{
						mobile.AddItem( (Item)child );
					}
				}

				// If more than one Item has been duped they will be placed inside a colored bag.
				Container bag = null;
				if ( itemList.Count > 1 )
				{
					bag = new Server.Items.Bag();
					bag.Name = "Duped on " + DateTime.Today.ToString( "dd.MM.yyyy" );
					bag.Hue = 6;
				}

				// All Items will be placed inside the bag/backpack and all mobiles will be
				// moved to the position of the GM
				// (except for those on the internal map)
				int internalMapCount = 0;

				foreach ( Item item in itemList )
				{
					if ( item.Map == Map.Internal )
					{
						internalMapCount++;
						continue;
					}

					if ( (item.Parent == null || (!itemList.Contains( item.Parent as Item ) && !mobileList.Contains( item.Parent as Mobile ))) )
					// Lies on the floor or in any container that is not part of this duplication process
					{
						item.Parent = null; // Remove any reference to the former parent
						if ( bag == null )
						{
							from.AddToBackpack( item );
						}
						else
						{
							if ( bag.Items.Count <= 0 )
							{
								// The first non-internal item to be placed adds the bag to the callers backpack
								from.AddToBackpack( bag );
							}

							bag.AddItem( item );
							from.SendMessage( item.GetType().ToString() );
						}
					}

					item.UpdateTotals();
					item.InvalidateProperties();
				}
				foreach ( Mobile mobile in mobileList )
				{
					if ( mobile.Map == Map.Internal )
					{
						internalMapCount++;
						continue;
					}

					mobile.MoveToWorld( from.Location, from.Map );

					mobile.UpdateResistances();
					mobile.UpdateTotals();
					mobile.InvalidateProperties();
				}

				// Sending the success message.
				from.SendMessage( "{0} Items and {1} Mobiles have been successfully duplicated.", itemList.Count, mobileList.Count );
				if ( internalMapCount > 0 )
				{
					from.SendMessage( "{0} of which are on the internal map.", internalMapCount );
				}

				#region InterfaceGump
				ArrayList completeList = new ArrayList();
				completeList.AddRange( mobileList );
				completeList.AddRange( itemList );

				from.SendGump( new InterfaceGump( from, new string[] { "Entity" }, completeList, 0, null ) );
				#endregion

				#region CommandLogging
				// Implementing command logging 
				StringBuilder sb = new StringBuilder();
				sb.AppendFormat( "{0} {1} duping ", from.AccessLevel, CommandLogging.Format( from ) );
				sb.AppendFormat( "at {0} in {1}: ", from.Location, from.Map );
				int serial = (targeted is Item ? ((Item)targeted).Serial : ((Mobile)targeted).Serial);
				sb.AppendFormat( "{0} Items and {1} Mobiles via target 0x{2:X}", itemList.Count, mobileList.Count, serial );
				CommandLogging.WriteLine( from, sb.ToString() );

				sb = new StringBuilder();
				StringBuilder sbm = new StringBuilder();

				sb.Append( "Serials been duped:" );
				sbm.Append( "Serials:" );

				foreach ( KeyValuePair<int, int> kvp in serialMapping )
				{
					sb.AppendFormat( " 0x{0:X};", kvp.Key );
					sbm.AppendFormat( " 0x{0:X};", kvp.Value );
				}
				CommandLogging.WriteLine( from, sb.ToString() );
				CommandLogging.WriteLine( from, sbm.ToString() );
				#endregion
			}
			catch ( DupeException ex )
			{
				from.SendMessage( ex.Message );
				DeleteAll( itemList, mobileList );
			}
		}

		/// <summary>
		/// Deletes all entities, created so far.
		/// </summary>
		/// <param name="itemList">List of Items</param>
		/// <param name="mobileList">List of Mobiles</param>
		private void DeleteAll( List<Item> itemList, List<Mobile> mobileList )
		{
			foreach ( Item item in itemList )
			{
				item.Delete();
			}
			foreach ( Mobile mobile in mobileList )
			{
				mobile.Delete();
			}
		}

		/// <summary>
		/// Primary dupe method. Will be called recursively.
		/// </summary>
		/// <param name="from">Caller of the dupe command</param>
		/// <param name="toDupe">Item to be duped</param>
		/// <param name="recursionDepth">Recursions left</param>
		/// <param name="itemList">List of items created so far</param>
		/// <param name="mobileList">List of mobiles created so far</param>
		/// <param name="serialMapping">Mapping of serials of duped items and their copied counterparts</param>
		/// <returns>Duplicated entity</returns>
		public static object Dupe( Mobile from, object toDupe, int recursionDepth, List<Item> itemList, List<Mobile> mobileList, Dictionary<int, int> serialMapping )
		{
			object toReturn = null;

			Type type = toDupe.GetType();

			// Getting all properties
			PropertyInfo[] allProps = type.GetProperties( BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public );

			// Checking if the caller's AccessLevel is high enough to write those
			foreach ( PropertyInfo thisProp in allProps )
			{
				CPA attr = Properties.GetCPA( thisProp );
				if ( attr != null && (from.AccessLevel < attr.ReadLevel || from.AccessLevel < attr.WriteLevel) )
				{
					// Ignoring all properties declared by BaseCreature and Mobile
					if ( thisProp.DeclaringType != typeof( BaseCreature ) && thisProp.DeclaringType != typeof( Mobile ) )
					{
						throw new AccessLevelTooLowException( "Your AccessLevel is too low to dupe this: " + thisProp.Name );
					}
				}

			}

			MemoryStream stream = new MemoryStream();
			DupeWriter writer = new DupeWriter( stream, recursionDepth );
			DupeReader reader = new DupeReader( stream, from, itemList, mobileList, serialMapping );

			try
			{
				if ( toDupe is Item )
				{
					Item item = (Item)toDupe;
					item.Serialize( writer );
				}
				else
				{
					Mobile mobile = (Mobile)toDupe;
					mobile.Serialize( writer );
				}

				// YAY! If we arrived here we are allowed to duplicate the item and have collected all necessary data
				writer.Flush();
				reader.Seek( 0, SeekOrigin.Begin ); // Reset position of the reader

				// Fetch constructor with serial as parameter
				ConstructorInfo ctor = type.GetConstructor( new Type[] { typeof( Serial ) } );

				if ( toDupe is Item )
				{
					Item item = (Item)ctor.Invoke( new object[] { Serial.NewItem } );
					World.AddItem( item ); // We don't want duplicate serials so we add it to the world right away to block its serial.
					serialMapping.Add( ((Item)toDupe).Serial, item.Serial );
					itemList.Insert( 0, item ); // Insert at the beginning to reverse the recursive order

					item.Deserialize( reader ); // Deserialize calls Dupe again if it reaches a reference.
					toReturn = item;
				}
				else if ( toDupe is Mobile )
				{
					// Konstruktor mit Serial aufrufen
					Mobile mobile = (Mobile)ctor.Invoke( new object[] { Serial.NewMobile } );
					World.AddMobile( mobile ); // We don't want duplicate serials so we add it to the world right away to block its serial.
					serialMapping.Add( ((Mobile)toDupe).Serial, mobile.Serial );
					mobileList.Insert( 0, mobile ); // Insert at the beginning to reverse the recursive order

					mobile.Deserialize( reader ); // Deserialize calls Dupe again if it reaches a reference.
					toReturn = mobile;
				}

				if ( !reader.End() )
				{
					// The stream is not empty?
					throw new DeserializeException( "Cannot dupe " + toReturn.GetType().Name + ". Serialize/Deserialize is either broken or uses hacks." );
				}

				if ( itemList.Count + mobileList.Count > DupeCommand.MAX_ENTITIES )
				{
					throw new EntitiesExceededException( "Cannot dupe more than " + DupeCommand.MAX_ENTITIES + " Items/Mobiles!" );
				}
			}
			finally
			{
				writer.Close();
				reader.Close();
				stream.Close();
			}

			return toReturn;
		}
	}
}

namespace Server
{
	/// <summary>
	/// Modified <see cref="BinaryFileWriter"/>, which writes in a
	/// <see cref="MemoryStream"/> instead of a file.
	/// </summary>
	public class DupeWriter : GenericWriter
	{
		private bool PrefixStrings = true;
		private MemoryStream m_File;

		int recursionDepth;

		protected virtual int BufferSize
		{
			get
			{
				return 64 * 1024;
			}
		}

		private byte[] m_Buffer;

		private int m_Index;

		private Encoding m_Encoding;

		public DupeWriter( MemoryStream strm, int recursionDepth )
		{
			m_Encoding = Utility.UTF8;
			m_Buffer = new byte[BufferSize];
			m_File = strm;
			this.recursionDepth = recursionDepth;
		}

		public void Flush()
		{
			if ( m_Index > 0 )
			{
				m_Position += m_Index;

				m_File.Write( m_Buffer, 0, m_Index );
				m_Index = 0;
			}
		}

		private long m_Position;

		public override long Position
		{
			get
			{
				return m_Position + m_Index;
			}
		}

		public Stream UnderlyingStream
		{
			get
			{
				if ( m_Index > 0 )
					Flush();

				return m_File;
			}
		}

		public override void Close()
		{
			if ( m_Index > 0 )
				Flush();

			m_File.Close();
		}

		public override void WriteEncodedInt( int value )
		{
			// May dupe
			uint v = (uint)value;

			while ( v >= 0x80 )
			{
				if ( (m_Index + 1) > BufferSize )
					Flush();

				m_Buffer[m_Index++] = (byte)(v | 0x80);
				v >>= 7;
			}

			if ( (m_Index + 1) > BufferSize )
				Flush();

			m_Buffer[m_Index++] = (byte)v;
		}

		private byte[] m_CharacterBuffer;
		private int m_MaxBufferChars;
		private const int LargeByteBufferSize = 256;

		internal void InternalWriteString( string value )
		{
			int length = m_Encoding.GetByteCount( value );

			WriteEncodedInt( length );

			if ( m_CharacterBuffer == null )
			{
				m_CharacterBuffer = new byte[LargeByteBufferSize];
				m_MaxBufferChars = LargeByteBufferSize / m_Encoding.GetMaxByteCount( 1 );
			}

			if ( length > LargeByteBufferSize )
			{
				int current = 0;
				int charsLeft = value.Length;

				while ( charsLeft > 0 )
				{
					int charCount = (charsLeft > m_MaxBufferChars) ? m_MaxBufferChars : charsLeft;
					int byteLength = m_Encoding.GetBytes( value, current, charCount, m_CharacterBuffer, 0 );

					if ( (m_Index + byteLength) > BufferSize )
						Flush();

					Buffer.BlockCopy( m_CharacterBuffer, 0, m_Buffer, m_Index, byteLength );
					m_Index += byteLength;

					current += charCount;
					charsLeft -= charCount;
				}
			}
			else
			{
				int byteLength = m_Encoding.GetBytes( value, 0, value.Length, m_CharacterBuffer, 0 );

				if ( (m_Index + byteLength) > BufferSize )
					Flush();

				Buffer.BlockCopy( m_CharacterBuffer, 0, m_Buffer, m_Index, byteLength );
				m_Index += byteLength;
			}
		}

		public override void Write( string value )
		{
			// May dupe
			if ( PrefixStrings )
			{
				if ( value == null )
				{
					if ( (m_Index + 1) > BufferSize )
						Flush();

					m_Buffer[m_Index++] = 0;
				}
				else
				{
					if ( (m_Index + 1) > BufferSize )
						Flush();

					m_Buffer[m_Index++] = 1;

					InternalWriteString( value );
				}
			}
			else
			{
				InternalWriteString( value );
			}
		}

		public override void Write( DateTime value )
		{
			// May dupe
			Write( value.Ticks );
		}

		public override void WriteDeltaTime( DateTime value )
		{
			// May dupe
			long ticks = value.Ticks;
			long now = DateTime.Now.Ticks;

			TimeSpan d;

			try { d = new TimeSpan( ticks - now ); }
			catch { if ( ticks < now ) d = TimeSpan.MaxValue; else d = TimeSpan.MaxValue; }

			Write( d );
		}

		public override void Write( IPAddress value )
		{
			// May NOT dupe
			ThrowTypeException( "IPAddress" );

			Write( Utility.GetLongAddressValue( value ) );
		}

		public override void Write( TimeSpan value )
		{
			// May dupe
			Write( value.Ticks );
		}

		public override void Write( decimal value )
		{
			// May dupe
			int[] bits = Decimal.GetBits( value );

			for ( int i = 0; i < bits.Length; ++i )
				Write( bits[i] );
		}

		public override void Write( long value )
		{
			// May dupe
			if ( (m_Index + 8) > BufferSize )
				Flush();

			m_Buffer[m_Index] = (byte)value;
			m_Buffer[m_Index + 1] = (byte)(value >> 8);
			m_Buffer[m_Index + 2] = (byte)(value >> 16);
			m_Buffer[m_Index + 3] = (byte)(value >> 24);
			m_Buffer[m_Index + 4] = (byte)(value >> 32);
			m_Buffer[m_Index + 5] = (byte)(value >> 40);
			m_Buffer[m_Index + 6] = (byte)(value >> 48);
			m_Buffer[m_Index + 7] = (byte)(value >> 56);
			m_Index += 8;
		}

		public override void Write( ulong value )
		{
			// May dupe
			if ( (m_Index + 8) > BufferSize )
				Flush();

			m_Buffer[m_Index] = (byte)value;
			m_Buffer[m_Index + 1] = (byte)(value >> 8);
			m_Buffer[m_Index + 2] = (byte)(value >> 16);
			m_Buffer[m_Index + 3] = (byte)(value >> 24);
			m_Buffer[m_Index + 4] = (byte)(value >> 32);
			m_Buffer[m_Index + 5] = (byte)(value >> 40);
			m_Buffer[m_Index + 6] = (byte)(value >> 48);
			m_Buffer[m_Index + 7] = (byte)(value >> 56);
			m_Index += 8;
		}

		public override void Write( int value )
		{
			// May dupe
			if ( (m_Index + 4) > BufferSize )
				Flush();

			m_Buffer[m_Index] = (byte)value;
			m_Buffer[m_Index + 1] = (byte)(value >> 8);
			m_Buffer[m_Index + 2] = (byte)(value >> 16);
			m_Buffer[m_Index + 3] = (byte)(value >> 24);
			m_Index += 4;
		}

		public override void Write( uint value )
		{
			// May dupe
			if ( (m_Index + 4) > BufferSize )
				Flush();

			m_Buffer[m_Index] = (byte)value;
			m_Buffer[m_Index + 1] = (byte)(value >> 8);
			m_Buffer[m_Index + 2] = (byte)(value >> 16);
			m_Buffer[m_Index + 3] = (byte)(value >> 24);
			m_Index += 4;
		}

		public override void Write( short value )
		{
			// May dupe
			if ( (m_Index + 2) > BufferSize )
				Flush();

			m_Buffer[m_Index] = (byte)value;
			m_Buffer[m_Index + 1] = (byte)(value >> 8);
			m_Index += 2;
		}

		public override void Write( ushort value )
		{
			// May dupe
			if ( (m_Index + 2) > BufferSize )
				Flush();

			m_Buffer[m_Index] = (byte)value;
			m_Buffer[m_Index + 1] = (byte)(value >> 8);
			m_Index += 2;
		}

		public /*unsafe*/ override void Write( double value )
		{
			// May dupe; MODIFIED: new implementation since operation was unsafe

			byte[] eightBytes = System.BitConverter.GetBytes( value );
			for ( int i = 0; i < 8; i++ )
			{
				Write( eightBytes[i] );
			}

			/*
			if( (m_Index + 8) > m_Buffer.Length )
				Flush();

			fixed( byte* pBuffer = m_Buffer )
				*((double*)(pBuffer + m_Index)) = value;

			m_Index += 8;
			 */
		}

		public /*unsafe*/ override void Write( float value )
		{
			// May dupe; MODIFIED: new implementation since operation was unsafe

			byte[] fourBytes = System.BitConverter.GetBytes( value );
			for ( int i = 0; i < 4; i++ )
			{
				Write( fourBytes[i] );
			}

			/*
			if ( (m_Index + 4) > BufferSize )
				Flush();

			fixed ( byte* pBuffer = m_Buffer )
				*((float*)(&pBuffer[m_Index])) = value;

			m_Index += 4;
			 */
		}

		private char[] m_SingleCharBuffer = new char[1];

		public override void Write( char value )
		{
			// May dupe
			if ( (m_Index + 8) > BufferSize )
				Flush();

			m_SingleCharBuffer[0] = value;

			int byteCount = m_Encoding.GetBytes( m_SingleCharBuffer, 0, 1, m_Buffer, m_Index );
			m_Index += byteCount;
		}

		public override void Write( byte value )
		{
			// May dupe
			if ( (m_Index + 1) > BufferSize )
				Flush();

			m_Buffer[m_Index++] = value;
		}

		public override void Write( sbyte value )
		{
			// May dupe
			if ( (m_Index + 1) > BufferSize )
				Flush();

			m_Buffer[m_Index++] = (byte)value;
		}

		public override void Write( bool value )
		{
			// May dupe
			if ( (m_Index + 1) > BufferSize )
				Flush();

			m_Buffer[m_Index++] = (byte)(value ? 1 : 0);
		}

		public override void Write( Point3D value )
		{
			// May dupe
			Write( value.X );
			Write( value.Y );
			Write( value.Z );
		}

		public override void Write( Point2D value )
		{
			// May dupe
			Write( value.X );
			Write( value.Y );
		}

		public override void Write( Rectangle2D value )
		{
			// May dupe
			Write( value.Start );
			Write( value.End );
		}

		public override void Write( Rectangle3D value )
		{
			Write( value.Start );
			Write( value.End );
		}

		public override void Write( Map value )
		{
			// May dupe

			if ( value != null )
				Write( (byte)value.MapIndex );
			else
				Write( (byte)0xFF );
		}

		public override void Write( Item value )
		{
			// May dupe; MODIFIED
			if ( recursionDepth < 1 )
			{
				ThrowRecursionException();
			}
			Write( (int)(recursionDepth - 1) ); // ADDED to keep track of recursions

			if ( value == null || value.Deleted )
				Write( Serial.MinusOne );
			else
				Write( value.Serial );
		}

		public override void Write( Mobile value )
		{
			// May dupe; MODIFIED
			if ( recursionDepth < 1 )
			{
				ThrowRecursionException();
			}

			if ( value is PlayerMobile )
			{
				ThrowTypeException( "PlayerMobile" );
			}

			Write( (int)(recursionDepth - 1) ); // ADDED to keep track of recursions

			if ( value == null || value.Deleted )
				Write( Serial.MinusOne );
			else
				Write( value.Serial );
		}

		public override void Write( BaseGuild value )
		{
			if ( value == null )
				Write( 0 );
			else
			{
				// May NOT dupe
				ThrowTypeException( "BaseGuild" );
				Write( value.Id );
			}
		}

		public override void WriteItem<T>( T value )
		{
			// May dupe
			Write( value );
		}

		public override void WriteMobile<T>( T value )
		{
			// May dupe
			Write( value );
		}

		public override void WriteGuild<T>( T value )
		{
			// May dupe
			Write( value );
		}

		public override void WriteMobileList( ArrayList list )
		{
			// May dupe
			WriteMobileList( list, false );
		}

		public override void WriteMobileList( ArrayList list, bool tidy )
		{
			// May dupe
			if ( tidy )
			{
				for ( int i = 0; i < list.Count; )
				{
					if ( ((Mobile)list[i]).Deleted )
						list.RemoveAt( i );
					else
						++i;
				}
			}

			Write( list.Count );

			for ( int i = 0; i < list.Count; ++i )
				Write( (Mobile)list[i] );
		}

		public override void WriteItemList( ArrayList list )
		{
			// May dupe
			WriteItemList( list, false );
		}

		public override void WriteItemList( ArrayList list, bool tidy )
		{
			// May dupe
			if ( tidy )
			{
				for ( int i = 0; i < list.Count; )
				{
					if ( ((Item)list[i]).Deleted )
						list.RemoveAt( i );
					else
						++i;
				}
			}

			Write( list.Count );

			for ( int i = 0; i < list.Count; ++i )
				Write( (Item)list[i] );
		}

		public override void WriteGuildList( ArrayList list )
		{
			// May dupe
			WriteGuildList( list, false );
		}

		public override void WriteGuildList( ArrayList list, bool tidy )
		{
			// May dupe
			if ( tidy )
			{
				for ( int i = 0; i < list.Count; )
				{
					if ( ((BaseGuild)list[i]).Disbanded )
						list.RemoveAt( i );
					else
						++i;
				}
			}

			Write( list.Count );

			for ( int i = 0; i < list.Count; ++i )
				Write( (BaseGuild)list[i] );
		}

		public override void Write( List<Item> list )
		{
			Write( list, false );
		}

		public override void Write( List<Item> list, bool tidy )
		{
			// May dupe
			if ( tidy )
			{
				for ( int i = 0; i < list.Count; )
				{
					if ( list[i].Deleted )
						list.RemoveAt( i );
					else
						++i;
				}
			}

			Write( list.Count );

			for ( int i = 0; i < list.Count; ++i )
				Write( list[i] );
		}

		public override void WriteItemList<T>( List<T> list )
		{
			// May dupe
			WriteItemList<T>( list, false );
		}

		public override void WriteItemList<T>( List<T> list, bool tidy )
		{
			// May dupe
			if ( tidy )
			{
				for ( int i = 0; i < list.Count; )
				{
					if ( list[i].Deleted )
						list.RemoveAt( i );
					else
						++i;
				}
			}

			Write( list.Count );

			for ( int i = 0; i < list.Count; ++i )
				Write( list[i] );
		}

		public override void Write( List<Mobile> list )
		{
			// May dupe
			Write( list, false );
		}

		public override void Write( List<Mobile> list, bool tidy )
		{
			// May dupe
			if ( tidy )
			{
				for ( int i = 0; i < list.Count; )
				{
					if ( list[i].Deleted )
						list.RemoveAt( i );
					else
						++i;
				}
			}

			Write( list.Count );

			for ( int i = 0; i < list.Count; ++i )
				Write( list[i] );
		}

		public override void WriteMobileList<T>( List<T> list )
		{
			// May dupe
			WriteMobileList<T>( list, false );
		}

		public override void WriteMobileList<T>( List<T> list, bool tidy )
		{
			// May dupe
			if ( tidy )
			{
				for ( int i = 0; i < list.Count; )
				{
					if ( list[i].Deleted )
						list.RemoveAt( i );
					else
						++i;
				}
			}

			Write( list.Count );

			for ( int i = 0; i < list.Count; ++i )
				Write( list[i] );
		}

		public override void Write( List<BaseGuild> list )
		{
			// May dupe
			Write( list, false );
		}

		public override void Write( List<BaseGuild> list, bool tidy )
		{
			// May dupe
			if ( tidy )
			{
				for ( int i = 0; i < list.Count; )
				{
					if ( list[i].Disbanded )
						list.RemoveAt( i );
					else
						++i;
				}
			}

			Write( list.Count );

			for ( int i = 0; i < list.Count; ++i )
				Write( list[i] );
		}

		public override void WriteGuildList<T>( List<T> list )
		{
			// May dupe
			WriteGuildList<T>( list, false );
		}

		public override void WriteGuildList<T>( List<T> list, bool tidy )
		{
			// May dupe
			if ( tidy )
			{
				for ( int i = 0; i < list.Count; )
				{
					if ( list[i].Disbanded )
						list.RemoveAt( i );
					else
						++i;
				}
			}

			Write( list.Count );

			for ( int i = 0; i < list.Count; ++i )
				Write( list[i] );
		}

		public override void Write( Race value )
		{
			// May dupe
			if ( value != null )
				Write( (byte)value.RaceIndex );
			else
				Write( (byte)0xFF );
		}

		private void ThrowTypeException( String typename )
		{
			throw new MayNotDupeException( String.Format( "Property of type \"{0}\" is not allowed to dupe!", typename ) );
		}

		private void ThrowRecursionException()
		{
			throw new RecursionsExceededException( String.Format( "Maximum number of {0} recursions exceeded!", DupeCommand.MAX_RECURSIONS ) );
		}
	}

	public class DupeReader : GenericReader
	{
		private Mobile duper;
		private List<Item> itemList;
		private List<Mobile> mobileList;
		private Dictionary<int, int> serialMapping;
		private BinaryReader m_File;

		public DupeReader( MemoryStream stream, Mobile duper, List<Item> itemList, List<Mobile> mobileList, Dictionary<int, int> serialMapping )
			: base()
		{
			this.m_File = new BinaryReader( stream );
			this.duper = duper;
			this.itemList = itemList;
			this.mobileList = mobileList;
			this.serialMapping = serialMapping;
		}

		public void Close()
		{
			m_File.Close();
		}

		public long Position
		{
			get
			{
				return m_File.BaseStream.Position;
			}
		}

		public long Seek( long offset, SeekOrigin origin )
		{
			return m_File.BaseStream.Seek( offset, origin );
		}

		public override string ReadString()
		{
			if ( ReadByte() != 0 )
				return m_File.ReadString();
			else
				return null;
		}

		public override DateTime ReadDeltaTime()
		{
			long ticks = m_File.ReadInt64();
			long now = DateTime.Now.Ticks;

			if ( ticks > 0 && (ticks + now) < 0 )
				return DateTime.MaxValue;
			else if ( ticks < 0 && (ticks + now) < 0 )
				return DateTime.MinValue;

			try { return new DateTime( now + ticks ); }
			catch { if ( ticks > 0 ) return DateTime.MaxValue; else return DateTime.MinValue; }
		}

		public override IPAddress ReadIPAddress()
		{
			return new IPAddress( m_File.ReadInt64() );
		}

		public override int ReadEncodedInt()
		{
			int v = 0, shift = 0;
			byte b;

			do
			{
				b = m_File.ReadByte();
				v |= (b & 0x7F) << shift;
				shift += 7;
			} while ( b >= 0x80 );

			return v;
		}

		public override DateTime ReadDateTime()
		{
			return new DateTime( m_File.ReadInt64() );
		}

		public override TimeSpan ReadTimeSpan()
		{
			return new TimeSpan( m_File.ReadInt64() );
		}

		public override decimal ReadDecimal()
		{
			return m_File.ReadDecimal();
		}

		public override long ReadLong()
		{
			return m_File.ReadInt64();
		}

		public override ulong ReadULong()
		{
			return m_File.ReadUInt64();
		}

		public override int ReadInt()
		{
			return m_File.ReadInt32();
		}

		public override uint ReadUInt()
		{
			return m_File.ReadUInt32();
		}

		public override short ReadShort()
		{
			return m_File.ReadInt16();
		}

		public override ushort ReadUShort()
		{
			return m_File.ReadUInt16();
		}

		public override double ReadDouble()
		{
			return m_File.ReadDouble();
		}

		public override float ReadFloat()
		{
			return m_File.ReadSingle();
		}

		public override char ReadChar()
		{
			return m_File.ReadChar();
		}

		public override byte ReadByte()
		{
			return m_File.ReadByte();
		}

		public override sbyte ReadSByte()
		{
			return m_File.ReadSByte();
		}

		public override bool ReadBool()
		{
			return m_File.ReadBoolean();
		}

		public override Point3D ReadPoint3D()
		{
			return new Point3D( ReadInt(), ReadInt(), ReadInt() );
		}

		public override Point2D ReadPoint2D()
		{
			return new Point2D( ReadInt(), ReadInt() );
		}

		public override Rectangle2D ReadRect2D()
		{
			return new Rectangle2D( ReadPoint2D(), ReadPoint2D() );
		}

		public override Rectangle3D ReadRect3D()
		{
			return new Rectangle3D( ReadPoint3D(), ReadPoint3D() );
		}

		public override Map ReadMap()
		{
			return Map.Maps[ReadByte()];
		}

		/// <summary>
		/// Reads the serial of an item and duplicates that item if it hasn't been
		/// duplicated before.
		/// </summary>
		/// <returns>Reference to the duplicated item.</returns>
		public override Item ReadItem()
		{
			int recursionDepth = ReadInt();
			int serial = ReadInt();

			// We already duped this Item; use reference to previously duped one
			if ( serialMapping.ContainsKey( serial ) )
			{
				return World.FindItem( serialMapping[serial] );
			}

			Item toDupe = World.FindItem( serial );

			if ( toDupe == null ) { return null; }

			return DupeTarget.Dupe( duper, toDupe, recursionDepth, itemList, mobileList, serialMapping ) as Item;
		}

		/// <summary>
		/// Reads the serial of a mobile and duplicates that mobile if it hasn't been
		/// duplicated before.
		/// </summary>
		/// <returns>Reference to the duplicated mobile.</returns>
		public override Mobile ReadMobile()
		{
			int recursionDepth = ReadInt();
			int serial = ReadInt();

			// We already duped this Mobile; use reference to previously duped one
			if ( serialMapping.ContainsKey( serial ) )
			{
				return World.FindMobile( serialMapping[serial] );
			}

			Mobile toDupe = World.FindMobile( serial );

			if ( toDupe == null ) { return null; }

			return DupeTarget.Dupe( duper, toDupe, recursionDepth, itemList, mobileList, serialMapping ) as Mobile;
		}

		public override BaseGuild ReadGuild()
		{
			return BaseGuild.Find( ReadInt() );
		}

		public override T ReadItem<T>()
		{
			return ReadItem() as T;
		}

		public override T ReadMobile<T>()
		{
			return ReadMobile() as T;
		}

		public override T ReadGuild<T>()
		{
			return ReadGuild() as T;
		}

		public override ArrayList ReadItemList()
		{
			int count = ReadInt();

			if ( count > 0 )
			{
				ArrayList list = new ArrayList( count );

				for ( int i = 0; i < count; ++i )
				{
					Item item = ReadItem();

					if ( item != null )
					{
						list.Add( item );
					}
				}

				return list;
			}
			else
			{
				return new ArrayList();
			}
		}

		public override ArrayList ReadMobileList()
		{
			int count = ReadInt();

			if ( count > 0 )
			{
				ArrayList list = new ArrayList( count );

				for ( int i = 0; i < count; ++i )
				{
					Mobile m = ReadMobile();

					if ( m != null )
					{
						list.Add( m );
					}
				}

				return list;
			}
			else
			{
				return new ArrayList();
			}
		}

		public override ArrayList ReadGuildList()
		{
			int count = ReadInt();

			if ( count > 0 )
			{
				ArrayList list = new ArrayList( count );

				for ( int i = 0; i < count; ++i )
				{
					BaseGuild g = ReadGuild();

					if ( g != null )
					{
						list.Add( g );
					}
				}

				return list;
			}
			else
			{
				return new ArrayList();
			}
		}

		public override List<Item> ReadStrongItemList()
		{
			return ReadStrongItemList<Item>();
		}

		public override List<T> ReadStrongItemList<T>()
		{
			int count = ReadInt();

			if ( count > 0 )
			{
				List<T> list = new List<T>( count );

				for ( int i = 0; i < count; ++i )
				{
					T item = ReadItem() as T;

					if ( item != null )
					{
						list.Add( item );
					}
				}

				return list;
			}
			else
			{
				return new List<T>();
			}
		}

		public override List<Mobile> ReadStrongMobileList()
		{
			return ReadStrongMobileList<Mobile>();
		}

		public override List<T> ReadStrongMobileList<T>()
		{
			int count = ReadInt();

			if ( count > 0 )
			{
				List<T> list = new List<T>( count );

				for ( int i = 0; i < count; ++i )
				{
					T m = ReadMobile() as T;

					if ( m != null )
					{
						list.Add( m );
					}
				}

				return list;
			}
			else
			{
				return new List<T>();
			}
		}

		public override List<BaseGuild> ReadStrongGuildList()
		{
			return ReadStrongGuildList<BaseGuild>();
		}

		public override List<T> ReadStrongGuildList<T>()
		{
			int count = ReadInt();

			if ( count > 0 )
			{
				List<T> list = new List<T>( count );

				for ( int i = 0; i < count; ++i )
				{
					T g = ReadGuild() as T;

					if ( g != null )
					{
						list.Add( g );
					}
				}

				return list;
			}
			else
			{
				return new List<T>();
			}
		}

		public override bool End()
		{
			return m_File.PeekChar() == -1;
		}

		public override Race ReadRace()
		{
			return Race.Races[ReadByte()];
		}
	}

	/// <summary>
	/// Base Exception of all local exceptions. Used for structuring.
	/// </summary>
	public abstract class DupeException : Exception
	{
		public DupeException( string message )
			: base( message )
		{
		}
	}

	/// <summary>
	/// This exception is thrown when tying to duplicate a data type that cannot be duplicated.
	/// </summary>
	public class MayNotDupeException : DupeException
	{
		public MayNotDupeException( string message )
			: base( message )
		{
		}
	}

	/// <summary>
	/// This exception is thrown when the amount of <see cref="DupeCommand.MAX_RECUSIONS"/>
	/// has been exceeded.
	/// </summary>
	public class RecursionsExceededException : DupeException
	{
		public RecursionsExceededException( string message )
			: base( message )
		{
		}
	}

	/// <summary>
	/// This exception is thrown when the amount of <see cref="DupeCommand.MAX_ENTITIES"/>
	/// has been exceeded.
	/// </summary>
	public class EntitiesExceededException : DupeException
	{
		public EntitiesExceededException( string message )
			: base( message )
		{
		}
	}

	/// <summary>
	/// The Datastream is longer or shorter than its deserialization. Either Serialize/Deserialize
	/// is broken or different data types are written and read for the same variable.
	/// e.g.:
	///		Serialize():
	///			Write( (Mobile) abc);
	///		Deserialize:
	///			int serial = ReadInt();
	///			abc = World.FindMobile( serial );
	/// </summary>
	public class DeserializeException : DupeException
	{
		public DeserializeException( string message )
			: base( message )
		{
		}
	}

	/// <summary>
	/// The caller lacks rights to duplicate the property.
	/// </summary>
	public class AccessLevelTooLowException : DupeException
	{
		public AccessLevelTooLowException( string message )
			: base( message )
		{
		}
	}
}