using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;
using SpaceInvaders.Enums;
using SpaceInvaders.Ship;

namespace SpaceInvaders.Shot
{
	/// <summary>
	/// Der Standard-Schuss
	/// </summary>
	public class DefaultShot : ShotBase
	{
		/// <summary>
		/// Der Base-Konstruktor für alle Schüsse.
		/// </summary>
		/// <param name="location">Ändert die <see cref="IShot.Rect"/></param>
		/// <param name="direction">Ändert die <see cref="IShot.Direction"/></param>
		public DefaultShot(Point location, Direction direction) : base(new Rect(location, DefaultSize), direction, DefaultDamage, DefaultSpeed, DefaultTextures)
		{

		}

		private const double DefaultDamage = 10;
		private const int DefaultSpeed = 2;
		private static readonly Size DefaultSize = new Size(2, 4);
		private static readonly IEnumerable<BitmapSource> DefaultTextures = new List<BitmapSource>
		{
			//TODO Texturen
		};
	}
}