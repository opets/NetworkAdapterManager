using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
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

			NetworkInterface adapter = NetworkInterface
				.GetAllNetworkInterfaces()
				.FirstOrDefault( a => string.Equals( a.Id, adapterId, StringComparison.OrdinalIgnoreCase ) );

			if( adapter == null ) {
				throw new KeyNotFoundException( $"unable to found network adapter with id {adapterId}" );
			}

			return adapter
				.GetIPProperties()
				.UnicastAddresses
				.Where( p => p.Address.AddressFamily != AddressFamily.InterNetworkV6 )
				.Select( p => $"{p.Address}" );
		}

		public string AddNetworkAdapterAddress( string adapterId, string addressText ) {
			IPAddress ipAddress = ParseIpAddress( addressText );

			ManagementObject adapter = FindNetworkAdapter( adapterId );

			if( adapter == null ) {
				throw new KeyNotFoundException( $"unable to found network adapter with id {adapterId}" );
			}

			var ipAddresses = adapter.Properties.OfType<PropertyData>().FirstOrDefault( p => p.Name == "IPAddress" )?.Value as string[];
			var subnetMask = adapter.Properties.OfType<PropertyData>().FirstOrDefault( p => p.Name == "IPSubnet" )?.Value as string[];

			if( ipAddresses == null ) {
				throw new NotSupportedException("no IP Address info in adapter");
			}

			if( ipAddresses.Contains( ipAddress.ToString() ) ) {
				throw new NotSupportedException( "IP Address already exists" );
			}

			var ipAddressesNewValue = ipAddresses.Concat( new[] { ipAddress.ToString() } ).ToArray();


			ManagementBaseObject newIP =
				adapter.GetMethodParameters( "EnableStatic" );

			newIP["IPAddress"] = ipAddressesNewValue ;
			newIP["SubnetMask"] =  subnetMask?.Concat( new[] { "255.255.0.0" } ).ToArray(); ;

			ManagementBaseObject methodResult = adapter.InvokeMethod( "EnableStatic", newIP, null );

			return methodResult?["returnValue"].ToString();
		}

		public string TestMethod() {
			var sb = new StringBuilder();

			ManagementObjectSearcher networkAdapterSearcher = new ManagementObjectSearcher( "root\\cimv2", "select IPAddress, SettingID from Win32_NetworkAdapterConfiguration" );
			ManagementObjectCollection objectCollection = networkAdapterSearcher.Get();

			Console.WriteLine( "There are {0} network adapaters: ", objectCollection.Count );

			foreach( ManagementObject networkAdapter in objectCollection ) {
				PropertyDataCollection networkAdapterProperties = networkAdapter.Properties;
				foreach( PropertyData networkAdapterProperty in networkAdapterProperties ) {
					if( networkAdapterProperty.Value != null ) {
						sb.AppendLine( $"Network adapter property name: {networkAdapterProperty.Name}" );
						sb.AppendLine( $"Network adapter property value: {networkAdapterProperty.Value}" );
					}
				}
				sb.AppendLine( "---------------------------------------" );
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

		private IPAddress ParseIpAddress( string address )
			=> IPAddress.TryParse( address, out IPAddress ipAddress )
				? ipAddress
				: throw new FormatException( "IP Address Not Valid" );

		private ManagementObject FindNetworkAdapter( string adapterId ) {
			ManagementObjectSearcher networkAdapterSearcher = new ManagementObjectSearcher(
				"root\\cimv2",
				$"select IPAddress, IPSubnet, SettingID from Win32_NetworkAdapterConfiguration "
			);

			return networkAdapterSearcher.Get().OfType<ManagementObject>()
				.FirstOrDefault(
					adapter => adapter.Properties.OfType<PropertyData>().Any(
						networkAdapterProperty =>
							networkAdapterProperty.Name == "SettingID"
							&& string.Equals( networkAdapterProperty.Value?.ToString(), adapterId, StringComparison.OrdinalIgnoreCase )
					)
				);
		}

	}
}
