using System.Collections.Generic;
using NetManager.Domain.Dto;
using NetManager.Domain.Services;

namespace NetManager.Domain.Cache {

	public sealed class CachedAdapterService : IAdapterService {

		private readonly IAdapterService m_adapterService;
		//private readonly IDictionary<string, CacheValue> m_cache = new Dictionary<string, CacheValue>();

		public CachedAdapterService( IAdapterService adapterService) {
			m_adapterService = adapterService;
		}

		IEnumerable<AdapterInfo> IAdapterService.GetAdapters() 
			=> m_adapterService.GetAdapters();

		IEnumerable<string> IAdapterService.GetAddresses( string adapterId )
			=> m_adapterService.GetAddresses( adapterId );

		string IAdapterService.AddAddress( string adapterId, string addressText )
			=> m_adapterService.AddAddress( adapterId, addressText );
	}
}
