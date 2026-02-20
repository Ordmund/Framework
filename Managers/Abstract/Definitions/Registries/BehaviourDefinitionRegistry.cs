namespace Core.Managers.Definitions
{
	public abstract class BehaviourDefinitionRegistry<TDefinition> : DefinitionRegistry<TDefinition>, IBehaviourDefinitionRegistry<TDefinition> where TDefinition : BehaviourDefinition
	{
		public string GetBehaviourId(string id)
		{
			return Get(id).BehaviourId;
		}
	}
}