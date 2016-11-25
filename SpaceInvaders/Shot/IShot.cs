using System.ComponentModel;
using System.Windows;
using SpaceInvaders.Enums;
using SpaceInvaders.Ship;

namespace SpaceInvaders.Shot
{
	/// <summary>
	///     Das Interface für alle Schüsse mit den dazugehörigen Eigenschaften
	/// </summary>
	public interface IShot : INotifyPropertyChanged
	{
		/// <summary>
		///     Der Schaden, welcher der Schuss beim Aufprall mit einem <see cref="IShip" /> verursacht
		/// </summary>
		double Damage { get; }

		/// <summary>
		///     Die Location <see cref="Point" /> (top-left corner) und die Grösse <see cref="Size" /> des Schiffes in
		///     SpaceInvaders-Pixel
		/// </summary>
		Rect Rect { get; }

		/// <summary>
		///     Bewegt den Schuss in die <see cref="Direction" />
		/// </summary>
		void Move();

		/// <summary>
		///     Sagt dem Schuss sich zu aktualisieren
		/// </summary>
		void Update();
	}
}