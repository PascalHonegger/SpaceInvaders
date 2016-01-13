using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using SpaceInvaders;
using SpaceInvaders.Enums;
using SpaceInvaders.Ship;
using SpaceInvaders.Ship.Players;
using SpaceInvaders.Shot;
using SpaceInvaders_Test.TestBases;
using Assert = NUnit.Framework.Assert;

namespace SpaceInvaders_Test.ViewModel
{
	public class SpaceInvadersViewModelTest : UnitTestBase
	{
		private SpaceInvadersViewModel _unitUnderTest;

		[Test]
		public void TestCurrentLives()
		{
			// Arrange
			_unitUnderTest = new SpaceInvadersViewModel
			{
			    GameOver = false,
                CurrentLives = 10
			};
            

            // Act
            _unitUnderTest.CurrentLives = 0;

            // Assert
            Assert.That(_unitUnderTest.GameOver, Is.True);
		}

		[Test]
		public void TestFireShotPlayer()
		{
			// Arrange
			_unitUnderTest = new SpaceInvadersViewModel();
			var playerMock = new Mock<IShip>();
			var shotMock = new Mock<IShot>();

			playerMock.Setup(pl => pl.ShipType).Returns(ShipType.Player);
			playerMock.Setup(pl => pl.Shot).Returns(shotMock.Object);

			// Act
			_unitUnderTest.FireShot(playerMock.Object);

			// Assert
			Assert.That(_unitUnderTest.PlayerShots.Count, Is.EqualTo(1));
		}

		[Test]
		public void TestFireShotInvader()
		{
			// Arrange
			_unitUnderTest = new SpaceInvadersViewModel();
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
		public void TestDestroyEverthing()
		{
			// Arrange

			var invaderMock = new Mock<IShip>();
			var shotMock = new Mock<IShot>();

			_unitUnderTest = new SpaceInvadersViewModel();
			_unitUnderTest.Invaders.Add(invaderMock.Object);
			_unitUnderTest.InvaderShots.Add(shotMock.Object);
			
			invaderMock.Setup(pl => pl.ShipType).Returns(ShipType.Invader);




			// Act
			_unitUnderTest.DestroyEverything();

			// Assert
			Assert.That(_unitUnderTest.InvaderShots.Count, Is.EqualTo(0));
			Assert.That(_unitUnderTest.Invaders.Count, Is.EqualTo(0));
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