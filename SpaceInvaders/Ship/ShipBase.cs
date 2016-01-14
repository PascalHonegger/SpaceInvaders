using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media.Imaging;
using SpaceInvaders.Annotations;
using SpaceInvaders.Enums;
using SpaceInvaders.Shot;

namespace SpaceInvaders.Ship
{
	/// <summary>
	///     Die Grundimplementation des Schiffes
	/// </summary>
	public abstract class ShipBase : IShip, INotifyPropertyChanged
	{
		private double _health;
		private readonly double _totalHealth;

		private Guid Identification { get; } = Guid.NewGuid();

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
		protected ShipBase(int points, IShot shot, double health, string name, IEnumerable<BitmapSource> textures, Rect rect)
		{
			Shot = shot;
			Health = _totalHealth  = health;
			Name = name;
			Textures = textures;
			Speed = 30;
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
		protected ShipBase(IShot shot, double health, string name, IEnumerable<BitmapSource> textures, int totalLives, int speed, Rect rect)
		{
			Shot = shot;
			Health = _totalHealth = health;
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
		public double Health
		{
			get { return _health; }
			set
			{
				_health = value;

				if (_health <= 0 && Lives < 0)
				{
					Lives--;
					_health = _totalHealth;
				}
			}
		}

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
			var newX = Rect.X;
			var newY = Rect.Y;

			switch (direction)
			{
				case Direction.Left:
					newX -= Speed;
					break;
				case Direction.Right:
					newX += Speed;
					break;
				case Direction.Up:
					newY += Speed;
					break;
				case Direction.Down:
					newY -= Speed;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
			}
			Rect = new Rect(new Point(newX, newY), Rect.Size);
		}

		/// <summary>
		///     Die Respawns des Spielers
		/// </summary>
		public int Lives { get; private set; }

		/// <summary>
		///     Die Punkte, welche beim Tod
		/// </summary>
		public int Points { get; }

		/// <summary>
		///     Der Schifftyp, welcher darüber entscheided ob dieses Schiff durch den Spieler gelenkt wird
		/// </summary>
		public ShipType ShipType { get; }

		/// <summary>
		/// Tritt ein, wenn sich ein Eigenschaftswert ändert.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		///     Notifies the GUI, that the Porperty changed
		/// </summary>
		/// <param name="propertyName">The name of the Property, which got changed</param>
		[NotifyPropertyChangedInvocator]
		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		/// <summary>
		/// Bestimmt, ob das angegebene Objekt mit dem aktuellen Objekt identisch ist.
		/// </summary>
		/// <returns>
		/// true, wenn das angegebene Objekt und das aktuelle Objekt gleich sind, andernfalls false.
		/// </returns>
		/// <param name="obj">Das Objekt, das mit dem aktuellen Objekt verglichen werden soll. </param><filterpriority>2</filterpriority>
		public override bool Equals(object obj)
		{
			var shipBase = obj as ShipBase;
			if (shipBase == null) return false;

			return Equals(Identification, shipBase.Identification);
		}

		/// <summary>
		/// Fungiert als die Standardhashfunktion. 
		/// </summary>
		/// <returns>
		/// Ein Hashcode für das aktuelle Objekt.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode()
		{
			return Identification.GetHashCode();
		}
	}
}