﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Timers;
using System.Windows;
using Prism.Commands;
using SpaceInvaders.Enums;
using SpaceInvaders.ExtensionMethods;
using SpaceInvaders.Infrastructure;
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
	public sealed class PlayFieldViewModel : PropertyChangedBase, IDisposable
	{
		private readonly Rect _playArea = new Rect(new Size(1075, 575));
		private bool _gameOver = true;
		private Direction _invaderDirection = Direction.Right;
		private DateTime _invaderLastFired = DateTime.MinValue;
		private DateTime _invaderLastMoved = DateTime.MinValue;
		private int _score;
		private int _wave;

		/// <summary>
		///     Der Updatetimer bestimmt die Tickrate
		/// </summary>
		private Timer _updateTimer = new Timer(100);

		/// <summary>
		///     Constructor
		/// </summary>
		public PlayFieldViewModel()
		{
			ReallyEndGameCommand = new DelegateCommand(ReallyEndGame, () => !GameOver).ObservesProperty(() => GameOver);

			StartGameCommand = new DelegateCommand(StartGame).ObservesCanExecute(o => GameOver);
			ResetPlayerSelection();

			Player = PlayerSelection.First();
		}


		private readonly List<WaveInformation> _availableInvaders = new List<WaveInformation>
		{
			new WaveInformation(3, 3, p => new Ufo(p)),
			new WaveInformation(5, 3, p => new Alien(p))
		};

		/// <summary>
		///     Command für <see cref="ReallyEndGame" />
		/// </summary>
		public DelegateCommand ReallyEndGameCommand { get; }

		/// <summary>
		///     Command für <see cref="StartGame" />
		/// </summary>
		public DelegateCommand StartGameCommand { get; }

		/// <summary>
		///     Die Maximale Anzahl Zeichen, welche der Spielername lang sein darf
		/// </summary>
		public static int MaximumPlayerNameLength => 20;

		/// <summary>
		///     Liste aller aktiven Schüsse
		/// </summary>
		public IList<IShot> Shots => GameObjects.OfType<IShot>().ToList();

		/// <summary>
		///     Liste aller aktiven Schiffef
		/// </summary>
		public IList<IShip> Invaders => GameObjects.OfType<IShip>().Where(s => s.ShipType != ShipType.Player).ToList();

		public ObservableCollection<IGameObject> GameObjects { get; } = new ObservableCollection<IGameObject>();

		private Point PlayerSpawn => new Point(_playArea.Width / 2, _playArea.Height - 175);

		/// <summary>
		///     Alle Player-Schiffe, welche selektiert werden können
		/// </summary>
		public ObservableCollection<IShip> PlayerSelection { get; } = new ObservableCollection<IShip>();

		/// <summary>
		///     Der jetzige Spieler
		/// </summary>
		public IShip Player
		{
			get { return GameObjects.OfType<IShip>().FirstOrDefault(s => s.ShipType == ShipType.Player); }
			set
			{
				var selectedPlayer = Player;
				if (selectedPlayer != null)
				{
					GameObjects.Remove(selectedPlayer);
				}
				GameObjects.Add(value);
				OnPropertyChanged();
			}
		}

		/// <summary>
		///     Die aktuelle Punktzahl des Spielers
		/// </summary>
		public int Score
		{
			get { return _score; }
			set
			{
				if (Equals(_score, value)) return;
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
					return;
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

		private void ResetPlayerSelection()
		{
			PlayerSelection.Clear();

			PlayerSelection.Add(new DefaultPlayer(PlayerSpawn));
			PlayerSelection.Add(new FastPlayer(PlayerSpawn));

			Player = PlayerSelection.First();
		}

		private void ReallyEndGame()
		{
			_updateTimer.Stop();

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
					_updateTimer.Start();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		/// <summary>
		///     Schiesst einen Schuss vom dem mitgegebenen Schiff
		/// </summary>
		/// <param name="ship">Das Schiff, welches einen <see cref="IShot" /> schiesst</param>
		public void FireShot(IShip ship)
		{
			if (IsOutOfBounds(ship.Shot.Rect)) return;

			GameObjects.Add(ship.Shot);

			ship.Shot.Move();
		}

		/// <summary>
		///     Beendet das Spiel
		/// </summary>
		private void EndGame()
		{
			if (GameOver) return;
			GameOver = true;

			_updateTimer.Elapsed -= UpdateEvent;
			_updateTimer.Stop();

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
			GameObjects.Remove(ship);

			if (!GameOver)
				Score += ship.Points;
		}

		private void UpdateShips()
		{
			foreach (var invader in Invaders)
				invader.Update();

			Player.Update();
		}

		private void UpdateShots()
		{
			foreach (var shot in Shots)
				shot.Update();
		}

		private void RemoveShot(IShot shot)
		{
			GameObjects.Remove(shot);
		}

		/// <summary>
		///     Starte das Spiel und setzte alle Variablen zurück
		/// </summary>
		private void StartGame()
		{
			GameOver = false;

			DestroyEverything();

			_invaderDirection = Direction.Right;

			Wave = 0;
			Score = 0;

			NextWave();

			_updateTimer.Elapsed += UpdateEvent;
			_updateTimer.Start();
		}

		private void UpdateEvent(object sender, ElapsedEventArgs args) => Application.Current?.Dispatcher?.Invoke(Update);

		/// <summary>
		///     Zertört alle Einheiten auf dem Spielfeld
		/// </summary>
		public void DestroyEverything()
		{
			foreach (var invader in Invaders)
				RemoveInvader(invader);

			foreach (var shot in Shots)
				RemoveShot(shot);
		}

		private void NextWave()
		{
			Wave++;

			foreach (var invader in Invaders)
				RemoveInvader(invader);

			foreach (var createdInvader in CreateNewAttackWave())
			{
				GameObjects.Add(createdInvader);
			}
		}

		private IEnumerable<IShip> CreateNewAttackWave()
		{
			_invaderDirection = Direction.Right;
			IList<IShip> attackers = new List<IShip>();

			var waveInformation = _availableInvaders.PickRandom();

			var invaderColumns = waveInformation.WaveColumns;
			var invaderRows = waveInformation.WaveRows;

			for (var row = 0; row < invaderColumns; row++)
			for (var column = 0; column < invaderRows; column++)
			{
				var x = _playArea.Width / invaderColumns * row;
				var y = _playArea.Height / invaderRows / 3 * column;
				var invader = waveInformation.CreateShip(new Point(x, y));
				attackers.Add(invader);
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
				return;

			Player.Move(direction);

			if (IsOutOfBounds(Player.Rect))
				Player.Move(InvertDirection(direction));

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
		///     Aktualisiert das Spiel. Wird von <see cref="_updateTimer" /> aufgerufen
		/// </summary>
		public void Update()
		{
			if (Player.Lives <= 0)
			{
				EndGame();
				return;
			}

			if (!Invaders.Any())
				NextWave();

			foreach (var shot in Shots)
			{
				shot.Move();

				if (IsOutOfBounds(shot.Rect))
					RemoveShot(shot);
			}

			CheckForInvaderCollision();

			CheckForPlayerCollision();

			MoveInvaders();

			InvaderReturnFire();

			UpdateShips();

			UpdateShots();
		}

		/// <summary>
		///     Schaut ob sich rects überlappen
		/// </summary>
		/// <param name="rect"></param>
		/// <returns></returns>
		public bool IsOutOfBounds(Rect rect)
		{
			var overlappingRect = Rect.Intersect(_playArea, rect);

			var x1 = Math.Round(overlappingRect.X, 1);
			var x2 = Math.Round(rect.X, 1);

			if (!Equals(x1, x2))
				return true;

			var y1 = Math.Round(overlappingRect.Y, 1);
			var y2 = Math.Round(rect.Y, 1);

			if (!Equals(y1, y2))
				return true;

			var width1 = Math.Round(overlappingRect.Width, 1);
			var width2 = Math.Round(rect.Width, 1);

			if (!Equals(width1, width2))
				return true;

			var heigth1 = Math.Round(overlappingRect.Height, 1);
			var heigth2 = Math.Round(rect.Height, 1);

			if (!Equals(heigth1, heigth2))
				return true;

			// Das komplette 'rect' überlappt sich mit dem Spielfeld
			return false;
		}

		private void MoveInvaders()
		{
			// ReSharper disable once PossibleLossOfFraction
			var timeToWait = 2 - Wave / 20;

			if (timeToWait < 0)
				timeToWait = 0;

			if (_invaderLastMoved >= DateTime.Now.AddSeconds(-timeToWait))
				return;

			_invaderLastMoved = DateTime.Now;

			foreach (var invader in Invaders)
				invader.Move(_invaderDirection);

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
			foreach (var invader in Invaders)
			{
				foreach (var shot in Shots.Where(shot => RectsOverlap(invader.Rect, shot.Rect)))
				{
					invader.Health -= shot.Damage;
					if (invader.Health <= 0)
						RemoveInvader(invader);

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
			foreach (var shot in Shots.Where(shot => RectsOverlap(Player.Rect, shot.Rect)))
			{
				Player.Health -= shot.Damage;
				RemoveShot(shot);
			}
		}

		private void InvaderReturnFire()
		{
			// ReSharper disable once PossibleLossOfFraction
			var timeToWait = 2 - Wave / 20;

			if (timeToWait < 0)
				timeToWait = 0;

			if (_invaderLastFired >= DateTime.Now.AddSeconds(-timeToWait))
				return;

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
				_updateTimer.Dispose();
				_updateTimer = null;
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
		~PlayFieldViewModel()
		{
			// Useless
			Dispose(false);
		}
	}
}