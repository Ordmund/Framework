using UnityEngine.AddressableAssets;

namespace Core.MVC
{
	public abstract class AddressableViewBase : ViewBase
	{
		public void OnDestroy()
		{
			Addressables.ReleaseInstance(gameObject);
		}
	}
}