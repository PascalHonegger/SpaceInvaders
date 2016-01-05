using SpaceInvaders.Shot;

namespace SpaceInvaders.EventArgs
{
	/// <summary>
	///     Die Eventargumente, wenn ein <see cref="IShot" /> seine <see cref="IShot.Rect" /> geändert hat oder entweder
	///     eine Wand oder ein anderes Schiff getroffen hat
	///     wurde
	/// </summary>
	public class ShotMovedEventArgs : System.EventArgs
	{
		/// <summary>
		///     Konstruktor für die <see cref="ShipChangedEventArgs" /> Klasse
		/// </summary>
		/// <param name="shot">Der <see cref="Shot" /></param>
		/// <param name="disappeared">Der <see cref="Disappeared" />-Status</param>
		public ShotMovedEventArgs(IShot shot, bool disappeared)
		{
			Shot = shot;
			Disappeared = disappeared;
		}

		/// <summary>
		///     Der <see cref="IShot " />, welcher sich geändert hat
		/// </summary>
		public IShot Shot { get; private set; }

		/// <summary>
		///     True, wenn der <see cref="Shot" /> entweder eine Wand oder ein anderes Schiff getroffen hat
		/// </summary>
		public bool Disappeared { get; private set; }
	}
}