﻿using System.Collections.Generic;
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
		///	Die Location <see cref="Point"/> (top-left corner) und die Grösse <see cref="Size"/> des Schiffes in SpaceInvaders-Pixel
		/// </summary>
		Rect Rect { get; }

		/// <summary>
		///     Die Textures des Schiffes, welche im View angezeigt wird
		/// </summary>
		IEnumerable<BitmapSource> Textures { get; }

		/// <summary>
		///     Der Name des Schiffes. Beispielsweise 'The Destroyer'
		/// </summary>
		string Name { get; }

		/// <summary>
		///     Bewegt das Schiff in die gewünschte Richtung, indem es den <see cref="Rect" /> verändert
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
		///     Der Schifftyp, welcher darüber entscheided ob dieses Schiff durch den Spieler gelenkt wird
		/// </summary>
		ShipType ShipType { get; }

		/// <summary>
		///     Das Leben eines Schiffes. Wird bei einem Treffer reduziert. Unabhängig von den Respawns des Spielers!
		/// </summary>
		double Health { get; }
	}
}