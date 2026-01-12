using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Core.MVC
{
	public class GameObjectMVCFactory : IGameObjectMVCFactory
	{
		private readonly DiContainer _container;
		private readonly TickableManager _tickableManager;
		private readonly IPrefabsPathProvider _prefabsPathProvider;

		public GameObjectMVCFactory(DiContainer container, TickableManager tickableManager, IPrefabsPathProvider prefabsPathProvider)
		{
			_container = container;
			_tickableManager = tickableManager;
			_prefabsPathProvider = prefabsPathProvider;
		}

		public async Task<TController> InstantiateAndBindAsync<TController, TView, TModel>(string path = null, object id = null)
			where TController : BaseController<TView, TModel>
			where TView : BaseView
			where TModel : BaseModel
		{
			path ??= _prefabsPathProvider.GetPathByViewType<TView>();
			var asyncOperationHandle = Addressables.InstantiateAsync(path);
			var gameObject = await asyncOperationHandle.Task;

			var view = gameObject.GetComponent<TView>();
			var model = CreateModel<TModel>();
			var controller = BindAndResolve<TController, TView, TModel>(view, model, id);

			TryCallInitialize(controller);

			return controller;
		}

		public TController FindObjectAndBind<TController, TView, TModel>(object id = null)
			where TController : BaseController<TView, TModel>
			where TView : BaseView
			where TModel : BaseModel
		{
			var view = UnityEngine.Object.FindAnyObjectByType<TView>();
			if (view == null)
				throw new NullReferenceException($"No GameObject found with the {typeof(TView)} type.");

			var model = CreateModel<TModel>();
			var controller = BindAndResolve<TController, TView, TModel>(view, model, id);

			TryCallInitialize(controller);

			return controller;
		}

		public TController GetComponentAndBind<TController, TView, TModel>(GameObject gameObject, bool allowSearchInChildren, object id = null)
			where TController : BaseController<TView, TModel>
			where TView : BaseView
			where TModel : BaseModel
		{
			var view = allowSearchInChildren ? gameObject.GetComponentInChildren<TView>() : gameObject.GetComponent<TView>();

			if (view == null)
				throw new NullReferenceException($"{typeof(TView)} component not found on the {gameObject.name} GameObject.");

			var model = CreateModel<TModel>();
			var controller = BindAndResolve<TController, TView, TModel>(view, model, id);

			TryCallInitialize(controller);

			return controller;
		}

		public TController BindToView<TController, TView, TModel>(TView view, object id = null)
			where TController : BaseController<TView, TModel> where TView : BaseView where TModel : BaseModel
		{
			var model = CreateModel<TModel>();
			var controller = BindAndResolve<TController, TView, TModel>(view, model, id);

			TryCallInitialize(controller);

			return controller;
		}

		private TController BindAndResolve<TController, TView, TModel>(TView view, TModel model, object id = null)
		{
			TController controller;

			if (id != null)
			{
				_container.Bind<TController>().WithId(id).AsTransient().WithArguments(view, model).NonLazy();
				controller = _container.ResolveId<TController>(id);
			}
			else
			{
				_container.BindInterfacesAndSelfTo<TController>().AsSingle().WithArguments(view, model).NonLazy();
				controller = _container.Resolve<TController>();
			}

			TryRegisterTickable(controller);

			return controller;
		}

		private static void TryCallInitialize<TController>(TController controller)
		{
			if (controller is IInitializable initializable)
			{
				initializable.Initialize();
			}
		}

		private void TryRegisterTickable<TController>(TController controller)
		{
			if (controller is ITickable tickable)
			{
				_tickableManager.Add(tickable);
			}

			if (controller is IFixedTickable fixedTickable)
			{
				_tickableManager.AddFixed(fixedTickable);
			}

			if (controller is ILateTickable lateTickable)
			{
				_tickableManager.AddLate(lateTickable);
			}
		}

		private static TModel CreateModel<TModel>() where TModel : BaseModel
		{
			return Activator.CreateInstance<TModel>();
		}
	}
}