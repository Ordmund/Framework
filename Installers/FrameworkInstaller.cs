using Framework.Binders;
using Framework.MVC;
using Zenject;

namespace Framework.Installers
{
	public class FrameworkInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			BindMemoryPoolBinder();
			BindGameObjectMVCFactory();
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