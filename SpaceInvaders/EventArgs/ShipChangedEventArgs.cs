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
		/// <param name="gotShot">Der <see cref="GotShot" />-Status</param>
		public ShipChangedEventArgs(IShip ship, bool gotShot)
		{
			Ship = ship;
			GotShot = gotShot;
		}

		/// <summary>
		///     Das <see cref="IShip" />, welches sich geändert hat
		/// </summary>
		public IShip Ship { get; private set; }

		/// <summary>
		///     True, wenn das <see cref="Ship" /> getötet wurde
		/// </summary>
		public bool GotShot { get; private set; }
	}
}