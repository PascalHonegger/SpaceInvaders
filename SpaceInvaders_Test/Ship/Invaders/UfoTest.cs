using System.Windows;
using NUnit.Framework;
using SpaceInvaders.Enums;
using SpaceInvaders.Ship;
using SpaceInvaders.Ship.Invaders;
using SpaceInvaders_Test.TestBases;

namespace SpaceInvaders_Test.Ship.Invaders
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

		[Test]
		public void TestGetRec()
		{
			// Arrange
			var playArea = new Rect(new Size(1074, 587));

			var ship = new Rect(new Point(1000, 10), new Size(74, 100));

			// Act
			var overlappingRect = Rect.Intersect(playArea, ship);

			// Assert
			Assert.That(overlappingRect, Is.EqualTo(ship));
		}
	}
}