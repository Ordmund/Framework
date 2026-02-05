namespace Core.Managers.Definitions
{
	public interface IDefinitionRegistry <TDefinition> where TDefinition : Definition
	{
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
		/// <exception cref="Exceptions.InvalidCastException">Thrown if a definition with the specified identifier exists but is not of the requested type.</exception>
		/// <exception cref="Exceptions.DefinitionNotFoundException">Thrown if no definition with the specified identifier exists.</exception>
		TConcrete Get<TConcrete>(string id) where TConcrete : TDefinition;

		/// <summary>
		/// Registers a definition using its identifier. Overwrites any existing definition with the same identifier.
		/// </summary>
		/// <param name="item">The definition to register.</param>
		void Register(TDefinition item);
	}
}