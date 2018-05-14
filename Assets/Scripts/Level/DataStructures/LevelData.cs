using System;
using System.Collections.Generic;
using UnityEngine;

namespace AnotherTankGame{
    [Serializable]
    public class LevelData{
    
        [SerializeField]
        private string _levelName;
        [SerializeField]
        private Texture2D _levelGeometry;
        [SerializeField]
        private Vector3 _playerSpawnVector3;
        [SerializeField]
        private List<Vector3> _enemySpawnPoints;

        [SerializeField] private Vector3 _HQPosition;

        public Vector3 HqPosition {
            get { return _HQPosition; }
        }


        public List<Vector3> EnemySpawnPoints {
            get { return _enemySpawnPoints; }
        }

        public Vector3 PlayerSpawnVector3 {
            get { return _playerSpawnVector3; }
        }

        public string LevelName {
            get { return _levelName; }
            set { _levelName = value; }
        }

        public Texture2D LevelGeometry {
            get { return _levelGeometry; }
            set { _levelGeometry = value; }
        }
    }
}