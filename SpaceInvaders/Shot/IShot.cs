using System.Windows;
using System.Windows.Media.Imaging;

namespace SpaceInvaders.Shot
{
	/// <summary>
	/// Das Interface für alle Schüsse mit den dazugehörigen Eigenschaften
	/// </summary>
	public interface IShot
	{
		/// <summary>
		/// Die Grösse des Schussen in SpaceInvaders-Pixel
		/// </summary>
		Size Size { get; }

		/// <summary>
		/// Die Texture des Schusses, welche im View angezeigt wird
		/// </summary>
		BitmapSource Texture { get; }
	
		/// <summary>
		/// Der Schaden, welcher der Schuss beim Aufprall verursacht
		/// </summary>
		double Damage { get; }

		/// <summary>
		/// Die Geschwindigkeit, mit welcher der Schuss sich vortbewegt. Wird in SpaceInvaders-Pixel / Tick angegeben
		/// </summary>
		int Speed { get; }
	}
}