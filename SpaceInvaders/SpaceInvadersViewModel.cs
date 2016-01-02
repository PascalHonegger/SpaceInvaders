using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows;
using SpaceInvaders.Enums;
using SpaceInvaders.EventArgs;
using SpaceInvaders.ExtensionMethods;
using SpaceInvaders.Ship;
using SpaceInvaders.Ship.Invaders;
using SpaceInvaders.Shot;

namespace SpaceInvaders
{
	/// <summary>
	///     Das ViewModel des gesamten SpaceInvaders
	/// </summary>
	public class SpaceInvadersViewModel
	{
		private const int MaximumPlayerShotsAtTheSameTime = 3;

		private Rect _playArea = new Rect(new Point(0, 0), new Size(400, 300));

		private static readonly Random Random = new Random();
		private readonly List<IShot> _invaderShots = new List<IShot>();
		private readonly List<IShot> _playerShots = new List<IShot>();
		private Direction _invaderDirection = Direction.Left;
		private DateTime _invaderLastMoved = DateTime.MinValue;
		private List<IShip> _invaders = new List<IShip>();
		private DateTime? _playerDied = null;

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
		///     True, wenn das jetzige Spiel fertig ist => der <see cref="Player" /> keine <see cref="IShip.CurrentLives" /> übrig
		///     hat
		/// </summary>
		public bool GameOver { get; set; }

		private bool PlayerDying => _playerDied.HasValue;

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
			else if (ship.ShipType == ShipType.Invader)
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

		private void EndGame()
		{
			GameOver = true;
			UpdateTimer.Stop();
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

			Player.CurrentLives = Player.TotalLives;

			foreach (var invader in _invaders)
			{
				OnShipChangedEventHandler(new ShipChangedEventArgs(invader, true));
			}
			_invaders.Clear();

			foreach (var shot in _playerShots)
			{
				OnShotMovedEventHandler(new ShotMovedEventArgs(shot, true));
			}
			_playerShots.Clear();

			foreach (var shot in _invaderShots)
			{
				OnShotMovedEventHandler(new ShotMovedEventArgs(shot, true));
			}
			_invaderShots.Clear();

			//TODO ShipChangedEventHandler += Player.OnShipChanged;
			OnShipChangedEventHandler(new ShipChangedEventArgs(Player, false));

			Wave = 0;
			Score = 0;

			NextWave();

			UpdateTimer.Elapsed += (sender, args) => { Update(); };
			UpdateTimer.Start();
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
			if (PlayerDying)
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

			if (PlayerDying)
			{
				OnShipChangedEventHandler(new ShipChangedEventArgs(Player, true));
			}
			else
			{
				foreach (var shot in _invaderShots)
				{
					shot.Move();
					OnShotMovedEventHandler(new ShotMovedEventArgs(shot, IsOutOfBounds(shot.Rect)));
				}
				foreach (var shot in _playerShots)
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
			return Equals(overlappingRect, rect);
		}

		private void MoveInvaders()
		{
			// ReSharper disable once PossibleLossOfFraction
			var timeToWait = 2.1 - Wave/10;

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
			foreach (var shot in _invaderShots)
			{
				if (RectsOverlap(Player.Rect, shot.Rect))
				{
					OnShipChangedEventHandler(new ShipChangedEventArgs(Player, true));
				}
			}
		}

		private void InvaderReturnFire()
		{
			if (_invaderShots.Count > Wave + 1)
			{
				return;
			}

			if (Random.Next(10) < 10 - Wave)
			{
				return;
			}

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
	}
}