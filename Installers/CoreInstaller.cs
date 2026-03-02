using Core.Binders;
using Core.Managers.Injectable;
using Core.MVC;
using Zenject;

namespace Core.Installers
{
	public class CoreInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			BindTickManager();
			BindMemoryPoolBinder();
			BindGameObjectMVCFactory();
		}

		private void BindTickManager()
		{
			Container.BindInterfacesAndSelfTo<TickNotifier>().AsSingle();
		}

		private void BindMemoryPoolBinder()
		{
			Container.Bind<IMemoryPoolBinder>().To<MemoryPoolBinder>().AsSingle();
		}

		private void BindGameObjectMVCFactory()
		{
			Container.Bind<IGameObjectMVCFactory>().To<GameObjectMVCFactory>().AsSingle();
			Container.BindInterfacesAndSelfTo<PrefabsPathProvider>().AsSingle();
		}
	}
}