using System;
using UnityEngine;
using Zenject;

namespace AnotherTankGame{
    public class PlayerInstaller : MonoInstaller{
        [SerializeField] Settings _settings = null;

        public override void InstallBindings() {
            Container.Bind<Player>().AsSingle()
                .WithArguments(_settings.Rigidbody);

            Container.BindInterfacesTo<PlayerInputController>().AsSingle();
            Container.BindInterfacesTo<PlayerMoveController>().AsSingle();
            Container.Bind<PlayerShootHandler>().AsSingle();
            Container.Bind<IFixedTickable>().To<PlayerShootHandler>().AsSingle();
        }

        [Serializable]
        public class Settings{
            public Rigidbody Rigidbody;
        }
    }
}