using UnityEngine;
using Zenject;

namespace Framework.Binders
{
	public interface IMemoryPoolBinder
	{
		TMemoryPool Bind<TPoolable, TMemoryPool>(GameObject prefab, int initialSize, Transform parent = null) where TPoolable : IPoolable where TMemoryPool : IMemoryPool;
	}
}