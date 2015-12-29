using System.Windows;
using System.Windows.Media.Imaging;
using SpaceInvaders.Enums;
using SpaceInvaders.Ship;

namespace SpaceInvaders.Shot
{
	/// <summary>
	///     Das Interface für alle Schüsse mit den dazugehörigen Eigenschaften
	/// </summary>
	public interface IShot
	{
		/// <summary>
		///     Die Grösse des Schussen in SpaceInvaders-Pixel
		/// </summary>
		Size Size { get; }

		/// <summary>
		///     Die Textur des Schusses, welche im View angezeigt wird
		/// </summary>
		BitmapSource Texture { get; }

		/// <summary>
		///     Der Schaden, welcher der Schuss beim Aufprall mit einem <see cref="IShip" /> verursacht
		/// </summary>
		double Damage { get; }

		/// <summary>
		///     Die Geschwindigkeit des Schusses. Wird in SpaceInvaders-Pixel / Tick angegeben
		/// </summary>
		int Speed { get; }

		/// <summary>
		///     Die Richtung, in welche sich der Schuss bewegt
		/// </summary>
		Direction Direction { get; }

		/// <summary>
		///     Die Position des Schusses in SpaceInvaders-Pixel
		/// </summary>
		Point Location { get; }

		/// <summary>
		///     Bewegt den Schuss in die <see cref="Direction" /> mit der Hilfe des <see cref="Speed" />
		/// </summary>
		void Move();
	}
}