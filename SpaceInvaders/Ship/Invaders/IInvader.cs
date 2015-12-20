using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Ship.Invaders
{
	/// <summary>
	/// 
	/// </summary>
	public interface IInvader : IShip
	{

		/// <summary>
		/// Die Punkte, welche beim Tod 
		/// </summary>
		int Points { get; }
	}
}
