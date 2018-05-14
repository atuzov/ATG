
using Zenject;

namespace AnotherTankGame{

    public class SignalsInstaller: Installer<SignalsInstaller>  {

        public override void InstallBindings() {
            
            Container.DeclareSignal<ChangeGameStateSignal>();
            Container.DeclareSignal<StartGameSignal>();
            Container.DeclareSignal<GamePausedSignal>();
            Container.DeclareSignal<GameOverSignal>();
            Container.DeclareSignal<ResumeGameSignal>();
            Container.DeclareSignal<WaitingToStartSignal>();
            Container.DeclareSignal<CollidedWithBlock>();
            Container.DeclareSignal<DebugSignal>();
            Container.DeclareSignal<BulletIsDespawnedSignal>();
            Container.DeclareSignal<SetLevelNameSignal>();
            Container.DeclareSignal<SetPlayerLifesSignal>();
            Container.DeclareSignal<SetEnemyTankCountSignal>();
            Container.DeclareSignal<OnBaseExplodedSignal>();
            Container.DeclareSignal<OnBrickHitSignal>();
            Container.DeclareSignal<OnWallHitSignal>();
            Container.DeclareSignal<OnPlayerExplodedSignal>();
            Container.DeclareSignal<OnPlayerShootSignal>();
            Container.DeclareSignal<OnEnemyExplodedSignal>();
            Container.DeclareSignal<OnSFXVolChangedSignal>();
            Container.DeclareSignal<OnMusicVolChangedSignal>();
            Container.DeclareSignal<OnLevelSelectedSignal>();
        }
    }
}


