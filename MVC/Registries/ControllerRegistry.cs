using System.Collections.Generic;

namespace Core.MVC
{
	public class ControllerRegistry : IControllerRegistry
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
	}
}