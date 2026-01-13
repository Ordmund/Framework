using System;
using Zenject;

namespace Core.Controllers
{
	public abstract class ControllerBase : IDisposable
	{
		[Inject]
		public virtual void Initialize()
		{
		}

		public virtual void Dispose()
		{
		}
	}
}