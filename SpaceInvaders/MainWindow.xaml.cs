using System.Windows;
using System.Windows.Input;
using SpaceInvaders.Enums;
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
		}

		private SpaceInvadersViewModel ViewModel => DataContext as SpaceInvadersViewModel;

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

		private void button_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.StartGame();
		}
	}
}