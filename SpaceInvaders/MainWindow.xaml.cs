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
				Dispatcher.Invoke(() =>
				{
					var justSpawned = false;

					var control = ViewModel.ShipWithControls.FirstOrDefault(kvp => kvp.Key.Equals(e.Ship)).Value;

					if (control == null)
					{
						//TODO Remove control
						return;
					}

					if (!_ships.Contains(control.DataContext as IShip))
					{
						_ships.Add(control.DataContext as IShip);
						PlayArea51.Children.Add(control);
						justSpawned = true;
					}

					foreach (var cont in PlayArea51.Children)
					{
						var ship = cont as ShipControl;

						var shipBase = ship?.DataContext as ShipBase;

						var shipBase2 = control.DataContext as ShipBase;

						if (!Equals(shipBase, shipBase2)) continue;

						if (justSpawned)
						{
							Animate(e.Ship.Rect, control, 0);
						}
						else
						{
							Animate(e.Ship.Rect, control);
						}
					}
				});
			};

			ViewModel.ShotMovedEventHandler += (sender, e) =>
			{
				Dispatcher.Invoke(() =>
				{
					var justSpawned = false;

					var control = ViewModel.ShotsWithControl.FirstOrDefault(kvp => kvp.Key.Equals(e.Shot)).Value;

					if (control == null)
					{
						//TODO Remove control
						return;
					}

					if (!_shots.Contains(control.DataContext as IShot))
					{
						_shots.Add(control.DataContext as IShot);
						PlayArea51.Children.Add(control);
						justSpawned = true;
					}

					foreach (var con in PlayArea51.Children)
					{
						var shot = con as ShotControl;

						var shotBase = shot?.DataContext as ShotBase;

						var shotBase2 = control.DataContext as ShotBase;

						if (!Equals(shotBase, shotBase2)) continue;

						if (justSpawned)
						{
							Animate(e.Shot.Rect, control, 0);
						}
						else
						{
							Animate(e.Shot.Rect, control);
						}
					}
				});
			};
		}

		private SpaceInvadersViewModel ViewModel => DataContext as SpaceInvadersViewModel;

		/// <summary>
		/// Animiert das Schiff an die neue Position
		/// </summary>
		/// <param name="rect">Das zu animierende <see cref="Rect"/></param>
		/// <param name="control">Das zu animierende <see cref="ShipControl"/></param>
		/// <param name="animationSecond">Die Zeit der Animation in Sekunden</param>
		private static void Animate(Rect rect, UIElement control, int animationSecond = 1)
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
			var anim1 = new DoubleAnimation(top, rect.X, TimeSpan.FromSeconds(animationSecond));
			var anim2 = new DoubleAnimation(left, -rect.Y, TimeSpan.FromSeconds(animationSecond));
			trans.BeginAnimation(TranslateTransform.XProperty, anim1);
			trans.BeginAnimation(TranslateTransform.YProperty, anim2);

			/*var moveInvaders = new Storyboard();

			// X
			var moveAnimationX = new DoubleAnimation
			{
				Duration = new Duration(TimeSpan.FromSeconds(1)),
				To = ship.Rect.Location.X
			};
			Storyboard.SetTarget(moveAnimationX, control);
			Storyboard.SetTargetProperty(moveAnimationX, new PropertyPath(Canvas.LeftProperty));
			moveInvaders.Children.Add(moveAnimationX);

			// Y
			var moveAnimationY = new DoubleAnimation
			{
				Duration = new Duration(TimeSpan.FromSeconds(1)),
				To = ship.Rect.Location.Y
			};
			Storyboard.SetTarget(moveAnimationY, control);
			Storyboard.SetTargetProperty(moveAnimationY, new PropertyPath(Canvas.TopProperty));
			moveInvaders.Children.Add(moveAnimationY);


			// Begin
			//TODO moveInvaders.Begin();
			//TODO control.StartAnimation();

	*/
		}
		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.A || e.Key == Key.Left)
			{
				ViewModel.MovePlayer(Direction.Left);
			}
			else if (e.Key == Key.D || e.Key == Key.Right)
			{
				ViewModel.MovePlayer(Direction.Right);
			}
			else  if (e.Key == Key.Space)
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
			var rsltMessageBox = MessageBox.Show("Sind Sie sicher, dass Sie das jetzige Spiel beenden möchten? Ihr Highscore wird gespeichert!", "Sind Sie sich sicher?", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

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