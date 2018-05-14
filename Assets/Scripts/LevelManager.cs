using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace AnotherTankGame{
    public class LevelManager{
        [Inject] private LevelController _levelController;
        [Inject] private GameSettingsInstaller.LevelPrefabsSettings _levelPrefabsSettings;
        [Inject] private StartGameSignal _startGameSignal;
        [Inject] private GameOverSignal _gameOverSignal;
        [Inject] private OnLevelSelectedSignal _onLevelSelectedSignal;

        private LevelData _currentLevel;
        private GameObject _levelContainer;

        public void Initilaze(GameObject levelContainer) {
            Assert.IsNotNull(levelContainer);
            _levelContainer = levelContainer;

            Assert.IsNotNull(_levelController);
            Assert.IsNotNull(_levelPrefabsSettings);
            _currentLevel = _levelPrefabsSettings.levels[0];

            _startGameSignal += OnGameStart;
            _gameOverSignal += OnGameOver;
            _onLevelSelectedSignal += OnLevelSelected;

            _levelController.Initilize(_levelPrefabsSettings.BorderContainer);
        }

        private void OnGameStart() {
            _levelController.StartLevel(_currentLevel, _levelContainer);
        }

        private void OnGameOver(GameOverType gameOverType) {
            _levelController.Dispose();
        }

        private void OnLevelSelected(int levelNumber) {
            _currentLevel = _levelPrefabsSettings.levels[levelNumber];
        }
    }
}