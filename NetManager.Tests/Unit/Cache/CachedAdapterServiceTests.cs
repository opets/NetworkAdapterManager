using Microsoft.Extensions.Caching.Memory;
using Moq;
using NetManager.Domain.Cache;
using NetManager.Domain.Services;
using Xunit;

namespace NetManager.Tests.Unit.Cache {

	public class AdapterServiceTests {

		private readonly IAdapterService m_sut;
		private readonly Mock<IAdapterService> m_mock;
		private readonly Mock<IMemoryCache> m_cache;

		public AdapterServiceTests() {
			m_mock = new Mock<IAdapterService>( MockBehavior.Loose );
			m_cache = new Mock<IMemoryCache>( MockBehavior.Loose );
			m_sut = new CachedAdapterService( m_mock.Object, new MemoryCache( new MemoryCacheOptions() ) );
		}

		[Fact]
		public void GetAdapters_RunInternalOnce() {

			m_sut.GetAdapters();
			m_sut.GetAdapters();

			m_mock.Verify( x => x.GetAdapters(), Times.Once );
		}

		[Fact]
		public void GetAddresses_AddNew_CleanCache() {

			m_sut.GetAddresses( "123" );
			m_sut.GetAddresses( "123" );

			m_sut.AddAddress( "123", "xxx" );
			m_mock.Verify( x => x.GetAddresses( "123" ), Times.Once );

			m_sut.GetAddresses( "123" );
			m_sut.GetAddresses( "123" );

			m_mock.Verify( x => x.GetAddresses( "123" ), Times.Exactly( 2 ) );
		}

	}
}
