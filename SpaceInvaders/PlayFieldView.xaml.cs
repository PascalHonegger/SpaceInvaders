using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SpaceInvaders.Control;
using SpaceInvaders.Enums;
using SpaceInvaders.Ship;
using SpaceInvaders.Shot;

namespace SpaceInvaders
{
	/// <summary>
	///     Interaction logic for PlayFieldView.xaml
	/// </summary>
	public partial class PlayFieldView
	{
		private readonly Dictionary<IShip, ShipControl> _shipWithControls = new Dictionary<IShip, ShipControl>();
		private readonly Dictionary<IShot, ShotControl> _shotWithControls = new Dictionary<IShot, ShotControl>();
		private DateTime _lastKeyInput = DateTime.MinValue;
		private KeyValuePair<IShip, ShipControl> _playerWithControl;

		/// <summary>
		///     Constructor for PlayFieldView
		/// </summary>
		public PlayFieldView()
		{
			InitializeComponent();

			KeyDown += OnKeyDown;

			DataContext = new PlayFieldViewModel();

			ViewModel.UpdateTimer.Elapsed += (sender, e) =>
			{
				try
				{
					Dispatcher.InvokeAsync(ReDraw);
				}
				catch
				{
					// ignored
				}
			};
		}

		private PlayFieldViewModel ViewModel => DataContext as PlayFieldViewModel;

		/// <summary>
		///     Das <see cref="Dictionary{TKey,TValue}" /> mit dem Schiff und dem dazugehörigen Control
		/// </summary>
		private Dictionary<IShip, ShipControl> ShipsWithControls
		{
			get
			{
				var hasControl = _shipWithControls.Where(kvp => ViewModel.Invaders.Contains(kvp.Key)).ToList();

				var hasNoControl = ViewModel.Invaders.Where(ship => !_shipWithControls.Select(kvp => kvp.Key).Contains(ship));

				hasControl.AddRange(hasNoControl.Select(ship => new KeyValuePair<IShip, ShipControl>(ship, new ShipControl(ship))));

				_shipWithControls.Clear();

				foreach (var kvp in hasControl)
				{
					_shipWithControls.Add(kvp.Key, kvp.Value);
				}

				if (_playerWithControl.Key == null || !Equals(_playerWithControl.Key, ViewModel.Player))
				{
					_playerWithControl = new KeyValuePair<IShip, ShipControl>(ViewModel.Player, new ShipControl(ViewModel.Player));
				}

				_shipWithControls.Add(_playerWithControl.Key, _playerWithControl.Value);

				return _shipWithControls;
			}
		}

		/// <summary>
		///     Das <see cref="Dictionary{TKey,TValue}" /> mit dem Schuss und dem dazugehörigen Control
		/// </summary>
		private Dictionary<IShot, ShotControl> ShotsWithControl
		{
			get
			{
				var shots = ViewModel.InvaderShots.ToList();

				shots.AddRange(ViewModel.PlayerShots.ToList());

				var hasControl = _shotWithControls.Where(kvp => shots.Contains(kvp.Key)).ToList();

				var hasNoControl = shots.Where(shot => !_shotWithControls.Select(kvp => kvp.Key).Contains(shot));

				hasControl.AddRange(hasNoControl.Select(shot => new KeyValuePair<IShot, ShotControl>(shot, new ShotControl(shot))));

				_shotWithControls.Clear();

				foreach (var kvp in hasControl)
				{
					_shotWithControls.Add(kvp.Key, kvp.Value);
				}

				return _shotWithControls;
			}
		}

		private void ReDraw()
		{
			PlayArea51.Children.Clear();

			foreach (var shotWithControl in ShotsWithControl)
			{
				PlayArea51.Children.Add(shotWithControl.Value);
				Position(shotWithControl.Key.Rect, shotWithControl.Value);
			}

			foreach (var shipWithControl in ShipsWithControls)
			{
				PlayArea51.Children.Add(shipWithControl.Value);
				Position(shipWithControl.Key.Rect, shipWithControl.Value);
			}
		}

		/// <summary>
		///     Animiert das Schiff an die neue Position
		/// </summary>
		/// <param name="rect">Das zu animierende <see cref="Rect" /></param>
		/// <param name="control">Das zu animierende <see cref="ShipControl" /></param>
		private static void Position(Rect rect, UIElement control)
		{
			Canvas.SetTop(control, rect.Y);
			Canvas.SetLeft(control, rect.X);
		}

		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			const double timeToWait = 0.05;

			if (_lastKeyInput >= DateTime.Now.AddSeconds(-timeToWait))
			{
				return;
			}

			_lastKeyInput = DateTime.Now;

			// ReSharper disable once SwitchStatementMissingSomeCases
			switch (e.Key)
			{
				case Key.A:
				case Key.Left:
					ViewModel.MovePlayer(Direction.Left);
					break;
				case Key.D:
				case Key.Right:
					ViewModel.MovePlayer(Direction.Right);
					break;
				case Key.Space:
				case Key.W:
				case Key.Up:
					ViewModel.FireShotPlayer();
					break;
			}
		}
	}
}