using System;
using System.Collections.Generic;
using System.Linq;
using NetManager.Domain.Dto;
using NetManager.Domain.Services;
using Xunit;

namespace NetManager.Tests.Integration.Services {

	public class AdapterServiceTests {

		private readonly IAdapterService m_sut;
		private readonly IEnumerable<AdapterInfo> m_adapters;

		public AdapterServiceTests() {
			m_sut = new AdapterService();
			m_adapters = m_sut.GetAdapters();
		}

		[Fact]
		public void GetAdapters_Pass() {

			Assert.NotNull( m_adapters );

		}

		[Fact]
		public void GetAddresses_Pass() {
			if( !m_adapters.Any() ) {
				return;
			}

			IEnumerable<string> addresses = m_sut.GetAddresses( m_adapters.First().Id );

			Assert.NotNull( addresses );

		}

		[Fact]
		public void GetAddresses_IncorrectAdapterId_KeyNotFoundException() {
			if( !m_adapters.Any() ) {
				return;
			}

			Assert.Throws<KeyNotFoundException>( () => m_sut.GetAddresses( "12345" ) );

		}


		[Fact]
		public void AddAddress_IncorrectIp_FormatException() {

			Assert.Throws<FormatException>( () => m_sut.AddAddress( "12345", "XYZ" ) );

		}

		[Fact]
		public void AddAddress_IncorrectAdapterId_KeyNotFoundException() {

			Assert.Throws<KeyNotFoundException>( () => m_sut.AddAddress( "12345", "192.168.1.123" ) );

		}


	}

}
