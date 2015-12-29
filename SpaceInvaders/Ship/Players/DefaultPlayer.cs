using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;
using SpaceInvaders.Enums;
using SpaceInvaders.Shot;

namespace SpaceInvaders.Ship.Players
{
	class DefaultPlayer : ShipBase
	{
		public override IShot Shot => new DefaultShot(Location, Direction.Up);
		public override Size Size { get; } = new Size(10, 20);
		public override Point Location { get; protected set; }
		public override IEnumerable<BitmapSource> Textures { get; }
		public override double Health { get; } = 1;
		public override string Name { get; }
		public override int TotalLives { get; } = 3;
		public override int CurrentLives { get; set; }
		public override string PlayerName { get; set; } = string.Empty;
		public override int Points { get; } = 0;
		public override ShipType ShipType { get; } = ShipType.Player;
		public override int Speed { get; } = 30;
	}
}
