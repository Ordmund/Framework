using System.Collections.Generic;
using Framework.Binders;
using Framework.MVC.Pool;
using UnityEngine;
using Zenject;

namespace Framework.MVC
{
	public class MVCPoolBase<TController, TView, TModel> : IMVCPoolBase<TController, TView, TModel> 
		where TController : ControllerBase<TView, TModel>, IPoolable
		where TView : ViewBase, IPoolable
		where TModel : ModelBase
	{
		private readonly IMemoryPoolBinder _memoryPoolBinder;
		private readonly IGameObjectMVCFactory _gameObjectMVCFactory;

		private Stack<TController> _inactiveControllers;
		private MonoMemoryPool _memoryPool;
		private bool _isBound;

		protected MVCPoolBase(IMemoryPoolBinder memoryPoolBinder, IGameObjectMVCFactory gameObjectMVCFactory)
		{
			_memoryPoolBinder = memoryPoolBinder;
			_gameObjectMVCFactory = gameObjectMVCFactory;
		}

		public void BindPool(GameObject prefab, Transform parent, int initialSize)
		{
			_memoryPool = _memoryPoolBinder.Bind<TView, MonoMemoryPool>(prefab, initialSize, parent.transform);
			_inactiveControllers = new Stack<TController>(initialSize);

			_isBound = true;
		}

		public TController Spawn()
		{
			if (!_isBound)
			{
				WriteNotBoundLogError();
				return null;
			}

			TController controller;

			if (_inactiveControllers.Count > 0)
			{
				controller = _inactiveControllers.Pop();
			}
			else
			{
				var view = _memoryPool.Spawn();
				controller = _gameObjectMVCFactory.BindToView<TController, TView, TModel>(view);
			}

			controller.OnSpawned();

			return controller;
		}

		public void Despawn(TController controller)
		{
			if (!_isBound)
			{
				WriteNotBoundLogError();
				return;
			}

			controller.OnDespawned();

			_inactiveControllers.Push(controller);
		}

		private static void WriteNotBoundLogError()
		{
			Debug.LogError($"{nameof(MonoMemoryPool)} is not bound!");
		}

		private class MonoMemoryPool : MonoMemoryPool<TView>
		{
		}
	}
}