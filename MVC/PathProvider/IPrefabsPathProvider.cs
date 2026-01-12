namespace Core.MVC
{
	public interface IPrefabsPathProvider
	{
		string GetPathByViewType<T>() where T : BaseView;
	}
}