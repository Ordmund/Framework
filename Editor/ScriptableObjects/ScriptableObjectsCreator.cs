using System;
using System.IO;
using System.Linq;
using Core.Managers.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace Core.Editor
{
	public class ScriptableObjectsCreator : EditorWindow
	{
		private DefaultAsset _targetFolder;
		private Type[] _scriptableObjects;
		private string _scriptableObjectName;
		private int _selectedIndex;

		protected virtual Type AssemblyTargetType => typeof(ScriptableObjectsPathHandler);

		[MenuItem("Window/ScriptableObjects/Core Assembly Creator")]
		private static void OpenWindow()
		{
			var window = GetWindow<ScriptableObjectsCreator>();
			if (window)
				window.Initialize();
		}

		protected void Initialize()
		{
			var currentAssembly = AssemblyTargetType.Assembly;
			var scriptableObjectType = typeof(ScriptableObject);
			_scriptableObjects = currentAssembly.GetTypes().Where(classType => scriptableObjectType.IsAssignableFrom(classType) && !classType.IsAbstract).ToArray();
		}

		private void OnGUI()
		{
			_targetFolder = (DefaultAsset)EditorGUILayout.ObjectField("Select Folder", _targetFolder, typeof(DefaultAsset), false);
			_scriptableObjectName = EditorGUILayout.TextField("Name: ", _scriptableObjectName);
			var newIndex = EditorGUILayout.Popup("ScriptableObject", _selectedIndex, _scriptableObjects.Select(type => type.Name).ToArray());
			if (newIndex != _selectedIndex)
			{
				_scriptableObjectName = _scriptableObjects[newIndex].Name;
				_selectedIndex = newIndex;
			}

			if (GUILayout.Button("Create"))
			{
				var assetPath = AssetDatabase.GetAssetPath(_targetFolder);
				var fullPath = Path.Combine(assetPath, _scriptableObjectName + ".asset");
				if (!ContainsErrors(assetPath, fullPath))
					CreateScriptableObject(fullPath);
			}
		}

		private void CreateScriptableObject(string fullPath)
		{
			var asset = CreateInstance(_scriptableObjects[_selectedIndex]);
			AssetDatabase.CreateAsset(asset, fullPath);

			Debug.Log($"<color=green>{_scriptableObjects[_selectedIndex]} successfully created!</color>");
		}

		private bool ContainsErrors(string assetPath, string fullPath)
		{
			if (string.IsNullOrEmpty(_scriptableObjectName))
			{
				Debug.LogError("Name of a ScriptableObject is empty!");
				return true;
			}

			if (string.IsNullOrEmpty(assetPath))
			{
				Debug.LogError("Folder not selected!");
				return true;
			}

			if (File.Exists(fullPath))
			{
				Debug.LogError($"ScriptableObject with name {_scriptableObjectName} already exist by path {assetPath}!");
				return true;
			}

			if (_scriptableObjects.Length != 0) return false;

			Debug.LogError("Not found any available ScriptableObject to create!");
			return true;
		}
	}
}