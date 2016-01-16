﻿using System;
using System.Collections.Generic;
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
	public class WeakInvaderShot : ShotBase
	{
		/// <summary>
		/// Der Base-Konstruktor für alle Schüsse.
		/// </summary>
		/// <param name="location">Ändert die <see cref="IShot.Rect"/></param>
		/// <param name="direction">Ändert die <see cref="ShotBase.Direction"/></param>
		public WeakInvaderShot(Point location, Direction direction) : base(new Rect(location, DefaultSize), direction, DefaultDamage, DefaultSpeed)
		{

		}

		private const double DefaultDamage = 10;
		private const int DefaultSpeed = 15;
		private static readonly Size DefaultSize = new Size(20, 40);

		/// <summary>
		///     Die Textur des Schiffes, welche im View angezeigt wird
		/// </summary>
		public override BitmapSource CurrentTexture => DateTime.Now.Second < 5 ? Resources.shot1_animation_one.ToBitmapSource() : Resources.shot1_animation_two.ToBitmapSource();
	}
}