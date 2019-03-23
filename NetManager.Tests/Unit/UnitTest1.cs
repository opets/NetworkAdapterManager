using NetManager.Domain;
using NUnit.Framework;

namespace NetManager.Tests.Unit {

	public class Tests {
		[SetUp]
		public void Setup() {
		}

		[Test]
		public void When_Test_Pass() {
			var c = new Class1();
			Assert.AreEqual("123456",c.TestMethod());
		}

		[Test]
		public void When_Test_Fail() {
			Assert.Fail();
		}


	}
}