using Core.UI;
using UnityEditor;

namespace Core.Editor.UI
{
    [CustomEditor(typeof(TextToggle))]
    public class TextToggleEditor : UnityEditor.UI.ToggleEditor
    {
        private SerializedProperty _label;

        protected override void OnEnable()
        {
            base.OnEnable();

            _label = serializedObject.FindProperty(nameof(TextToggle.label));
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