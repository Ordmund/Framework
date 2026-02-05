using System.Collections.Generic;
using Core.Abstract.Exceptions;
using Core.Managers.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace Core.Abstract
{
	public abstract class DefinitionRegistry<TDefinition> : IInitializable where TDefinition : Definition
	{
		private readonly Dictionary<string, TDefinition> _definitions = new();

		protected abstract string FallbackId { get; }
		protected abstract string DefinitionLibraryName { get; }

		public void Initialize()
		{
			var definitionsLibrary = ScriptableObjectsManager.Load<DefinitionLibrary<TDefinition>>(DefinitionLibraryName);

			foreach (var definition in definitionsLibrary.Definitions)
			{
				Register(definition);
			}

			if (!_definitions.ContainsKey(FallbackId))
			{
				throw new DefinitionNotFoundException($"Fallback definition ({FallbackId}) not found!");
			}
		}

		public TDefinition Get(string id)
		{
			if (_definitions.TryGetValue(id, out var itemDefinition))
			{
				return itemDefinition;
			}

			Debug.LogError($"{DefinitionName} with id {id} not found.");
			return _definitions[FallbackId];
		}

		public TConcrete Get<TConcrete>(string id) where TConcrete : TDefinition
		{
			if (_definitions.TryGetValue(id, out var itemDefinition))
			{
				if (itemDefinition is TConcrete specifiedDefinition)
				{
					return specifiedDefinition;
				}

				if (itemDefinition.Id != FallbackId)
				{
					throw new InvalidCastException($"Definition '{id}' exists but is of type {itemDefinition.GetType().Name}, not {DefinitionName}.");
				}
			}

			throw new DefinitionNotFoundException($"{DefinitionName} with id {id} not found.");
		}

		public void Register(TDefinition item)
		{
			if (item == null)
			{
				Debug.LogError($"Attempted to register a null {DefinitionName}");
				return;
			}

			if (string.IsNullOrEmpty(item.Id))
			{
				Debug.LogError($"{DefinitionName} : {item.Name} has an empty Id.");
				return;
			}

			if (_definitions.TryAdd(item.Id, item))
				return;

			_definitions[item.Id] = item;
			Debug.LogWarning($"{DefinitionName} '{item.Id}' is already registered and will be overridden with {item.GetType().Name}.");
		}

		private static string DefinitionName => typeof(TDefinition).Name;
	}
}