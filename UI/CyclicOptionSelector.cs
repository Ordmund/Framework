using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.UI
{
	public class CyclicOptionSelector : MonoBehaviour
	{
		[SerializeField] private Button _leftButton;
		[SerializeField] private Button _rightButton;
		[SerializeField] private TMP_Text _label;

		private string[] _options;
		private int _selectedIndex;
		private bool _isLooped;

		public event Action<int, string> OptionChanged;

		public string SelectedOption => _options?[_selectedIndex];

		public void Initialize(string[] options, int startIndex = 0, bool isLooped = true)
		{
			Validate(options, startIndex);

			_options = options;
			_selectedIndex = startIndex;
			_isLooped = isLooped;

			UpdateLabel();
			UpdateButtonsInteractability();
		}

		public void SetIndex(int index)
		{
			Validate(_options, index);

			_selectedIndex = ClampIndex(index);

			ApplySelection();
		}

		private static void Validate(string[] options, int startIndex)
		{
			if (options == null || options.Length == 0)
				throw new ArgumentException("Options array is empty");

			if (startIndex < 0 || startIndex >= options.Length)
				throw new ArgumentOutOfRangeException(nameof(startIndex));
		}

		private void MoveLeft() => Move(-1);
		private void MoveRight() => Move(1);

		private void Move(int direction)
		{
			var oldIndex = _selectedIndex;
			var newIndex = _selectedIndex + direction;

			_selectedIndex = ClampIndex(newIndex);

			if (_selectedIndex != oldIndex)
			{
				ApplySelection();
			}
		}

		private int ClampIndex(int index)
		{
			var lastElementIndex = _options.Length - 1;

			if (_isLooped)
				return index < 0 ? lastElementIndex : index > lastElementIndex ? 0 : index;

			return Mathf.Clamp(index, 0, lastElementIndex);
		}

		private void ApplySelection()
		{
			UpdateLabel();
			UpdateButtonsInteractability();

			OptionChanged?.Invoke(_selectedIndex, SelectedOption);
		}

		private void UpdateLabel()
		{
			_label.text = _options[_selectedIndex];
		}

		private void UpdateButtonsInteractability()
		{
			if (_isLooped)
				return;

			_leftButton.interactable = _selectedIndex > 0;
			_rightButton.interactable = _selectedIndex < _options.Length - 1;
		}

		private void OnEnable()
		{
			_leftButton.onClick.AddListener(MoveLeft);
			_rightButton.onClick.AddListener(MoveRight);
		}

		private void OnDisable()
		{
			_leftButton.onClick.RemoveListener(MoveLeft);
			_rightButton.onClick.RemoveListener(MoveRight);
		}
	}
}