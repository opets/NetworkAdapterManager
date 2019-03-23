using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;

namespace NetManager.Api {
	public class Program {
		public static void Main( string[] args ) {
			var isService = !( Debugger.IsAttached || args.Contains( "--console" ) );

			var builder = BuildWebHost( args.Where( arg => arg != "--console" ).ToArray() );

			if( isService ) {
				var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
				var pathToContentRoot = Path.GetDirectoryName( pathToExe );
				builder.UseContentRoot( pathToContentRoot );
			}

			IWebHost host = builder.Build();

			if( isService ) {
				host.RunAsService();
			} else {
				host.Run();
			}			
		}

		public static IWebHostBuilder BuildWebHost( string[] args ) =>
			WebHost.CreateDefaultBuilder( args )
				//.ConfigureLogging( ( hostingContext, logging ) =>
				//{
				//	logging.AddEventLog();
				//} )
				.ConfigureAppConfiguration( ( context, config ) => {
					// Configure the app here.
				} )
				.UseStartup<Startup>();
	}
}
