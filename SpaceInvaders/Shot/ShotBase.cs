using System;
using System.Windows;
using System.Windows.Media.Imaging;
using SpaceInvaders.Enums;
using SpaceInvaders.Infrastructure;
using SpaceInvaders.Ship;

namespace SpaceInvaders.Shot
{
	/// <summary>
	///     Die Grundimplementation des Schusses
	/// </summary>
	public abstract class ShotBase : PropertyChangedBase, IShot
	{
		private double _x;
		private double _y;

		/// <summary>
		///     Der Base-Konstruktor für alle Schüsse.
		/// </summary>
		/// <param name="location">Ändert die <see cref="Rect" /></param>
		/// <param name="direction">Ändert die <see cref="Direction" /></param>
		/// <param name="damage">Ändert den <see cref="Damage" /></param>
		/// <param name="speed">Ändert den <see cref="Speed" /></param>
		protected ShotBase(Point location, Direction direction, double damage, int speed)
		{
			X = location.X - Size.Width / 2;
			var yOffset = Size.Height + 10;
			if (direction == Direction.Up)
			{
				yOffset *= -1;
			}

			Y = location.Y + yOffset;
			Direction = direction;
			Damage = damage;
			Speed = speed;
		}

		/// <summary>
		///     Die Textur des Schiffes, welche im View angezeigt wird
		/// </summary>
		public abstract BitmapSource CurrentTexture { get; }

		/// <summary>
		///     Die Geschwindigkeit des Schusses. Wird in SpaceInvaders-Pixel / Tick angegeben
		/// </summary>
		private int Speed { get; }

		/// <summary>
		///     Die Richtung, in welche sich der Schuss bewegt
		/// </summary>
		private Direction Direction { get; }

		private Guid Identification { get; } = Guid.NewGuid();

		/// <summary>
		///     Der Schaden, welcher der Schuss beim Aufprall mit einem <see cref="IShip" /> verursacht
		/// </summary>
		public double Damage { get; }

		/// <summary>
		///     Die Location <see cref="Point" /> (top-left corner) und die Grösse <see cref="Size" /> des Schiffes in
		///     SpaceInvaders-Pixel
		/// </summary>
		public Rect Rect => new Rect(new Point(X, Y), Size);

		protected abstract Size Size { get; }

		/// <summary>
		///     Bewegt den Schuss in die <see cref="ShotBase.Direction" /> mit der Hilfe des <see cref="ShotBase.Speed" />
		/// </summary>
		public void Move()
		{
			switch (Direction)
			{
				case Direction.Left:
					throw new ArgumentOutOfRangeException(nameof(Direction), Direction, null);
				case Direction.Right:
					throw new ArgumentOutOfRangeException(nameof(Direction), Direction, null);
				case Direction.Up:
					Y -= Speed;
					break;
				case Direction.Down:
					Y += Speed;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(Direction), Direction, null);
			}
		}

		/// <summary>
		///     Sagt dem Schuss sich zu aktualisieren
		/// </summary>
		public void Update()
		{
			OnPropertyChanged(nameof(CurrentTexture));
		}

		/// <summary>
		///     Bestimmt, ob das angegebene Objekt mit dem aktuellen Objekt identisch ist.
		/// </summary>
		/// <returns>
		///     true, wenn das angegebene Objekt und das aktuelle Objekt gleich sind, andernfalls false.
		/// </returns>
		/// <param name="obj">Das Objekt, das mit dem aktuellen Objekt verglichen werden soll. </param>
		/// <filterpriority>2</filterpriority>
		public override bool Equals(object obj)
		{
			var shotBase = obj as ShotBase;
			if (shotBase == null) return false;

			return Equals(Identification, shotBase.Identification);
		}

		/// <summary>
		///     Fungiert als die Standardhashfunktion.
		/// </summary>
		/// <returns>
		///     Ein Hashcode für das aktuelle Objekt.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode()
		{
			return Identification.GetHashCode();
		}

		public double X
		{
			get { return _x; }
			set
			{
				if (value.Equals(_x)) return;
				_x = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(Rect));
			}
		}

		public double Y
		{
			get { return _y; }
			set
			{
				if (value.Equals(_y)) return;
				_y = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(Rect));
			}
		}
	}
}