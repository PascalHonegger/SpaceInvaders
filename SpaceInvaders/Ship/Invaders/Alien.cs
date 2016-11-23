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
	///     Der Alien-Invader
	/// </summary>
	public class Alien : ShipBase
	{
		private const double DefaultHealth = 20;

		private const string DefaultName = "Player of Doom";

		private const int DefaultPoints = 40;
		private static readonly Size DefaultSize = new Size(50, 100);

		/// <summary>
		///     Der Konstruktor für <see cref="Ufo" />
		/// </summary>
		/// <param name="location">Die Location, an welcher das Ufo auftaucht</param>
		public Alien(Point location) : base(DefaultName, DefaultHealth, DefaultPoints, new Rect(location, DefaultSize))
		{
		}

		/// <summary>
		///     Der Schuss des Schiffes, welcher beim Schiessen geschossen wird
		/// </summary>
		public override IShot Shot => new StrongInvaderShot(Rect.Location, Direction.Down);

		/// <summary>
		///     Die Textur des Schiffes, welche im View angezeigt wird
		/// </summary>
		public override BitmapSource CurrentTexture
			=>
			DateTime.Now.Second < 5
				? Resources.invader1_animation_one.ToBitmapSource()
				: Resources.invader1_animation_two.ToBitmapSource();
	}
}