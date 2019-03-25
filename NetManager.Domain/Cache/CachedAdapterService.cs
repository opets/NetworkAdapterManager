using System.Collections.Generic;
using NetManager.Domain.Dto;
using NetManager.Domain.Services;

namespace NetManager.Domain.Cache {

	public sealed class CachedAdapterService : IAdapterService {

		private readonly IAdapterService m_adapterService;
		private readonly IDictionary<string, object> m_cache = new Dictionary<string, object>();


		public CachedAdapterService( IAdapterService adapterService ) {
			m_adapterService = adapterService;
		}

		IEnumerable<AdapterInfo> IAdapterService.GetAdapters() {
			string key = "GetAdapters";

			if( m_cache.TryGetValue( key, out object cacheValue )
				&& cacheValue is IEnumerable<AdapterInfo> cachedAdapters ) {
				return cachedAdapters;
			}

			var adapters = m_adapterService.GetAdapters();
			m_cache.Add( key, adapters );

			return adapters;
		}

		IEnumerable<string> IAdapterService.GetAddresses( string adapterId ) {
			string key = GetAddressesKey( adapterId );

			if( m_cache.TryGetValue( key, out object cacheValue ) ) {
				if( cacheValue is IEnumerable<string> cachedAddresses ) {
					return cachedAddresses;
				}

				m_cache.Remove( key );
			}

			var addresses = m_adapterService.GetAddresses( adapterId );
			m_cache.Add( key, addresses );

			return addresses;
		}

		string IAdapterService.AddAddress( string adapterId, string addressText ) {

			string addressesKey = GetAddressesKey( adapterId );
			if( m_cache.ContainsKey( addressesKey ) ) {
				m_cache.Remove( addressesKey );
			}

			return m_adapterService.AddAddress( adapterId, addressText );
		}

		private static string GetAddressesKey( string adapterId ) {
			return $"GetAddresses/{adapterId}";
		}

	}
}
