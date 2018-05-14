using System.Collections.Generic;
using UnityEngine;

namespace AnotherTankGame{
    public class EnemySpawnController{
        private int _unitsCount;
        private int _unitsInGame;
        private List<Vector3> _spawnPoints;
        private EnemyPool _enemyPool;
        private UnitSpawner[] _spawners;
        private List<EnemyFacade> _enemyInGame;
        private MapData _mapData;
        private int _spawnedUnitsCount = 0;

        public int UnitsCount {
            get { return _unitsCount; }
        }

        public int UnitsInGame {
            get { return _unitsInGame; }
        }

        public void Init(int unitsCount, List<Vector3> spawnPoints, EnemyPool enemyPool, MapData mapData) {
            _unitsCount = unitsCount;
            _spawnPoints = spawnPoints;
            _enemyPool = enemyPool;
            _enemyInGame = new List<EnemyFacade>();
            _mapData = mapData;
            CreateUnitSpawners();
        }

        private void CreateUnitSpawners() {
            _spawners = new UnitSpawner[_spawnPoints.Count];
            int numberOfUnitsPerSpawner = _unitsCount / _spawnPoints.Count;
            //TODO добавить проверку на остаток от деления
            for (int i = 0; i < _spawnPoints.Count; i++) {
                var spawner = new UnitSpawner(numberOfUnitsPerSpawner, _spawnPoints[i], _enemyPool);
                _spawners[i] = spawner;
            }
        }

        public void InitialSpawnAtStart() {
            for (int i = 0; i < _spawners.Length; i++) {
                var enemy = _spawners[i].SpawnUnit();
                enemy.MapData = _mapData;
                _enemyInGame.Add(enemy);
                _spawnedUnitsCount++;
            }
        }

        public void Dispose() {
            //TODO Добавить мехханизм переиспользования спаунеров.
            for (int i = 0; i < _spawners.Length; i++) {
                _spawners[i] = null;
            }

            foreach (var enemy in _enemyInGame) {
                _enemyPool.Despawn(enemy);
            }

            _spawnPoints = null;
            _spawners = null;
            _enemyInGame.Clear();
            _enemyInGame = null;
        }

        public void SpawnOneUnit() {
            UnitSpawner spawner = _spawners[0];
            int maxUnitInSpawner = _spawners[0].UnitsCount;
            for (int i = 0; i < _spawners.Length; i++) {
                if (maxUnitInSpawner < _spawners[i].UnitsCount) {
                    spawner = _spawners[i];
                }
            }

            var enemy = spawner.SpawnUnit();

            if (enemy == null) return;
            _spawnedUnitsCount++;
            enemy.MapData = _mapData;
            _enemyInGame.Add(enemy);
        }

        public void RemoveKilledUnit(EnemyFacade enemy) {
            _enemyPool.Despawn(enemy);
            _enemyInGame.Remove(enemy);
        }
    }

    public struct EnemyTankCount{
        public int InGameTanksCount;
        public int AllTanksCount;
    }
}