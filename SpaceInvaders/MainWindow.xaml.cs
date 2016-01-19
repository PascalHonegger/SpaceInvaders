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
	///     Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly Dictionary<IShip, ShipControl> _shipWithControls = new Dictionary<IShip, ShipControl>();
		private readonly Dictionary<IShot, ShotControl> _shotWithControls = new Dictionary<IShot, ShotControl>();
		private DateTime _lastKeyInput = DateTime.MinValue;
		private KeyValuePair<IShip, ShipControl> _playerWithControl;

		/// <summary>
		///     Constructor for MainWindow
		/// </summary>
		public MainWindow()
		{
			InitializeComponent();

			KeyDown += OnKeyDown;

			DataContext = new SpaceInvadersViewModel();

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

		private SpaceInvadersViewModel ViewModel => DataContext as SpaceInvadersViewModel;

		/// <summary>
		///     Das <see cref="Dictionary{TKey,TValue}" /> mit dem Schiff und dem dazugehörigen Control
		/// </summary>
		private Dictionary<IShip, ShipControl> ShipWithControls
		{
			get
			{
				var hasControl = _shipWithControls.Where(kvp => ViewModel.Invaders.Contains(kvp.Key)).ToList();

				var hasNoControl = ViewModel.Invaders.Where(inv => !_shipWithControls.Select(kvp => kvp.Key).Contains(inv));

				hasControl.AddRange(hasNoControl.Select(ship => new KeyValuePair<IShip, ShipControl>(ship, new ShipControl(ship))));

				_shipWithControls.Clear();

				foreach (var kvp in hasControl)
				{
					_shipWithControls.Add(kvp.Key, kvp.Value);
				}

				if (_playerWithControl.Key == null || Equals(_playerWithControl.Key, ViewModel.Player))
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

				var hasNoControl = shots.Where(inv => !_shotWithControls.Select(kvp => kvp.Key).Contains(inv));

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
				Animate(shotWithControl.Key.Rect, shotWithControl.Value);
			}

			foreach (var shipWithControl in ShipWithControls)
			{
				PlayArea51.Children.Add(shipWithControl.Value);
				Animate(shipWithControl.Key.Rect, shipWithControl.Value);
			}
		}

		/// <summary>
		///     Animiert das Schiff an die neue Position
		/// </summary>
		/// <param name="rect">Das zu animierende <see cref="Rect" /></param>
		/// <param name="control">Das zu animierende <see cref="ShipControl" /></param>
		private static void Animate(Rect rect, UIElement control)
		{
			Canvas.SetTop(control, rect.Y);

			Canvas.SetLeft(control, rect.X);
		}

		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			const double timeToWait = 0.1;

			if (_lastKeyInput >= DateTime.Now.AddSeconds(-timeToWait))
			{
				return;
			}

			_lastKeyInput = DateTime.Now;


			if (e.Key == Key.A || e.Key == Key.Left)
			{
				ViewModel.MovePlayer(Direction.Left);
			}
			else if (e.Key == Key.D || e.Key == Key.Right)
			{
				ViewModel.MovePlayer(Direction.Right);
			}
			else if (e.Key == Key.Space)
			{
				ViewModel.FireShotPlayer();
			}
		}

		private void StartGame_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.StartGame();

			StartButton.IsDefault = false;
			StartButton.Visibility = Visibility.Collapsed;

			StopButton.IsCancel = true;
			StopButton.Visibility = Visibility.Visible;
		}

		private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ViewModel.Player = e.AddedItems[0] as IShip;
		}

		private void StopButton_OnClick(object sender, RoutedEventArgs e)
		{
			var rsltMessageBox =
				MessageBox.Show("Sind Sie sicher, dass Sie das jetzige Spiel beenden möchten? Ihr Highscore wird gespeichert!",
					"Sind Sie sich sicher?", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

			switch (rsltMessageBox)
			{
				case MessageBoxResult.Yes:
					ViewModel.EndGame();

					StartButton.IsDefault = true;
					StartButton.Visibility = Visibility.Visible;

					StopButton.IsCancel = false;
					StopButton.Visibility = Visibility.Collapsed;
					break;
				case MessageBoxResult.No:
					break;
				case MessageBoxResult.Cancel:
					break;
				case MessageBoxResult.None:
					break;
				case MessageBoxResult.OK:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}