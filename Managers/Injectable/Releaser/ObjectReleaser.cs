using System;
using Zenject;

namespace Core.Managers.Injectable
{
	public class ObjectReleaser : IObjectReleaser
	{
		private readonly TickableManager _tickableManager;

		public ObjectReleaser(TickableManager tickableManager)
		{
			_tickableManager = tickableManager;
		}

		public void Release(object instance)
		{
			if (instance is ITickable tickable)
			{
				_tickableManager.Remove(tickable);
			}

			if (instance is IFixedTickable fixedTickable)
			{
				_tickableManager.RemoveFixed(fixedTickable);
			}

			if (instance is ILateTickable lateTickable)
			{
				_tickableManager.RemoveLate(lateTickable);
			}

			if (instance is IDisposable disposable)
			{
				disposable.Dispose();
			}

			if (instance is ILateDisposable lateDisposable)
			{
				lateDisposable.LateDispose();
			}
		}
	}
}