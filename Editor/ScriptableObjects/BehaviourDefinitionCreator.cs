using Core.Editor.CustomEditors;
using Core.Managers.Behaviours;
using Core.Managers.Definitions;
using UnityEditor;

namespace Core.Editor
{
	public abstract class BehaviourDefinitionCreator<TDefinition, TBehaviour, TDefinitionLibrary> : DefinitionCreator<TDefinition, TDefinitionLibrary>
		where TDefinition : BehaviourDefinition
		where TBehaviour : Behaviour
		where TDefinitionLibrary : DefinitionLibrary<TDefinition>
	{
		private string[] _behavioursTypesNames;
		private int _behaviourIndex;

		protected override void Initialize()
		{
			base.Initialize();

			UpdateTypesNamesArray<TBehaviour>(out _behavioursTypesNames);
		}

		protected override void DrawConfigurationFields()
		{
			base.DrawConfigurationFields();

			_behaviourIndex = EditorGUILayout.Popup("Behaviour:", _behaviourIndex, _behavioursTypesNames);
		}

		protected override void FillDefaultDefinitionFields(TDefinition definition)
		{
			base.FillDefaultDefinitionFields(definition);

			var serializedObject = new SerializedObject(definition);
			serializedObject.Update();

			var behaviourId = _behavioursTypesNames[_behaviourIndex];
			SetSerializeFieldValue(serializedObject, DefinitionFieldNames.BehaviourIdFieldName, behaviourId);

			serializedObject.ApplyModifiedProperties();
		}
	}
}