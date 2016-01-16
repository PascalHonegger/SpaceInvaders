using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;
using SpaceInvaders.Enums;
using SpaceInvaders.ExtensionMethods;
using SpaceInvaders.Properties;
using SpaceInvaders.Shot;

namespace SpaceInvaders.Ship.Invaders
{
	/// <summary>
	/// Der Alien-Invader
	/// </summary>
	public class Alien : ShipBase
	{
		private static readonly Size DefaultSize = new Size(50, 100);

		private static readonly IEnumerable<BitmapSource> DefaultTextures = new List<BitmapSource>
		{
			Resources.invader1_animation_one.ToBitmapSource(),
			Resources.invader1_animation_two.ToBitmapSource()
		};

		private const double DefaultHealth = 20;

		private const string DefaultName = "Player of Doom";
		
		private const int DefaultPoints = 40;

		/// <summary>
		/// Der Konstruktor für <see cref="Ufo"/>
		/// </summary>
		/// <param name="location">Die Location, an welcher das Ufo auftaucht</param>
		public Alien(Point location) : base(DefaultPoints, new StrongInvaderShot(location, Direction.Down), DefaultHealth, DefaultName, DefaultTextures, new Rect(location, DefaultSize))
		{

		}
	}
}