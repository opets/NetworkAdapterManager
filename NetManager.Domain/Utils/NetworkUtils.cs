using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

namespace NetManager.Domain.Utils {

	internal static class NetworkUtils {

		internal static NetworkInterface FindNetworkAdapter( string adapterId ) {
			string normalizedAdapterId = NormalizeAdapterId( adapterId );

			NetworkInterface adapter = NetworkInterface
				.GetAllNetworkInterfaces()
				.FirstOrDefault( a => string.Equals( NormalizeAdapterId( a.Id ), normalizedAdapterId, StringComparison.OrdinalIgnoreCase ) );

			return adapter;
		}

		internal static IPAddress ParseIpAddress( string address ) {
			return IPAddress.TryParse( address, out IPAddress ipAddress )
				? ipAddress
				: throw new FormatException( "IP Address Not Valid" );
		}

		internal static string NormalizeAdapterId( string adapterId ) {
			return adapterId.Replace( "{", string.Empty ).Replace( "}", string.Empty );
		}

	}
}
