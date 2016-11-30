using System;
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
	public class WeakInvaderShot : ShotBase
	{
		private const double DefaultDamage = 10;
		private const int DefaultSpeed = 15;
		protected override Size Size => new Size(20, 40);

		/// <summary>
		///     Der Base-Konstruktor für alle Schüsse.
		/// </summary>
		/// <param name="location">Ändert die <see cref="IShot.Rect" /></param>
		/// <param name="direction">Ändert die <see cref="ShotBase.Direction" /></param>
		public WeakInvaderShot(Point location, Direction direction)
			: base(location, direction, DefaultDamage, DefaultSpeed)
		{
		}

		/// <summary>
		///     Die Textur des Schiffes, welche im View angezeigt wird
		/// </summary>
		public override BitmapSource CurrentTexture
			=>
			DateTime.Now.Second % 2 == 0
				? Resources.shot1_animation_one.ToBitmapSource("Shot_Weak1")
				: Resources.shot1_animation_two.ToBitmapSource("Shot_Weak2");
	}
}