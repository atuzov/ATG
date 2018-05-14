using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace AnotherTankGame{
    public class MainMenuView : MonoBehaviour{
        [SerializeField] private Button _startGameButton;

        [SerializeField] private Button _quitGameButton;

        [SerializeField] private ExtendedToggleGroup _levelSelection;

        public ExtendedToggleGroup LevelSelection {
            get { return _levelSelection; }
        }

        public Button StartGameButton {
            get { return _startGameButton; }
        }

        public Button QuitGameButton {
            get { return _quitGameButton; }
        }
    }
}