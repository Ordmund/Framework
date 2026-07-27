using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Framework.Managers
{
	public static class ResourcesManager
	{
		private static readonly Dictionary<string, Object> Assets = new();

		/// <summary>
		/// Loads an asset of the specified type from the specified path
		/// </summary>
		/// <param name="path">Path to asset</param>
		/// <typeparam name="T">Type of asset that inherited from Unity.Object</typeparam>
		/// <returns>Loaded asset</returns>
		public static T Load<T>(string path) where T : Object
		{
			if (Assets.TryGetValue(path, out var preloadedAsset))
				return preloadedAsset as T;

			var asset = Resources.Load<T>(path);
			Assets.Add(path, asset);

			return asset;
		}

		/// <summary>
		/// Unload specified asset
		/// </summary>
		/// <param name="asset">Asset that needs to be unloaded</param>
		public static void Unload(Object asset)
		{
			var assetPair = Assets.FirstOrDefault(pair => pair.Value == asset);
			if (!string.IsNullOrEmpty(assetPair.Key))
				Assets.Remove(assetPair.Key);

			if (IsUnloadable(asset))
				Resources.UnloadAsset(asset);
		}

		/// <summary>
		/// Unload an asset by a specified path
		/// </summary>
		/// <param name="path">Path to the asset that needs to be unloaded</param>
		public static void Unload(string path)
		{
			if (Assets.ContainsKey(path))
			{
				var asset = Assets[path];
				if (IsUnloadable(asset))
					Resources.UnloadAsset(asset);

				Assets.Remove(path);
			}
		}

		/// <summary>
		/// Unload all resources loaded by the manager
		/// </summary>
		public static void UnloadAll()
		{
			var assetsToUnload = Assets.Where(asset => asset.Value is not GameObject);
			foreach (var asset in assetsToUnload)
				Resources.UnloadAsset(asset.Value);

			Assets.Clear();

			Resources.UnloadUnusedAssets();
		}

		private static bool IsUnloadable(Object asset)
		{
			return asset switch
			{
				Texture or Mesh or Material or Shader or Sprite or Font or AudioClip or AnimationClip or ScriptableObject => true,
				_ => false
			};
		}
	}
}