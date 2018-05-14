using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace AnotherTankGame{
    public class PlayerMoveController : IFixedTickable{
        readonly Settings _settings;
        readonly Player _player;

        public PlayerMoveController(Settings settings, Player player) {
            _settings = settings;
            _player = player;
        }

        public void FixedTick() {
            _player.Velocity = Vector3.zero;
            if (_player.IsDead || _player.IsPaused) return;
            if (_player.IsMovingLeft) {
                _player.SetRoundPosition(Vector3.left);
                _player.Rotate(0f);
                _player.AddForce(
                    Vector3.left * _settings.MoveSpeed);
            }

            if (_player.IsMovingRight) {
                _player.SetRoundPosition(Vector3.right);
                _player.Rotate(180f);
                _player.AddForce(
                    Vector3.right * _settings.MoveSpeed);
            }

            if (_player.IsMovingUp) {
                _player.SetRoundPosition(Vector3.forward);
                _player.Rotate(90f);
                _player.AddForce(
                    Vector3.forward * _settings.MoveSpeed);
            }

            if (_player.IsMovingDown) {
                _player.SetRoundPosition(Vector3.back);
                _player.Rotate(270f);
                _player.AddForce(
                    Vector3.back * _settings.MoveSpeed);
            }
        }

        [Serializable]
        public class Settings{
            public float MoveSpeed;
        }
    }
}