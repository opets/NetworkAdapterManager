using Moq;
using NetManager.Domain.Cache;
using NetManager.Domain.Services;
using Xunit;

namespace NetManager.Tests.Unit.Cache {

	public class CachedAdapterServiceTests {

		private readonly IAdapterService m_sut;
		private Mock<IAdapterService> m_mock;

		public CachedAdapterServiceTests() {
			m_mock = new Mock<IAdapterService>( MockBehavior.Loose );
			m_sut = new CachedAdapterService( m_mock.Object );
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
