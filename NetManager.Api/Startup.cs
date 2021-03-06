﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetManager.Domain.Cache;
using NetManager.Domain.Services;
using Serilog;

namespace NetManager.Api {

	public class Startup {

		public Startup( IConfiguration configuration ) {
			Log.Logger = new LoggerConfiguration().ReadFrom.Configuration( configuration ).CreateLogger();

			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices( IServiceCollection services ) {
			try {

				services.AddMemoryCache();
				services.AddSingleton<IAdapterService>( p => new CachedAdapterService( new AdapterService(), p.GetService<IMemoryCache>() ) );

				services.AddMvc();

			} catch( Exception e ) {
				Log.Logger.Error( e, "Startup/ConfigureServices exception" );
			}
		}

		public void Configure( IApplicationBuilder app, IHostingEnvironment env ) {
			try {

				if( env.IsDevelopment() ) {
					app.UseBrowserLink();
					app.UseDeveloperExceptionPage();
				} else {
					app.UseExceptionHandler( "/Error" );
				}

				app.UseStaticFiles();

				app.UseMvc();

			} catch( Exception e ) {
				Log.Logger.Error( e, "Startup/Configure exception" );
			}
		}

	}
}
