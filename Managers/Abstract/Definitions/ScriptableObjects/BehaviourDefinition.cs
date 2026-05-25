using UnityEngine;

namespace Framework.Managers.Definitions
{
	public abstract class BehaviourDefinition : Definition
	{
		[SerializeField] private string _behaviourId;

		public string BehaviourId => _behaviourId;
	}
}