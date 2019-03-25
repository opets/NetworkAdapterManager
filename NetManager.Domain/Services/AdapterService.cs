using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using NetManager.Domain.Dto;
using NetManager.Domain.Utils;

namespace NetManager.Domain.Services {

	public sealed class AdapterService : IAdapterService {

		IEnumerable<AdapterInfo> IAdapterService.GetAdapters() 
			=> NetworkInterface
				.GetAllNetworkInterfaces()
				.Select( 
					adapter => new AdapterInfo( 
						adapter.Id, 
						adapter.Name, 
						adapter.Description 
					) 
				);

		IEnumerable<string> IAdapterService.GetAddresses( string adapterId ) {

			NetworkInterface adapter = NetworkHelper.FindNetworkAdapter( adapterId );

			if( adapter == null ) {
				throw new KeyNotFoundException( $"unable to find network adapter with id {adapterId}" );
			}

			return adapter
				.GetIPProperties()
				.UnicastAddresses
				.Where( p => p.Address.AddressFamily != AddressFamily.InterNetworkV6 )
				.Select( p => $"{p.Address}" );
		}

		string IAdapterService.AddAddress( string adapterId, string addressText ) {

			IPAddress ipAddress = NetworkHelper.ParseIpAddress( addressText );

			NetworkInterface adapter = NetworkHelper.FindNetworkAdapter( adapterId );

			if( adapter == null ) {
				throw new KeyNotFoundException( $"unable to find network adapter with id {adapterId}" );
			}

			UnicastIPAddressInformationCollection ipAddresses = adapter.GetIPProperties().UnicastAddresses;

			if( ipAddresses == null ) {
				throw new NotSupportedException( "no IP Address info in adapter" );
			}


			if( ipAddresses.Any( x => x.Address.ToString() == ipAddress.ToString() ) ) {
				throw new NotSupportedException( "IP Address already exists" );
			}

			return NetworkHelper.NetshAddIpAddress( adapter.Name, ipAddress );
		}

	}
}
