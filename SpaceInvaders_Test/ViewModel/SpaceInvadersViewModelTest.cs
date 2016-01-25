using System.Windows;
using Moq;
using NUnit.Framework;
using SpaceInvaders;
using SpaceInvaders.Enums;
using SpaceInvaders.Ship;
using SpaceInvaders.Shot;
using SpaceInvaders_Test.TestBases;

namespace SpaceInvaders_Test.ViewModel
{
	public class SpaceInvadersViewModelTest : UnitTestBase
	{
		private SpaceInvadersViewModel _unitUnderTest;

		[TestCase("Pascal", ExpectedResult = true)]
		[TestCase("20zeichenqqqqqqqqqqq", ExpectedResult = false)]
		[TestCase("19zeichenqqqqqqqqqq", ExpectedResult = true)]
		public bool TestPlayerNameInput(string newName)
		{
			// Act
			_unitUnderTest.PlayerName = newName;

			// Assert
			return Equals(_unitUnderTest.PlayerName, newName);
		}


		protected override void OnSetUp()
			{
			_unitUnderTest = new SpaceInvadersViewModel();
		}

		protected override void OnTearDown()
		{
			// Nothing
		}

		[Test]
		public void TestCurrentLives()
		{
			// Arrange
			var playerMock = new Mock<IShip>();

			playerMock.Setup(p => p.Lives).Returns(0);
			playerMock.Setup(p => p.Health).Returns(0);


			_unitUnderTest.GameOver = false;
			_unitUnderTest.Player = playerMock.Object;

			// Act
			_unitUnderTest.Update();

			// Assert
			Assert.That(_unitUnderTest.GameOver, Is.True);
		}

		[Test]
		public void TestDestroyEverthing()
		{
			// Arrange
			var invaderMock = new Mock<IShip>();
			var shotMock = new Mock<IShot>();

			_unitUnderTest.Invaders.Add(invaderMock.Object);
			_unitUnderTest.InvaderShots.Add(shotMock.Object);

			invaderMock.Setup(pl => pl.ShipType).Returns(ShipType.Invader);

			// Act
			_unitUnderTest.DestroyEverything();

			// Assert
			Assert.That(_unitUnderTest.InvaderShots.Count, Is.EqualTo(0));
			Assert.That(_unitUnderTest.Invaders.Count, Is.EqualTo(0));
		}

		[Test]
		public void TestFireShotBoss()
		{
			// Arrange
			_unitUnderTest = new SpaceInvadersViewModel();
			var bossMock = new Mock<IShip>();
			var shotMock = new Mock<IShot>();

			bossMock.Setup(pl => pl.ShipType).Returns(ShipType.Invader);
			bossMock.Setup(pl => pl.Shot).Returns(shotMock.Object);

			// Act
			_unitUnderTest.FireShot(bossMock.Object);

			// Assert
			Assert.That(_unitUnderTest.InvaderShots.Count, Is.EqualTo(1));
		}

		[Test]
		public void TestFireShotInvader()
		{
			// Arrange
			var invaderMock = new Mock<IShip>();
			var shotMock = new Mock<IShot>();

			invaderMock.Setup(pl => pl.ShipType).Returns(ShipType.Invader);
			invaderMock.Setup(pl => pl.Shot).Returns(shotMock.Object);
			
			// Act
			_unitUnderTest.FireShot(invaderMock.Object);

			// Assert
			Assert.That(_unitUnderTest.InvaderShots.Count, Is.EqualTo(1));
		}
		[Test]
		public void TestIsOutOfBounds()
		{
			// Arrang
			_unitUnderTest = new SpaceInvadersViewModel();

			// Act

			// Assert
			for (int i = 0; i < 1074; i++)
			{
				for (int j = 0; j < 587; j++)
				{
					Assert.That(_unitUnderTest.IsOutOfBounds(new Rect(i, j, 1, 1)), Is.False, "Rect bei Position " + i.ToString() + j.ToString());
				}
			}
			
		}

		[Test]
		public void TestFireShotPlayer()
		{
			// Arrange
			var playerMock = new Mock<IShip>();
			var shotMock = new Mock<IShot>();

			playerMock.Setup(pl => pl.ShipType).Returns(ShipType.Player);
			playerMock.Setup(pl => pl.Shot).Returns(shotMock.Object);

			// Act
			_unitUnderTest.FireShot(playerMock.Object);

			// Assert
			Assert.That(_unitUnderTest.PlayerShots.Count, Is.EqualTo(1));
		}
	}
}