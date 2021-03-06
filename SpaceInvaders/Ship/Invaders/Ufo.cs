﻿using System;
using System.Windows;
using System.Windows.Media.Imaging;
using SpaceInvaders.Enums;
using SpaceInvaders.ExtensionMethods;
using SpaceInvaders.Properties;
using SpaceInvaders.Shot;

namespace SpaceInvaders.Ship.Invaders
{
	/// <summary>
	///     Der Ufo-Invader
	/// </summary>
	public class Ufo : ShipBase
	{
		private const double DefaultHealth = 30;

		private const string DefaultName = "Ufo of Doom";

		private const int DefaultPoints = 50;
		protected override Size Size => new Size(100, 50);

		/// <summary>
		///     Der Konstruktor für <see cref="Ufo" />
		/// </summary>
		/// <param name="location">Die Location, an welcher das Ufo auftaucht</param>
		public Ufo(Point location) : base(DefaultName, DefaultHealth, DefaultPoints, location)
		{
		}

		/// <summary>
		///     Der Schuss des Schiffes, welcher beim Schiessen geschossen wird
		/// </summary>
		public override IShot Shot => new StrongInvaderShot(ShotSpawnPoint, Direction.Down);

		/// <summary>
		///     Die Textur des Schiffes, welche im View angezeigt wird
		/// </summary>
		public override BitmapSource CurrentTexture
			=>
			DateTime.Now.Second % 2 == 0
				? Resources.invaderboss_animation_1.ToBitmapSource("Invader_Ufo1")
				: Resources.invaderboss_animation_2.ToBitmapSource("Invader_Ufo2");
	}
}