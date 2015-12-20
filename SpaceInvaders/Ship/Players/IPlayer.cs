namespace SpaceInvaders.Ship.Player
{
	/// <summary>
	/// Das Interface für alle Spieler (Durch Menschen gesteuerte Schiffe)
	/// </summary>
	public interface IPlayer : IShip
	{
		/// <summary>
		/// Die Respawns des Spielers
		/// </summary>
		int Lives { get; }
	}
}
