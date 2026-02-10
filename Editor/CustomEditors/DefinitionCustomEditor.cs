using System;
using System.Linq;
using Core.Managers.Definitions;
using Core.Managers.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace Core.Editor.CustomEditors
{
	public abstract class DefinitionCustomEditor<TDefinition, TDefinitionLibrary> : UnityEditor.Editor where TDefinition : Definition where TDefinitionLibrary : DefinitionLibrary<TDefinition>
	{
		protected bool UseDefaultInspector;
		protected SerializedProperty IdProperty;
		protected SerializedProperty NameProperty;

		private TDefinitionLibrary _definitionLibrary;

		private GUIStyle _headerStyle;
		private GUIStyle _blockHeaderStyle;
		private GUIStyle _libraryStatusStyle;
		private bool _definitionIsInLibrary;

		protected virtual void OnEnable()
		{
			InitializeProperties();
			InitializeLibrary();
		}

		protected virtual void InitializeProperties()
		{
			IdProperty = serializedObject.FindProperty(DefinitionFieldNames.IdFieldName);
			NameProperty = serializedObject.FindProperty(DefinitionFieldNames.NameFieldName);
		}

		private void InitializeLibrary()
		{
			_definitionLibrary = ScriptableObjectsManager.Load<TDefinitionLibrary>();

			UpdateIsInLibraryFlag();
		}

		private void UpdateIsInLibraryFlag()
		{
			var id = IdProperty.stringValue;

			_definitionIsInLibrary = _definitionLibrary.Definitions.Any(item => item.Id == id);
		}

		public override void OnInspectorGUI()
		{
			if (UseDefaultInspector)
			{
				DrawDefaultInspectorWithToggle();
				return;
			}

			DrawLabel($"{typeof(TDefinition).Name} Custom Editor", GetUpdatedHeaderStyle);
			DrawDefaultInspectorToggle();
			DrawSerializeFields();
			DrawLibraryStatusLabel();
		}

		private void DrawDefaultInspectorWithToggle()
		{
			DrawLabel("Default Editor", GetUpdatedHeaderStyle);
			DrawDefaultInspectorToggle();
			DrawDefaultInspector();
		}

		protected void DrawLabel(string label, Func<GUIStyle> getStyleFunc)
		{
			var style = getStyleFunc.Invoke();

			EditorGUILayout.LabelField(label, style);
		}

		private void DrawDefaultInspectorToggle()
		{
			EditorGUILayout.Space();

			var rect = EditorGUILayout.GetControlRect();
			var newValue = EditorGUI.Toggle(rect, "Use Default Inspector", UseDefaultInspector);
			if (newValue != UseDefaultInspector)
			{
				UseDefaultInspector = newValue;

				if (!UseDefaultInspector)
					InitializeProperties();
			}

			EditorGUILayout.Space();
		}

		protected virtual void DrawSerializeFields()
		{
			serializedObject.Update();

			DrawLabel("Serialize Fields:", GetUpdatedBlockHeaderStyle);

			EditorGUILayout.BeginVertical("Box");
			DrawBaseFields();
			EditorGUILayout.EndVertical();

			serializedObject.ApplyModifiedProperties();
		}

		protected virtual void DrawBaseFields()
		{
			EditorGUILayout.PropertyField(IdProperty);
			EditorGUILayout.PropertyField(NameProperty);
		}

		private void DrawLibraryStatusLabel()
		{
			if (_definitionIsInLibrary)
			{
				EditorGUILayout.Space();
				DrawLabel($"✔ {serializedObject.targetObject.name} is in the library.", GetUpdatedLibraryLabelStyle);
			}
			else
			{
				EditorGUILayout.Space();
				EditorGUILayout.HelpBox($"{serializedObject.targetObject.name} is not in the library.", MessageType.Warning);
				if (GUILayout.Button("Add to Library"))
				{
					_definitionLibrary.ScanForDefinitions();

					UpdateIsInLibraryFlag();
				}
			}
		}

		private GUIStyle GetUpdatedHeaderStyle()
		{
			_headerStyle ??= new GUIStyle(EditorStyles.boldLabel)
			{
				alignment = TextAnchor.MiddleCenter,
				fontSize = 16,
				normal =
				{
					textColor = Color.sandyBrown
				}
			};

			return _headerStyle;
		}

		protected GUIStyle GetUpdatedBlockHeaderStyle()
		{
			_blockHeaderStyle ??= new GUIStyle(EditorStyles.boldLabel)
			{
				normal =
				{
					textColor = Color.softBlue
				}
			};

			return _blockHeaderStyle;
		}

		private GUIStyle GetUpdatedLibraryLabelStyle()
		{
			_libraryStatusStyle ??= new GUIStyle(EditorStyles.label)
			{
				normal =
				{
					textColor = Color.forestGreen
				}
			};

			return _libraryStatusStyle;
		}
	}
}