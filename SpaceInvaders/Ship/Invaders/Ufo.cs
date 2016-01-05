using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;
using SpaceInvaders.Enums;
using SpaceInvaders.Shot;

namespace SpaceInvaders.Ship.Invaders
{
	internal class Ufo : ShipBase
	{
		private static readonly Size DefaultSize = new Size(20, 10);

		private static readonly IEnumerable<BitmapSource> DefaultTextures = new List<BitmapSource>();

		private const double DefaultHealth = 30;

		private const string DefaultName = "Ufo of Doom";
		

		private const ShipType DefaultShipType = ShipType.Invader;
		private const int DefaultPoints = 50;

		private const int DefaultSpeed = 30;

		public Ufo(Point location) : base(DefaultPoints, new DefaultShot(location, Direction.Down), DefaultHealth, DefaultName, DefaultTextures, DefaultSpeed, new Rect(location, DefaultSize))
		{

		}
	}
}