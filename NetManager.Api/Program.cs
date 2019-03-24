using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

			try {
				IWebHost host = BuildWebHost( args.Where( arg => arg != "--console" ).ToArray(), isService );

				Log.Logger.Information( $"WebService started: {GetServerAddress( host )}. isService:{isService}" );

				if( isService )
					host.RunAsService();
				else
					host.Run();

			} catch( Exception e ) {
				Log.Logger.Error( e, "Program/Main exception" );
			}
		}

		private static string GetServerAddress( IWebHost host ) 
			=> string.Join( "; ", host.ServerFeatures.Get<IServerAddressesFeature>()?.Addresses ?? Enumerable.Empty<string>() );

		public static IWebHost BuildWebHost( string[] args, bool isService ) {

			string contentRoot = isService
				? Path.GetDirectoryName( Process.GetCurrentProcess().MainModule.FileName )
				: Directory.GetCurrentDirectory();

			IWebHostBuilder webHostBuilder = WebHost.CreateDefaultBuilder( args );

			IConfigurationRoot configuration = new ConfigurationBuilder()
				.SetBasePath( contentRoot )
				.AddJsonFile( "appsettings.json", true, true )
				.AddJsonFile( $"appsettings.{webHostBuilder.GetSetting( "environment" )}.json", true )
				.Build();

			webHostBuilder
				.ConfigureLogging( ( context, builder ) => { builder.AddSerilog(); } )
				.ConfigureAppConfiguration(
					( context, config ) => {
						// Configure the app here.
					} )
				.UseContentRoot( contentRoot )
				.UseUrls( configuration["ServerUrl"] )
				.UseStartup<Startup>();

			return webHostBuilder.Build();
		}

	}
}
