using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;
using SpaceInvaders.Enums;
using SpaceInvaders.ExtensionMethods;
using SpaceInvaders.Properties;
using SpaceInvaders.Shot;

namespace SpaceInvaders.Ship.Players
{
	/// <summary>
	///     Der Standard-Spieler
	/// </summary>
	public class FastPlayer : ShipBase
	{
		private const int DefaultSpeed = 20;
		private const int DefaultLives = 2;
		private const int DefaultHealth = 50;
		private const string DefaultName = "Speedy McLight";
		private static readonly Size DefaultSize = new Size(10, 20);

		private static readonly List<BitmapSource> DefaultTextures = new List<BitmapSource>
		{
			Resources.speedship.ToBitmapSource()
		};

		/// <summary>
		///     Der Konstruktor für den <see cref="FastPlayer" />
		/// </summary>
		/// <param name="location">Die Location, an welcher der Spieler startet</param>
		public FastPlayer(Point location)
			: base(new DefaultShot(location, Direction.Down), DefaultHealth, DefaultName, DefaultTextures, DefaultLives, DefaultSpeed,
				new Rect(location, DefaultSize))
		{
		}
	}
}