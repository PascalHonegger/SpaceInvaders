using System;
using System.Windows.Input;
using SpaceInvaders.Enums;

namespace SpaceInvaders
{
	/// <summary>
	///     Interaction logic for PlayFieldView.xaml
	/// </summary>
	public partial class PlayFieldView
	{
		private DateTime _lastKeyInput = DateTime.MinValue;
		private DateTime _lastShotFired = DateTime.MinValue;

		/// <summary>
		///     Constructor for PlayFieldView
		/// </summary>
		public PlayFieldView()
		{
			InitializeComponent();

			KeyDown += OnKeyDown;

			DataContext = new PlayFieldViewModel();
		}

		private PlayFieldViewModel ViewModel => DataContext as PlayFieldViewModel;

		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			if (ViewModel.GameOver) return;

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
					if (_lastShotFired <= DateTime.Now.AddSeconds(-1))
					{
						ViewModel.FireShotPlayer();
						_lastShotFired = DateTime.Now;
					}
					break;
			}
		}
	}
}