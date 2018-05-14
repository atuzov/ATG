using Zenject;

namespace AnotherTankGame{
    public class EnemySignalInstaller:Installer<EnemySignalInstaller>{
        public override void InstallBindings() {
            Container.DeclareSignal<OnCollisionEnterSignal>();
            Container.DeclareSignal<OnCollisionStaySignal>();
        }
    }
}