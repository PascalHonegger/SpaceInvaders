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
		private static readonly Size DefaultSize = new Size(150, 100);

		/// <summary>
		///     Der Konstruktor für den <see cref="FastPlayer" />
		/// </summary>
		/// <param name="location">Die Location, an welcher der Spieler startet</param>
		public FastPlayer(Point location)
			: base(DefaultLives, DefaultName, DefaultHealth, DefaultSpeed, new Rect(location, DefaultSize))
		{
		}

		/// <summary>
		///     Der Schuss des Schiffes, welcher beim Schiessen geschossen wird
		/// </summary>
		public override IShot Shot => new DefaultShot(Rect.Location, Direction.Up);

		/// <summary>
		///     Die Textur des Schiffes, welche im View angezeigt wird
		/// </summary>
		public override BitmapSource CurrentTexture => Resources.speedship.ToBitmapSource();
	}
}