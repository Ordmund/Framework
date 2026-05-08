namespace Core.MVC
{
	public interface IModelLoader<out TModel>
	{
		TModel LoadModel();
	}
}