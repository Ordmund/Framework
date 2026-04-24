using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.MVC
{
	public class PrefabsPaths : ScriptableObject
	{
		[SerializeField] private List<PathNamePair> paths = new();

		public string GetPath<T>() where T : ViewBase
		{
			var typeName = typeof(T).Name;

			foreach (var pathNamePair in paths)
			{
				if (pathNamePair.viewClassName == typeName)
					return pathNamePair.path;
			}

			Debug.LogError($"Path for {typeName} not found");

			return string.Empty;
		}
	}

	[Serializable]
	public class PathNamePair
	{
		public string viewClassName;
		public string path;
	}
}