using SpaceInvaders.Ship;

namespace SpaceInvaders.EventArgs
{
	/// <summary>
	///     Die Eventargumente, wenn ein <see cref="IShip" /> seine <see cref="IShip.Rect" /> geändert hat oder er getötet
	///     wurde
	/// </summary>
	public class ShipChangedEventArgs : System.EventArgs
	{
		/// <summary>
		///     Konstruktor für die <see cref="ShipChangedEventArgs" /> Klasse
		/// </summary>
		/// <param name="ship">Das <see cref="Ship" /></param>
		public ShipChangedEventArgs(IShip ship)
		{
			Ship = ship;
		}

		/// <summary>
		///     Das <see cref="IShip" />, welches sich geändert hat
		/// </summary>
		public IShip Ship { get; private set; }
	}
}