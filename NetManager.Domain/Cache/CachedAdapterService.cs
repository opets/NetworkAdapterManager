using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using NetManager.Domain.Dto;
using NetManager.Domain.Services;

namespace NetManager.Domain.Cache {

	public sealed class CachedAdapterService : IAdapterService {

		private readonly IAdapterService m_adapterService;
		private readonly IMemoryCache m_cache;
		private readonly MemoryCacheEntryOptions m_cacheEntryOptions;

		public CachedAdapterService( IAdapterService adapterService, IMemoryCache memoryCache ) {
			m_adapterService = adapterService;
			m_cache = memoryCache;
			m_cacheEntryOptions = new MemoryCacheEntryOptions()
				.SetSlidingExpiration( TimeSpan.FromMinutes( 1 ) );
		}

		IEnumerable<AdapterInfo> IAdapterService.GetAdapters() {
			string key = "GetAdapters";

			if( m_cache.TryGetValue( key, out object cacheValue )
				&& cacheValue is IEnumerable<AdapterInfo> cachedAdapters ) {
				return cachedAdapters;
			}

			var adapters = m_adapterService.GetAdapters().ToArray();
			m_cache.Set( key, adapters, m_cacheEntryOptions );

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

			var addresses = m_adapterService.GetAddresses( adapterId ).ToArray();
			m_cache.Set( key, addresses, m_cacheEntryOptions );

			return addresses;
		}

		string IAdapterService.AddAddress( string adapterId, string addressText ) {

			string addressesKey = GetAddressesKey( adapterId );
			m_cache.Remove( addressesKey );

			return m_adapterService.AddAddress( adapterId, addressText );
		}

		private static string GetAddressesKey( string adapterId ) {
			return $"GetAddresses/{adapterId}";
		}

	}
}
