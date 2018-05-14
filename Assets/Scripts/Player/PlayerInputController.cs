using UnityEngine;
using Zenject;

namespace AnotherTankGame{
    public class PlayerInputController : ITickable{
        private readonly Player _playerModel;

        public PlayerInputController(Player playerModel) {
            _playerModel = playerModel;
        }

        public void Tick() {
            _playerModel.IsMovingLeft = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
            _playerModel.IsMovingRight = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
            _playerModel.IsMovingUp = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);
            _playerModel.IsMovingDown = Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);
            _playerModel.IsFiring = Input.GetKey(KeyCode.Space); //|| Input.GetMouseButton(0);
        }
    }
}