using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.MVC
{
	public interface IGameObjectMVCFactory
	{
		UniTask<TController> InstantiateAndBindAsync<TController, TView, TModel>(string path = null, object id = null, Transform parent = null, CancellationToken token = default)
			where TController : ControllerBase<TView, TModel>
			where TView : AddressableViewBase
			where TModel : ModelBase;

		UniTask<TController> InstantiateAndBindAsync<TController, TView, TModel>(AssetReferenceGameObject assetReference, object id = null, Transform parent = null, CancellationToken token = default)
			where TController : ControllerBase<TView, TModel>
			where TView : AddressableViewBase
			where TModel : ModelBase;

		TController FindObjectAndBind<TController, TView, TModel>(object id = null)
			where TController : ControllerBase<TView, TModel>
			where TView : ViewBase
			where TModel : ModelBase;

		TController GetComponentAndBind<TController, TView, TModel>(GameObject gameObject, bool allowSearchInChildren, object id = null)
			where TController : ControllerBase<TView, TModel>
			where TView : ViewBase
			where TModel : ModelBase;

		TController BindToView<TController, TView, TModel>(TView view, object id = null)
			where TController : ControllerBase<TView, TModel>
			where TView : ViewBase
			where TModel : ModelBase;
	}
}