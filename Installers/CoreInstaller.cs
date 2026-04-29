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
			Container.BindInterfacesAndSelfTo<PrefabsPathProvider>().AsSingle();
		}
	}
}