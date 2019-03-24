using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using NetManager.Domain.Dto;

namespace NetManager.Domain.Hardware {

	public class AdapterService {


		public IEnumerable<AdapterInfo> GetNetworkAdapters() {

			return NetworkInterface
				.GetAllNetworkInterfaces()
				.Select( adapter => new AdapterInfo( adapter.Id, adapter.Name, adapter.Description ) );
		}

		public IEnumerable<string> GetNetworkAdapterAddresses( string adapterId ) {

			NetworkInterface adapter = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault( a => string.Equals( a.Id, adapterId ) );
			if( adapter == null ) {
				throw new KeyNotFoundException( $"unable to found network adapter by id {adapterId}" );
			}

			var adapterProperties = adapter.GetIPProperties();

			foreach( IPAddressInformation p in adapterProperties.UnicastAddresses ) {
				if( p.Address.AddressFamily != AddressFamily.InterNetworkV6 ) {
					yield return $"{p.Address}";
				}
			}
		}


		public string TestMethod() {
			var sb = new StringBuilder();

			ManagementObjectSearcher networkAdapterSearcher = new ManagementObjectSearcher( "root\\cimv2", "select * from Win32_NetworkAdapterConfiguration" );
			ManagementObjectCollection objectCollection = networkAdapterSearcher.Get();

			Console.WriteLine( "There are {0} network adapaters: ", objectCollection.Count );

			foreach( ManagementObject networkAdapter in objectCollection ) {
				PropertyDataCollection networkAdapterProperties = networkAdapter.Properties;
				foreach( PropertyData networkAdapterProperty in networkAdapterProperties ) {
					if( networkAdapterProperty.Value != null ) {
						Console.WriteLine( "Network adapter property name: {0}", networkAdapterProperty.Name );
						Console.WriteLine( "Network adapter property value: {0}", networkAdapterProperty.Value );
					}
				}
				Console.WriteLine( "---------------------------------------" );
			}

			//NetworkInterface[] adapter2s = NetworkInterface.GetAllNetworkInterfaces();
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
