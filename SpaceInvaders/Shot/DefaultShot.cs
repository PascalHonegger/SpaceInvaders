using System;
using System.Windows;
using System.Windows.Media.Imaging;
using SpaceInvaders.Enums;
using SpaceInvaders.Ship;

namespace SpaceInvaders.Shot
{
	internal class DefaultShot : IShot
	{
		private DateTime _lastMoved;

		public DefaultShot(Point location, Direction direction)
		{
			Location = location;
			Direction = direction;
		}

		/// <summary>
		///     Die Grösse des Schussen in SpaceInvaders-Pixel
		/// </summary>
		public Size Size { get; } = new Size(2, 4);

		/// <summary>
		///     Die Textur des Schusses, welche im View angezeigt wird
		/// </summary>
		public BitmapSource Texture { get; }

		/// <summary>
		///     Der Schaden, welcher der Schuss beim Aufprall mit einem <see cref="IShip" /> verursacht
		/// </summary>
		public double Damage { get; } = 10;

		/// <summary>
		///     Die Geschwindigkeit des Schusses. Wird in SpaceInvaders-Pixel / Tick angegeben
		/// </summary>
		public int Speed { get; } = 95;

		/// <summary>
		///     Die Richtung, in welche sich der Schuss bewegt
		/// </summary>
		public Direction Direction { get; }

		/// <summary>
		///     Die Position des Schusses in SpaceInvaders-Pixel
		/// </summary>
		public Point Location { get; private set; }

		/// <summary>
		///     Bewegt den Schuss in die <see cref="IShot.Direction" /> mit der Hilfe des <see cref="IShot.Speed" />
		/// </summary>
		public void Move()
		{
			var timeSinceLastMoved = DateTime.Now - _lastMoved;
			var distance = timeSinceLastMoved.Milliseconds*Speed/1000;
			if (Direction == Direction.Up) distance *= -1;
			Location = new Point(Location.X, Location.Y + distance);
			_lastMoved = DateTime.Now;
		}
	}
}