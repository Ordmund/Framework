namespace Core.MVC
{
	public interface IControllerRegistry
	{
		void Register(IController controller);
		void Release(IController controller);
		void ReleaseAll();
	}
}