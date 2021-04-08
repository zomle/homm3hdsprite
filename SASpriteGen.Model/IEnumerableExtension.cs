using System;
using System.Collections.Generic;

namespace SASpriteGen.Model
{
	public static class IEnumerableExtension
	{
		public static void ForAll<T>(this IEnumerable<T> collection, Action<T> action)
		{
			foreach (var item in collection)
			{
				action(item);
			}
		}
	}
}
