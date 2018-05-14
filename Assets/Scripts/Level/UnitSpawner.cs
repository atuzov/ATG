using UnityEngine;

namespace AnotherTankGame{
    public class UnitSpawner{
        private int _unitsCount;
        private Vector3 _spawnPoint;
        private EnemyPool _enemyPool;

        public Vector3 SpawnPoint {
            get { return _spawnPoint; }
        }

        public int UnitsCount {
            get { return _unitsCount; }
        }

        public UnitSpawner(int unitsCount, Vector3 spawnPoint, EnemyPool enemyPool) {
            _unitsCount = unitsCount;
            _spawnPoint = spawnPoint;
            _enemyPool = enemyPool;
        }

        public EnemyFacade SpawnUnit() {
            if (_unitsCount <= 0) return null;

            _unitsCount--;
            var enemy = _enemyPool.Spawn();
            enemy.Position = _spawnPoint;
            enemy.IsDead = false;

            return enemy;
        }
    }
}