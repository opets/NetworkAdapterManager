using System.Collections.Generic;
using System.Linq;
using NetManager.Domain.Dto;
using NetManager.Domain.Services;
using Xunit;

namespace NetManager.Tests.Integration.Services {

	public class AdapterServiceTests {

		private readonly IAdapterService m_sut;
		private IEnumerable<AdapterInfo> m_adapters;

		public AdapterServiceTests() {
			m_sut = new AdapterService();
			m_adapters = m_sut.GetAdapters();
		}

		[Fact]
		public void When_GGetAdapters_Pass() {

			Assert.NotNull( m_adapters );

		}

		[Fact]
		public void When_GetAddresses_Pass() {
			if( !m_adapters.Any() ) {
				return;
			}

			IEnumerable<string> addresses = m_sut.GetAddresses( m_adapters.First().Id );

			Assert.NotNull( addresses );

		}


	}

}
