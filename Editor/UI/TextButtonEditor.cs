using Framework.UI;
using UnityEditor;

namespace Framework.Editor.UI
{
	[CustomEditor(typeof(TextButton))]
	public class TextButtonEditor : UnityEditor.UI.ButtonEditor
	{
		private SerializedProperty _label;

		protected override void OnEnable()
		{
			base.OnEnable();

			_label = serializedObject.FindProperty(nameof(TextButton.label));
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			serializedObject.Update();
			EditorGUILayout.PropertyField(_label);
			serializedObject.ApplyModifiedProperties();
		}
	}
}