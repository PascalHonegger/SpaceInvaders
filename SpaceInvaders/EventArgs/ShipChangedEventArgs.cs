using SpaceInvaders.Ship;

namespace SpaceInvaders.EventArgs
{
	/// <summary>
	///     Die Eventargumente, wenn ein <see cref="IShip" /> seine <see cref="IShip.Location" /> geändert hat oder er getötet
	///     wurde
	/// </summary>
	public class ShipChangedEventArgs : System.EventArgs
	{
		/// <summary>
		///     Konstruktor für die <see cref="ShipChangedEventArgs" /> Klasse
		/// </summary>
		/// <param name="ship">Das <see cref="Ship" /></param>
		/// <param name="killed">Der <see cref="Killed" />-Status</param>
		public ShipChangedEventArgs(IShip ship, bool killed)
		{
			Ship = ship;
			Killed = killed;
		}

		/// <summary>
		///     Das <see cref="IShip" />, welches sich geändert hat
		/// </summary>
		public IShip Ship { get; private set; }

		/// <summary>
		///     True, wenn das <see cref="Ship" /> getötet wurde
		/// </summary>
		public bool Killed { get; private set; }
	}
}