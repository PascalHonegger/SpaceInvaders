using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;
using SpaceInvaders.Enums;
using SpaceInvaders.Shot;

namespace SpaceInvaders.Ship.Invaders
{
	internal class Ufo : ShipBase
	{
		/// <summary>
		///     Der Schuss des Schiffes, welcher beim Schiessen geschossen wird
		/// </summary>
		public override IShot Shot => new DefaultShot(Location, Direction.Down);

		/// <summary>
		///     Die Grösse des Schiffes in SpaceInvaders-Pixel
		/// </summary>
		public override Size Size { get; } = new Size(20, 10);

		/// <summary>
		///     Die Position des Schiffes in SpaceInvaders-Pixel
		/// </summary>
		public override Point Location { get; protected set; }

		/// <summary>
		///     Die Textures des Schiffes, welche im View angezeigt wird
		/// </summary>
		public override IEnumerable<BitmapSource> Textures { get; }

		/// <summary>
		///     Das Leben eines Schiffes. Wird bei einem Treffer reduziert. Unabhängig von den Respawns des Spielers!
		/// </summary>
		public override double Health { get; } = 30;

		/// <summary>
		///     Der Name des Schiffes. Beispielsweise 'The Destroyer'
		/// </summary>
		public override string Name { get; } = "Ufo of Doom";

		/// <summary>
		///     Die totalen Respawns des Spielers
		/// </summary>
		public override int TotalLives { get; } = 0;

		/// <summary>
		///     Die Respawns des Spielers
		/// </summary>
		public override int CurrentLives { get; set; } = 0;

		/// <summary>
		///     Der Name des Menschen, welcher das Schiff steuert
		/// </summary>
		public override string PlayerName { get; set; } = string.Empty;

		/// <summary>
		///     Die Punkte, welche beim Tod
		/// </summary>
		public override int Points { get; } = 50;

		/// <summary>
		///     Der Schifftyp, welcher darüber entscheided ob dieses Schiff durch den Spieler gelenkt wird
		/// </summary>
		public override ShipType ShipType { get; } = ShipType.Invader;

		/// <summary>
		///     Die Geschwindigkeit, mit welcher das Schiff sich vortbewegt. Wird in SpaceInvaders-Pixel / Tick angegeben
		/// </summary>
		public override int Speed { get; } = 30;
	}
}