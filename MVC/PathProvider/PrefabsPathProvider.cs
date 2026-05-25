using System;
using Framework.Managers.ScriptableObjects;
using Zenject;

namespace Framework.MVC
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