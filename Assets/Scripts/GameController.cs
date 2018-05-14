using UnityEngine;
using Zenject;

namespace AnotherTankGame{
    public enum GameStates{
        WaitingToStart,
        StartGame,
        Playing,
        Paused,
        GameOver
    }

    public class GameController{
        [Inject] private ChangeGameStateSignal _changeGameStateSignal;
        [Inject] private StartGameSignal _startGameSignal;
        [Inject] private GamePausedSignal _gamePausedSignal;
        [Inject] private ResumeGameSignal _resumeGameSignal;
        [Inject] private GameOverSignal _gameOverSignal;
        [Inject] private WaitingToStartSignal _waitingToStartSignal;

        private GameStates _currentState;

        public GameStates CurrentState {
            get { return _currentState; }
        }

        public GameController() {
            _currentState = GameStates.WaitingToStart;
        }

        public void Initialize() {
            _changeGameStateSignal += ChangeGameState;
        }

        private void ChangeGameState(GameStates state, GameOverType gameOverType) {
            if (_currentState == state) return;

            switch (state) {
                case GameStates.WaitingToStart: {
                    _currentState = GameStates.WaitingToStart;
                    _waitingToStartSignal.Fire();
                    break;
                }
                case GameStates.Playing: {
                    if (_currentState == GameStates.Paused)
                        _resumeGameSignal.Fire();

                    _currentState = GameStates.Playing;
                    Debug.Log(GameStates.Playing);
                    break;
                }
                case GameStates.GameOver: {
                    _currentState = GameStates.GameOver;

                    if (gameOverType == GameOverType.Win) {
                        _gameOverSignal.Fire(GameOverType.Win);
                    }
                    else if (gameOverType == GameOverType.Lose) {
                        _gameOverSignal.Fire(GameOverType.Lose);
                    }

                    break;
                }
                case GameStates.StartGame: {
                    _currentState = GameStates.StartGame;
                    Debug.Log(_currentState);
                    _startGameSignal.Fire();
                    break;
                }
                case GameStates.Paused: {
                    _currentState = GameStates.Paused;
                    Debug.Log(_currentState);
                    _gamePausedSignal.Fire();

                    break;
                }
                default: {
                    break;
                }
            }
        }
    }
}