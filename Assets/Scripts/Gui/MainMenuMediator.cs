using System;
using AnotherTankGame;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Zenject;

namespace AnotherTankGame{
    public class MainMenuMediator : IDisposable{
        [Inject] private ChangeGameStateSignal _changeGameStateSignal;
        [Inject] private OnLevelSelectedSignal _onLevelSelectedSignal;

        private MainMenuView _view;
        private bool _viewVisible;

        public bool ViewVisible {
            get { return _viewVisible; }
            set {
                _viewVisible = value;
                _view.gameObject.SetActive(_viewVisible);
            }
        }

        public void Initialize(MainMenuView view) {
            Assert.IsNotNull(view);
            _view = view;
            //TODO: Добавить ассерты на кнопки
            _view.StartGameButton.onClick.AddListener(OnStartButtonClick);
            _view.QuitGameButton.onClick.AddListener(OnQuitButtonClick);
            _view.LevelSelection.OnChange += OnLevelChange;
        }

        private void OnLevelChange(Toggle toggle) {
            //TODO: Добавить более вменяемый способ связывания уровней и нажатия кнопок
            if (toggle.name == "Level 1") {
                _onLevelSelectedSignal.Fire(0);
            }
            else if (toggle.name == "Level 2") {
                _onLevelSelectedSignal.Fire(1);
            }
        }

        private void OnStartButtonClick() {
            _changeGameStateSignal.Fire(GameStates.StartGame, GameOverType.Blank);
        }

        private void OnQuitButtonClick() {
            Debug.Log("OnQuitButtonClick");
        }

        public void Dispose() {
            _view.StartGameButton.onClick.RemoveListener(OnStartButtonClick);
            _view.QuitGameButton.onClick.RemoveListener(OnQuitButtonClick);
        }
    }
}