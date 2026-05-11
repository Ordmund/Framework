using System;
using System.Collections.Generic;
using Core.Managers.Injectable;
using Zenject;

namespace Core.MVC
{
	public class ControllerLifetimeRegistry : IControllerLifetimeRegistry, IDisposable, ILateDisposable
	{
		private readonly IObjectReleaser _objectReleaser;

		private readonly List<IController> _controllers = new();

		public ControllerLifetimeRegistry(IObjectReleaser objectReleaser)
		{
			_objectReleaser = objectReleaser;
		}

		public void Register(IController controller)
		{
			if (_controllers.Contains(controller))
				throw new InvalidOperationException($"Controller {controller} already registered.");

			_controllers.Add(controller);
		}

		public void Release(IController controller)
		{
			if (!_controllers.Remove(controller))
				throw new InvalidOperationException($"Controller {controller} not registered.");

			_objectReleaser.Release(controller);
		}

		public void ReleaseAll()
		{
			foreach (var controller in _controllers)
			{
				_objectReleaser.Release(controller);
			}

			_controllers.Clear();
		}

		public void Dispose()
		{
			foreach (var controller in _controllers)
			{
				_objectReleaser.UnsubscribeFromTickable(controller);
				_objectReleaser.Dispose(controller);
			}
		}

		public void LateDispose()
		{
			foreach (var controller in _controllers)
			{
				_objectReleaser.LateDispose(controller);
			}

			_controllers.Clear();
		}
	}
}