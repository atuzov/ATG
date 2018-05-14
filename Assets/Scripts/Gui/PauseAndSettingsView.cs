using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AnotherTankGame{
    public class PauseAndSettingsView : MonoBehaviour{
        [SerializeField] private Text _sfxLabelText;
        [SerializeField] private Text _musicLabelText;
        [SerializeField] private Slider _sfxSlider;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Toggle _sfxToggle;
        [SerializeField] private Toggle _musicToggle;
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _toMainMenuButton;

        public Text SfxLabelText {
            get { return _sfxLabelText; }
        }

        public Text MusicLabelText {
            get { return _musicLabelText; }
        }

        public Slider SfxSlider {
            get { return _sfxSlider; }
        }

        public Slider MusicSlider {
            get { return _musicSlider; }
        }

        public Toggle SfxToggle {
            get { return _sfxToggle; }
        }

        public Toggle MusicToggle {
            get { return _musicToggle; }
        }

        public Button ResumeButton {
            get { return _resumeButton; }
        }

        public Button ToMainMenuButton {
            get { return _toMainMenuButton; }
        }
    }
}