using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceInvaders.ExtensionMethods
{
	/// <summary>
	///     Alle ExtensionMethods im Zusammenhang mit <see cref="IEnumerable{T}" />
	/// </summary>
	public static class ExtensionMethodsIEnumerable
	{
		/// <summary>
		///     Wähle einen zufälligen Eintrag aus einer Liste
		/// </summary>
		/// <param name="source">Die Liste, von welcher ein Eintrag ausgewählt wird</param>
		/// <typeparam name="T">Irrelevant für die Selektion</typeparam>
		/// <returns>Einen zufälligen Eintrag mit Hilfe von <see cref="PickRandom{T}(IEnumerable{T})" /></returns>
		public static T PickRandom<T>(this IEnumerable<T> source)
		{
			return source.PickRandom(1).Single();
		}

		/// <summary>
		///     Wähle eine zufällige Selektion von der mitgegebenen <see cref="IEnumerable{T}" /> aus
		/// </summary>
		/// <param name="source">Die Liste, von welcher eine zufällige Selektion ausgewählt wird</param>
		/// <param name="count">Die anzahl Objekte, welche zurückgegeben werden</param>
		/// <typeparam name="T">Irrelevant für die Selektion</typeparam>
		/// <returns></returns>
		public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
		{
			return source.Shuffle().Take(count);
		}

		/// <summary>
		///     Sortiert die mitegebene <see cref="IEnumerable{T}" /> nach einer neu generierten <see cref="Guid" />
		/// </summary>
		/// <param name="source">Die Liste, welche zufällig sortiert wir.</param>
		/// <typeparam name="T">Irrelevant für die zufällige Sortierung</typeparam>
		/// <returns>Die Liste mit den selben Werten in einer zufälligen Reihenfolge</returns>
		public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
		{
			return source.OrderBy(x => Guid.NewGuid());
		}
	}
}