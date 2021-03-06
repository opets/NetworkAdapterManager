﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NetManager.Api.Dto;
using NetManager.Domain.Dto;
using NetManager.Domain.Services;
using Newtonsoft.Json;
using Serilog;

namespace NetManager.Api.Controllers {

	[Route( "api/[controller]" )]
	public sealed class AdapterController : ControllerBase {

		private readonly IAdapterService m_adapterService;

		public AdapterController( IAdapterService adapterService ) {
			m_adapterService = adapterService;
		}

		[HttpGet( "[action]" )]
		public string HealthCheck() => "ok";

		// GET api/adapter/all
		[HttpGet( "all" )]
		public IEnumerable<AdapterInfo> GetAdapters() {
			try {
				Log.Logger.Information( "API: GetAdapters" );

				var adapters = m_adapterService.GetAdapters();

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

				var adapters = m_adapterService.GetAddresses( adapterId );

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

				string result = m_adapterService.AddAddress( adapterId, addIpAddressRequest.IpAddress );

				Log.Logger.Debug( $"API: AddIPAddress.Result: {JsonConvert.SerializeObject( result )}" );

				return Ok( result );

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
