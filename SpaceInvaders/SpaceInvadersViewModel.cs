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

		private static readonly Random Random = new Random();
		private readonly List<IShot> _invaderShots = new List<IShot>();

		private readonly Rect _playArea = new Rect(new Point(0, 0), new Size(1074, 587));
		private readonly List<IShot> _playerShots = new List<IShot>();
		private int _currentLives;
		private Direction _invaderDirection = Direction.Left;
		private DateTime _invaderLastMoved = DateTime.MinValue;
		private List<IShip> _invaders = new List<IShip>();
		private Dictionary<IShip, ShipControl> _invadersAndControls =  new Dictionary<IShip, ShipControl>();
		private bool _gameOver;

		/// <summary>
		/// Das <see cref="Dictionary{TKey,TValue}"/> mit dem Schiff und dem dazugehörigen Dictionary
		/// </summary>
		public Dictionary<IShip, ShipControl> InvadersWithControls
		{
			get
			{
				var hasControl = _invadersAndControls.Where(kvp => _invaders.Contains(kvp.Key)).ToList();

				var hasNoControl = _invaders.Where(inv=> _invadersAndControls.Select(kvp => kvp.Key).Contains(inv));

				hasControl.AddRange(hasNoControl.Select(ship => new KeyValuePair<IShip, ShipControl>(ship, new ShipControl(ship))));

				_invadersAndControls.Clear();

				foreach (var kvp in hasControl)
				{
					_invadersAndControls.Add(kvp.Key, kvp.Value);
				}

				return _invadersAndControls;
			}
		}

		private Point PlayerSpawn => new Point(_playArea.Width/2, 20);

		/// <summary>
		///     Alle Player-Schiffe, welche selektiert werden können
		/// </summary>
		public ObservableCollection<IShip> PlayerSelection => new ObservableCollection<IShip>
		{
			new DefaultPlayer(PlayerSpawn),
			new DefaultPlayer(PlayerSpawn),
			new DefaultPlayer(PlayerSpawn),
			new DefaultPlayer(PlayerSpawn),
			new DefaultPlayer(PlayerSpawn)
		};

		/// <summary>
		///     Die aktuellen Respawns des Spielers
		/// </summary>
		private int CurrentLives
		{
			get { return _currentLives; }
			set
			{
				if (value == _currentLives) return;
				_currentLives = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		///     Der jetzige Spieler
		/// </summary>
		public IShip Player { get; set; }

		private Timer UpdateTimer { get; } = new Timer(100);

		/// <summary>
		///     Die aktuelle Punktzahl des Spielers
		/// </summary>
		public int Score { get; set; }

		private int Wave { get; set; }

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
		private void FireShot(IShip ship)
		{
			if (ship.ShipType == ShipType.Player && _playerShots.Count < MaximumPlayerShotsAtTheSameTime)
			{
				_playerShots.Add(ship.Shot);

				OnShotMovedEventHandler(new ShotMovedEventArgs(ship.Shot, false));
			}
			else if (ship.ShipType == ShipType.Invader || ship.ShipType == ShipType.Boss)
			{
				_invaderShots.Add(ship.Shot);
			}
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

		private void OnShipChangedEventHandler(ShipChangedEventArgs e)
		{
			ShipChangedEventHandler?.Invoke(this, e);
		}

		private void OnShotMovedEventHandler(ShotMovedEventArgs e)
		{
			ShotMovedEventHandler?.Invoke(this, e);
		}

		/// <summary>
		///     Starte das Spiel und setzte alle Variablen zurück
		/// </summary>
		public void StartGame()
		{
			GameOver = false;

			CurrentLives = Player.Lives;

			DestroyEverything();

			ShipChangedEventHandler += (sender, e) =>
			{
				if (!e.GotShot) return;

				switch (e.Ship.ShipType)
				{
					case ShipType.Player:
						CurrentLives--;
						if (CurrentLives <= 0)
						{
							EndGame();
						}
						break;
					case ShipType.Invader:
					case ShipType.Boss:
						_invaders.Remove(e.Ship);
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(ShipType));
				}
			};

			ShotMovedEventHandler += (sender, e) =>
			{
				if (e.Disappeared)
				{
					_invaderShots.Remove(e.Shot);
					_invaderShots.Remove(e.Shot);
				}
			};

			//TODO ShipChangedEventHandler += GUI;
			//TODO ShotMovedEventHandler += GUI;
			OnShipChangedEventHandler(new ShipChangedEventArgs(Player, false));

			Wave = 0;
			Score = 0;

			NextWave();

			UpdateTimer.Elapsed += (sender, args) => { Update(); };
			UpdateTimer.Start();
		}

		private void DestroyEverything()
		{
			foreach (var invader in _invaders.ToList())
			{
				OnShipChangedEventHandler(new ShipChangedEventArgs(invader, true));
			}

			foreach (var shot in _playerShots.ToList())
			{
				OnShotMovedEventHandler(new ShotMovedEventArgs(shot, true));
			}

			foreach (var shot in _invaderShots.ToList())
			{
				OnShotMovedEventHandler(new ShotMovedEventArgs(shot, true));
			}
		}

		private void NextWave()
		{
			Wave++;
			_invaders = CreateNewAttackWave().ToList();
		}

		private IEnumerable<IShip> CreateNewAttackWave()
		{
			/*
			IList<IInvader> attackers = new List<IInvader>();

			var currentX = Invader.Height*1.4;
			var currentY = Invader.Width*1.4;
			for (var i = 0; i < 16; i++)
			{
				var invader = new Invader(new Point(currentX, currentY), GetInvaderType(), new BitmapImage());
				ShipChangedEventHandler += invader.OnShipChanged;
				attackers.Add(invader);
				currentX += Invader.Width*2.4;
				if (IsOutOfBounds(new Point(currentX*2.4, currentY)))
				{
					currentX = Invader.Height*1.4;
					currentY += Invader.Height + Invader.Height*1.4;
				}
			}

			return attackers;
			*/

			return new List<IShip>
			{
				new Ufo(new Point()),
				new Ufo(new Point()),
				new Ufo(new Point())
			};
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
			OnShipChangedEventHandler(new ShipChangedEventArgs(Player, false));
		}

		private void Update()
		{
			if (_invaders.Count == 0)
			{
				NextWave();
			}

			if (Player.Health <= 0)
			{
				OnShipChangedEventHandler(new ShipChangedEventArgs(Player, true));
			}
			else
			{
				foreach (var shot in _invaderShots.ToList())
				{
					shot.Move();
					OnShotMovedEventHandler(new ShotMovedEventArgs(shot, IsOutOfBounds(shot.Rect)));
				}
				foreach (var shot in _playerShots.ToList())
				{
					shot.Move();
					OnShotMovedEventHandler(new ShotMovedEventArgs(shot, IsOutOfBounds(shot.Rect)));
				}
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

			if (_invaderLastMoved <= DateTime.Now.AddSeconds(-timeToWait))
			{
				return;
			}

			_invaderLastMoved = DateTime.Now;

			foreach (var invader in _invaders)
			{
				invader.Move(_invaderDirection);
			}

			if (_invaders.Any(invader => IsOutOfBounds(invader.Rect)))
			{
				_invaderDirection = _invaderDirection == Direction.Left ? Direction.Right : Direction.Left;
				foreach (var invader in _invaders)
				{
					invader.Move(_invaderDirection);
					invader.Move(Direction.Down);
				}
			}
		}

		private void CheckForInvaderCollision()
		{
			foreach (var invader in _invaders.ToList())
			{
				foreach (var shot in _playerShots)
				{
					if (RectsOverlap(invader.Rect, shot.Rect))
					{
						OnShipChangedEventHandler(new ShipChangedEventArgs(invader, true));
					}
				}

				if (RectsOverlap(invader.Rect, Player.Rect))
				{
					OnShipChangedEventHandler(new ShipChangedEventArgs(invader, true));
				}
			}
		}

		private void CheckForPlayerCollision()
		{
			foreach (var shot in _invaderShots.Where(shot => RectsOverlap(Player.Rect, shot.Rect)))
			{
				OnShipChangedEventHandler(new ShipChangedEventArgs(Player, true));
				OnShotMovedEventHandler(new ShotMovedEventArgs(shot, true));
			}
		}

		private void InvaderReturnFire()
		{
			if (_invaderShots.Count > Wave + 1)
			{
				return;
			}

			//TODO Mehr Schüsse jede Wave
			/*if (Random.Next(10) < 10 - Wave)
			{
				return;
			}*/

			var invader = _invaders.PickRandom();

			FireShot(invader);
		}

		private static bool RectsOverlap(Rect rect1, Rect rect2)
		{
			rect1.Intersect(rect2);
			return rect1.Width > 0 || rect1.Height > 0;
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
				_invaderLastMoved = DateTime.MinValue;
				_invaders = null;
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