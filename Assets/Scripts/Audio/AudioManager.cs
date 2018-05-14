using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Audio;
using Zenject;

namespace AnotherTankGame{
    public class AudioManager{
        private const string SFX_VOL = "SFXVol";
        private const string Music_VOL = "MUSICVol";
        
        [Inject] private GameOverSignal _gameOverSignal;
        [Inject] private GamePausedSignal _gamePausedSignal;
        [Inject] private OnBaseExplodedSignal _onBaseExplodedSignal;
        [Inject] private OnBrickHitSignal _onBrickHitSignal;
        [Inject] private OnEnemyExplodedSignal _onEnemyExplodedSignal;
        [Inject] private OnMusicVolChangedSignal _onMusicVolChangedSignal;
        [Inject] private OnPlayerExplodedSignal _onPlayerExplodedSignal;
        [Inject] private OnPlayerShootSignal _onPlayerShootSignal;
        [Inject] private OnSFXVolChangedSignal _onSfxVolChangedSignal;
        [Inject] private OnWallHitSignal _onWallHitSignal;
        [Inject] private Settings _settings;
        [Inject] private StartGameSignal _startGameSignal;
        
        private AudioMixer _mixer;
        private AudioSource _musicSource;
        private float _musicVolume;

        private AudioSource _sfxSource;
        private float _sfxVolume;

        public float SfxVolume {
            get {
                if (PlayerPrefs.HasKey(SFX_VOL)) {
                    _sfxVolume = PlayerPrefs.GetFloat(SFX_VOL);
                }
                else {
                    _sfxVolume = _settings.DefaultSfxVolume;
                    PlayerPrefs.SetFloat(SFX_VOL, _settings.DefaultSfxVolume);
                }
                return _sfxVolume;
            }
            set {
                _sfxVolume = value;
                PlayerPrefs.SetFloat(SFX_VOL, _sfxVolume);
            }
        }

        public float MusicVolume {
            get {
                if (PlayerPrefs.HasKey(Music_VOL)) {
                    _musicVolume = PlayerPrefs.GetFloat(Music_VOL);
                }
                else {
                    _musicVolume = _settings.DefaultMusicVolume;
                    PlayerPrefs.SetFloat(Music_VOL, _settings.DefaultMusicVolume);
                }

                return _musicVolume;
            }
            set {
                _musicVolume = value;
                PlayerPrefs.SetFloat(Music_VOL, _musicVolume);
            }
        }

        public void Initialize() {
            var musicChannelHolder = GameObject.Find("Music");
            Assert.IsNotNull(musicChannelHolder);
            _musicSource = musicChannelHolder.GetComponent<AudioSource>();
            Assert.IsNotNull(_musicSource);
            var sfxChannelHolder = GameObject.Find("SFX");
            Assert.IsNotNull(sfxChannelHolder);
            _sfxSource = sfxChannelHolder.GetComponent<AudioSource>();
            Assert.IsNotNull(_sfxSource);
            var soundChennels = GameObject.Find("SoundChannels");
            Assert.IsNotNull(soundChennels);
            var audioMixerHolder = soundChennels.GetComponent<AudioMixerHolder>();
            _mixer = audioMixerHolder.AudioMixer;
            Assert.IsNotNull(_mixer);
            AddListeners();
            SetVolume();
        }

        private void OnSFXVolumeChange(float value) {
            SfxVolume = value;
            _mixer.SetFloat(SFX_VOL, LinearToDecibel(SfxVolume));
        }

        private void OnMusicVolumeChange(float value) {
            MusicVolume = value;
            _mixer.SetFloat(Music_VOL, LinearToDecibel(MusicVolume));
        }

        private void SetVolume() {
            _mixer.SetFloat(SFX_VOL, LinearToDecibel(SfxVolume));
            _mixer.SetFloat(Music_VOL, LinearToDecibel(MusicVolume));
        }

        private void AddListeners() {
            _startGameSignal += OnGameStarted;
            _gamePausedSignal += OnGamePause;
            _gameOverSignal += OnGameOver;

            _onBaseExplodedSignal += OnBaseExploded;
            _onBrickHitSignal += OnBrickHit;
            _onWallHitSignal += OnWallHit;
            _onPlayerExplodedSignal += OnPlayerExploded;
            _onPlayerShootSignal += OnPlayerShoot;
            _onEnemyExplodedSignal += OnEnemyExploded;

            _onSfxVolChangedSignal += OnSFXVolumeChange;
            _onMusicVolChangedSignal += OnMusicVolumeChange;
        }

        private void OnGameStarted() {
            _musicSource.PlayOneShot(_settings.LevelStart);
        }

        private void OnBaseExploded() {
            _sfxSource.PlayOneShot(_settings.BaseExplode);
        }

        private void OnEnemyExploded() {
            _sfxSource.PlayOneShot(_settings.EnemyExplode);
        }

        private void OnGameOver(GameOverType gameOverType) {
            if (gameOverType == GameOverType.Lose)
                _musicSource.PlayOneShot(_settings.GameOverLose);
            else if (gameOverType == GameOverType.Win) _musicSource.PlayOneShot(_settings.GameOverWin);
        }

        private void OnBrickHit() {
            _sfxSource.PlayOneShot(_settings.HitBtick);
        }

        private void OnWallHit() {
            _sfxSource.PlayOneShot(_settings.HitWall);
        }

        private void OnGamePause() {
            _sfxSource.PlayOneShot(_settings.PauseSound);
        }

        private void OnPlayerExploded() {
            _sfxSource.PlayOneShot(_settings.PlayerExplode);
        }

        private void OnPlayerShoot() {
            _sfxSource.PlayOneShot(_settings.PlayerShoot);
        }

        private float LinearToDecibel(float linear) {
            float dB;
            if (linear != 0)
                dB = 20.0f * Mathf.Log10(linear);
            else
                dB = -144.0f;
            return dB;
            
        }

        [Serializable]
        public class Settings{
            public AudioClip BaseExplode;
            public float DefaultMusicVolume;
            public float DefaultSfxVolume;
            public AudioClip EnemyExplode;
            public AudioClip GameOverLose;
            public AudioClip GameOverWin;
            public AudioClip HitBtick;
            public AudioClip HitWall;
            public AudioClip LevelStart;
            public AudioClip PauseSound;
            public AudioClip PlayerExplode;
            public AudioClip PlayerShoot;
        }
    }
}