using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace NetManager.Api.Controllers {

	[Route( "api/[controller]" )]
	public sealed class AdapterController: ControllerBase {

		[HttpGet]
		[Route( "[action]" )]
		public string HealthCheck() => "ok";


	}
}
