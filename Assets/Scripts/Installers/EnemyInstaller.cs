using System;
using UnityEngine;
using Zenject;

namespace AnotherTankGame{
    public class EnemyInstaller : MonoInstaller{
        [SerializeField] private Settings _settings;

        public override void InstallBindings() {
            EnemySignalInstaller.Install(Container);

            Container.Bind<Enemy>().AsSingle()
                .WithArguments(_settings.Renderer, _settings.Collider, _settings.Rigidbody);

            Container.Bind<EnemyAIController>().AsSingle();
            Container.Bind<IFixedTickable>().To<EnemyAIController>().AsSingle();
            Container.Bind<ITickable>().To<EnemyAIController>().AsSingle();


            Container.BindInterfacesTo<EnemyMoveController>().AsSingle();
            Container.Bind<EnemyShootController>().AsSingle();
            Container.Bind<IFixedTickable>().To<EnemyShootController>().AsSingle();
        }

        [Serializable]
        public class Settings{
            public GameObject RootObject;
            public Rigidbody Rigidbody;
            public Collider Collider;
            public MeshRenderer Renderer;
        }
    }
}