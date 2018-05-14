using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace AnotherTankGame{
    public class TankIconHandler : MonoBehaviour{
        [Inject] private Settings _settings;

        [SerializeField] private Image _tankIcon;

        public void SetBlankIcon() {
            _tankIcon.sprite = _settings.BlankTankSprite;
        }

        public void SetFiiledIcon() {
            _tankIcon.sprite = _settings.DestroedTankSprite;
        }

        public void ChangeIconToFill() {
            if (_tankIcon.sprite == _settings.BlankTankSprite) {
                SetFiiledIcon();
            }
        }

        [Serializable]
        public class Settings{
            public Sprite BlankTankSprite;
            public Sprite DestroedTankSprite;
        }
    }

    public class TankIconPool : MonoMemoryPool<TankIconHandler>{
        protected override void OnDespawned(TankIconHandler handler) {
            handler.transform.SetParent(null);
        }

        protected override void Reinitialize(TankIconHandler handler) {
            handler.SetBlankIcon();
        }
    }
}