using System.Windows;

namespace SpaceInvaders.Infrastructure
{
	public interface IGameObject
	{
		/// <summary>
		///     Die Location <see cref="Point" /> (top-left corner) und die Grösse <see cref="Size" /> des Objektes in
		///     SpaceInvaders-Pixel
		/// </summary>
		Rect Rect { get; }

		double X { get; set; }
		double Y { get; set; }
	}
}
