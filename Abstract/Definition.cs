using UnityEngine;

namespace Core.Abstract
{
	public class Definition : ScriptableObject
	{
		[SerializeField] private string _id;
		[SerializeField] private string _name;

		public string Id => _id;
		public string Name => name;
	}
}