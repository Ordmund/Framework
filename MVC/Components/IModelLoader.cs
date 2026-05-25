namespace Framework.MVC
{
	public interface IModelLoader<out TModel>
	{
		TModel LoadModel();
	}
}