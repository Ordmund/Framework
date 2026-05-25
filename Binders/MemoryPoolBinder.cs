using UnityEngine;
using Zenject;

namespace Framework.Binders
{
	public class MemoryPoolBinder : IMemoryPoolBinder
	{
		private readonly DiContainer _container;

		public MemoryPoolBinder(DiContainer container)
		{
			_container = container;
		}

		public TMemoryPool Bind<TPoolable, TMemoryPool>(GameObject prefab, int initialSize, Transform parent = null) where TPoolable : IPoolable where TMemoryPool : IMemoryPool
		{
			_container.BindMemoryPool<TPoolable, TMemoryPool>()
				.WithId(prefab.name)
				.WithInitialSize(initialSize)
				.FromComponentInNewPrefab(prefab)
				.UnderTransform(parent);

			return _container.ResolveId<TMemoryPool>(prefab.name);
		}
	}
}