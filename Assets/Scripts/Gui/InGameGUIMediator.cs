using System;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace AnotherTankGame{
    public class InGameGUIMediator : IDisposable{
        
        [Inject] private ChangeGameStateSignal _changeGameStateSignal;
        [Inject] private SetLevelNameSignal _setLevelNameSignal;
        [Inject] private SetEnemyTankCountSignal _setEnemyTankCountSignal;
        [Inject] private SetPlayerLifesSignal _setPlayerLifesSignal;
        [Inject] private TankIconPool _tankIconPool;
        
        private InGameView _view;
        private bool _viewVisible;
        private EnemyTanksWidgetMediator _enemyTanksWidgetMediator;

        public bool ViewVisible {
            get { return _viewVisible; }
            set {
                _viewVisible = value;
                _view.gameObject.SetActive(_viewVisible);
            }
        }

        public void Initilaze(InGameView view) {
            Assert.IsNotNull(view);
            _view = view;
            Assert.IsNotNull(_view.PauseButton);
            Assert.IsNotNull( _view.LevelLabelText);
            Assert.IsNotNull(_view.LifesCountText);

            _enemyTanksWidgetMediator = new EnemyTanksWidgetMediator();
            var enemyTnaksWidgetView = _view.EnemyTanksWidget.GetComponentInChildren<EnemyTnaksWidgetView>(true);
            _enemyTanksWidgetMediator.Initialize(enemyTnaksWidgetView, _tankIconPool, _setEnemyTankCountSignal);
            _view.PauseButton.onClick.AddListener(OnPauseButtonClick);
            
            _setLevelNameSignal += OnSetLevelName;
            _changeGameStateSignal += OnChangeGameState;
            _setPlayerLifesSignal += OnPlayerLifesCountChamge;
        }

        private void OnChangeGameState(GameStates state, GameOverType gameOverType) {
            if (state == GameStates.GameOver) {
                _enemyTanksWidgetMediator.Reset();
            }
        }

        private void OnSetLevelName(string levleName) {
            _view.LevelLabelText.text = levleName;
        }

        private void OnPlayerLifesCountChamge(int lifes) {
            _view.LifesCountText.text = "Player Lifes x " + lifes.ToString();
        }

        private void OnPauseButtonClick() {
            _changeGameStateSignal.Fire(GameStates.Paused, GameOverType.Blank);
        }

        public void Dispose() {
            _view.PauseButton.onClick.RemoveListener(OnPauseButtonClick);
        }
    }
}