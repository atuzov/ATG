using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace AnotherTankGame{
//[CreateAssetMenu(fileName = "GameSettingsInstaller", menuName = "Installers/GameSettingsInstaller")]
    public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>{
        public LevelPrefabsSettings LevelPrefabs;
        public GameSettings GamePref;
        
        [Serializable]
        public class LevelPrefabsSettings{
            
            public GameObject BlockPrefab;
            public GameObject BulletPrefb;
            public GameObject EnemyPrefab;
            public GameObject BorderContainer;
            public GameObject PlayerPrefab;
            public GameObject HeadQuarterPrefab;
            public List<LevelData> levels;
            public GameObject TankIconPrefab;

        }
        
        [Serializable]
        public class GameSettings
        {
            public PlayerMoveController.Settings PlayerMoveHandler;
            public PlayerShootHandler.Settings PlayerShootHandler;
            public EnemyMoveController.Settings EnemyMoveSettings;
            public EnemyShootController.Settings EnemyShootSettings;
            public AudioManager.Settings AudioSettings;
            public TankIconHandler.Settings TankIconSettings;

        }
        
        public override void InstallBindings()
        {
            Assert.IsNotNull(LevelPrefabs);
            Container.BindInstance(LevelPrefabs);
            Container.BindInstance(GamePref.PlayerMoveHandler);
            Container.BindInstance(GamePref.PlayerShootHandler);
            Container.BindInstance(GamePref.EnemyMoveSettings);
            Container.BindInstance(GamePref.EnemyShootSettings);
            Container.BindInstances(GamePref.AudioSettings);
            Container.BindInstances(GamePref.TankIconSettings);
        }
    }
}
