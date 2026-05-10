using System.Collections.Generic;
using Zenject;

namespace Core.MVC
{
	public class ControllerRegistry : IControllerRegistry, ILateDisposable
	{
		private readonly List<IController> _controllers = new();

		public void Register(IController controller)
		{
			if (!_controllers.Contains(controller))
			{
				_controllers.Add(controller);
			}
		}

		public void Release(IController controller)
		{
			if (_controllers.Contains(controller))
			{
				_controllers.Remove(controller);
			}
		}

		public void ReleaseAll()
		{
			_controllers.Clear();
		}

		public void LateDispose()
		{
			ReleaseAll();
		}
	}
}