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
		public ShipControl()
		{
			InitializeComponent();
		}

		/// <summary>
		///     Constructor for ShipControl
		/// </summary>
		public ShipControl(IShip datacontext) : this()
		{
			DataContext = datacontext;
		}
	}
}