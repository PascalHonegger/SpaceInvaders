using System.Windows;
using NUnit.Framework;
using SpaceInvaders.Enums;
using SpaceInvaders.Ship;
using SpaceInvaders.Ship.Invaders;
using SpaceInvaders.Ship.Players;
using SpaceInvaders_Test.TestBases;

namespace SpaceInvaders_Test.Ship.Players
{
	public class UfoTest : UnitTestBase
	{
		private IShip _unitUnderTest;

		[Test]
		public void TestMove()
		{
			//TODO Example Test erweitern

			// Arrange
			_unitUnderTest = new Ufo(new Point(100,100));

			// Act
			_unitUnderTest.Move(Direction.Right);

			// Assert
			var shouldArriveHerePoint = new Point(130, 100);

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