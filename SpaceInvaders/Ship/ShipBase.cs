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
		///     Der Schuss des Schiffes, welcher beim Schiessen geschossen wird
		/// </summary>
		public abstract IShot Shot { get; }

		/// <summary>
		///     Die Grösse des Schiffes in SpaceInvaders-Pixel
		/// </summary>
		public abstract Size Size { get; }

		/// <summary>
		///     Die Position des Schiffes in SpaceInvaders-Pixel
		/// </summary>
		public abstract Point Location { get; protected set; }

		/// <summary>
		///     Die Textures des Schiffes, welche im View angezeigt wird
		/// </summary>
		public abstract IEnumerable<BitmapSource> Textures { get; }

		/// <summary>
		///     Das Leben eines Schiffes. Wird bei einem Treffer reduziert. Unabhängig von den Respawns des Spielers!
		/// </summary>
		public abstract double Health { get; }

		/// <summary>
		///     Der Name des Schiffes. Beispielsweise 'The Destroyer'
		/// </summary>
		public abstract string Name { get; }

		/// <summary>
		///     Bewegt das Schiff in die gewünschte Richtung, indem es den <see cref="IShip.Location" /> verändert
		/// </summary>
		/// <param name="direction">Die Richtung, in welche sich das Schiff bewegt</param>
		public void Move(Direction direction)
		{
			var oldX = Location.X;
			var oldY = Location.Y;

			double newX = 0;
			double newY = 0;

			switch (direction)
			{
				case Direction.Left:
					newX = oldX - Speed;
					break;
				case Direction.Right:
					newX = oldX + Speed;
					break;
				case Direction.Up:
					newY = oldY + Speed;
					break;
				case Direction.Down:
					newY = oldY - Speed;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
			}

			Location = new Point(newX, newY);
		}

		/// <summary>
		///     Die totalen Respawns des Spielers
		/// </summary>
		public abstract int TotalLives { get; }

		/// <summary>
		///     Die Respawns des Spielers
		/// </summary>
		public abstract int CurrentLives { get; set; }

		/// <summary>
		///     Der Name des Menschen, welcher das Schiff steuert
		/// </summary>
		public abstract string PlayerName { get; set; }

		/// <summary>
		///     Die Punkte, welche beim Tod
		/// </summary>
		public abstract int Points { get; }

		/// <summary>
		/// Der Schifftyp, welcher darüber entscheided ob dieses Schiff durch den Spieler gelenkt wird
		/// </summary>
		public abstract ShipType ShipType { get; }

		/// <summary>
		///     Die Geschwindigkeit, mit welcher das Schiff sich vortbewegt. Wird in SpaceInvaders-Pixel / Tick angegeben
		/// </summary>
		public abstract int Speed { get; }
	}
}