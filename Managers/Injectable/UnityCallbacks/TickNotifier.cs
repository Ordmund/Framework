using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Managers.Injectable
{
	public class TickNotifier : ITickNotifier, IDisposable
	{
		private readonly List<Action> _onTickActions = new();
		private readonly List<Action> _onFixedTickActions = new();
		private readonly List<Action> _onLateTickActions = new();

		public void Tick()
		{
			foreach (var onTickAction in _onTickActions.ToList())
			{
				onTickAction.Invoke();
			}
		}

		public void FixedTick()
		{
			foreach (var onFixedTickAction in _onFixedTickActions.ToList())
			{
				onFixedTickAction.Invoke();
			}
		}

		public void LateTick()
		{
			foreach (var onLateTickActions in _onLateTickActions.ToList())
			{
				onLateTickActions.Invoke();
			}
		}

		public void SubscribeOnTick(Action action)
		{
			if (action != null)
			{
				_onTickActions.Add(action);
			}
		}

		public void SubscribeOnFixedTick(Action action)
		{
			if (action != null)
			{
				_onFixedTickActions.Add(action);
			}
		}

		public void SubscribeOnLateTick(Action action)
		{
			if (action != null)
			{
				_onLateTickActions.Add(action);
			}
		}

		public void UnsubscribeFromTick(Action action)
		{
			if (_onTickActions.Contains(action))
			{
				_onTickActions.Remove(action);
			}
		}

		public void UnsubscribeFromFixedTick(Action action)
		{
			if (_onFixedTickActions.Contains(action))
			{
				_onFixedTickActions.Remove(action);
			}
		}

		public void UnsubscribeFromLateTick(Action action)
		{
			if (_onLateTickActions.Contains(action))
			{
				_onLateTickActions.Remove(action);
			}
		}

		public void Dispose()
		{
			_onTickActions.Clear();
			_onFixedTickActions.Clear();
		}
	}
}