using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Framework.Managers.Behaviours
{
	public abstract class BehaviourFactory<TBehaviour, TFallbackBehaviour> : IBehaviourFactory<TBehaviour>, IInitializable where TBehaviour : Behaviour where TFallbackBehaviour : TBehaviour
	{
		private readonly DiContainer _container;

		private readonly Dictionary<string, Type> _behaviours = new();

		protected BehaviourFactory(DiContainer container)
		{
			_container = container;
		}

		public virtual void Initialize()
		{
			var type = typeof(TFallbackBehaviour);
			if (type.IsAbstract)
			{
				throw new FactoryInitializationException($"Fallback behaviour type '{type.Name}' cannot be an abstract class.");
			}

			Register<TFallbackBehaviour>();
		}

		public TBehaviour Create(string id, object[] extraArguments = null)
		{
			var formattedId = GetFormattedId(id);

			if (_behaviours.TryGetValue(formattedId, out var type))
			{
				return (TBehaviour)_container.Instantiate(type, extraArguments);
			}

			Debug.LogError($"The behaviour for {formattedId} not found");
			return _container.Instantiate<TFallbackBehaviour>();
		}

		public void Register<TConcrete>() where TConcrete : TBehaviour
		{
			Register<TConcrete>(GetFormattedId(typeof(TConcrete).Name));
		}

		public void Register<TConcrete>(string id) where TConcrete : TBehaviour
		{
			var type = typeof(TConcrete);

			if (type.IsAbstract)
			{
				Debug.LogError($"Cannot register abstract behaviour of type {type.Name} for Id: {id}");
				return;
			}

			if (!_behaviours.TryAdd(id, type))
			{
				Debug.LogError($"Behaviour Id {id} is already registered with {_behaviours[id].Name}");
			}
		}

		private static string GetFormattedId(string id)
		{
			return id.Trim().ToLowerInvariant();
		}
	}
}