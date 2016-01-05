using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;
using SpaceInvaders.Enums;
using SpaceInvaders.Ship;

namespace SpaceInvaders.Shot
{
	/// <summary>
	/// Die Grundimplementation des Schusses
	/// </summary>
	public abstract class ShotBase : IShot
	{
		/// <summary>
		///     Die Textures des Schusses, welche im View angezeigt wird
		/// </summary>
		public IEnumerable<BitmapSource> Textures { get; }

		/// <summary>
		///     Der Schaden, welcher der Schuss beim Aufprall mit einem <see cref="IShip" /> verursacht
		/// </summary>
		public double Damage { get; }

		/// <summary>
		///     Die Geschwindigkeit des Schusses. Wird in SpaceInvaders-Pixel / Tick angegeben
		/// </summary>
		public int Speed { get; }

		/// <summary>
		///     Die Richtung, in welche sich der Schuss bewegt
		/// </summary>
		public Direction Direction { get; }

		/// <summary>
		///	Die Location <see cref="Point"/> (top-left corner) und die Grösse <see cref="Size"/> des Schiffes in SpaceInvaders-Pixel
		/// </summary>
		public Rect Rect { get; private set; }

		/// <summary>
		///     Bewegt den Schuss in die <see cref="IShot.Direction" /> mit der Hilfe des <see cref="IShot.Speed" />
		/// </summary>
		public void Move()
		{
			double newY;

			switch (Direction)
			{
				case Direction.Left:
					throw new ArgumentOutOfRangeException(nameof(Direction), Direction, null);
				case Direction.Right:
					throw new ArgumentOutOfRangeException(nameof(Direction), Direction, null);
				case Direction.Up:
					newY = Rect.Y + Speed;
					break;
				case Direction.Down:
					newY = Rect.Y - Speed;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(Direction), Direction, null);
			}

			// TODO eleganter wäre 
			// Rect.X = Rect.X;
			// Rect.Y = Rect.Y + distance;
			Rect = new Rect(new Point(Rect.X, newY), Rect.Size);
		}

		/// <summary>
		/// Der Base-Konstruktor für alle Schüsse.
		/// </summary>
		/// <param name="rect">Ändert die <see cref="Rect"/></param>
		/// <param name="direction">Ändert die <see cref="Direction"/></param>
		/// <param name="damage">Ändert den <see cref="Damage"/></param>
		/// <param name="speed">Ändert den <see cref="Speed"/></param>
		/// <param name="textures">Ändert die <see cref="Textures"/></param>
		public ShotBase(Rect rect, Direction direction, double damage, int speed, IEnumerable<BitmapSource> textures)
		{
			Rect = rect;
			Direction = direction;
			Damage = damage;
			Speed = speed;
			Textures = textures;
		}
	}
}
