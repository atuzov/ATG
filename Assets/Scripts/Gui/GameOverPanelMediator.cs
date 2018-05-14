using System;
using Zenject;

namespace AnotherTankGame{
    public class GameOverPanelMediator : IDisposable{
        [Inject] private ChangeGameStateSignal _changeGameStateSignal;

        private GameOverPanelView _view;
        private bool _viewVisible;

        public void Initilaze(GameOverPanelView view) {
            _view = view;
            _view.ToMainMenuBtn.onClick.AddListener(OnToMainMenuClick);
        }

        public void SetToWin() {
            _view.OverText.text = "YOU ARE WIN";
        }

        public void SetToGameOver() {
            _view.OverText.text = "GAME OVER! MWA HA HA";
        }

        private void OnToMainMenuClick() {
            _changeGameStateSignal.Fire(GameStates.WaitingToStart, GameOverType.Blank);
        }

        public bool ViewVisible {
            get { return _viewVisible; }
            set {
                _viewVisible = value;
                _view.gameObject.SetActive(_viewVisible);
            }
        }

        public void Dispose() {
        }
    }
}