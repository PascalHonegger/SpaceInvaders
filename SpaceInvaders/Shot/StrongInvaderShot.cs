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
	public class StrongInvaderShot : ShotBase
	{
		private const double DefaultDamage = 30;
		private const int DefaultSpeed = 12;
		protected override Size Size => new Size(50, 50);

		/// <summary>
		///     Der Base-Konstruktor für alle Schüsse.
		/// </summary>
		/// <param name="location">Ändert die <see cref="IShot.Rect" /></param>
		/// <param name="direction">Ändert die <see cref="ShotBase.Direction" /></param>
		public StrongInvaderShot(Point location, Direction direction)
			: base(location, direction, DefaultDamage, DefaultSpeed)
		{
		}

		/// <summary>
		///     Die Textur des Schiffes, welche im View angezeigt wird
		/// </summary>
		public override BitmapSource CurrentTexture => Resources.bossshot.ToBitmapSource("Shot_Strong1");
	}
}