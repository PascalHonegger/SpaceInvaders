using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows;
using Prism.Commands;
using SpaceInvaders.Annotations;
using SpaceInvaders.Enums;
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
		private Direction _invaderDirection = Direction.Right;
		private DateTime _invaderLastFired = DateTime.MinValue;
		private DateTime _invaderLastMoved = DateTime.MinValue;
		private IShip _player;
		private int _score;
		private int _wave;

		/// <summary>
		/// Command für <see cref="ReallyEndGame"/>
		/// </summary>
		public DelegateCommand ReallyEndGameCommand { get; }
		
		/// <summary>
		/// Command für <see cref="StartGame"/>
		/// </summary>
		public DelegateCommand StartGameCommand { get; }

		/// <summary>
		/// Constructor
		/// </summary>
		public SpaceInvadersViewModel()
		{
			ReallyEndGameCommand = new DelegateCommand(ReallyEndGame, () => !GameOver).ObservesProperty(() => GameOver);

			StartGameCommand = new DelegateCommand(StartGame).ObservesCanExecute(o => GameOver);
			ResetPlayerSelection();

			_player = PlayerSelection.First();
		}

		private void ResetPlayerSelection()
		{
			PlayerSelection.Clear();

			PlayerSelection.Add(new DefaultPlayer(PlayerSpawn));
			PlayerSelection.Add(new FastPlayer(PlayerSpawn));
		}

		/// <summary>
		///     Die Maximale Anzahl Zeichen, welche der Spielername lang sein darf
		/// </summary>
		public static int MaximumPlayerNameLength => 20;

		/// <summary>
		///     Liste aller aktiven Schüsse der Invader
		/// </summary>
		public List<IShot> InvaderShots { get; } = new List<IShot>();

		/// <summary>
		///     Liste aller aktiven Schüsse des Spielers
		/// </summary>
		public List<IShot> PlayerShots { get; } = new List<IShot>();

		/// <summary>
		///     Liste aller aktiven Invader
		/// </summary>
		public List<IShip> Invaders { get; } = new List<IShip>();

		private Point PlayerSpawn => new Point(_playArea.Width/2, _playArea.Height - 175);

		/// <summary>
		///     Alle Player-Schiffe, welche selektiert werden können
		/// </summary>
		public ObservableCollection<IShip> PlayerSelection { get; } = new ObservableCollection<IShip>();

		/// <summary>
		///     Der jetzige Spieler
		/// </summary>
		public IShip Player
		{
			get { return _player; }
			set
			{
				if (Equals(_player, value)) return;
				_player = value;
				_player?.Update();
				OnPropertyChanged();
			}
		}

		private void ReallyEndGame()
		{
			UpdateTimer.Stop();

			var rsltMessageBox =
				MessageBox.Show("Sind Sie sicher, dass Sie das jetzige Spiel beenden möchten? Ihr Highscore wird gespeichert!",
					"Sind Sie sich sicher?", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

			switch (rsltMessageBox)
			{
				case MessageBoxResult.Yes:
					EndGame();
					break;
				case MessageBoxResult.No:
				case MessageBoxResult.Cancel:
				case MessageBoxResult.None:
				case MessageBoxResult.OK:
					UpdateTimer.Start();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		/// <summary>
		///     Der Updatetimer bestimmt die Tickrate
		/// </summary>
		public Timer UpdateTimer = new Timer(100);

		/// <summary>
		///     Die aktuelle Punktzahl des Spielers
		/// </summary>
		public int Score
		{
			get { return _score; }
			set
			{
				if(Equals(_score, value)) return;
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
				if (Equals(_wave, value)) return;
				_wave = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		///     True, wenn das jetzige Spiel fertig ist => der <see cref="Player" /> keine Respawns übrig
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
		public string PlayerName
		{
			get { return Settings.Default.Username; }
			set
			{
				if (value?.Length >= MaximumPlayerNameLength)
				{
					return;
				}
				Settings.Default.Username = value;
				Settings.Default.Save();
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
			if (IsOutOfBounds(ship.Shot.Rect)) return;
			if (ship.ShipType == ShipType.Player && PlayerShots.Count < MaximumPlayerShotsAtTheSameTime)
			{
				PlayerShots.Add(ship.Shot);
				ship.Shot.Move();
			}
			else if (ship.ShipType == ShipType.Invader || ship.ShipType == ShipType.Boss)
			{
				InvaderShots.Add(ship.Shot);
				ship.Shot.Move();
			}
		}

		/// <summary>
		///     Beendet das Spiel
		/// </summary>
		private void EndGame()
		{
			GameOver = true;

			UpdateTimer.Elapsed -= UpdateEvent;
			UpdateTimer.Stop();

			//TODO Highscore-service inklusive Username
			if (Settings.Default.HighScore < Score)
			{
				Settings.Default.HighScore = Score;
				Settings.Default.Save();
			}

			ResetPlayerSelection();

			DestroyEverything();
		}

		private void RemoveInvader(IShip ship)
		{
			Invaders.Remove(ship);

			if (!GameOver)
			{
				Score += ship.Points;
			}
		}

		private void UpdateShips()
		{
			foreach (var invader in Invaders)
			{
				invader.Update();
			}

			Player.Update();
		}

		private void RemoveShot(IShot shot)
		{
			PlayerShots.Remove(shot);
			InvaderShots.Remove(shot);
		}

		/// <summary>
		///     Starte das Spiel und setzte alle Variablen zurück
		/// </summary>
		private void StartGame()
		{
			GameOver = false;

			DestroyEverything();

			_invaderDirection = Direction.Right;

			UpdateShips();

			Wave = 0;
			Score = 0;

			NextWave();

			UpdateTimer.Elapsed += UpdateEvent;
			UpdateTimer.Start();
		}

		private void UpdateEvent(object sender, ElapsedEventArgs args) => Update();

		/// <summary>
		///     Zertört alle Einheiten auf dem Spielfeld
		/// </summary>
		public void DestroyEverything()
		{
			foreach (var invader in Invaders.ToList())
			{
				RemoveInvader(invader);
			}

			foreach (var shot in PlayerShots.ToList())
			{
				RemoveShot(shot);
			}

			foreach (var shot in InvaderShots.ToList())
			{
				RemoveShot(shot);
			}
		}

		private void NextWave()
		{
			Wave++;

			foreach (var invader in Invaders)
			{
				RemoveInvader(invader);
			}

			Invaders.AddRange(CreateNewAttackWave());

			UpdateShips();
		}

		private IEnumerable<IShip> CreateNewAttackWave()
		{
			_invaderDirection = Direction.Right;
			IList<IShip> attackers = new List<IShip>();

			for (var row = 0; row < InvaderColumns; row++)
			{
				for (var column = 0; column < InvaderRows; column++)
				{
					var x = _playArea.Width / InvaderColumns * row;
					var y = _playArea.Height / InvaderRows / 3 * column;
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

			Player.Update();
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
		///     Aktualisiert das Spiel. Wird von <see cref="UpdateTimer" /> aufgerufen
		/// </summary>
		public void Update()
		{
			if (Player.Lives <= 0)
			{
				EndGame();
			}

			if (Invaders.Count == 0)
			{
				NextWave();
			}

			foreach (var shot in InvaderShots.Concat(PlayerShots))
			{
				shot.Move();

				if (IsOutOfBounds(shot.Rect))
				{
					RemoveShot(shot);
				}
			}

			CheckForInvaderCollision();

			CheckForPlayerCollision();

			MoveInvaders();

			InvaderReturnFire();
		}

		/// <summary>
		/// Schaut ob sich rects überlappen
		/// </summary>
		/// <param name="rect"></param>
		/// <returns></returns>
		public bool IsOutOfBounds(Rect rect)
		{
			var overlappingRect = Rect.Intersect(_playArea, rect);

			var x1 = Math.Round(overlappingRect.X, 1);
			var x2 = Math.Round(rect.X, 1);

			if (!Equals(x1, x2))
			{
				return true;
			}

			var y1 = Math.Round(overlappingRect.Y, 1);
			var y2 = Math.Round(rect.Y, 1);

			if (!Equals(y1, y2))
			{
				return true;
			}

			var width1 = Math.Round(overlappingRect.Width, 1);
			var width2 = Math.Round(rect.Width, 1);

			if (!Equals(width1, width2))
			{
				return true;
			}

			var heigth1 = Math.Round(overlappingRect.Height, 1);
			var heigth2 = Math.Round(rect.Height, 1);

			if (!Equals(heigth1, heigth2))
			{
				return true;
			}

			// Das komplette 'rect' überlappt sich mit dem Spielfeld
			return false;
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
					if (invader.Health <= 0)
					{
						RemoveInvader(invader);
					}

					RemoveShot(shot);
				}

				if (RectsOverlap(invader.Rect, Player.Rect))
				{
					Player.Health -= invader.Health;
					RemoveInvader(invader);
				}
			}
		}

		private void CheckForPlayerCollision()
		{
			foreach (var shot in InvaderShots.Where(shot => RectsOverlap(Player.Rect, shot.Rect)).ToList())
			{
				Player.Health -= shot.Damage;
				RemoveShot(shot);
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

			var highestY = Invaders.Max(i => i.Rect.Bottom);

			var invader = Invaders.Where(i => Equals(i.Rect.Bottom, highestY)).PickRandom();

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
				UpdateTimer = null;
				DestroyEverything();
				_invaderLastMoved = DateTime.MinValue;
				Player = null;
			}

			// Unmanaged Resources

			// Leer, da wir keine Serververbindung aufbauen und alle werte nur in dieser Instanz gespeichert werden
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