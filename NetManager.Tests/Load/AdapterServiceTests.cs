using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using NetManager.Domain.Cache;
using NetManager.Domain.Services;
using Xunit;
using Xunit.Abstractions;

namespace NetManager.Tests.Load {

	public class AdapterServiceTests {

		private readonly ITestOutputHelper m_testOutputHelper;
		private readonly IAdapterService m_adapterService;
		private readonly IAdapterService m_cachedAdapterService;

		public AdapterServiceTests( ITestOutputHelper testOutputHelper ) {
			m_testOutputHelper = testOutputHelper;
			m_adapterService = new AdapterService();
			m_cachedAdapterService = new CachedAdapterService( m_adapterService, new MemoryCache( new MemoryCacheOptions() ) );
		}

		[Fact]
		public void GetAdapters_100_1s() {

			var pureSw = Stopwatch.StartNew();
			Enumerable.Range( 1, 100 ).ToList()
				.ForEach( ( i ) => {
					m_adapterService.GetAdapters();
				} );
			pureSw.Stop();

			m_testOutputHelper.WriteLine( $"AdapterService.GetAdapters() x 100: {pureSw.Elapsed} elapsed" );

			var cachedSw = Stopwatch.StartNew();
			Enumerable.Range( 1, 100 ).ToList()
				.ForEach( ( i ) => {
					m_cachedAdapterService.GetAdapters();
				} );
			cachedSw.Stop();

			m_testOutputHelper.WriteLine( $"CachedAdapterService.GetAdapters() x 100: {cachedSw.Elapsed} elapsed" );

			Assert.True( cachedSw.Elapsed < TimeSpan.FromSeconds( 1 ) );

		}

	}
}
