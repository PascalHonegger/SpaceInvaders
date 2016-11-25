using System;
using System.Windows;

namespace SpaceInvaders.Ship.Invaders
{
	public class WaveInformation
	{
		public int WaveColumns { get; private set; }
		public int WaveRows { get; private set; }
		public Func<Point, IShip> CreateShip { get; private set; }

		public WaveInformation(int waveColumns, int waveRows, Func<Point, IShip> createShip)
		{
			WaveColumns = waveColumns;
			WaveRows = waveRows;
			CreateShip = createShip;
		}
	}
}
