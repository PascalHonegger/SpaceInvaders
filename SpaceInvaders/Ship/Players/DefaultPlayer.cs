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
	public class DefaultPlayer : ShipBase
	{
		private const int DefaultSpeed = 20;
		private const int DefaultLives = 3;
		private const int DefaultHealth = 50;
		private const string DefaultName = "Player01";
		private static readonly Size DefaultSize = new Size(100, 150);

		private static readonly List<BitmapSource> DefaultTextures = new List<BitmapSource>
		{
			Resources.basicship.ToBitmapSource()
		};

		/// <summary>
		///     Der Konstruktor für den <see cref="DefaultPlayer" />
		/// </summary>
		/// <param name="location">Die Location, an welcher der Spieler startet</param>
		public DefaultPlayer(Point location)
			: base(new DefaultShot(location, Direction.Up), DefaultHealth, DefaultName, DefaultTextures, DefaultLives, DefaultSpeed,
				new Rect(location, DefaultSize))
		{
		}
	}
}