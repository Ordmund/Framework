using UnityEngine;

namespace Framework.Extensions
{
	public static class VectorExtensions
	{
		public static Vector3 ClampToBounds(this Vector3 vector, Bounds bounds)
		{
			return new Vector3(Mathf.Clamp(vector.x, bounds.min.x, bounds.max.x), 
				Mathf.Clamp(vector.y, bounds.min.y, bounds.max.y), 
				Mathf.Clamp(vector.z, bounds.min.z, bounds.max.z));
		}
	}
}