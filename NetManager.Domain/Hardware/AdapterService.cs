
using System;
using System.Net.NetworkInformation;
using System.Text;

namespace NetManager.Domain.Hardware {

	public class AdapterService {

		public string TestMethod() {
			var sb = new StringBuilder();

			var adapters =  NetworkInterface.GetAllNetworkInterfaces();
			foreach( NetworkInterface adapter in adapters ) {
				//adapter.
			}



			//NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
			//foreach( NetworkInterface adapter in adapters ) {
			//	IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
			//	// Try to get the IPv4 interface properties.
			//	IPv4InterfaceProperties p = adapterProperties.GetIPv4Properties();

			//	sb.AppendLine( adapter.Description );
			//	//sb.AppendLine( $"  DNS suffix .............................. : {p.}");
			//	//sb.AppendLine( $"  DNS enabled ............................. : {properties.IsDnsEnabled}");
			//	//sb.AppendLine( $"  Dynamically configured DNS .............. : {properties.IsDynamicDnsEnabled}");

			//	sb.AppendLine( "----" );
			//}
			return sb.ToString();
		}
	}
}
