using System;
using System.Collections.Generic;
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
		///     Konstruktor für die <see cref="AnimatedImageControl" /> Klasse, ruft <see cref="StartAnimation" /> mit den
		///     Parametern auf
		/// </summary>
		/// <param name="images">Die Bilder, welche an <see cref="StartAnimation" /> übergeben werden</param>
		/// <param name="interval">Der Intervall, welcher an <see cref="StartAnimation" /> übergeben wird</param>
		public AnimatedImageControl(IEnumerable<BitmapSource> images, TimeSpan interval) : this()
		{
			StartAnimation(images, interval);
		}

		/// <summary>
		///     Startet die Animation aller mitgegebenen Bilder
		/// </summary>
		/// <param name="images">Die Bilder, welche abgewechselt werden</param>
		/// <param name="interval">Die Zeit in Milisekunden, welche das jeweilige Bild angezeigt wird</param>
		public void StartAnimation(IEnumerable<BitmapSource> images, TimeSpan interval)
		{
			var storyboard = new Storyboard();
			var animation = new ObjectAnimationUsingKeyFrames();
			Storyboard.SetTarget(animation, Image);
			Storyboard.SetTargetProperty(animation, new PropertyPath("Source"));

			var currentInterval = TimeSpan.FromMilliseconds(0);
			foreach (var image in images)
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
			storyboard.AutoReverse = true;
			storyboard.Children.Add(animation);
			storyboard.Begin();
		}
	}
}