using UnityEngine;

namespace Core.MVC
{
	public abstract class ViewBase : MonoBehaviour
	{
		private GameObject _gameObject;
		private Transform _transform;

		protected GameObject GameObject => _gameObject ??= gameObject;
		protected Transform Transform => _transform ??= transform;

		public void SetActive(bool isActive)
		{
			GameObject.SetActive(isActive);
		}
	}
}