using UnityEngine;
using Zenject;

namespace Core.Binders
{
	public interface IMemoryPoolBinder
	{
		TMemoryPool Bind<TPoolable, TMemoryPool>(GameObject prefab, int initialSize, Transform parent = null) where TPoolable : IPoolable where TMemoryPool : IMemoryPool;
	}
}