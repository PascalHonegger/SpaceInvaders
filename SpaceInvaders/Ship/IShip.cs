using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;
using SpaceInvaders.Enums;
using SpaceInvaders.Shot;

namespace SpaceInvaders.Ship
{
	/// <summary>
	///     Das Interface für alle Schiffe
	/// </summary>
	public interface IShip
	{
		/// <summary>
		///     Der Schuss des Schiffes, welcher beim Schiessen geschossen wird
		/// </summary>
		IShot Shot { get; }

		/// <summary>
		///     Die Grösse des Schiffes in SpaceInvaders-Pixel für die Kollision
		/// </summary>
		Size Size { get; }

		/// <summary>
		///     Die Position des Schiffes in SpaceInvaders-Pixel
		/// </summary>
		Point Location { get; }

		/// <summary>
		///     Die Textures des Schiffes, welche im View angezeigt wird
		/// </summary>
		IEnumerable<BitmapSource> Textures { get; }

		/// <summary>
		///     Das Leben eines Schiffes. Wird bei einem Treffer reduziert. Unabhängig von den Respawns des Spielers!
		/// </summary>
		double Health { get; }

		/// <summary>
		///     Der Name des Schiffes. Beispielsweise 'The Destroyer'
		/// </summary>
		string Name { get; }

		/// <summary>
		///     Bewegt das Schiff in die gewünschte Richtung, indem es die <see cref="Location" /> verändert
		/// </summary>
		/// <param name="direction">Die Richtung, in welche sich das Schiff bewegt</param>
		void Move(Direction direction);

		/// <summary>
		///     Die totalen Respawns des Spielers
		/// </summary>
		int TotalLives { get; }

		/// <summary>
		///     Die Respawns des Spielers
		/// </summary>
		int CurrentLives { get; set; }

		/// <summary>
		///     Der Name des Menschen, welcher das Schiff steuert
		/// </summary>
		string PlayerName { get; set; }

		/// <summary>
		///     Die Punkte, welche dieses Schiff wert ist
		/// </summary>
		int Points { get; }

		/// <summary>
		/// Der Schifftyp, welcher darüber entscheided ob dieses Schiff durch den Spieler gelenkt wird
		/// </summary>
		ShipType ShipType { get; }
	}
}