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
		///     Constructor for ShotControl
		/// </summary>
		public ShotControl(IShot datacontext)
		{
			InitializeComponent();

			DataContext = datacontext;
		}
	}
}