
namespace Server.Engines.Encryption
{
    public class Configuration
    {
        // Set this to true to enable this subsystem.
        public static bool Enabled = true;

        // Set this to false to disconnect unencrypted connections.
        public static bool AllowUnencryptedClients = true;

        // This is the list of supported game encryption keys.
        // You can use the utility found at http://www.hartte.de/ExtractKeys.exe
        // to extract the neccesary keys from any client version.
        public static LoginKey[] LoginKeys = new LoginKey[]
		{
            new LoginKey("7.0.34.6", 0x28EAAFDD, 0xA157227F),
            new LoginKey("7.0.34.2", 0x28EAAFDD, 0xA157227F),
            new LoginKey("7.0.34.0", 0x28EAAFDD, 0xA157227F),
            new LoginKey("7.0.33.0", 0x28A325ED, 0xA1767E7F),
            new LoginKey("7.0.32.11", 0x289BA7FD, 0xA169527F),
            new LoginKey("7.0.31.0", 0x295C260D, 0xA197BE7F),
            new LoginKey("7.0.30.1", 0x2904AC1D, 0xA1BCA27F),
            new LoginKey("7.0.29.2", 0x29CD362D, 0xA1D59E7F),
        	new LoginKey("7.0.29.0", 0x29CD362D, 0xA1D59E7F),
        	new LoginKey("7.0.28.0", 0x29B5843D, 0xA1EA127F),
        	new LoginKey("7.0.27.66", 0x2A7E164D, 0xA0081E7F),
        	new LoginKey("7.0.27.9", 0x2A7E164D, 0xA0081E7F),
        	new LoginKey("7.0.27.8", 0x2A7E164D, 0xA0081E7F),
            // previous keys
            new LoginKey("7.0.27.7", 0x2A7E164D, 0xA0081E7F),
			new LoginKey("7.0.26.5", 0x2A26EC5D, 0xA019A27F),
            new LoginKey("7.0.26.4", 0x2A26EC5D, 0xA019A27F),
            new LoginKey("7.0.25.7", 0x2AEF466D, 0xA07F3E7F),
            new LoginKey("7.0.25.6", 0x2AEF466D, 0xA07F3E7F),
            new LoginKey("7.0.25.0", 0x2AEF466D, 0xA07F3E7F),
            new LoginKey("7.0.24.5", 0x2AD7247D, 0xA065527F),
            new LoginKey("7.0.24.3", 0x2AD7247D, 0xA065527F),
            new LoginKey("7.0.24.2", 0x2AD7247D, 0xA065527F),
        	new LoginKey("7.0.23.1", 0x2A9F868D, 0xA0437E7F),
            new LoginKey("7.0.23.0", 0x2A9F868D, 0xA0437E7F),
            new LoginKey("7.0.22.8", 0x2B406C9D, 0xA0A1227F),
            new LoginKey("7.0.22.0", 0x2B406C9D, 0xA0A1227F),
            new LoginKey("7.0.21.2", 0x2B08D6AD, 0xA0875E7F),
            new LoginKey("7.0.21.1", 0x2B08D6AD, 0xA0875E7F),
            new LoginKey("7.0.21.0", 0x2B08D6AD, 0xA0875E7F),
            new LoginKey("7.0.20.0", 0x2BF084BD, 0xA0FD127F),
            //new LoginKey("7.0.19.1", 0x2BB976CD, 0xA0DBDE7F),
            //new LoginKey("7.0.19.0", 0x2BB976CD, 0xA0DBDE7F),
            //new LoginKey("7.0.18.0", 0x2C612CDD, 0xA328227F),
            //new LoginKey("7.0.17.0", 0x2C29E6ED, 0xA30EFE7F),
            //new LoginKey("7.0.16.3", 0x2C11A4FD, 0xA313527F),
            //new LoginKey("7.0.16.1", 0x2C11A4FD, 0xA313527F),
            //new LoginKey("7.0.16.0", 0x2C11A4FD, 0xA313527F),
            //new LoginKey("7.0.15.1", 0x2CDA670D, 0xA3723E7F),
            //new LoginKey("7.0.15.0", 0x2CDA670D, 0xA3723E7F),
            //new LoginKey("7.0.14.4", 0x2C822D1D, 0xA35DA27F),
            //new LoginKey("7.0.14.3", 0x2C822D1D, 0xA35DA27F),
            //new LoginKey("7.0.14.2", 0x2C822D1D, 0xA35DA27F),
            //new LoginKey("7.0.14.0", 0x2C822D1D, 0xA35DA27F),
		};
    }
}
