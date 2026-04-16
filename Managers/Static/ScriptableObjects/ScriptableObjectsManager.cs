using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Core.Dependencies.DependenciesProvider;

namespace Core.Managers.ScriptableObjects
{
	public static class ScriptableObjectsManager
	{
		private static readonly List<ScriptableObject> ScriptableObjects = new();
		private static ScriptableObjectsPathHandler _pathHandler;

		private static ScriptableObjectsPathHandler PathHandler => _pathHandler != null ? _pathHandler : _pathHandler = ResourcesManager.Load<ScriptableObjectsPathHandler>(PathHandlerPath);

		public static bool Exist<TScriptableObject>(string name = null) where TScriptableObject : ScriptableObject
		{
			name ??= typeof(TScriptableObject).Name;

			return PathHandler.Contains(name);
		}

		public static TScriptableObject Load<TScriptableObject>(string name = null) where TScriptableObject : ScriptableObject
		{
			name ??= typeof(TScriptableObject).Name;

			var searchResult = ScriptableObjects.FirstOrDefault(loadedScriptableObject => loadedScriptableObject.name == name);
			if (searchResult != null)
				return searchResult as TScriptableObject;

			var path = PathHandler.GetPath(name);
			var scriptableObject = ResourcesManager.Load<TScriptableObject>(path);

			if (scriptableObject != null)
			{
				ScriptableObjects.Add(scriptableObject);
				return scriptableObject;
			}

			Debug.LogError($"ScriptableObject with name [{name}] and type {typeof(TScriptableObject)} not found!");
			return null;
		}

		public static void Unload(ScriptableObject scriptableObject)
		{
			if (ScriptableObjects.Contains(scriptableObject))
				ScriptableObjects.Remove(scriptableObject);

			ResourcesManager.Unload(scriptableObject);
		}

		public static void Unload(string name)
		{
			var searchResult = ScriptableObjects.FirstOrDefault(loadedScriptableObject => loadedScriptableObject.name == name);
			if (searchResult == null)
				return;

			ScriptableObjects.Remove(searchResult);
			ResourcesManager.Unload(searchResult);
		}

		public static void UnloadAll()
		{
			ScriptableObjects.ForEach(ResourcesManager.Unload);
			ScriptableObjects.Clear();
		}
	}
}