using UnityEngine;

namespace Framework.MVC
{
	public abstract class ViewBase : MonoBehaviour
	{
		private GameObject _gameObject;
		private Transform _transform;

		public Vector3 Position => Transform.position;
		public Vector3 LocalPosition => Transform.position;

		protected GameObject GameObject => _gameObject ??= gameObject;
		protected Transform Transform => _transform ??= transform;

		public void SetActive(bool isActive)
		{
			GameObject.SetActive(isActive);
		}
	}
}