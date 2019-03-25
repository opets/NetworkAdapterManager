using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

namespace NetManager.Domain.Utils {

	internal static class NetworkHelper {

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

		internal static string NetshAddIpAddress( string adapterName, IPAddress ipAddress ) {

			var p = new Process();
			var psi = new ProcessStartInfo( "netsh", $"interface ipv4 add address \"{adapterName}\" {ipAddress} " );
			p.StartInfo = psi;
			p.Start();
			p.WaitForExit( 3000 );

			//todo: parse errors from process output
			return "ok";
		}


	}
}
