using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
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

			ViewModel.ShipChangedEventHandler += (sender, e) => Animationes(e.Ship);
		}

		private SpaceInvadersViewModel ViewModel => DataContext as SpaceInvadersViewModel;

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="ship"></param>
		public void Animationes(IShip ship)
		{
			Storyboard moveInvaders = new Storyboard();
			var control = ViewModel.InvadersWithControls.First(kvp => kvp.Key.Equals(ship)).Value;

			if (control == null) return;
			if (!PlayArea51.Children.Contains(control))
			{
				PlayArea51.Children.Add(control);
			}
			DoubleAnimation moveAnimationX = new DoubleAnimation();
			moveAnimationX.Duration = new Duration(new TimeSpan(0, 0, 1));
			moveAnimationX.To = ship.Rect.Location.X;
			moveAnimationX.AutoReverse = false;

			DoubleAnimation moveAnimationY = new DoubleAnimation();
			moveAnimationY.Duration = new Duration(new TimeSpan(0, 0, 1));
			moveAnimationY.To = ship.Rect.Location.Y;
			moveAnimationY.AutoReverse = false;

			Storyboard.SetTarget(moveAnimationY, control);
			Storyboard.SetTargetProperty(moveAnimationY, new PropertyPath(Canvas.TopProperty));
			Storyboard.SetTarget(moveAnimationX, control);
			Storyboard.SetTargetProperty(moveAnimationX, new PropertyPath(Canvas.LeftProperty));
			moveInvaders.Children.Add(moveAnimationX);
			moveInvaders.Children.Add(moveAnimationY);
			moveInvaders.Begin();
			control.startAnimation();


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