﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;
using SpaceInvaders.Enums;
using SpaceInvaders.ExtensionMethods;
using SpaceInvaders.Properties;

namespace SpaceInvaders.Shot
{
	/// <summary>
	/// Der Standard-Schuss
	/// </summary>
	public class StrongInvaderShot : ShotBase
	{
		/// <summary>
		/// Der Base-Konstruktor für alle Schüsse.
		/// </summary>
		/// <param name="location">Ändert die <see cref="IShot.Rect"/></param>
		/// <param name="direction">Ändert die <see cref="ShotBase.Direction"/></param>
		public StrongInvaderShot(Point location, Direction direction) : base(new Rect(location, DefaultSize), direction, DefaultDamage, DefaultSpeed)
		{

		}

		private const double DefaultDamage = 30;
		private const int DefaultSpeed = 12;
		private static readonly Size DefaultSize = new Size(50, 50);

		/// <summary>
		///     Die Textur des Schiffes, welche im View angezeigt wird
		/// </summary>
		public override BitmapSource CurrentTexture => Resources.bossshot.ToBitmapSource();
	}
}