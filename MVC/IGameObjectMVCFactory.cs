using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Framework.MVC
{
	public interface IGameObjectMVCFactory
	{
		UniTask<TController> InstantiateAndBindAsync<TController, TView, TModel>(string path = null, Transform parent = null, object[] extraArgs = null, CancellationToken token = default)
			where TController : ControllerBase<TView, TModel>
			where TView : AddressableViewBase
			where TModel : ModelBase;

		UniTask<TController> InstantiateAndBindAsync<TController, TView, TModel>(AssetReferenceGameObject assetReference, Transform parent = null, object[] extraArgs = null,
			CancellationToken token = default)
			where TController : ControllerBase<TView, TModel>
			where TView : AddressableViewBase
			where TModel : ModelBase;

		TController FindObjectAndBind<TController, TView, TModel>(object[] extraArgs = null)
			where TController : ControllerBase<TView, TModel>
			where TView : ViewBase
			where TModel : ModelBase;

		TController GetComponentAndBind<TController, TView, TModel>(GameObject gameObject, bool allowSearchInChildren, object[] extraArgs = null)
			where TController : ControllerBase<TView, TModel>
			where TView : ViewBase
			where TModel : ModelBase;

		TController BindToView<TController, TView, TModel>(TView view, object[] extraArgs = null)
			where TController : ControllerBase<TView, TModel>
			where TView : ViewBase
			where TModel : ModelBase;
	}
}