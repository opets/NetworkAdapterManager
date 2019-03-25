using System.Collections.Generic;
using NetManager.Domain.Dto;

namespace NetManager.Domain.Services {

	public interface IAdapterService {

		IEnumerable<AdapterInfo> GetAdapters();

		IEnumerable<string> GetAddresses( string adapterId );

		string AddAddress( string adapterId, string addressText );

	}
}