using System;
using System.Linq;
using Framework.Managers.Behaviours;
using Framework.Managers.Definitions;
using UnityEditor;

namespace Framework.Editor.CustomEditors
{
	public class BehaviourDefinitionCustomEditor<TDefinition, TBehaviour, TDefinitionLibrary> : DefinitionCustomEditor<TDefinition, TDefinitionLibrary>
		where TDefinition : BehaviourDefinition
		where TBehaviour : Behaviour
		where TDefinitionLibrary : DefinitionLibrary<TDefinition>
	{
		private SerializedProperty _behaviourIdProperty;

		private string[] _behavioursIdsNames;
		private int _behaviourIdIndex;

		protected override void OnEnable()
		{
			UpdateBehavioursIdsNames();

			base.OnEnable();
		}

		private void UpdateBehavioursIdsNames()
		{
			var behaviourType = typeof(TBehaviour);

			_behavioursIdsNames = AppDomain.CurrentDomain
				.GetAssemblies().SelectMany(assembly => assembly.GetTypes())
				.Where(type => behaviourType.IsAssignableFrom(type) && !type.IsAbstract)
				.Select(type => type.Name).ToArray();
		}

		protected override void InitializeProperties()
		{
			base.InitializeProperties();

			_behaviourIdProperty = serializedObject.FindProperty(DefinitionFieldNames.BehaviourIdFieldName);

			var value = _behaviourIdProperty.stringValue;
			var index = Array.IndexOf(_behavioursIdsNames, value);
			if (index >= 0)
			{
				_behaviourIdIndex = index;
			}
		}

		protected override void DrawBaseFields()
		{
			base.DrawBaseFields();
			
			var newIndex = EditorGUILayout.Popup("Behaviour Id:", _behaviourIdIndex, _behavioursIdsNames);
			if (newIndex != _behaviourIdIndex)
			{
				_behaviourIdProperty.stringValue = _behavioursIdsNames[newIndex];
				_behaviourIdIndex = newIndex;
			}
		}
	}
}