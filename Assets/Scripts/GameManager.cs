using UnityEngine;
using UnityEngine.Assertions;
using Zenject;


namespace AnotherTankGame{
    public class GameManager : MonoBehaviour{
        [Inject] private GameController _gameController;
        [Inject] private UIManager _uiManager;
        [Inject] private LevelManager _levelManager;
        [Inject] private AudioManager _audioManager;

        private void Start() {
            Init();
        }

        private void Init() {
            Assert.IsNotNull(_uiManager);
            Assert.IsNotNull(_audioManager);

            _audioManager.Initialize();
            _uiManager.Initialize();
            _gameController.Initialize();
            _levelManager.Initilaze();
        }
    }
}