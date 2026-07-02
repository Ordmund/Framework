using System;
using System.Collections.Generic;
using Zenject;

namespace Framework.MVC
{
	public class ControllerLifetimeRegistry : IControllerLifetimeRegistry, IDisposable, ILateDisposable
	{
		private readonly List<IController> _controllers = new();

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

			CallDispose(controller);
			CallLateDispose(controller);
		}

		public void ReleaseAll()
		{
			foreach (var controller in _controllers)
			{
				CallDispose(controller);
			}

			foreach (var controller in _controllers)
			{
				CallLateDispose(controller);
			}

			_controllers.Clear();
		}

		private static void CallDispose(IController controller)
		{
			if (controller is IDisposable disposable)
			{
				disposable.Dispose();
			}
		}

		private static void CallLateDispose(IController controller)
		{
			if (controller is ILateDisposable disposable)
			{
				disposable.LateDispose();
			}
		}

		public void Dispose()
		{
			foreach (var controller in _controllers)
			{
				CallDispose(controller);
			}
		}

		public void LateDispose()
		{
			foreach (var controller in _controllers)
			{
				CallLateDispose(controller);
			}

			_controllers.Clear();
		}
	}
}