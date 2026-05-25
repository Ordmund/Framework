namespace Framework.MVC
{
	public interface IPrefabsPathProvider
	{
		string GetPathByViewType<T>() where T : ViewBase;
	}
}