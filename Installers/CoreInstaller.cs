using Framework.Binders;
using Framework.Managers.Injectable;
using Framework.MVC;
using Zenject;

namespace Framework.Installers
{
	public class CoreInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			BindObjectReleaser();
			BindMemoryPoolBinder();
			BindGameObjectMVCFactory();
		}

		private void BindObjectReleaser()
		{
			Container.Bind<IObjectReleaser>().To<ObjectReleaser>().AsSingle();
		}

		private void BindMemoryPoolBinder()
		{
			Container.Bind<IMemoryPoolBinder>().To<MemoryPoolBinder>().AsSingle();
		}

		private void BindGameObjectMVCFactory()
		{
			Container.Bind<IGameObjectMVCFactory>().To<GameObjectMVCFactory>().AsSingle();

			Container.BindInterfacesAndSelfTo<ControllerLifetimeRegistry>().AsSingle();
			Container.BindInterfacesAndSelfTo<PrefabsPathProvider>().AsSingle();
		}
	}
}