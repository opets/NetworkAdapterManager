using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NetManager.Domain.Dto;
using NetManager.Domain.Hardware;
using Newtonsoft.Json;
using Serilog;

namespace NetManager.Api.Controllers {

	[Route( "api/[controller]" )]
	public sealed class AdapterController: ControllerBase {

		private AdapterService m_adapterService;

		public AdapterController() {
			m_adapterService = new AdapterService();
		}

		[HttpGet( "healthcheck" )]
		public string HealthCheck() => "ok";

		// GET api/adapter/list
		[HttpGet( "list" )]
		public IEnumerable<AdapterInfo> GetAdapters() {
			try {
				Log.Logger.Information( "API: GetAdapters" );

				var adapters = m_adapterService.GetNetworkAdapters();

				Log.Logger.Debug( $"API: GetAdapters.Result: {JsonConvert.SerializeObject( adapters )}" );

				return adapters;

			} catch( Exception e ) {
				Log.Logger.Error( e, $"API: GetAdapters.Exception: {e.Message}" );
				throw;
			}
		}

		// GET api/adapter/123-123-123-123/addresses
		[HttpGet( "{adapterId}/addresses" )]
		public IEnumerable<string> GetAdapterAddresses( string adapterId) {
			try {
				Log.Logger.Information( "API: GetAdapterAddresses" );

				var adapters = m_adapterService.GetNetworkAdapterAddresses( adapterId);

				Log.Logger.Debug( $"API: GetAdapterAddresses.Result: {JsonConvert.SerializeObject( adapters )}" );

				return adapters;

			} catch( Exception e ) {
				Log.Logger.Error( e, $"API: v.Exception: {e.Message}" );
				throw;
			}
		}

	}
}
