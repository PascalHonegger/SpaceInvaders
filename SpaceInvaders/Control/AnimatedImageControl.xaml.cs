using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace SpaceInvaders.Control
{
	/// <summary>
	///     Interaction logic for AnimatedImageControl.xaml
	/// </summary>
	public partial class AnimatedImageControl
	{
		/// <summary>
		///     Konstruktor für die <see cref="AnimatedImageControl" /> Klasse
		/// </summary>
		public AnimatedImageControl()
		{
			InitializeComponent();
		}

		/// <summary>
		///     Startet die Animation aller mitgegebenen Bilder
		/// </summary>
		/// <param name="images">Die Bilder, welche abgewechselt werden</param>
		/// <param name="interval">Die Zeit in Milisekunden, welche das jeweilige Bild angezeigt wird</param>
		public void StartAnimation(IEnumerable<BitmapSource> images, TimeSpan interval)
		{
			var bitmapSources = images as IList<BitmapSource> ?? images.ToList();
			if (!bitmapSources.Any())
			{
				return;
			}

			bitmapSources.Add(bitmapSources.First());

			var storyboard = new Storyboard();
			var animation = new ObjectAnimationUsingKeyFrames();
			Storyboard.SetTarget(animation, Image);
			Storyboard.SetTargetProperty(animation, new PropertyPath("Source"));

			var currentInterval = TimeSpan.Zero;
			foreach (var image in bitmapSources)
			{
				var keyFrame = new DiscreteObjectKeyFrame
				{
					Value = image,
					KeyTime = currentInterval
				};
				animation.KeyFrames.Add(keyFrame);
				currentInterval = currentInterval.Add(interval);
			}
			storyboard.RepeatBehavior = RepeatBehavior.Forever;
			//TODO maybe true / fale
			storyboard.AutoReverse = true;
			storyboard.Children.Add(animation);
			storyboard.Begin();
		}
	}
}