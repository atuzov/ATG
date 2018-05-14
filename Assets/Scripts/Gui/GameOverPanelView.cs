using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AnotherTankGame{
    public class GameOverPanelView : MonoBehaviour{
        [SerializeField] private Text _overText;

        public Text OverText {
            get { return _overText; }
        }

        public Button ToMainMenuBtn {
            get { return _toMainMenuBtn; }
        }

        [SerializeField] private Button _toMainMenuBtn;
    }
}