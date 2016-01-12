using System;
using System.Windows.Controls;
using SpaceInvaders.Ship;

namespace SpaceInvaders.Control
{
	/// <summary>
	///     Interaction logic for ShipControl.xaml
	/// </summary>
	public partial class ShipControl : UserControl
	{
		/// <summary>
		///     Constructor for MainWindow
		/// </summary>
		public ShipControl()
		{
			InitializeComponent();

			var ship = DataContext as IShip;

			AnimatedImageControl.StartAnimation(ship?.Textures, TimeSpan.FromSeconds(1));
		}

		/// <summary>
		///     Constructor for MainWindow
		/// </summary>
		public ShipControl(IShip datacontext)
		{
			InitializeComponent();

			DataContext = datacontext;

			AnimatedImageControl.StartAnimation(datacontext.Textures, TimeSpan.FromSeconds(1));
		}

		internal void StartAnimation()
		{
			AnimatedImageControl.StartAnimation((DataContext as IShip)?.Textures, TimeSpan.Zero);
		}
	}
}