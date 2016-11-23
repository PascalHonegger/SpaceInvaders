using System;
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
	public abstract class ShipBase : IShip
	{
		private readonly double _totalHealth;
		private double _health;
		private int _lives;

		/// <summary>
		///     Der Base-Konstruktor für alle Invader.
		/// </summary>
		/// <param name="health">Ändert das <see cref="Health" /></param>
		/// <param name="name">Ändert den <see cref="Name" /></param>
		/// <param name="points">Ändert die <see cref="Points" /></param>
		/// <param name="rect">Ändert die <see cref="Rect" /></param>
		protected ShipBase(string name, double health, int points, Rect rect)
		{
			Name = name;
			Health = _totalHealth = health;
			Points = points;
			Rect = rect;

			Speed = 30;
			ShipType = ShipType.Invader;
			Lives = 0;
		}

		/// <summary>
		///     Der Base-Konstruktor für alle Player.
		/// </summary>
		/// <param name="totalLives">Ändert die <see cref="Lives" /></param>
		/// <param name="name">Ändert den <see cref="Name" /></param>
		/// <param name="health">Ändert das <see cref="Health" /></param>
		/// <param name="speed">Ändert den <see cref="Speed" /></param>
		/// <param name="rect">Ändert die <see cref="Rect" /></param>
		protected ShipBase(int totalLives, string name, double health, int speed, Rect rect)
		{
			Lives = totalLives;
			Name = name;
			Health = _totalHealth = health;
			Speed = speed;
			Rect = rect;

			ShipType = ShipType.Player;
			Points = 0;
		}

		private readonly Guid _identification = Guid.NewGuid();

		/// <summary>
		///     Die Geschwindigkeit, mit welcher das Schiff sich vortbewegt. Wird in SpaceInvaders-Pixel / Tick angegeben
		/// </summary>
		private int Speed { get; }

		/// <summary>
		///     Die Textur des Schiffes, welche im View angezeigt wird
		/// </summary>
		public abstract BitmapSource CurrentTexture { get; }

		/// <summary>
		///     Tritt ein, wenn sich ein Eigenschaftswert ändert.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		///     Das Leben eines Schiffes. Wird bei einem Treffer reduziert. Unabhängig von den Respawns des Spielers!
		/// </summary>
		public double Health
		{
			get { return _health; }
			set
			{
				_health = value;

				if (_health <= 0 && Lives > 0)
				{
					Lives--;
					_health = _totalHealth;
				}

				OnPropertyChanged();
			}
		}

		/// <summary>
		///     Der Schuss des Schiffes, welcher beim Schiessen geschossen wird
		/// </summary>
		public abstract IShot Shot { get; }

		/// <summary>
		///     Die Location <see cref="Point" /> (top-left corner) und die Grösse <see cref="Size" /> des Schiffes in
		///     SpaceInvaders-Pixel
		/// </summary>
		public Rect Rect { get; private set; }


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
					newY -= Speed;
					break;
				case Direction.Down:
					newY += Speed;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
			}
			Rect = new Rect(new Point(newX, newY), Rect.Size);
		}

		/// <summary>
		///     Wird aufgerufen, wenn sich das Schiff aktualisieren sollte
		/// </summary>
		public void Update()
		{
			OnPropertyChanged(nameof(CurrentTexture));
		}

		/// <summary>
		///     Die Respawns des Spielers
		/// </summary>
		public int Lives
		{
			get { return _lives; }
			private set
			{
				if(Equals(_lives, value)) return;
				_lives = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		///     Die Punkte, welche beim Tod
		/// </summary>
		public int Points { get; }

		/// <summary>
		///     Der Schifftyp, welcher darüber entscheided ob dieses Schiff durch den Spieler gelenkt wird
		/// </summary>
		public ShipType ShipType { get; }

		/// <summary>
		///     Notifies the GUI, that the Porperty changed
		/// </summary>
		/// <param name="propertyName">The name of the Property, which got changed</param>
		[NotifyPropertyChangedInvocator]
		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
			var shipBase = obj as ShipBase;
			if (shipBase == null) return false;

			return Equals(_identification, shipBase._identification);
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
			return _identification.GetHashCode();
		}
	}
}