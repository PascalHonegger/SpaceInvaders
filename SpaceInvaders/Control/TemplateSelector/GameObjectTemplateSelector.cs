using System.Windows;
using System.Windows.Controls;
using SpaceInvaders.Ship;
using SpaceInvaders.Shot;

namespace SpaceInvaders.Control.TemplateSelector
{
	public class GameObjectTemplateSelector : DataTemplateSelector
	{
		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			if (container is FrameworkElement element)
			{
				if (item is IShip)
				{
					return element.FindResource("ShipDataTemplate") as DataTemplate;
				}

				if (item is IShot)
				{
					return element.FindResource("ShotDataTemplate") as DataTemplate;
				}
			}
			
			return null;
		}
	}
}
