using NUnit.Framework;

namespace NetManager.Tests.Unit {

	public class Tests {
		[SetUp]
		public void Setup() {
		}

		[Test]
		public void When_Test_Pass() {
			Assert.Pass();
		}

		[Test]
		public void When_Test_Fail() {
			Assert.Fail();
		}


	}
}