using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace AnotherTankGame{
    public class PauseAndSettingsMediator{
        [Inject] private AudioManager.Settings _settings;
        [Inject] private OnSFXVolChangedSignal _onSfxVolChangedSignal;
        [Inject] private OnMusicVolChangedSignal _onMusicVolChangedSignal;
        [Inject] private ChangeGameStateSignal _changeGameStateSignal;

        private const string SFX_VOL = "SFXVol";
        private const string Music_VOL = "MUSICVol";

        private PauseAndSettingsView _view;
        private bool _viewVisible;
        private float _sfxVolume;
        private float _musicVolume;

        public bool ViewVisible {
            get { return _viewVisible; }
            set {
                _viewVisible = value;
                _view.gameObject.SetActive(_viewVisible);
            }
        }

        public float SfxVolume {
            get {
                if (PlayerPrefs.HasKey(SFX_VOL)) {
                    _sfxVolume = PlayerPrefs.GetFloat(SFX_VOL);
                }
                else {
                    _sfxVolume = _settings.DefaultSfxVolume;
                }

                return _sfxVolume;
            }
        }

        public float MusicVolume {
            get {
                if (PlayerPrefs.HasKey(Music_VOL)) {
                    _musicVolume = PlayerPrefs.GetFloat(Music_VOL);
                }
                else {
                    _musicVolume = _settings.DefaultMusicVolume;
                }

                return _musicVolume;
            }
        }

        public void Initilaze(PauseAndSettingsView view) {
            Assert.IsNotNull(view);
            _view = view;
            //TODO: Добавить ассерты на кнопки и прочие UI
            _view.ToMainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
            _view.ResumeButton.onClick.AddListener(OnResumeButtonClick);
            _view.SfxSlider.onValueChanged.AddListener(OnSfxVolumeChange);
            _view.MusicSlider.onValueChanged.AddListener(OnMusicVolumeChange);
            _view.MusicToggle.onValueChanged.AddListener(OnMusicToggleChange);
            _view.SfxToggle.onValueChanged.AddListener(OnSfxToggleChange);
            SetAudioVol();
        }

        private void SetAudioVol() {
            _view.SfxSlider.value = SfxVolume;
            _view.MusicSlider.value = MusicVolume;

            if (_view.SfxSlider.value == 0.0f) {
                _view.SfxToggle.isOn = true;
            }
            else {
                _view.SfxToggle.isOn = false;
            }

            if (_view.MusicSlider.value == 0.0f) {
                _view.MusicToggle.isOn = true;
            }
            else {
                _view.MusicToggle.isOn = false;
            }
        }

        private void OnSfxVolumeChange(float value) {
            if (_view.SfxSlider.value == 0.0f) {
                _view.SfxToggle.isOn = true;
            }
            else {
                _view.SfxToggle.isOn = false;
            }

            _onSfxVolChangedSignal.Fire(value);
        }

        private void OnMusicVolumeChange(float value) {
            if (_view.MusicSlider.value == 0.0f) {
                _view.MusicToggle.isOn = true;
            }
            else {
                _view.MusicToggle.isOn = false;
            }

            _onMusicVolChangedSignal.Fire(value);
        }

        private void OnSfxToggleChange(bool value) {
            if (value) {
                _view.SfxSlider.value = 0.0f;
                _onSfxVolChangedSignal.Fire(0.0f);
            }
            else {
                _view.SfxSlider.value = _settings.DefaultSfxVolume;
            }
        }

        private void OnMusicToggleChange(bool value) {
            if (value) {
                _view.MusicSlider.value = 0.0f;
                _onMusicVolChangedSignal.Fire(0.0f);
            }
            else {
                _view.MusicSlider.value = _settings.DefaultMusicVolume;
            }
        }

        private void OnResumeButtonClick() {
            _changeGameStateSignal.Fire(GameStates.Playing, GameOverType.Blank);
        }

        private void OnMainMenuButtonClick() {
            _changeGameStateSignal.Fire(GameStates.GameOver, GameOverType.Lose);
        }

        public void Dispose() {
            _view.ToMainMenuButton.onClick.RemoveListener(OnMainMenuButtonClick);
            _view.ResumeButton.onClick.RemoveListener(OnResumeButtonClick);
        }
    }
}