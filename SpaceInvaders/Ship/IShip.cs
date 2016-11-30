using System.ComponentModel;
using System.Windows;
using SpaceInvaders.Enums;
using SpaceInvaders.Infrastructure;
using SpaceInvaders.Shot;

namespace SpaceInvaders.Ship
{
	/// <summary>
	///     Das Interface für alle Schiffe
	/// </summary>
	public interface IShip : IGameObject, INotifyPropertyChanged
	{
		/// <summary>
		///     Der Schuss des Schiffes, welcher beim Schiessen geschossen wird
		/// </summary>
		IShot Shot { get; }

		/// <summary>
		///     Der Name des Schiffes. Beispielsweise 'The Destroyer'
		/// </summary>
		string Name { get; }

		/// <summary>
		///     Die Respawns des Spielers
		/// </summary>
		int Lives { get; }

		/// <summary>
		///     Die Punkte, welche dieses Schiff wert ist
		/// </summary>
		int Points { get; }

		/// <summary>
		///     Der Schifftyp, welcher darüber entscheided ob dieses Schiff durch den Spieler gelenkt wird
		/// </summary>
		ShipType ShipType { get; }

		/// <summary>
		///     Das Leben eines Schiffes. Wird bei einem Treffer reduziert. Unabhängig von den Respawns des Spielers!
		/// </summary>
		double Health { get; set; }

		/// <summary>
		///     Das maximale Leben eines Schiffes.
		/// </summary>
		double MaxHealth { get; }

		/// <summary>
		///     Bewegt das Schiff in die gewünschte Richtung, indem es den <see cref="Rect" /> verändert
		/// </summary>
		/// <param name="direction">Die Richtung, in welche sich das Schiff bewegt</param>
		void Move(Direction direction);


		/// <summary>
		///     Wird aufgerufen, wenn sich das Schiff aktualisieren sollte
		/// </summary>
		void Update();
	}
}