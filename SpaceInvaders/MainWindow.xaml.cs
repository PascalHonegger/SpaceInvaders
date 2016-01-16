using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
		private readonly IList<IShip> _ships = new List<IShip>();
		private readonly IList<IShot> _shots = new List<IShot>();
		private DateTime _lastKeyInput = DateTime.MinValue;

		/// <summary>
		///     Constructor for MainWindow
		/// </summary>
		public MainWindow()
		{
			InitializeComponent();

			KeyDown += OnKeyDown;

			DataContext = new SpaceInvadersViewModel();

			ViewModel.ShipChangedEventHandler += (sender, e) =>
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

			ViewModel.ShotMovedEventHandler += (sender, e) =>
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

		private void ReDraw()
		{
			PlayArea51.Children.Clear();
			var shots = ViewModel.InvaderShots.Select(s => new KeyValuePair<IShot, ShotControl>(s, new ShotControl(s))).ToList();

			shots.AddRange(
				ViewModel.PlayerShots.Select(s => new KeyValuePair<IShot, ShotControl>(s, new ShotControl(s))).ToList());


			foreach (var shotWithControl in shots)
			{
				PlayArea51.Children.Add(shotWithControl.Value);
				AnimateControl(shotWithControl.Value, shotWithControl.Key.Rect);
			}


			var ships =
				ViewModel.Invaders.ToList().Select(s => new KeyValuePair<IShip, ShipControl>(s, new ShipControl(s))).ToList();

			ships.Add(new KeyValuePair<IShip, ShipControl>(ViewModel.Player, new ShipControl(ViewModel.Player)));

			foreach (var shipWithControl in ships)
			{
				PlayArea51.Children.Add(shipWithControl.Value);
				AnimateControl(shipWithControl.Value, shipWithControl.Key.Rect);
			}
		}

		private void AnimateControl(FrameworkElement control, Rect newRect)
		{
			foreach (var element in PlayArea51.Children)
			{
				var shot = element as ShotControl;
				var ship = element as ShipControl;

				if (shot != null)
				{
					var shotBase = shot.DataContext as ShotBase;

					var shotBase2 = control.DataContext as ShotBase;

					if (!Equals(shotBase, shotBase2)) continue;
				}
				else if (ship != null)
				{
					var shipBase = ship.DataContext as ShipBase;

					var shipBase2 = control.DataContext as ShipBase;

					if (!Equals(shipBase, shipBase2)) continue;
				}
				else
				{
					throw new NotImplementedException("Control has to be a ShipControl or ShotControl");
				}

				Animate(newRect, control);
			}
		}

		/// <summary>
		///     Animiert das Schiff an die neue Position
		/// </summary>
		/// <param name="rect">Das zu animierende <see cref="Rect" /></param>
		/// <param name="control">Das zu animierende <see cref="ShipControl" /></param>
		private static void Animate(Rect rect, UIElement control)
		{
			var top = Canvas.GetTop(control);
			var left = Canvas.GetLeft(control);

			if (double.IsNaN(top))
			{
				Canvas.SetTop(control, 0);
				top = Canvas.GetTop(control);
			}

			if (double.IsNaN(left))
			{
				Canvas.SetLeft(control, 0);
				left = Canvas.GetLeft(control);
			}


			var trans = new TranslateTransform();
			control.RenderTransform = trans;
			var anim1 = new DoubleAnimation(top, rect.X, TimeSpan.FromSeconds(0));
			var anim2 = new DoubleAnimation(left, rect.Y, TimeSpan.FromSeconds(0));
			trans.BeginAnimation(TranslateTransform.XProperty, anim1);
			trans.BeginAnimation(TranslateTransform.YProperty, anim2);

			//TODO control.NextImage();
		}

		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			// ReSharper disable once PossibleLossOfFraction
			var timeToWait = 0.1;

			if (timeToWait < 0)
			{
				timeToWait = 0;
			}

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