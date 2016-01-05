using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;
using SpaceInvaders.Enums;
using SpaceInvaders.Shot;

namespace SpaceInvaders.Ship
{
	/// <summary>
	///     Die Grundimplementation des Schiffes
	/// </summary>
	public abstract class ShipBase : IShip
	{
		/// <summary>
		///     Der Base-Konstruktor für alle Invader.
		/// </summary>
		/// <param name="shot">Ändert den <see cref="Shot" /></param>
		/// <param name="health">Ändert das <see cref="Health" /></param>
		/// <param name="name">Ändert den <see cref="Name" /></param>
		/// <param name="textures">Ändert die <see cref="Textures" /></param>
		/// <param name="speed">Ändert den <see cref="Speed" /></param>
		/// <param name="points">Ändert die <see cref="Points" /></param>
		/// <param name="rect">Ändert die <see cref="Rect" /></param>
		public ShipBase(int points, IShot shot, double health, string name, IEnumerable<BitmapSource> textures, int speed, Rect rect)
		{
			Shot = shot;
			Health = health;
			Name = name;
			Textures = textures;
			Speed = speed;
			ShipType = ShipType.Invader;
			Points = points;
			Lives = 0;
			Rect = rect;
		}

		/// <summary>
		///     Der Base-Konstruktor für alle Player.
		/// </summary>
		/// <param name="shot">Ändert den <see cref="Shot" /></param>
		/// <param name="health">Ändert das <see cref="Health" /></param>
		/// <param name="name">Ändert den <see cref="Name" /></param>
		/// <param name="textures">Ändert die <see cref="Textures" /></param>
		/// <param name="totalLives">Ändert die <see cref="Lives"/></param>
		/// <param name="speed">Ändert den <see cref="Speed" /></param>
		/// <param name="rect">Ändert die <see cref="Rect" /></param>
		public ShipBase(IShot shot, double health, string name, IEnumerable<BitmapSource> textures, int totalLives, int speed, Rect rect)
		{
			Shot = shot;
			Health = health;
			Name = name;
			Textures = textures;
			Speed = speed;
			ShipType = ShipType.Player;
			Points = 0;
			Lives = totalLives;
			Rect = rect;
		}

		/// <summary>
		///     Das Leben eines Schiffes. Wird bei einem Treffer reduziert. Unabhängig von den Respawns des Spielers!
		/// </summary>
		public double Health { get; }

		/// <summary>
		///     Die Geschwindigkeit, mit welcher das Schiff sich vortbewegt. Wird in SpaceInvaders-Pixel / Tick angegeben
		/// </summary>
		private int Speed { get; }

		/// <summary>
		///     Der Schuss des Schiffes, welcher beim Schiessen geschossen wird
		/// </summary>
		public IShot Shot { get; }

		/// <summary>
		///     Die Location <see cref="Point" /> (top-left corner) und die Grösse <see cref="Size" /> des Schiffes in
		///     SpaceInvaders-Pixel
		/// </summary>
		public Rect Rect { get; private set; }

		/// <summary>
		///     Die Textures des Schiffes, welche im View angezeigt wird
		/// </summary>
		public IEnumerable<BitmapSource> Textures { get; }

		/// <summary>
		///     Der Name des Schiffes. Beispielsweise 'The Destroyer'
		/// </summary>
		public string Name { get; }

		/// <summary>
		///     Bewegt das Schiff in die gewünschte Richtung, indem es den <see cref="IShip.Rect" /> verändert. Kann beliebig oft
		///     aufgerufen werden (kein Cooldown)
		/// </summary>
		/// <param name="direction">Die Richtung, in welche sich das Schiff bewegt</param>
		public void Move(Direction direction)
		{
			double newX = 0;
			double newY = 0;

			switch (direction)
			{
				case Direction.Left:
					newX = Rect.X - Speed;
					break;
				case Direction.Right:
					newX = Rect.X + Speed;
					break;
				case Direction.Up:
					newY = Rect.Y + Speed;
					break;
				case Direction.Down:
					newY = Rect.Y - Speed;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
			}
			// TODO eleganter wäre 
			// Rect.X = newX;
			// Rect.Y = newY;

			Rect = new Rect(new Point(newX, newY), Rect.Size);
		}

		/// <summary>
		///     Die totalen Respawns des Spielers
		/// </summary>
		public int Lives { get; }

		/// <summary>
		///     Der Name des Menschen, welcher das Schiff steuert
		/// </summary>
		public string PlayerName { get; set; }

		/// <summary>
		///     Die Punkte, welche beim Tod
		/// </summary>
		public int Points { get; }

		/// <summary>
		///     Der Schifftyp, welcher darüber entscheided ob dieses Schiff durch den Spieler gelenkt wird
		/// </summary>
		public ShipType ShipType { get; }
	}
}