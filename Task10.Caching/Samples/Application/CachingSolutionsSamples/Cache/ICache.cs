using System.Collections.Generic;

namespace CachingSolutionsSamples.Cache
{
	public interface ICache<T>
	{
		IEnumerable<T> Get(string forUser);

		void Set(string forUser, IEnumerable<T> entities);
	}
}
