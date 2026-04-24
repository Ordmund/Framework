using System;
using Core.Managers.ScriptableObjects;
using Zenject;

namespace Core.MVC
{
	public class PrefabsPathProvider : IPrefabsPathProvider, IInitializable, IDisposable
	{
		private PrefabsPaths _prefabsPaths;

		public void Initialize()
		{
			_prefabsPaths = ScriptableObjectsManager.Load<PrefabsPaths>();
		}

		public string GetPathByViewType<T>() where T : ViewBase
		{
			return _prefabsPaths.GetPath<T>();
		}

		public void Dispose()
		{
			ScriptableObjectsManager.Unload(_prefabsPaths);
		}
	}
}