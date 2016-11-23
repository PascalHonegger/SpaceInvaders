using NUnit.Framework;

namespace SpaceInvaders_Test.TestBases
{
	[TestFixture]
	[Category("UnitTest")]
	public abstract class UnitTestBase
	{
		[SetUp]
		public void SetUp()
		{
			OnSetUp();
		}

		[TearDown]
		public void TearDown()
		{
			OnTearDown();
		}

		protected abstract void OnSetUp();

		protected abstract void OnTearDown();
	}
}