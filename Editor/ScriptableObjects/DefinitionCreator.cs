using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Core.Editor.CustomEditors;
using Core.Managers.Definitions;
using Core.Managers.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace Core.Editor
{
	public abstract class DefinitionCreator<TDefinition, TDefinitionLibrary> : EditorWindow where TDefinition : Definition where TDefinitionLibrary : DefinitionLibrary<TDefinition>
	{
		private string[] _definitionsTypesNames;
		private int _typeIndex;
		private bool _defaultFolderFound;

		private string _labelMessage;
		private GUIStyle _labelStyle;

		private const string NameRegexExpression = "(?<=[a-z])(?=[A-Z0-9])|(?<=[A-Z0-9])(?=[A-Z][a-z])";
		protected const string ErrorPlaceholder = "Error";

		protected string DefinitionName;
		protected bool InitializationError;
		protected DefaultAsset TargetFolder;
		
		protected abstract string DefaultFolderPath { get; }

		private void OnEnable()
		{
			Initialize();
		}

		#region Initialization

		protected virtual void Initialize()
		{
			SetDefaultFolder();

			UpdateTypesNamesArray<TDefinition>(out _definitionsTypesNames);
		}

		private void SetDefaultFolder()
		{
			var defaultFolder = AssetDatabase.LoadAssetAtPath<DefaultAsset>(DefaultFolderPath);
			if (defaultFolder != null)
			{
				_defaultFolderFound = true;
				TargetFolder = defaultFolder;
			}
			else
			{
				_defaultFolderFound = false;
				Debug.LogError($"Default folder not found! Path: {DefaultFolderPath}");
			}
		}

		protected void UpdateTypesNamesArray<T>(out string[] targetArray)
		{
			var currentAssembly = typeof(T).Assembly;
			var type = typeof(T);
			var concreteTypes = currentAssembly.GetTypes().Where(classType => type.IsAssignableFrom(classType) && !classType.IsAbstract).ToArray();

			if (concreteTypes.Length != 0)
			{
				targetArray = concreteTypes.Select(concreteType => concreteType.Name).ToArray();
			}
			else
			{
				InitializationError = true;
				targetArray = new[] { ErrorPlaceholder };
				SetLabel($"No concrete {typeof(T).Name} classes found!", Color.softRed);
			}
		}

		#endregion

		private void OnGUI()
		{
			DrawConfigurationFields();
			DrawTargetFolder();
			DrawCreateButton();
			DrawLabel();
		}

		protected virtual void DrawConfigurationFields()
		{
			DefinitionName = EditorGUILayout.TextField("Name: ", DefinitionName);
			_typeIndex = EditorGUILayout.Popup("Type:", _typeIndex, _definitionsTypesNames);
		}

		private void DrawTargetFolder()
		{
			EditorGUI.BeginDisabledGroup(_defaultFolderFound);
			TargetFolder = (DefaultAsset)EditorGUILayout.ObjectField("Target Folder", TargetFolder, typeof(DefaultAsset), false);
			EditorGUI.EndDisabledGroup();
		}

		private void DrawCreateButton()
		{
			var createIsNotAllowed = string.IsNullOrEmpty(DefinitionName) || TargetFolder == null || InitializationError;
			EditorGUI.BeginDisabledGroup(createIsNotAllowed);
			if (GUILayout.Button("Create"))
			{
				var fullPath = GetTargetFullPath();
				if (File.Exists(fullPath))
				{
					SetLabel($"Definition {DefinitionName} already exists!", Color.softRed);
				}
				else
				{
					var definition = CreateNewDefinition(fullPath);
					SelectCreatedAsset(definition);
					FillDefaultDefinitionFields(definition);
					UpdateDefinitionLibrary();

					SetLabel($"{DefinitionName} successfully created!", Color.mediumSeaGreen);
				}
			}

			EditorGUI.EndDisabledGroup();
		}

		private void DrawLabel()
		{
			if (_labelMessage != null)
			{
				EditorGUILayout.LabelField(_labelMessage, _labelStyle);
			}
		}

		protected void SetLabel(string message, Color color)
		{
			_labelMessage = message;
			_labelStyle = new GUIStyle(EditorStyles.boldLabel)
			{
				normal = { textColor = color }
			};
		}

		private TDefinition CreateNewDefinition(string fullPath)
		{
			var baseDefinitionType = typeof(TDefinition);
			var definitionsTypes = baseDefinitionType.Assembly.GetTypes().Where(classType => baseDefinitionType.IsAssignableFrom(classType) && !classType.IsAbstract).ToArray();

			var selectedType = definitionsTypes[_typeIndex];
			var scriptableObject = CreateInstance(selectedType);
			AssetDatabase.CreateAsset(scriptableObject, fullPath);
			AssetDatabase.SaveAssets();

			return AssetDatabase.LoadAssetAtPath<TDefinition>(fullPath);
		}

		private void SelectCreatedAsset(TDefinition definition)
		{
			Selection.activeObject = definition;
		}

		protected virtual void FillDefaultDefinitionFields(TDefinition definition)
		{
			var serializedObject = new SerializedObject(definition);
			serializedObject.Update();

			SetSerializeFieldValue(serializedObject, DefinitionFieldNames.IdFieldName, DefinitionName);

			var displayName = Regex.Replace(DefinitionName, NameRegexExpression, " ");
			SetSerializeFieldValue(serializedObject, DefinitionFieldNames.NameFieldName, displayName);

			serializedObject.ApplyModifiedProperties();
		}

		protected void SetSerializeFieldValue(SerializedObject serializedObject, string propertyName, object value)
		{
			var property = serializedObject.FindProperty(propertyName);
			if (property == null)
			{
				Debug.LogError($"Property {propertyName} not found!");
				return;
			}

			switch (property.propertyType)
			{
				case SerializedPropertyType.Boolean:
					property.boolValue = (bool)value;
					break;

				case SerializedPropertyType.Enum:
					property.enumValueIndex = (int)value;
					break;

				case SerializedPropertyType.String:
					property.stringValue = (string)value;
					break;

				case SerializedPropertyType.ObjectReference:
					property.objectReferenceValue = (Sprite)value;
					serializedObject.ApplyModifiedProperties();
					break;

				default:
					Debug.LogError($"Unhandled serialized property type: {property.propertyType}");
					break;
			}
		}

		private void UpdateDefinitionLibrary()
		{
			var definitionLibrary = ScriptableObjectsManager.Load<TDefinitionLibrary>();
			definitionLibrary.ScanForDefinitions();
		}

		protected virtual string GetTargetFullPath()
		{
			return Path.Combine(AssetDatabase.GetAssetPath(TargetFolder), DefinitionName + ".asset");
		}
	}
}