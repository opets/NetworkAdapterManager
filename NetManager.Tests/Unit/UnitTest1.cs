using NetManager.Domain;
using Xunit;

namespace NetManager.Tests.Unit {

	public class UnitTest1 {
		[Fact]
		public void Test1() {
			var c = new Class1();
			Assert.Equal( "123456", c.TestMethod() );
		}
	}

}
