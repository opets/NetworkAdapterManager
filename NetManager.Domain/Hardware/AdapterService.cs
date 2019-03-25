using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using NetManager.Domain.Dto;
using NetManager.Domain.Utils;

namespace NetManager.Domain.Hardware {

	public class AdapterService {


		public IEnumerable<AdapterInfo> GetNetworkAdapters() {

			return NetworkInterface
				.GetAllNetworkInterfaces()
				.Select( adapter => new AdapterInfo( adapter.Id, adapter.Name, adapter.Description ) );
		}

		public IEnumerable<string> GetNetworkAdapterAddresses( string adapterId ) {

			NetworkInterface adapter = NetworkUtils.FindNetworkAdapter( adapterId );

			if( adapter == null ) {
				throw new KeyNotFoundException( $"unable to find network adapter with id {adapterId}" );
			}

			return adapter
				.GetIPProperties()
				.UnicastAddresses
				.Where( p => p.Address.AddressFamily != AddressFamily.InterNetworkV6 )
				.Select( p => $"{p.Address}" );
		}

		public string AddNetworkAdapterAddress( string adapterId, string addressText ) {

			IPAddress ipAddress = NetworkUtils.ParseIpAddress( addressText );

			NetworkInterface adapter = NetworkUtils.FindNetworkAdapter( adapterId );

			var ipAddresses = adapter.GetIPProperties().UnicastAddresses;


			if( ipAddresses == null ) {
				throw new NotSupportedException( "no IP Address info in adapter" );
			}

			if( ipAddresses.Any( x => x.Address.ToString() == ipAddress.ToString() ) ) {
				throw new NotSupportedException( "IP Address already exists" );
			}

			Process p = new Process();
			ProcessStartInfo psi = new ProcessStartInfo( "netsh", $"interface ipv4 add address \"{adapter.Name}\" {ipAddress} " );
			p.StartInfo = psi;
			p.Start();
			p.WaitForExit(3000);

			return "ok";

//			Registry

			//ManagementObject adapter = FindNetworkAdapter( adapterId );

			//			if( adapter == null ) {
			//				throw new KeyNotFoundException( $"unable to found network adapter with id {adapterId}" );
			//			}

			//			var ipAddresses = adapter.Properties.OfType<PropertyData>().FirstOrDefault( p => p.Name == "IPAddress" )?.Value as string[];
			//			var subnetMask = adapter.Properties.OfType<PropertyData>().FirstOrDefault( p => p.Name == "IPSubnet" )?.Value as string[];

			//			if( ipAddresses == null ) {
			//				throw new NotSupportedException("no IP Address info in adapter");
			//			}

			//			if( ipAddresses.Contains( ipAddress.ToString() ) ) {
			//				throw new NotSupportedException( "IP Address already exists" );
			//			}

			//			var ipAddressesNewValue = ipAddresses.Concat( new[] { ipAddress.ToString() } ).ToArray();


			//			ManagementBaseObject newIP =
			//				adapter.GetMethodParameters( "EnableStatic" );

			//			newIP["IPAddress"] = ipAddressesNewValue ;
			//			newIP["SubnetMask"] =  subnetMask?.Concat( new[] { "255.255.0.0" } ).ToArray(); ;

			//			ManagementBaseObject methodResult = adapter.InvokeMethod( "EnableStatic", newIP, null );

			//			return methodResult?["returnValue"].ToString();
		}

		public string TestMethod() {
			var sb = new StringBuilder();

			
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
