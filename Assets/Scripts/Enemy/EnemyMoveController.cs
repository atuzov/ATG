using System;
using UnityEngine;
using Zenject;

namespace AnotherTankGame{
    public class EnemyMoveController : IFixedTickable{
        private readonly Enemy _enemy;
        private readonly Settings _settings;

        public EnemyMoveController(Enemy enemy, Settings settings) {
            _enemy = enemy;
            _settings = settings;
        }

        public void FixedTick() {
            _enemy.Velocity = Vector3.zero;
            if (_enemy.IsDead || _enemy.IsPaused) return;
            if (_enemy.IsMovingLeft) {
                _enemy.Rotate(0f);
                _enemy.AddForce(
                    Vector3.left * _settings.MoveSpeed);
            }

            if (_enemy.IsMovingRight) {
                _enemy.Rotate(180f);
                _enemy.AddForce(
                    Vector3.right * _settings.MoveSpeed);
            }

            if (_enemy.IsMovingUp) {
                _enemy.Rotate(90f);
                _enemy.AddForce(
                    Vector3.forward * _settings.MoveSpeed);
            }

            if (_enemy.IsMovingDown) {
                _enemy.Rotate(270f);
                _enemy.AddForce(
                    Vector3.back * _settings.MoveSpeed);
            }
        }

        [Serializable]
        public class Settings{
            public float MoveSpeed;
        }
    }
}