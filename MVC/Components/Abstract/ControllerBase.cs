using Framework.Managers.ScriptableObjects;
using UnityEngine;

namespace Framework.MVC
{
	public abstract class ControllerBase<TView, TModel> : IController where TView : ViewBase where TModel : ModelBase
	{
		protected TView View { get; }
		protected TModel Model { get; private set; }

		protected ControllerBase(TView view, TModel model)
		{
			View = view;
			Model = model;
		}

		protected void TryLoadModelFromScriptableObject<TScriptableObject>() where TScriptableObject : ScriptableObject, IModelLoader<TModel>
		{
			IModelLoader<TModel> modelLoader = ScriptableObjectsManager.Load<TScriptableObject>();

			Model = modelLoader.LoadModel();
		}
	}
}