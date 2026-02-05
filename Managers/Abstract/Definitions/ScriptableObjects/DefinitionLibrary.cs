using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.Linq;
#endif

namespace Core.Managers.Definitions
{
	public abstract class DefinitionLibrary<T> : ScriptableObject where T : ScriptableObject
	{
		[SerializeField] protected List<T> _definitions;

		protected abstract string PathToDefinitions { get; }

		public List<T> Definitions => _definitions;

		public virtual void ScanForDefinitions()
		{
#if UNITY_EDITOR
			var guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}", new[] { PathToDefinitions });
			var items = guids.Select(guid => AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid))).ToList();

			_definitions = new List<T>();
			foreach (var item in items)
			{
				_definitions.Add(item);
			}

			Debug.Log($"<color=green>{typeof(T).Name} Library updated with {_definitions.Count} definitions!</color>");
#endif
		}
	}
}