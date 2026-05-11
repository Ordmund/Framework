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
			UnsubscribeFromTickable(instance);
			Dispose(instance);
			LateDispose(instance);
		}

		public void UnsubscribeFromTickable(object instance)
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
		}

		public void Dispose(object instance)
		{
			if (instance is IDisposable disposable)
			{
				disposable.Dispose();
			}
		}

		public void LateDispose(object instance)
		{
			if (instance is ILateDisposable lateDisposable)
			{
				lateDisposable.LateDispose();
			}
		}
	}
}