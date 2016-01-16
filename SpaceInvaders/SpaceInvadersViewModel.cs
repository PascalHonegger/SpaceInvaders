using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows;
using SpaceInvaders.Control;
using SpaceInvaders.Enums;
using SpaceInvaders.EventArgs;
using SpaceInvaders.ExtensionMethods;
using SpaceInvaders.Properties;
using SpaceInvaders.Ship;
using SpaceInvaders.Ship.Invaders;
using SpaceInvaders.Ship.Players;
using SpaceInvaders.Shot;

namespace SpaceInvaders
{
	/// <summary>
	///     Das ViewModel des gesamten SpaceInvaders
	/// </summary>
	public sealed class SpaceInvadersViewModel : IDisposable, INotifyPropertyChanged
	{
		private const int MaximumPlayerShotsAtTheSameTime = 3;
		private const int InvaderColumns = 4;
		private const int InvaderRows = 3;
		private readonly Rect _playArea = new Rect(new Size(1074, 587));
		private bool _gameOver = true;
		private Direction _invaderDirection = Direction.Left;
		private DateTime _invaderLastFired = DateTime.MinValue;
		private DateTime _invaderLastMoved = DateTime.MinValue;
		private string _playerName = "Player1";
		private int _score;
		private int _wave;
		private IShip _player;

		/// <summary>
		///     Liste aller aktiven Schüsse der Invader
		/// </summary>
		public List<IShot> InvaderShots { get; } = new List<IShot>();

		/// <summary>
		///     Liste aller aktiven Schüsse des Spielers
		/// </summary>
		public List<IShot> PlayerShots { get; } = new List<IShot>();

		/// <summary>
		/// Liste aller aktiven Invader
		/// </summary>
		public List<IShip> Invaders { get; } = new List<IShip>();

		private Point PlayerSpawn => new Point(_playArea.Width / 2, _playArea.Height - 175);

		/// <summary>
		///     Alle Player-Schiffe, welche selektiert werden können
		/// </summary>
		public IEnumerable<IShip> PlayerSelection => new ObservableCollection<IShip>
		{
			new DefaultPlayer(PlayerSpawn),
			new FastPlayer(PlayerSpawn)
		};

		/// <summary>
		///     Die aktuellen Respawns des Spielers
		/// </summary>
		public int CurrentLives => Player.Lives;

		/// <summary>
		///     Der jetzige Spieler
		/// </summary>
		public IShip Player
		{
			get { return _player; }
			set
			{
				_player = value;
				OnPropertyChanged();
				// ReSharper disable once ExplicitCallerInfoArgument
				OnPropertyChanged(nameof(CurrentLives));
				OnShipChangedEventHandler(new ShipChangedEventArgs(_player));
			}
		}

		private KeyValuePair<IShip, ShipControl> PlayerWithControl => new KeyValuePair<IShip, ShipControl>(_player, new ShipControl(_player));

		private Timer UpdateTimer { get; } = new Timer(100);

		/// <summary>
		///     Die aktuelle Punktzahl des Spielers
		/// </summary>
		public int Score
		{
			get { return _score; }
			set
			{
				_score = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		///     Die jetzige Wave des Spieler, beeinflusst die Schwierigkeit
		/// </summary>
		public int Wave
		{
			get { return _wave; }
			set
			{
				_wave = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		///     True, wenn das jetzige Spiel fertig ist => der <see cref="Player" /> keine <see cref="CurrentLives" /> übrig
		///     hat
		/// </summary>
		public bool GameOver
		{
			get { return _gameOver; }
			set
			{
				if (value == _gameOver) return;
				_gameOver = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		///     Der ausgewählte Name des Spielers, wird für den Highscore verwendet
		/// </summary>
		/// <exception cref="NotImplementedException"></exception>
		public string PlayerName
		{
			get { return _playerName; }
			set
			{
				_playerName = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		///     OnPropertyChanged Event
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		///     Schiesst einen Schuss vom dem mitgegebenen Schiff
		/// </summary>
		/// <param name="ship">Das Schiff, welches einen <see cref="IShot" /> schiesst</param>
		public void FireShot(IShip ship)
		{
			if (ship.ShipType == ShipType.Player && PlayerShots.Count < MaximumPlayerShotsAtTheSameTime)
			{
				PlayerShots.Add(ship.Shot);
			}
			else if (ship.ShipType == ShipType.Invader || ship.ShipType == ShipType.Boss)
			{
				InvaderShots.Add(ship.Shot);
			}
			OnShotMovedEventHandler(new ShotMovedEventArgs(ship.Shot), IsOutOfBounds(ship.Shot.Rect));
		}

		/// <summary>
		///     Der EventHandel für die <see cref="ShipChangedEventArgs" />
		/// </summary>
		public event EventHandler<ShipChangedEventArgs> ShipChangedEventHandler;

		/// <summary>
		///     Der EventHandel für die <see cref="ShotMovedEventArgs" />
		/// </summary>
		public event EventHandler<ShotMovedEventArgs> ShotMovedEventHandler;

		/// <summary>
		///     Beendet das Spiel
		/// </summary>
		public void EndGame()
		{
			GameOver = true;
			UpdateTimer.Stop();
			//TODO  Save Highscore
			DestroyEverything();
		}

		private void OnShipChangedEventHandler(ShipChangedEventArgs e, bool removeInvader = false)
		{
			if (removeInvader)
			{
				Invaders.Remove(e.Ship);
				Score += e.Ship.Points;
			}
			ShipChangedEventHandler?.Invoke(this, e);
		}

		private void OnShotMovedEventHandler(ShotMovedEventArgs e, bool removeShot = false)
		{
			if (removeShot)
			{
				PlayerShots.Remove(e.Shot);
				InvaderShots.Remove(e.Shot);
			}

			ShotMovedEventHandler?.Invoke(this, e);
		}

		/// <summary>
		///     Starte das Spiel und setzte alle Variablen zurück
		/// </summary>
		public void StartGame()
		{
			GameOver = false;

			DestroyEverything();

			OnShipChangedEventHandler(new ShipChangedEventArgs(Player));

			Wave = 0;
			Score = 0;

			NextWave();

			UpdateTimer.Elapsed += (sender, args) => { Update(); };
			UpdateTimer.Start();
		}

		/// <summary>
		///     Zertört alle Einheiten auf dem Spielfeld
		/// </summary>
		public void DestroyEverything()
		{
			foreach (var invader in Invaders.ToList())
			{
				OnShipChangedEventHandler(new ShipChangedEventArgs(invader), true);
			}

			foreach (var shot in PlayerShots.ToList())
			{
				OnShotMovedEventHandler(new ShotMovedEventArgs(shot), true);
			}

			foreach (var shot in InvaderShots.ToList())
			{
				OnShotMovedEventHandler(new ShotMovedEventArgs(shot), true);
			}
		}

		private void NextWave()
		{
			Wave++;

			foreach (var invader in Invaders)
			{
				OnShipChangedEventHandler(new ShipChangedEventArgs(invader), true);
			}


			Invaders.AddRange(CreateNewAttackWave());

			foreach (var invader in Invaders)
			{
				OnShipChangedEventHandler(new ShipChangedEventArgs(invader));
			}
		}

		private IEnumerable<IShip> CreateNewAttackWave()
		{
			IList<IShip> attackers = new List<IShip>();

			for (var row = 0; row < InvaderColumns; row++)
			{
				for (var column = 0; column < InvaderRows; column++)
				{
					var x = _playArea.Width/InvaderColumns*row + 10;
					var y = _playArea.Height/InvaderRows/3*column;
					var invader = new Ufo(new Point(x, y));
					attackers.Add(invader);
				}
			}

			return attackers;
		}

		/// <summary>
		///     Bewegt den <see cref="Player" />
		/// </summary>
		/// <param name="direction">Die Richtung, in welche sich der <see cref="Player" /> bewegt</param>
		public void MovePlayer(Direction direction)
		{
			if (Player.Health <= 0)
			{
				return;
			}

			Player.Move(direction);

			if (IsOutOfBounds(Player.Rect))
			{
				Player.Move(InvertDirection(direction));
			}

			OnShipChangedEventHandler(new ShipChangedEventArgs(Player));
		}

		private Direction InvertDirection(Direction direction)
		{
			switch (direction)
			{
				case Direction.Left:
					return Direction.Right;
				case Direction.Right:
					return Direction.Left;
				case Direction.Up:
					return Direction.Down;
				case Direction.Down:
					return Direction.Up;
				default:
					throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
			}
		}

		/// <summary>
		/// Aktualisiert das Spiel. Wird von <see cref="UpdateTimer"/> aufgerufen
		/// </summary>
		public void Update()
		{
			if (Player.Lives == 0)
			{
				EndGame();
			}

			if (Invaders.Count == 0)
			{
				NextWave();
			}

			foreach (var shot in InvaderShots.ToList())
			{
				shot.Move();
				OnShotMovedEventHandler(new ShotMovedEventArgs(shot), IsOutOfBounds(shot.Rect));
			}
			foreach (var shot in PlayerShots.ToList())
			{
				shot.Move();
				OnShotMovedEventHandler(new ShotMovedEventArgs(shot), IsOutOfBounds(shot.Rect));
			}

			CheckForInvaderCollision();

			CheckForPlayerCollision();

			MoveInvaders();

			InvaderReturnFire();
		}

		private bool IsOutOfBounds(Rect rect)
		{
			var overlappingRect = Rect.Intersect(_playArea, rect);

			// Das komplette 'rect' überlappt sich mit dem Spielfeld
			return !Equals(overlappingRect, rect);
		}

		private void MoveInvaders()
		{
			// ReSharper disable once PossibleLossOfFraction
			var timeToWait = 2 - Wave/20;

			if (timeToWait < 0)
			{
				timeToWait = 0;
			}

			if (_invaderLastMoved >= DateTime.Now.AddSeconds(-timeToWait))
			{
				return;
			}

			_invaderLastMoved = DateTime.Now;

			foreach (var invader in Invaders)
			{
				invader.Move(_invaderDirection);
			}

			if (Invaders.Any(invader => IsOutOfBounds(invader.Rect)))
			{
				_invaderDirection = InvertDirection(_invaderDirection);
				foreach (var invader in Invaders)
				{
					invader.Move(_invaderDirection);
					invader.Move(Direction.Down);
				}
			}
		}

		private void CheckForInvaderCollision()
		{
			foreach (var invader in Invaders.ToList())
			{
				foreach (var shot in PlayerShots.ToList().Where(shot => RectsOverlap(invader.Rect, shot.Rect)))
				{
					invader.Health -= shot.Damage;
					OnShipChangedEventHandler(new ShipChangedEventArgs(invader), invader.Health <= 0);
				}
			}
		}

		private void CheckForPlayerCollision()
		{
			foreach (var shot in InvaderShots.Where(shot => RectsOverlap(Player.Rect, shot.Rect)).ToList())
			{
				Player.Health -= shot.Damage;
				OnShipChangedEventHandler(new ShipChangedEventArgs(Player));
				OnShotMovedEventHandler(new ShotMovedEventArgs(shot), true);
			}
		}

		private void InvaderReturnFire()
		{
			// ReSharper disable once PossibleLossOfFraction
			var timeToWait = 2 - Wave/20;

			if (timeToWait < 0)
			{
				timeToWait = 0;
			}

			if (_invaderLastFired >= DateTime.Now.AddSeconds(-timeToWait))
			{
				return;
			}

			_invaderLastFired = DateTime.Now;

			var invader = Invaders.PickRandom();

			FireShot(invader);
		}

		private static bool RectsOverlap(Rect rect1, Rect rect2)
		{
			rect1.Intersect(rect2);
			return !rect1.IsEmpty;
		}

		/// <summary>
		///     Rufe die Methode <see cref="FireShot" /> mit dem Spieler aus
		/// </summary>
		public void FireShotPlayer()
		{
			FireShot(Player);
		}

		// Analysefehler
		[SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed",
			MessageId = "<UpdateTimer>k__BackingField")]
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				// Managed Resources
				UpdateTimer.Dispose();
				DestroyEverything();
				_invaderLastMoved = DateTime.MinValue;
				Player = null;
				ShipChangedEventHandler = null;
				ShotMovedEventHandler = null;
			}

			// Unmanaged Resources

			// Leer, da wir keine Serververbindung aufbauen
		}

		/// <summary>
		///     Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage
		///     collection.
		/// </summary>
		~SpaceInvadersViewModel()
		{
			// Useless
			Dispose(false);
		}

		/// <summary>
		///     Notifies the GUI, that the Porperty changed
		/// </summary>
		/// <param name="propertyName">The name of the Property, which got changed</param>
		[NotifyPropertyChangedInvocator]
		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}