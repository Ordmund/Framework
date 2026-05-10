namespace Core.MVC
{
	public interface IControllerLifetimeRegistry
	{
		void Register(IController controller);
		void Release(IController controller);
		void ReleaseAll();
	}
}