using System;
using SpaceInvaders.Ship;
using SpaceInvaders.Shot;

namespace SpaceInvaders.Control
{
	/// <summary>
	///     Interaction logic for ShipControl.xaml
	/// </summary>
	public partial class ShotControl
	{
		/// <summary>
		///     Constructor for MainWindow
		/// </summary>
		public ShotControl()
		{
			InitializeComponent();

			var shot = DataContext as IShot;

			AnimatedImageControl.StartAnimation(shot?.Textures, TimeSpan.FromSeconds(1));
		}

		/// <summary>
		///     Constructor for MainWindow
		/// </summary>
		public ShotControl(IShot datacontext)
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