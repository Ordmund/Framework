using UnityEngine.AddressableAssets;

namespace Core.MVC
{
	public abstract class BaseAddressableView : BaseView
	{
		public void OnDestroy()
		{
			Addressables.ReleaseInstance(gameObject);
		}
	}
}