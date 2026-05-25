using UnityEngine;
using UnityEngine.UI;

namespace Framework.UI
{
	public class MaskedBar : MonoBehaviour
	{
		[SerializeField] private RectTransform _rectTransform;
		[SerializeField] private RectMask2D _rectMask2D;

		public void SetValue(float value)
		{
			var clampedValue = Mathf.Clamp01(value);

			var padding = _rectMask2D.padding;
			padding.z = _rectTransform.rect.width * (1 - clampedValue);

			_rectMask2D.padding = padding;
		}
	}
}