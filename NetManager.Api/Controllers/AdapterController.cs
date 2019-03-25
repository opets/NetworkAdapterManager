using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NetManager.Api.Dto;
using NetManager.Domain.Dto;
using NetManager.Domain.Hardware;
using Newtonsoft.Json;
using Serilog;

namespace NetManager.Api.Controllers {

	[Route( "api/[controller]" )]
	public sealed class AdapterController : ControllerBase {

		private AdapterService m_adapterService;

		public AdapterController() {
			m_adapterService = new AdapterService();
		}

		[HttpGet( "healthcheck" )]
		public string HealthCheck() => "ok";

		// GET api/adapter/all
		[HttpGet( "all" )]
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

		// GET api/adapter/123-456-789-123/all
		[HttpGet( "{adapterId}/all" )]
		public IActionResult GetAdapterAddresses( string adapterId ) {
			try {
				Log.Logger.Information( "API: GetAdapterAddresses" );

				var adapters = m_adapterService.GetNetworkAdapterAddresses( adapterId );

				Log.Logger.Debug( $"API: GetAdapterAddresses.Result: {JsonConvert.SerializeObject( adapters )}" );

				return Ok( adapters );

			} catch( KeyNotFoundException e ) {
				Log.Logger.Error( e, $"API: GetAdapterAddresses.NotFound: {e.Message}" );
				return NotFound( "network adapter not found" );

			} catch( Exception e ) {
				Log.Logger.Error( e, $"API: GetAdapterAddresses.Exception: {e.Message}" );
				throw;
			}
		}

		// POST api/adapter/123-123-123-123/add
		[HttpPost( "{adapterId}/add" )]
		public IActionResult AddIpAddress(
			string adapterId,
			[FromForm] AddIpAddressRequest addIpAddressRequest
		) {
			try {
				Log.Logger.Information( "API: AddIPAddress" );

				string result = m_adapterService.AddNetworkAdapterAddress( adapterId, addIpAddressRequest.IpAddress );

				Log.Logger.Debug( $"API: AddIPAddress.Result: {JsonConvert.SerializeObject( result )}" );

				//				if(string.Equals("ok", result, StringComparison.InvariantCultureIgnoreCase ) ) {
				return Ok( result );
				//				}else return BadRequest( result );


			} catch( KeyNotFoundException e ) {
				Log.Logger.Error( e, $"API: AddIpAddress.NotFound: {e.Message}" );
				return NotFound( "network adapter not found" );

			} catch( FormatException e ) {
				Log.Logger.Error( e, $"API: AddIpAddress.FormatException: {e.Message}" );
				return BadRequest( e.Message );

			} catch( NotSupportedException e ) {
				Log.Logger.Error( e, $"API: AddIpAddress.NotSupportedException: {e.Message}" );
				return BadRequest( e.Message );

			} catch( Exception e ) {
				Log.Logger.Error( e, $"API: AddIPAddress.Exception: {e.Message}" );
				throw;
			}
		}

	}
}
