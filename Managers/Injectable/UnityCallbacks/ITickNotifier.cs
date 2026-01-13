using System;
using Zenject;

namespace Core.Managers.Injectable
{
	public interface ITickNotifier : ITickable, IFixedTickable, ILateTickable
	{
		public void SubscribeOnTick(Action action);
		public void SubscribeOnFixedTick(Action action);
		public void SubscribeOnLateTick(Action action);
		public void UnsubscribeFromTick(Action action);
		public void UnsubscribeFromFixedTick(Action action);
		public void UnsubscribeFromLateTick(Action action);
	}
}