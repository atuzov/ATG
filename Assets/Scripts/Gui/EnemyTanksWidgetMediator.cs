using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AnotherTankGame{
    public class EnemyTanksWidgetMediator{
        private EnemyTnaksWidgetView _enemyTnaksWidgetView;
        private TankIconPool _tankIconPool;
        private SetEnemyTankCountSignal _enemyTankCountSignal;
        private List<TankIconHandler> _iconList;

        public void Initialize(EnemyTnaksWidgetView view, TankIconPool tankIconPool,
            SetEnemyTankCountSignal tankCountSignal) {
            _enemyTnaksWidgetView = view;
            _tankIconPool = tankIconPool;
            _enemyTankCountSignal = tankCountSignal;
            _iconList = new List<TankIconHandler>();
            tankCountSignal += OnTankCountSet;
        }

        private void OnTankCountSet(EnemyTankCount tankCount) {
            if (_iconList.Count == 0) {
                for (int i = 0; i < tankCount.AllTanksCount; i++) {
                    var image = _tankIconPool.Spawn();
                    image.SetBlankIcon();
                    image.transform.SetParent(_enemyTnaksWidgetView.IconHolder.transform);
                    _iconList.Add(image);
                }
            }
            else {
                for (int i = 0; i < tankCount.InGameTanksCount; i++) {
                    _iconList[i].ChangeIconToFill();
                }
            }
        }

        public void Reset() {
            foreach (var icon in _iconList) {
                icon.SetBlankIcon();
            }
        }
    }
}