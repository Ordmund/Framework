namespace Core.Managers.Definitions
{
	public interface IDefinitionRegistry<TDefinition> where TDefinition : Definition
	{
		/// <summary>
		/// Retrieves the name of a definition by its identifier. Returns a fallback string if no matching definition is found.
		/// </summary>
		/// <param name="id">The unique identifier of the definition.</param>
		/// <returns>The name of the matching definition, or a fallback string.</returns>
		string GetName(string id);

		/// <summary>
		/// Retrieves a definition by its identifier. Returns a fallback definition if no matching definition exists.
		/// </summary>
		/// <param name="id">The unique identifier of the definition.</param>
		/// <returns>The matching definition, or the fallback definition.</returns>
		TDefinition Get(string id);

		/// <summary>
		/// Retrieves a definition of the specified type by its identifier. Returns the fallback definition if no matching definition exists.
		/// </summary>
		/// <param name="id">The unique identifier of the definition.</param>
		/// <typeparam name="TConcrete">The expected concrete definition type.</typeparam>
		/// <returns>The matching definition of the specified type, or the fallback definition.</returns>
		/// <exception cref="InvalidCastException">Thrown if a definition with the specified identifier exists but is not of the requested type.</exception>
		/// <exception cref="DefinitionNotFoundException">Thrown if no definition with the specified identifier exists.</exception>
		TConcrete Get<TConcrete>(string id) where TConcrete : TDefinition;

		/// <summary>
		/// Determines whether a definition with the given identifier is registered.
		/// </summary>
		/// <param name="id">The identifier to check.</param>
		/// <returns><c>true</c> if the definition is registered; otherwise, <c>false</c>.</returns>
		bool IsRegistered(string id);

		/// <summary>
		/// Registers a definition using its identifier. Overwrites any existing definition with the same identifier.
		/// </summary>
		/// <param name="item">The definition to register.</param>
		void Register(TDefinition item);
	}
}