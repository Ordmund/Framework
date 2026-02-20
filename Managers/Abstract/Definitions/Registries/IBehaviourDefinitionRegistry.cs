namespace Core.Managers.Definitions
{
	public interface IBehaviourDefinitionRegistry<TDefinition> : IDefinitionRegistry<TDefinition> where TDefinition : BehaviourDefinition
	{
		/// <summary>
		/// Retrieves the behaviour id from a definition by its identifier. If the definition is not found, returns a fallback empty behaviour id.
		/// </summary>
		/// <param name="id">The unique identifier of the definition.</param>
		/// <returns>The behaviour id of the matching definition, or a fallback empty behaviour id.</returns>
		string GetBehaviourId(string id);
	}
}