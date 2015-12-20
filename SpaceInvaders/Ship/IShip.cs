using System.Windows;
using System.Windows.Media.Imaging;
using SpaceInvaders.Shot;

namespace SpaceInvaders.Ship
{
	/// <summary>
	/// Das Interface für alle Schiffe
	/// </summary>
	public interface IShip
	{
		/// <summary>
		/// Der Schuss des Schiffes, welcher beim Schiessen geschossen wird
		/// </summary>
		IShot Shot { get; }

		/// <summary>
		/// Die Grösse des Schiffes in SpaceInvaders-Pixel
		/// </summary>
		Size Size { get; }

		/// <summary>
		/// Die Position des Schiffes in SpaceInvaders-Pixel
		/// </summary>
		Point Location { get; }

		/// <summary>
		/// Die Texture des Schiffes, welche im View angezeigt wird
		/// </summary>
		BitmapSource Texture { get; }

		/// <summary>
		/// Das Leben eines Schiffes. Wird bei einem Treffer reduziert. Unabhängig von den Respawns des Spielers!
		/// </summary>
		double Health { get; }

		/// <summary>
		/// Der Name des Schiffes. Beispielsweise 'The Destroyer'
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Bewegt das Schiff in die gewünschte Richtung, indem es den <see cref="Location"/> verändert
		/// </summary>
		/// <param name="direction">Die Richtung, in welche sich das Schiff bewegt</param>
		void Move(Direction direction);

		/// <summary>
		/// Die Geschwindigkeit, mit welcher das Schiff sich vortbewegt. Wird in SpaceInvaders-Pixel / Tick angegeben
		/// </summary>
		int Speed { get; }
	}
}