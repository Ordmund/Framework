using Framework.UI;
using UnityEditor;

namespace Framework.Editor.UI
{
	[CustomEditor(typeof(TextSlider))]
	public class TextSliderEditor : UnityEditor.UI.SliderEditor
	{
		private SerializedProperty _label;

		protected override void OnEnable()
		{
			base.OnEnable();

			_label = serializedObject.FindProperty(nameof(TextSlider.label));
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