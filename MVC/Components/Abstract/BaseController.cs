using Core.Managers.ScriptableObjects;
using UnityEngine;

namespace Core.MVC
{
	public abstract class BaseController<TView, TModel>
	{
		protected TView View { get; }
		protected TModel Model { get; private set; }

		protected BaseController(TView view, TModel model)
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