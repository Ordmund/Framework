namespace Framework.Managers.Behaviours
{
	public interface IBehaviourFactory<TBehaviour> where TBehaviour : Behaviour
	{
		/// <summary>
		/// Creates the behaviour associated with the specified identifier, or returns a fallback behaviour if none is registered.
		/// </summary>
		/// <param name="id">The behaviour identifier.</param>
		/// <param name="extraArguments">Optional extra data injected into the created behaviour.</param>
		/// <returns>The behaviour instance associated with the identifier.</returns>
		TBehaviour Create(string id, object[] extraArguments = null);

		/// <summary>
		/// Registers a new behaviour using the type name of <typeparamref name="TConcrete"/> as its identifier.
		/// </summary>
		/// <typeparam name="TConcrete">A non-abstract behaviour type.</typeparam>
		void Register<TConcrete>() where TConcrete : TBehaviour;

		/// <summary>
		/// Registers a new behaviour with the specified identifier.
		/// </summary>
		/// <typeparam name="TConcrete">A non-abstract behaviour type.</typeparam>
		/// <param name="id">The behaviour identifier.</param>
		void Register<TConcrete>(string id) where TConcrete : TBehaviour;
	}
}