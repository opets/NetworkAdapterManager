using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace NetManager.Api {
	public class Program {

		public static void Main( string[] args ) {
			bool isService = !( Debugger.IsAttached || args.Contains( "--console" ) );

			var host = BuildWebHost( args.Where( arg => arg != "--console" ).ToArray(), isService );

			try {

				Log.Logger.Information( $"WebService started: {GetServerAddress( host )}. isService:{isService}" );

				if( isService ){
					host.RunAsService();
				}
				else
					host.Run();

			} catch( Exception e ) {
				Log.Logger.Error( e, "Program/Main exception" );
			}
		}

		private static string GetServerAddress( IWebHost host ) {
			return string.Join( "; ", host.ServerFeatures.Get<IServerAddressesFeature>()?.Addresses ?? Enumerable.Empty<string>() );
		}

		public static IWebHost BuildWebHost( string[] args, bool isService ) {

			string contentRoot = isService
				? Path.GetDirectoryName( Process.GetCurrentProcess().MainModule.FileName )
				: Directory.GetCurrentDirectory();

			var webHostBuilder =
			WebHost.CreateDefaultBuilder( args )
				.ConfigureLogging( ( context, builder ) => { builder.AddSerilog(); } )
				.ConfigureAppConfiguration( ( context, config ) => {
					// Configure the app here.
				} )
				.UseContentRoot(contentRoot )
				.UseStartup<Startup>();

			var configuration = new ConfigurationBuilder()
				.SetBasePath( contentRoot )
				.AddJsonFile( "appsettings.json", optional: true, reloadOnChange: true )
				.AddJsonFile( $"appsettings.{webHostBuilder.GetSetting( "environment" )}.json", optional: true )
				.Build();

			webHostBuilder.UseUrls( configuration["ServerUrl"] );

			return webHostBuilder.Build();
		}

	}
}
