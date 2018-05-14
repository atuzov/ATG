using UnityEngine;
using UnityEngine.UI;

namespace AnotherTankGame{
    public class InGameView : MonoBehaviour{
        [SerializeField]
        private Button _pauseButton;

        public Button PauseButton {
            get { return _pauseButton; }
        }
        
        [SerializeField]
        private Text _levelLabelText;

        public Text LevelLabelText {
            get { return _levelLabelText; }
        }
        
        [SerializeField] private RectTransform _enemyTanksWidget;

        public RectTransform EnemyTanksWidget {
            get { return _enemyTanksWidget; }
        }

        [SerializeField] private Text _lifesCountText;

        public Text LifesCountText {
            get { return _lifesCountText; }
            set { _lifesCountText = value; }
        }
    }
}


