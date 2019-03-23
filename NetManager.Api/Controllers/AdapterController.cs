using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace NetManager.Api.Controllers {

	[Route( "api/[controller]" )]
	public sealed class AdapterController: ControllerBase {

		[HttpGet]
		[Route( "[action]" )]
		public string HealthCheck() => "ok";
	
		// GET api/adapter/list
		[HttpGet( "list" )]
		public string[] GetAdapters() {
			return new string[] {
				"1", "2"
			};
		}

		// GET api/adapter/1
		[HttpGet( "{id}/addresses" )]
		public string[] GetAdapterAddresses( string adapterCode) {
			return new string[] {
				"127.0.0.1", "192.168.1.1"
			};
		}

	}
}
