using System;
using Framework.UI;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Framework.Editor.UI
{
	public class CyclicOptionSelectorMenu
	{
		private static readonly Vector2 Size = new(140, 20);
		private static readonly Vector2 ButtonSize = new(20, 20);
		private static readonly Vector2 LabelSize = new(100, 20);

		private const string GameObjectName = "Cyclic Option Selector";
		private const string ButtonLabelName = "Label";
		private const string OptionLabelName = "Option Label";
		private const string LeftButtonText = "<";
		private const string RightButtonText = ">";
		private const string UnregisterStepNameFormat = "Create {0}";

		private const string LeftButtonFieldName = "_leftButton";
		private const string RightButtonFieldName = "_rightButton";
		private const string LabelFieldName = "_label";

		private const int LabelsFontSize = 12;

		[MenuItem("GameObject/UI (Canvas)/Cyclic Option Selector")]
		private static void CreateCyclicOptionSelector(MenuCommand menuCommand)
		{
			var parent = ResolveParent(menuCommand.context as GameObject);

			var rootGameObject = new GameObject(GameObjectName, typeof(RectTransform));
			GameObjectUtility.SetParentAndAlign(rootGameObject, parent.gameObject);

			var rectTransform = rootGameObject.GetComponent<RectTransform>();
			rectTransform.sizeDelta = Size;

			var layout = rootGameObject.AddComponent<HorizontalLayoutGroup>();
			layout.childAlignment = TextAnchor.MiddleCenter;

			var leftButton = CreateArrowButton(rootGameObject.transform, ObjectNames.NicifyVariableName(LeftButtonFieldName), LeftButtonText);
			var label = CreateLabel(rootGameObject.transform);
			var rightButton = CreateArrowButton(rootGameObject.transform, ObjectNames.NicifyVariableName(RightButtonFieldName), RightButtonText);

			var behaviour = rootGameObject.AddComponent<CyclicOptionSelector>();
			AssignField(behaviour, LeftButtonFieldName, leftButton);
			AssignField(behaviour, RightButtonFieldName, rightButton);
			AssignField(behaviour, LabelFieldName, label);

			Undo.RegisterCreatedObjectUndo(rootGameObject, string.Format(UnregisterStepNameFormat, rootGameObject.name));
			Selection.activeObject = rootGameObject;
		}

		private static Button CreateArrowButton(Transform parent, string name, string text)
		{
			var gameObject = CreateGameObject(name, ButtonSize, new[] { typeof(RectTransform), typeof(Image), typeof(Button) }, parent);
			gameObject.GetComponent<Image>().color = Color.gray2;

			var labelGameObject = CreateGameObject(ButtonLabelName, ButtonSize, new[] { typeof(RectTransform), typeof(TextMeshProUGUI) }, gameObject.transform);

			var labelText = labelGameObject.GetComponent<TextMeshProUGUI>();
			labelText.text = text;
			labelText.fontSize = LabelsFontSize;
			labelText.alignment = TextAlignmentOptions.Center;
			labelText.color = Color.antiqueWhite;

			Undo.RegisterCreatedObjectUndo(gameObject, string.Format(UnregisterStepNameFormat, name));

			return gameObject.GetComponent<Button>();
		}

		private static TextMeshProUGUI CreateLabel(Transform parent)
		{
			var gameObject = CreateGameObject(OptionLabelName, LabelSize, new[] { typeof(RectTransform), typeof(TextMeshProUGUI) }, parent);

			var labelText = gameObject.GetComponent<TextMeshProUGUI>();
			labelText.text = OptionLabelName;
			labelText.fontSize = LabelsFontSize;
			labelText.alignment = TextAlignmentOptions.Center;

			return labelText;
		}

		private static Transform ResolveParent(GameObject selected)
		{
			var canvas = selected != null ? selected.GetComponentInParent<Canvas>() : Object.FindFirstObjectByType<Canvas>();
			if (canvas == null)
			{
				var canvasGameObject = new GameObject(nameof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
				canvas = canvasGameObject.GetComponent<Canvas>();
				canvas.renderMode = RenderMode.ScreenSpaceOverlay;

				Undo.RegisterCreatedObjectUndo(canvasGameObject, string.Format(UnregisterStepNameFormat, nameof(Canvas)));
			}

			if (Object.FindFirstObjectByType<EventSystem>() == null)
			{
				var eventSystemGameObject = new GameObject(nameof(EventSystem), typeof(EventSystem), typeof(StandaloneInputModule));

				Undo.RegisterCreatedObjectUndo(eventSystemGameObject, string.Format(UnregisterStepNameFormat, nameof(EventSystem)));
			}

			return selected != null ? selected.transform : canvas.transform;
		}

		private static void AssignField(Object target, string fieldName, Object value)
		{
			var serializedObject = new SerializedObject(target);
			var property = serializedObject.FindProperty(fieldName);
			if (property == null)
				throw new NullReferenceException($"{nameof(CyclicOptionSelector)} does not have a field named {fieldName}");

			property.objectReferenceValue = value;
			serializedObject.ApplyModifiedProperties();
		}

		private static GameObject CreateGameObject(string name, Vector2 size, Type[] components, Transform parent)
		{
			var gameObject = new GameObject(name, components);
			gameObject.transform.SetParent(parent, false);
			gameObject.GetComponent<RectTransform>().sizeDelta = size;

			return gameObject;
		}
	}
}