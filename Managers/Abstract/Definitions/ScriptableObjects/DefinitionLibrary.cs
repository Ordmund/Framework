using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.Linq;
#endif

namespace Framework.Managers.Definitions
{
	public abstract class DefinitionLibrary<T> : ScriptableObject where T : ScriptableObject
	{
#if UNITY_EDITOR
		[SerializeField] private DefaultAsset _definitionsFolder;
#endif
		[SerializeField] protected List<T> definitions;

		public IReadOnlyList<T> Definitions => definitions;

		public virtual void ScanForDefinitions()
		{
#if UNITY_EDITOR
			if (!TryGetDefinitionsFolderPath(out var path))
				return;

			var guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}", new[] { path });
			var items = guids.Select(guid => AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid))).ToList();

			definitions = new List<T>();
			foreach (var item in items)
			{
				definitions.Add(item);
			}

			EditorUtility.SetDirty(this);
			AssetDatabase.SaveAssets();

			Debug.Log($"<color=green>{typeof(T).Name} Library updated with {definitions.Count} definitions!</color>");
#endif
		}

		protected bool TryGetDefinitionsFolderPath(out string path)
		{
#if UNITY_EDITOR
			path = null;

			if (_definitionsFolder == null)
			{
				Debug.LogError($"<color=red>Path to {typeof(T).Name} definitions folder is not set!</color>");
				return false;
			}

			var assetPath = AssetDatabase.GetAssetPath(_definitionsFolder);
			if (!AssetDatabase.IsValidFolder(assetPath))
			{
				Debug.LogError($"<color=red>Path to {typeof(T).Name} definitions folder is not valid!</color>");
				return false;
			}

			path = assetPath;
			return true;
#endif
		}
	}
}