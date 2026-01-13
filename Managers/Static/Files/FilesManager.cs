using System;
using System.IO;
using System.Linq;
using System.Text;
using Core.Dependencies;
using UnityEngine;

namespace Core.Managers.Files
{
	public static class FilesManager
	{
		private const string BinaryFileExtension = ".bin";
		private const string JsonFileExtension = ".json";

		public static bool IsFileExist(string fileName, string directoryName = null, FileType fileType = FileType.Default)
		{
			var directoryPath = GetDirectoryPath(directoryName);
			ValidateDirectoryExistence(directoryPath);

			if (Directory.Exists(directoryPath))
			{
				var extension = fileType == FileType.Binary ? BinaryFileExtension : JsonFileExtension;
				var fullPath = GetFullPath(directoryPath, fileName) + extension;

				return File.Exists(fullPath);
			}

			return false;
		}

		public static void WriteBytes(this byte[] bytes, string name, string directoryName = null)
		{
			var directoryPath = GetDirectoryPath(directoryName);
			ValidateDirectoryExistence(directoryPath);

			var path = GetFullPath(directoryPath, name);

			File.WriteAllBytes(path + BinaryFileExtension, bytes);
		}

		public static void Write<T>(this T data, string name = null, string directoryName = null, FileType fileType = FileType.Default)
		{
			var type = data.GetType();
			ValidateSerializability(type);

			var directoryPath = GetDirectoryPath(directoryName);
			ValidateDirectoryExistence(directoryPath);

			var path = GetFullPath(directoryPath, name ?? type.Name);
			var json = JsonUtility.ToJson(data);
			var encryptedJson = json.Encrypt();

			switch (fileType)
			{
				case FileType.Default:
				case FileType.Json:
					File.WriteAllText(path + JsonFileExtension, encryptedJson);
					break;

				case FileType.Binary:
					var bytes = Encoding.ASCII.GetBytes(encryptedJson);

					File.WriteAllBytes(path + BinaryFileExtension, bytes);
					break;
			}
		}

		public static byte[] ReadBytes(string name, string directoryName = null)
		{
			var directoryPath = GetDirectoryPath(directoryName);
			ValidateDirectoryExistence(directoryPath);

			var path = GetFullPath(directoryPath, name) + BinaryFileExtension;

			return File.Exists(path)
				? File.ReadAllBytes(path)
				: throw new FileNotFoundException($"File {path} not found!");
		}

		public static T Read<T>(string name = null, string directoryName = null, FileType fileType = FileType.Default)
		{
			var directoryPath = GetDirectoryPath(directoryName);
			ValidateDirectoryExistence(directoryPath);

			var path = GetFullPath(directoryPath, name ?? typeof(T).Name);

			var encryptedJson = string.Empty;
			switch (fileType)
			{
				case FileType.Default:
				case FileType.Json:
					var pathWithJsonExtension = path + JsonFileExtension;
					if (!File.Exists(pathWithJsonExtension))
						throw new FileNotFoundException($"File {pathWithJsonExtension} not found!");

					encryptedJson = File.ReadAllText(pathWithJsonExtension);
					break;

				case FileType.Binary:
					var pathWithBinaryExtension = path + BinaryFileExtension;
					if (!File.Exists(pathWithBinaryExtension))
						throw new FileNotFoundException($"File {pathWithBinaryExtension} not found!");

					var bytes = File.ReadAllBytes(pathWithBinaryExtension);
					encryptedJson = Encoding.ASCII.GetString(bytes);
					break;
			}

			var json = encryptedJson.Decrypt();

			return JsonUtility.FromJson<T>(json);
		}

		public static string[] GetDirectoriesNames(string directoryName = null)
		{
			var directoryPath = GetDirectoryPath(directoryName);
			if (Directory.Exists(directoryPath))
			{
				var directoriesPaths = Directory.GetDirectories(directoryPath);
				return directoriesPaths.Select(Path.GetFileName).ToArray();
			}

			return Array.Empty<string>();
		}

		private static void ValidateSerializability(Type type)
		{
			if (type.IsPrimitive || type.IsArray || type == typeof(string) || (!type.IsClass && !type.IsValueType) || !type.IsSerializable)
				throw new TypeIsNotSerializableException($"Type {type.FullName} is not serializable!");
		}

		private static void ValidateDirectoryExistence(string path)
		{
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
		}

		private static string GetFullPath(string path, string fileName)
		{
			var fullPath = Path.Combine(path, fileName);

			return fullPath;
		}

		private static string GetDirectoryPath(string directoryName = null)
		{
			var filesPath = GetPathToFilesDirectory();
			var fullPath = string.IsNullOrEmpty(directoryName) ? filesPath : Path.Combine(filesPath, directoryName);

			return fullPath;
		}

		private static string GetPathToFilesDirectory()
		{
			return string.IsNullOrEmpty(DependenciesProvider.PathToFilesDirectory)
				? throw new PathNotFoundException("Path to save files directory not found!")
				: DependenciesProvider.PathToFilesDirectory;
		}
	}
}