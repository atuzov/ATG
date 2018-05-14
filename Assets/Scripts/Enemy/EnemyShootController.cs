using System;
using UnityEngine;
using Zenject;

namespace AnotherTankGame{
    public class EnemyShootController : IFixedTickable{
        private readonly Enemy _enemy;
        readonly Settings _settings;
        readonly BulletPool _bulletPool;
        private float _lastFireTime;
        private Bullet _bullet;
        private bool _isAllowFire;
        private float _shootInterval;

        public EnemyShootController(Enemy enemy, BulletPool bulletPool, Settings settings) {
            _enemy = enemy;
            _bulletPool = bulletPool;
            _settings = settings;
            _isAllowFire = true;
            _shootInterval = UnityEngine.Random.Range(_settings.MinShootInterval, _settings.MaxShootInterval);
        }

        public void FixedTick() {
            if (_enemy.IsDead || _enemy.IsPaused) return;

            if (_isAllowFire && Time.realtimeSinceStartup - _lastFireTime > _shootInterval) {
                _lastFireTime = Time.realtimeSinceStartup;
                _shootInterval = UnityEngine.Random.Range(_settings.MinShootInterval, _settings.MaxShootInterval);
                Fire();
            }
        }

        void Fire() {
            if (_enemy.LookDir == Vector3.back || _enemy.LookDir == Vector3.forward || _enemy.LookDir == Vector3.left ||
                _enemy.LookDir == Vector3.right) {
                _bullet = _bulletPool.Spawn();
                _bullet.MoveDirection = _enemy.LookDir;
                _bullet.OnBulletColided += OnBulletDespawn;
                _bullet.BulletOwner = BulletOwnerType.Enemy;
                _bullet.transform.position = _enemy.Position + _enemy.LookDir * _settings.BulletOffsetDistance;
                _bullet.transform.rotation = Quaternion.LookRotation(_enemy.LookDir);
                _isAllowFire = false;
            }
        }

        private void OnBulletDespawn() {
            _bullet.OnBulletColided -= OnBulletDespawn;
            _isAllowFire = true;
        }

        [Serializable]
        public class Settings{
            public float BulletSpeed;
            public float MaxShootInterval;
            public float MinShootInterval;
            public float BulletOffsetDistance;
            public GameObject BulletSpawnerPoint;
        }
    }
}