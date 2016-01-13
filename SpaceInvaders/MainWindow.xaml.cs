using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using SpaceInvaders.Control;
using SpaceInvaders.Enums;
using SpaceInvaders.Ship;
using SpaceInvaders.Ship.Players;

namespace SpaceInvaders
{
	/// <summary>
	///     Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		/// <summary>
		///     Constructor for MainWindow
		/// </summary>
		public MainWindow()
		{
			InitializeComponent();

			DataContext = new SpaceInvadersViewModel
			{
				Player = new DefaultPlayer(new Point())
			};

			ViewModel.ShipChangedEventHandler += (sender, e) =>
			{
				var control = ViewModel.ShipWithControls.First(kvp => kvp.Key.Equals(e.Ship)).Value;

				if (PlayArea51.Children.Contains(control))
				{
					Animate(e.Ship, control);
				}
				else
				{
					PlayArea51.Children.Add(control);
				}
			};
		}

		private SpaceInvadersViewModel ViewModel => DataContext as SpaceInvadersViewModel;

		/// <summary>
		/// Animiert das Schiff an die neue Position
		/// </summary>
		/// <param name="ship">Das zu animierende <see cref="IShip"/></param>
		private void Animate(IShip ship, ShipControl control)
		{
			var moveInvaders = new Storyboard();

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
			moveInvaders.Begin();
			//TODO control.StartAnimation();
		}
		private void UIElement_OnKeyDown(object sender, KeyEventArgs e)
		{
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