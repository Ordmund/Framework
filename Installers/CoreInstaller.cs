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
			BindGameObjectMVCFactory();
		}

		private void BindTickManager()
		{
			Container.BindInterfacesAndSelfTo<TickNotifier>().AsSingle();
		}

		private void BindGameObjectMVCFactory()
		{
			Container.Bind<IGameObjectMVCFactory>().To<GameObjectMVCFactory>().AsSingle();
			Container.BindInterfacesAndSelfTo<PrefabsPathProvider>().AsSingle();
		}
	}
}