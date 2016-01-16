using SpaceInvaders.Ship;

namespace SpaceInvaders.Control
{
	/// <summary>
	///     Interaction logic for ShipControl.xaml
	/// </summary>
	public partial class ShipControl
	{
		/// <summary>
		///     Constructor for ShipControl
		/// </summary>
		public ShipControl(IShip datacontext)
		{
			InitializeComponent();

			DataContext = datacontext;
		}
	}
}