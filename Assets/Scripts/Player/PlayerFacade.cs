using UnityEngine;
using Zenject;

namespace AnotherTankGame{
    public class PlayerFacade : MonoBehaviour{
        [Inject] private Player _player;
        [Inject] private GamePausedSignal _gamePausedSignal;
        [Inject] private ResumeGameSignal _resumeGameSignal;
        [Inject] private OnPlayerShootSignal _onPlayerShootSignal;
        [Inject] private PlayerShootHandler _playerShootHandler;

        public Player Player {
            get { return _player; }
        }

        private void OnPaused() {
            _player.IsPaused = true;
        }

        private void OnResume() {
            _player.IsPaused = false;
        }

        public void AddSignals() {
            _gamePausedSignal += OnPaused;
            _resumeGameSignal += OnResume;
            _player.IsPaused = false;
            _playerShootHandler.AttachOnShootSignal(_onPlayerShootSignal);
        }

        public void RemoveSignal() {
            _gamePausedSignal -= OnPaused;
            _resumeGameSignal -= OnResume;
        }
    }

    public class PlayerPool : MonoMemoryPool<PlayerFacade>{
        protected override void Reinitialize(PlayerFacade player) {
            player.AddSignals();
        }

        protected override void OnDespawned(PlayerFacade player) {
            player.gameObject.SetActive(false);
            player.Player.Position = new Vector3(-4, 0, -4);
            player.RemoveSignal();
        }
    }
}