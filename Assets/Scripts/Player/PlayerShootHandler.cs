using System;
using UnityEngine;
using Zenject;

namespace AnotherTankGame{
    public class PlayerShootHandler : IFixedTickable{
        readonly Player _player;
        readonly Settings _settings;
        readonly BulletPool _bulletPool;
        private OnPlayerShootSignal _onPlayerShootSignal;
        private float _lastFireTime;

        public PlayerShootHandler(
            BulletPool bulletPool,
            Settings settings,
            Player player) {
            _player = player;
            _settings = settings;
            _bulletPool = bulletPool;
        }

        public void AttachOnShootSignal(OnPlayerShootSignal onPlayerShootSignal) {
            _onPlayerShootSignal = onPlayerShootSignal;
        }

        public void FixedTick() {
            if (_player.IsDead || _player.IsPaused) return;
            if (_player.IsFiring && Time.realtimeSinceStartup - _lastFireTime > _settings.MaxShootInterval) {
                _lastFireTime = Time.realtimeSinceStartup;
                Fire();
            }
        }

        private void Fire() {
            var bullet = _bulletPool.Spawn();
            bullet.BulletOwner = BulletOwnerType.Player;
            bullet.MoveDirection = _player.LookDir;
            bullet.transform.position = _player.Position +
                                        _player.LookDir * _settings.BulletOffsetDistance;
            _onPlayerShootSignal.Fire();
            bullet.transform.rotation = Quaternion.LookRotation(_player.LookDir);
        }

        [Serializable]
        public class Settings{
            public float BulletSpeed;
            public float MaxShootInterval;
            public float BulletOffsetDistance;
            public GameObject BulletSpawnerPoint;
        }
    }
}