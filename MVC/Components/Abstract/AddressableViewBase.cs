using UnityEngine.AddressableAssets;

namespace Framework.MVC
{
	public abstract class AddressableViewBase : ViewBase
	{
		public void OnDestroy()
		{
			Addressables.ReleaseInstance(gameObject);
		}
	}
}