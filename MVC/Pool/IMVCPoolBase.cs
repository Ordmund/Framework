using UnityEngine;
using Zenject;

namespace Framework.MVC.Pool
{
	public interface IMVCPoolBase<TController, TView, TModel>
		where TController : ControllerBase<TView, TModel>, IPoolable
		where TView : ViewBase, IPoolable
		where TModel : ModelBase
	{
		void BindPool(GameObject prefab, Transform parent, int initialSize);
		TController Spawn();
		void Despawn(TController controller);
	}
}