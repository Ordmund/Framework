using System.Linq;
using UnityEngine;

namespace Framework.Managers.ScriptableObjects
{
	public class ScriptableObjectsPathHandler : ScriptableObject
	{
		[SerializeField] private string[] _pathsToScriptableObjects;
		[SerializeField] private string[] _ignoredSubfoldersPaths;
		[SerializeField] private ScriptableObjectPath[] _paths;

		public string GetPath(string objectName)
		{
			var pathModel = _paths.FirstOrDefault(model => model.name == objectName);
			if (pathModel != null)
				return pathModel.path;

			Debug.LogError($"Path for ScriptableObject {objectName} not found!");
			return string.Empty;
		}

		public bool Contains(string objectName)
		{
			return _paths.Any(model => model.name == objectName);
		}

#if UNITY_EDITOR
		[ContextMenu("Update paths")]
		public void UpdatePaths()
		{
			if (_pathsToScriptableObjects == null)
			{
				Debug.LogError("Path to ScriptableObjects folder is empty!");
				return;
			}

			var guids = UnityEditor.AssetDatabase.FindAssets($"t:{nameof(ScriptableObject)}", _pathsToScriptableObjects);
			var filteredGuids = guids.Where(guid =>
			{
				var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
				return _ignoredSubfoldersPaths.All(ignoredPath => !path.StartsWith(ignoredPath + '/'));
			}).ToArray();

			_paths = new ScriptableObjectPath[filteredGuids.Length];

			for (var assetIndex = 0; assetIndex < filteredGuids.Length; assetIndex++)
			{
				var path = UnityEditor.AssetDatabase.GUIDToAssetPath(filteredGuids[assetIndex]);
				var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);

				var conflictingPath = _paths.FirstOrDefault(scriptableObjectPath => scriptableObjectPath != null && scriptableObjectPath.name == asset.name);
				if (conflictingPath != null)
					Debug.LogError($"ScriptableObjects path handler already contains path for name [{asset.name}]. Conflicting objects are: {conflictingPath.path} and {path}.");

				path = path.Replace("Assets/Resources/", string.Empty).Replace(".asset", string.Empty);
				_paths[assetIndex] = new ScriptableObjectPath { name = asset.name, path = path };
			}

			Debug.Log("<color=green>ScriptableObjects paths successfully updated!</color>");
		}
#endif
	}
}