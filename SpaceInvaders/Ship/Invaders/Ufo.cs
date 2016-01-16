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
	/// Der Ufo-Invader
	/// </summary>
	public class Ufo : ShipBase
	{
		private static readonly Size DefaultSize = new Size(100, 50);

		private static readonly IEnumerable<BitmapSource> DefaultTextures = new List<BitmapSource>
		{
			Resources.invaderboss_animation_1.ToBitmapSource(),
			Resources.invaderboss_animation_2.ToBitmapSource()
		};

		private const double DefaultHealth = 30;

		private const string DefaultName = "Ufo of Doom";
		
		private const int DefaultPoints = 50;

		/// <summary>
		/// Der Konstruktor für <see cref="Ufo"/>
		/// </summary>
		/// <param name="location">Die Location, an welcher das Ufo auftaucht</param>
		public Ufo(Point location) : base(DefaultPoints, new StrongInvaderShot(location, Direction.Down), DefaultHealth, DefaultName, DefaultTextures, new Rect(location, DefaultSize))
		{

		}
	}
}