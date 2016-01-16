using System.Windows;
using NUnit.Framework;
using SpaceInvaders.Enums;
using SpaceInvaders.Ship;
using SpaceInvaders.Ship.Players;
using SpaceInvaders_Test.TestBases;

namespace SpaceInvaders_Test.Ship.Players
{
	public class DefaultPlayerTest : UnitTestBase
	{
		private IShip _unitUnderTest;

		[Test]
		public void TestMove()
		{
			// Arrange
			_unitUnderTest = new DefaultPlayer(new Point(100, 100));

			// Act
			_unitUnderTest.Move(Direction.Right);

			// Assert
			var shouldArriveHerePoint = new Point(120, 100);

			Assert.That(_unitUnderTest.Rect.Location, Is.EqualTo(shouldArriveHerePoint));
		}

     

		protected override void OnSetUp()
		{
			// Nothing
		}

		protected override void OnTearDown()
		{
			// Nothing
		}
	}
}