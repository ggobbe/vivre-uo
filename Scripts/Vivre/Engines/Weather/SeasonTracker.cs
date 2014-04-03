//To enable RUNUO 2.0 compatibility, simply comment out the next line (#define);
//#define RUNUO_1

using System;
using System.Security;
using System.Security.Cryptography;
using System.Net;
using System.Collections;
using System.Text;
using System.IO;

using Server;

namespace Server.ServerSeasons
{
	public static class SeasonTracker
	{
		private static int LoadedCount = 0;
		private static int SavedCount = 0;

		private static MD5CryptoServiceProvider m_MD5HashProvider;
		private static byte[] m_MD5HashBuffer;

		public static string MD5Hash(string plainText)
		{
			if (m_MD5HashProvider == null)
				m_MD5HashProvider = new MD5CryptoServiceProvider();

			if (m_MD5HashBuffer == null)
				m_MD5HashBuffer = new byte[256];

			int length = Encoding.ASCII.GetBytes(plainText, 0, plainText.Length > 256 ? 256 : plainText.Length, m_MD5HashBuffer, 0);
			byte[] hashed = m_MD5HashProvider.ComputeHash(m_MD5HashBuffer, 0, length);

			return BitConverter.ToString(hashed);
		}

		/// <summary>
		/// Configuration Events
		/// </summary>
		public static void Configure()
		{
			EventSink.ServerStarted += new ServerStartedEventHandler(OnLoad);
			EventSink.WorldSave += new WorldSaveEventHandler(OnSave);
		}

		/// <summary>
		/// Serialize
		/// </summary>
		/// <param name="e"></param>
		private static void OnSave(WorldSaveEventArgs e)
		{
			string CurRegion = "";

			try
			{
				if (Directory.Exists("Saves/Regions/Seasons/"))
					Directory.Delete("Saves/Regions/Seasons/", true);

				foreach (Region r in Region.Regions)
				{
					if (r != null)
					{
						if (!(r is ISeasons))
							continue;

						if (r.GetType().IsAbstract || !r.GetType().IsSubclassOf(typeof(Region)))
							continue;

						CurRegion = r.Name;


						if (!Directory.Exists("Saves/Regions/Seasons/" + r.Map + "/"))
							Directory.CreateDirectory("Saves/Regions/Seasons/" + r.Map + "/");

						string name = "(" + r.Name + ")";
						string nameTmp = "";

#if(RUNUO_1)
						if( r.Coords != null )
						{
							for( int i = 0; i < r.Coords.Count; ++i )
							{
								object obj = r.Coords[i];

								if( obj is Rectangle3D )
								{
									Rectangle3D r3d = ( Rectangle3D )obj;

									nameTmp += r3d.ToString( );
								}
								else if( obj is Rectangle2D )
								{
									Rectangle2D r2d = ( Rectangle2D )obj;

									nameTmp += r2d.ToString( );
								}
							}

							name += "(" + MD5Hash( nameTmp ) + ")";
						}

						if( name == null || name == "" || name.Length == 0 )
						{
							name += "(" + r.UId + ")";
						}
#else
						if (r.Area != null)
						{
							for (int i = 0; i < r.Area.Length; ++i)
							{
								Rectangle3D r3d = r.Area[i];
								nameTmp += r3d.ToString();
							}

							name += "(" + MD5Hash(nameTmp) + ")";
						}
#endif

						try
						{
							GenericWriter writer = new BinaryFileWriter(Path.Combine("Saves/Regions/Seasons/" + r.Map + "/", name + ".bin"), true);
							Season season = ((ISeasons)r).Season;
							writer.Write((int)season);
							SavedCount++;
							writer.Close();
						}
						catch (Exception ex1)
						{
							Console.WriteLine("[SeasonTracker] Serialize: ({0})", CurRegion);
							Console.WriteLine(ex1.ToString());
						}
					}
				}
			}
			catch (Exception ex2)
			{
				Console.WriteLine("[SeasonTracker] OnSave: ({0})", CurRegion);
				Console.WriteLine(ex2.ToString());
			}

			Console.WriteLine("[SeasonTracker] Saved {0} Region Seasons!", SavedCount);
			SavedCount = 0;
		}

		/// <summary>
		/// Deserialize
		/// </summary>
		private static void OnLoad()
		{
			string CurRegion = "";

			try
			{
				foreach (Region r in Region.Regions)
				{
					if (r != null)
					{
						if (!(r is ISeasons))
							continue;

						if (r.GetType().IsAbstract || !r.GetType().IsSubclassOf(typeof(Region)))
							continue;

						CurRegion = r.Name;

						if (!Directory.Exists("Saves/Regions/Seasons/" + r.Map + "/"))
							Directory.CreateDirectory("Saves/Regions/Seasons/" + r.Map + "/");

						string name = "(" + r.Name + ")";
						string nameTmp = "";

#if(RUNUO_1)
						if (r.Coords != null)
						{
							for (int i = 0; i < r.Coords.Count; ++i)
							{
								object obj = r.Coords[i];

								if (obj is Rectangle3D)
								{
									Rectangle3D r3d = (Rectangle3D)obj;

									nameTmp += r3d.ToString();
								}
								else if (obj is Rectangle2D)
								{
									Rectangle2D r2d = (Rectangle2D)obj;

									nameTmp += r2d.ToString();
								}
							}

							name += "(" + MD5Hash(nameTmp) + ")";
						}

						if (name == null || name == "" || name.Length == 0)
						{
							name += "(" + r.UId + ")";
						}
#else
						if (r.Area != null)
						{
							for (int i = 0; i < r.Area.Length; ++i)
							{
								Rectangle3D r3d = r.Area[i];
								nameTmp += r3d.ToString();
							}

							name += "(" + MD5Hash(nameTmp) + ")";
						}
#endif

						if (!File.Exists(Path.Combine("Saves/Regions/Seasons/" + r.Map + "/", name + ".bin")))
							return;

						using (FileStream bin = new FileStream(Path.Combine("Saves/Regions/Seasons/" + r.Map + "/", name + ".bin"), FileMode.Open, FileAccess.Read, FileShare.Read))
						{
							try
							{
								GenericReader reader = new BinaryFileReader(new BinaryReader(bin));
								Season season = (Season)reader.ReadInt();
								((ISeasons)r).Season = season;
								LoadedCount++;
							}
							catch (Exception ex1)
							{
								Console.WriteLine("[SeasonTracker] Deserialize: ({0})", CurRegion);
								Console.WriteLine(ex1.ToString());
							}
						}
					}
				}
			}
			catch (Exception ex2)
			{
				Console.WriteLine("[SeasonTracker] OnLoad: ({0})", CurRegion);
				Console.WriteLine(ex2.ToString());
			}

			Console.WriteLine("[SeasonTracker] Tracking {0} Regions' Seasons...", LoadedCount);
			LoadedCount = 0;
		}
	}
}