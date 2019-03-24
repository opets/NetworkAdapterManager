
using System;
using System.Net.NetworkInformation;
using System.Text;

namespace NetManager.Domain.Hardware {

	public class AdapterService {

		public string TestMethod() {
			var sb = new StringBuilder();

			NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
			foreach( NetworkInterface adapter in adapters ) {
				IPInterfaceProperties properties = adapter.GetIPProperties();
				sb.AppendLine( adapter.Description );

				sb.AppendLine( adapter.Description );
				sb.AppendLine( $"  DNS suffix .............................. : {properties.DnsSuffix}");
				sb.AppendLine( $"  DNS enabled ............................. : {properties.IsDnsEnabled}");
				sb.AppendLine( $"  Dynamically configured DNS .............. : {properties.IsDynamicDnsEnabled}");

				sb.AppendLine( "----" );
			}
			return sb.ToString();
		}
	}
}
