using System.Windows;
using System.Windows.Media.Imaging;
using SpaceInvaders.Enums;
using SpaceInvaders.ExtensionMethods;
using SpaceInvaders.Properties;

namespace SpaceInvaders.Shot
{
	/// <summary>
	///     Der Standard-Schuss
	/// </summary>
	public class DefaultShot : ShotBase
	{
		private const double DefaultDamage = 20;
		private const int DefaultSpeed = 15;
		private static readonly Size DefaultSize = new Size(20, 40);

		/// <summary>
		///     Der Base-Konstruktor für alle Schüsse.
		/// </summary>
		/// <param name="location">Ändert die <see cref="IShot.Rect" /></param>
		/// <param name="direction">Ändert die <see cref="ShotBase.Direction" /></param>
		public DefaultShot(Point location, Direction direction)
			: base(new Rect(location, DefaultSize), direction, DefaultDamage, DefaultSpeed)
		{
		}

		/// <summary>
		///     Die Textur des Schiffes, welche im View angezeigt wird
		/// </summary>
		public override BitmapSource CurrentTexture => Resources.player_shot.ToBitmapSource();
	}
}