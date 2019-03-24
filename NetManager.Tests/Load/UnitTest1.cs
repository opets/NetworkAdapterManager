using NetManager.Domain.Hardware;
using Xunit;

namespace NetManager.Tests.Load {

	public class UnitTest1 {

		[Fact]
		public void Test1() {
			var c = new AdapterService();
			string testMethod = c.TestMethod();

			Assert.NotNull( testMethod );
		}
	}

}
