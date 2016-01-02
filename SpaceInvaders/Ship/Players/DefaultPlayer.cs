using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;
using SpaceInvaders.Enums;
using SpaceInvaders.Shot;

namespace SpaceInvaders.Ship.Players
{
	internal class DefaultPlayer : ShipBase
	{
		private const int DefaultSpeed = 20;
		private const int DefaultLives = 2;
		private const int DefaultHealth = 50;
		private const string DefaultName = "The Default Player";
		private static readonly Size DefaultSize = new Size(10, 20);

		private static readonly List<BitmapSource> DefaultTextures = new List<BitmapSource>();

		public DefaultPlayer(Point location)
			: base(
				DefaultSpeed, new DefaultShot(location, Direction.Down), DefaultHealth, DefaultName, DefaultTextures, DefaultLives,
				new Rect(location, DefaultSize))
		{
		}
	}
}