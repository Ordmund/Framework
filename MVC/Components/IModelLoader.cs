namespace Core.MVC
{
	public interface IModelLoader<out TModel>
	{
		public TModel LoadModel();
	}
}