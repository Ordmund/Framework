namespace Framework.Managers.Injectable
{
	public interface IObjectReleaser
	{
		void Release(object instance);
		void UnsubscribeFromTickable(object instance);
		void Dispose(object instance);
		void LateDispose(object instance);
	}
}