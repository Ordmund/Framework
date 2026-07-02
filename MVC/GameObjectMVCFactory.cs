using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;
using Object = UnityEngine.Object;

namespace Framework.MVC
{
	public class GameObjectMVCFactory : IGameObjectMVCFactory
	{
		private readonly DiContainer _container;
		private readonly IPrefabsPathProvider _prefabsPathProvider;

		public GameObjectMVCFactory(DiContainer container, IPrefabsPathProvider prefabsPathProvider)
		{
			_container = container;
			_prefabsPathProvider = prefabsPathProvider;
		}

		public async UniTask<TController> InstantiateAndBindAsync<TController, TView, TModel>(string path = null, Transform parent = null, object[] extraArgs = null, CancellationToken token = default)
			where TController : ControllerBase<TView, TModel>
			where TView : AddressableViewBase
			where TModel : ModelBase
		{
			path ??= _prefabsPathProvider.GetPathByViewType<TView>();
			var asyncOperationHandle = Addressables.InstantiateAsync(path, parent);
			var gameObject = await AwaitSafely(asyncOperationHandle, token);

			var view = gameObject.GetComponent<TView>();
			var model = CreateModel<TModel>();
			var controller = CreateAndInitialize<TController, TView, TModel>(view, model, extraArgs);

			return controller;
		}

		public async UniTask<TController> InstantiateAndBindAsync<TController, TView, TModel>(AssetReferenceGameObject assetReference, Transform parent = null, object[] extraArgs = null,
			CancellationToken token = default)
			where TController : ControllerBase<TView, TModel>
			where TView : AddressableViewBase
			where TModel : ModelBase
		{
			if (!assetReference.IsValid())
			{
				var asyncOperationHandle = assetReference.LoadAssetAsync();

				await AwaitSafely(asyncOperationHandle, token);
			}

			var prefab = assetReference.Asset as GameObject;
			var gameObject = Object.Instantiate(prefab, parent);

			var view = gameObject.GetComponent<TView>();
			var model = CreateModel<TModel>();
			var controller = CreateAndInitialize<TController, TView, TModel>(view, model, extraArgs);

			return controller;
		}

		public TController FindObjectAndBind<TController, TView, TModel>(object[] extraArgs = null)
			where TController : ControllerBase<TView, TModel>
			where TView : ViewBase
			where TModel : ModelBase
		{
			var view = Object.FindAnyObjectByType<TView>();
			if (view == null)
				throw new NullReferenceException($"No GameObject found with the {typeof(TView)} type.");

			var model = CreateModel<TModel>();
			var controller = CreateAndInitialize<TController, TView, TModel>(view, model, extraArgs);

			return controller;
		}

		public TController GetComponentAndBind<TController, TView, TModel>(GameObject gameObject, bool allowSearchInChildren, object[] extraArgs = null)
			where TController : ControllerBase<TView, TModel>
			where TView : ViewBase
			where TModel : ModelBase
		{
			var view = allowSearchInChildren ? gameObject.GetComponentInChildren<TView>() : gameObject.GetComponent<TView>();

			if (view == null)
				throw new NullReferenceException($"{typeof(TView)} component not found on the {gameObject.name} GameObject.");

			var model = CreateModel<TModel>();
			var controller = CreateAndInitialize<TController, TView, TModel>(view, model, extraArgs);

			return controller;
		}

		public TController BindToView<TController, TView, TModel>(TView view, object[] extraArgs = null)
			where TController : ControllerBase<TView, TModel> 
			where TView : ViewBase 
			where TModel : ModelBase
		{
			var model = CreateModel<TModel>();
			var controller = CreateAndInitialize<TController, TView, TModel>(view, model, extraArgs);

			return controller;
		}

		private TController CreateAndInitialize<TController, TView, TModel>(TView view, TModel model, object[] extraArgs = null)
		{
			var baseArgs = new object[] { view, model };
			var args = extraArgs != null ? baseArgs.Concat(extraArgs).ToArray() : baseArgs;

			var controller = _container.Instantiate<TController>(args);

			TryCallInitialize(controller);

			return controller;
		}

		private static void TryCallInitialize<TController>(TController controller)
		{
			if (controller is IInitializable initializable)
			{
				initializable.Initialize();
			}
		}

		private static async UniTask<TObject> AwaitSafely<TObject>(AsyncOperationHandle<TObject> asyncOperationHandle, CancellationToken token)
		{
			try
			{
				return await asyncOperationHandle.ToUniTask(cancellationToken: token);
			}
			catch (Exception)
			{
				Addressables.Release(asyncOperationHandle);
				throw;
			}
		}

		private static TModel CreateModel<TModel>() where TModel : ModelBase
		{
			return Activator.CreateInstance<TModel>();
		}
	}
}