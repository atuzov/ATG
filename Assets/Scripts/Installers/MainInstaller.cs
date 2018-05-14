using ModestTree;
using Zenject;

namespace AnotherTankGame{
    public class MainInstaller : MonoInstaller<MainInstaller>{
        
        [Inject]
        private GameSettingsInstaller.LevelPrefabsSettings _settings = null;
        
        public override void InstallBindings() {

            SignalsInstaller.Install(Container);
            Container.Bind<GameController>().AsSingle();
            Container.Bind<UIManager>().AsSingle();
            Container.Bind<MainMenuMediator>().AsSingle();
            Container.Bind<InGameGUIMediator>().AsSingle();
            Container.Bind<PauseAndSettingsMediator>().AsSingle();
            Container.Bind<GameOverPanelMediator>().AsSingle();
            Container.Bind<LevelManager>().AsSingle();
            Container.Bind<LevelController>().AsSingle();
            Container.Bind<ITickable>().To<LevelController>().AsSingle();
            Container.Bind<AudioManager>().AsSingle();
                   
            
            Assert.IsNotNull(_settings);
            
            Container.BindMemoryPool<Block, BlockPool>()
                .WithInitialSize(25)
                .FromComponentInNewPrefab(_settings.BlockPrefab)
                .UnderTransformGroup("Blocks");
            
            Container.BindMemoryPool<HeadQuarter, HQPool>()
                .WithInitialSize(1)
                .FromComponentInNewPrefab(_settings.HeadQuarterPrefab)
                .UnderTransformGroup("Blocks");

            Container.BindMemoryPool<Bullet, BulletPool>()
                .WithFixedSize(10)
                .FromComponentInNewPrefab(_settings.BulletPrefb)
                .UnderTransformGroup("Bullets");
            
            Container.BindMemoryPool<EnemyFacade, EnemyPool>()
                .WithInitialSize(6)
                .FromSubContainerResolve()
                .ByNewPrefab(_settings.EnemyPrefab)
                .UnderTransformGroup("Enemies");
            
            Container.BindMemoryPool<PlayerFacade, PlayerPool>()
                .WithInitialSize(1)
                .FromSubContainerResolve()
                .ByNewPrefab(_settings.PlayerPrefab)
                .UnderTransformGroup("Player");

            Container.BindMemoryPool<TankIconHandler, TankIconPool>()
                .WithInitialSize(16)
                .FromComponentInNewPrefab(_settings.TankIconPrefab);

        }
    }
}