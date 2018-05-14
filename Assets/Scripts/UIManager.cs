using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace AnotherTankGame{
    public class UIManager{
        [Inject] private MainMenuMediator _mainMenuMediator;
        [Inject] private InGameGUIMediator _inGameGuiMediator;
        [Inject] private PauseAndSettingsMediator _pauseAndSettingsMediator;
        [Inject] private StartGameSignal _startGameSignal;
        [Inject] private GamePausedSignal _gamePausedSignal;
        [Inject] private ResumeGameSignal _resumeGameSignal;
        [Inject] private GameOverSignal _gameOverSignal;
        [Inject] private ChangeGameStateSignal _changeGameStateSignal;
        [Inject] private GameOverPanelMediator _gameOverPanelMediator;
        [Inject] private WaitingToStartSignal _waitingToStartSignal;

        public UIManager() {
        }

        public void Initialize() {
            var canvas = GameObject.Find("Canvas");
            Assert.IsNotNull(canvas);
            Assert.IsNotNull(_mainMenuMediator);

            var mainMenuView = canvas.GetComponentInChildren<MainMenuView>(true);
            Assert.IsNotNull(mainMenuView);

            var inGameView = canvas.GetComponentInChildren<InGameView>(true);
            Assert.IsNotNull(inGameView);

            var pauseAndSettingsView = canvas.GetComponentInChildren<PauseAndSettingsView>(true);
            Assert.IsNotNull(pauseAndSettingsView);

            var gamOverPanelView = canvas.GetComponentInChildren<GameOverPanelView>(true);
            Assert.IsNotNull(gamOverPanelView);

            _mainMenuMediator.Initialize(mainMenuView);
            _inGameGuiMediator.Initilaze(inGameView);
            _pauseAndSettingsMediator.Initilaze(pauseAndSettingsView);
            _gameOverPanelMediator.Initilaze(gamOverPanelView);

            _mainMenuMediator.ViewVisible = true;
            _inGameGuiMediator.ViewVisible = false;
            _pauseAndSettingsMediator.ViewVisible = false;
            _gameOverPanelMediator.ViewVisible = false;

            ListenSignals();
        }

        private void ListenSignals() {
            _startGameSignal += OnStartGame;
            _gamePausedSignal += OnPauseGame;
            _resumeGameSignal += OnResumeGame;
            _gameOverSignal += OnGameOver;
            _waitingToStartSignal += OnWaitingToStart;
        }

        private void OnStartGame() {
            Debug.Log("OnStartGame");
            _mainMenuMediator.ViewVisible = false;
            _inGameGuiMediator.ViewVisible = true;
            _changeGameStateSignal.Fire(GameStates.Playing, GameOverType.Blank);
        }

        private void OnPauseGame() {
            Debug.Log("OnPauseGame");
            _pauseAndSettingsMediator.ViewVisible = true;
        }

        private void OnResumeGame() {
            Debug.Log("OnResumeGame");
            _pauseAndSettingsMediator.ViewVisible = false;
        }

        private void OnWaitingToStart() {
            _pauseAndSettingsMediator.ViewVisible = false;
            _gameOverPanelMediator.ViewVisible = false;
            _mainMenuMediator.ViewVisible = true;
            _inGameGuiMediator.ViewVisible = false;
        }

        private void OnGameOver(GameOverType gameOverType) {
            Debug.Log("OnGameOver");
            _pauseAndSettingsMediator.ViewVisible = false;
            _gameOverPanelMediator.ViewVisible = true;
            _mainMenuMediator.ViewVisible = false;
            _inGameGuiMediator.ViewVisible = false;

            if (gameOverType == GameOverType.Win) {
                _gameOverPanelMediator.SetToWin();
            }
            else if (gameOverType == GameOverType.Lose) {
                _gameOverPanelMediator.SetToGameOver();
            }
        }

        public void Dispose() {
            _startGameSignal -= OnStartGame;
            _gamePausedSignal -= OnPauseGame;
            _resumeGameSignal -= OnResumeGame;
            _gameOverSignal -= OnGameOver;
            _waitingToStartSignal -= OnWaitingToStart;
        }
    }

    public enum GameOverType{
        Lose,
        Win,
        Blank
    }
}