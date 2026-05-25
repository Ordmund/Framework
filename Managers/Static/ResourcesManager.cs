using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Framework.Managers
{
	public static class ResourcesManager
	{
		private static readonly Dictionary<string, Object> _assets = new();

		/// <summary>
		/// Loads asset with specified type by specified path
		/// </summary>
		/// <param name="path">Path to asset</param>
		/// <typeparam name="T">Type of asset that inherited from Unity.Object</typeparam>
		/// <returns>Loaded asset</returns>
		public static T Load<T>(string path) where T : Object
		{
			if (_assets.TryGetValue(path, out var preloadedAsset))
				return preloadedAsset as T;

			var asset = Resources.Load<T>(path);
			_assets.Add(path, asset);

			return asset;
		}

		/// <summary>
		/// Unload specified asset
		/// </summary>
		/// <param name="asset">Asset that need to be unloaded</param>
		public static void Unload(Object asset)
		{
			var assetPair = _assets.FirstOrDefault(pair => pair.Value == asset);
			if (!string.IsNullOrEmpty(assetPair.Key))
				_assets.Remove(assetPair.Key);

			Resources.UnloadAsset(asset);
		}

		/// <summary>
		/// Unload asset by specified path
		/// </summary>
		/// <param name="path">Path to asset that need to be unloaded</param>
		public static void Unload(string path)
		{
			if (_assets.ContainsKey(path))
			{
				var asset = _assets[path];
				if (asset is not GameObject)
					Resources.UnloadAsset(asset);

				_assets.Remove(path);
			}
		}

		/// <summary>
		/// Unload all resources loaded by the manager
		/// </summary>
		public static void UnloadAll()
		{
			var assetsToUnload = _assets.Where(asset => asset.Value is not GameObject);
			foreach (var asset in assetsToUnload)
				Resources.UnloadAsset(asset.Value);

			_assets.Clear();

			Resources.UnloadUnusedAssets();
		}
	}
}