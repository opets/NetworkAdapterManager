using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

				services.AddSingleton<IAdapterService, AdapterService>();

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
