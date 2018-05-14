using UnityEngine;
using Zenject;

namespace AnotherTankGame{
    public enum BlockType{
        Empty,
        Brick,
        Steel,
        Como
    }

    public class Block : MonoBehaviour{
        [SerializeField] private GameObject _brickBlock;
        [SerializeField] private GameObject _steelBlock;
        [SerializeField] private GameObject _camoBlock;

        public BlockModel BlockModel;

        public void SetBlockType(BlockType blockType) {
            BlockModel = new BlockModel(blockType);
            switch (blockType) {
                case BlockType.Brick:
                    SetBlockIsBrick();
                    break;
                case BlockType.Steel:
                    SetBlockIsSteel();
                    break;
                case BlockType.Como:
                    SetBlockIsComo();
                    break;
            }
        }

        private void SetBlockIsBrick() {
            _brickBlock.SetActive(true);
            _steelBlock.SetActive(false);
            _camoBlock.SetActive(false);
        }

        private void SetBlockIsSteel() {
            _brickBlock.SetActive(false);
            _steelBlock.SetActive(true);
            _camoBlock.SetActive(false);
        }

        private void SetBlockIsComo() {
            _brickBlock.SetActive(false);
            _steelBlock.SetActive(false);
            _camoBlock.SetActive(true);
        }
    }

    public class BlockPool : MonoMemoryPool<BlockType, Block>{
        protected override void Reinitialize(BlockType type, Block block) {
            block.SetBlockType(type);
        }

        protected override void OnDespawned(Block block) {
            block.gameObject.SetActive(false);
            block.transform.position = Vector3.zero;
        }
    }

    public struct BlockModel{
        public bool IsDestroyable;
        public int Hitpoins;
        public bool IsDestroyed;
        public int XIndex;
        public int YIndex;
        public BlockType BlockType;

        public BlockModel(BlockType blockType) {
            IsDestroyed = false;
            BlockType = blockType;
            Hitpoins = 2;
            IsDestroyable = true;
            XIndex = -1;
            YIndex = -1;

            switch (blockType) {
                case BlockType.Brick:
                    Hitpoins = 2;
                    break;
                case BlockType.Steel:
                    Hitpoins = 1;
                    IsDestroyable = false;
                    break;
            }
        }
    }
}