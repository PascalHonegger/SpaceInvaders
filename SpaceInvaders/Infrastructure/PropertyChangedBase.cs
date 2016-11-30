using System.ComponentModel;
using System.Runtime.CompilerServices;
using SpaceInvaders.Annotations;

namespace SpaceInvaders.Infrastructure
{
	/// <summary>
	///     Basisimplementation für <see cref="INotifyPropertyChanged"/>
	/// </summary>
	public class PropertyChangedBase : INotifyPropertyChanged
	{
		/// <summary>
		///     <see cref="INotifyPropertyChanged.PropertyChanged"/>
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		///     Event invocator for <see cref="PropertyChanged"/>
		/// </summary>
		/// <param name="propertyName"></param>
		[NotifyPropertyChangedInvocator]
		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
