using UnityEngine;
using Zenject;

namespace AnotherTankGame{
    public class HeadQuarter : MonoBehaviour{
    }

    public class HQPool : MonoMemoryPool<HeadQuarter>{
        protected override void OnDespawned(HeadQuarter hqItem) {
            hqItem.gameObject.SetActive(false);
        }

        protected override void Reinitialize(HeadQuarter hqItem) {
        }
    }
}