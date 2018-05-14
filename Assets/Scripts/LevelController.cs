using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace AnotherTankGame{
    public class LevelController : ITickable{
        [Inject] private BlockPool _blockPool;
        [Inject] private CollidedWithBlock _collidedWithBlockSignal;
        [Inject] private EnemyPool _enemyPool;
        [Inject] private PlayerPool _playerPool;
        [Inject] private HQPool _hqPool;
        [Inject] private ChangeGameStateSignal _changeGameStateSignal;
        [Inject] private SetLevelNameSignal _setLevelNameSignal;
        [Inject] private SetPlayerLifesSignal _setPlayerLifesSignal;
        [Inject] private SetEnemyTankCountSignal _setEnemyTankCountSignal;
        [Inject] private OnBaseExplodedSignal _onBaseExplodedSignal;
        [Inject] private OnBrickHitSignal _onBrickHitSignal;
        [Inject] private OnWallHitSignal _onWallHitSignal;
        [Inject] private OnPlayerExplodedSignal _onPlayerExplodedSignal;
        [Inject] private OnEnemyExplodedSignal _onEnemyExplodedSignal;
        
        private const int CELL_SIZE = 2;

        private Vector3 _playerSpewnPoint;
        private PlayerFacade _player;
        private HeadQuarter _headQuarter;
        private LevelData _levelData;
        private MapData _mapData;
        private List<Block> _blocks;
        private int _killedTankCount = 0;
        private EnemySpawnController _enemySpawnController;

        public void StartLevel(LevelData levelData) {
            _blocks = new List<Block>();
            _levelData = levelData;
            _setLevelNameSignal.Fire(levelData.LevelName);
            _headQuarter = _hqPool.Spawn();
            _headQuarter.transform.position = levelData.HqPosition;
            _playerSpewnPoint = levelData.PlayerSpawnVector3;
            GetMapFromTexture(_levelData.LevelGeometry);
            _mapData = new MapData(_levelData.LevelGeometry);
            _collidedWithBlockSignal += onCollidedWithBlock;
            _enemySpawnController = new EnemySpawnController();
            _enemySpawnController.Init(15, levelData.EnemySpawnPoints, _enemyPool, _mapData);

            _player = _playerPool.Spawn();
            _player.Player.Position = _playerSpewnPoint;
            _player.Player.LifesCount = 3;

            _enemySpawnController.InitialSpawnAtStart();

            EnemyTankCount tankCount = new EnemyTankCount();
            tankCount.AllTanksCount = _enemySpawnController.UnitsCount;
            tankCount.InGameTanksCount = _killedTankCount;
            _setPlayerLifesSignal.Fire(_player.Player.LifesCount);
            _setEnemyTankCountSignal.Fire(tankCount);
            SendTankCountInfo();
        }

        private void onCollidedWithBlock(Collision collision) {
            var hq = collision.gameObject.GetComponent<HeadQuarter>();
            if (hq != null) {
                _onBaseExplodedSignal.Fire();
                _changeGameStateSignal.Fire(GameStates.GameOver, GameOverType.Lose);
            }

            var enemey = collision.gameObject.GetComponent<EnemyFacade>();
            if (enemey != null) {
                OnEnemyKilled(enemey);
            }

            var player = collision.gameObject.GetComponent<PlayerFacade>();
            if (player != null) {
                OnPlayerKilled(player);
            }

            var block = collision.gameObject.transform.parent.gameObject.GetComponent<Block>();
            if (block == null) return;

            OnBlockDestroyed(block);
        }

        private void OnBlockDestroyed(Block block) {
            if (!block.BlockModel.IsDestroyable || block.BlockModel.IsDestroyed) return;

            _onBrickHitSignal.Fire();
            block.BlockModel.Hitpoins--;
            if (block.BlockModel.Hitpoins == 0) {
                block.BlockModel.IsDestroyed = true;
            }

            if (block.BlockModel.IsDestroyed) {
                _blockPool.Despawn(block);
                _blocks.Remove(block);

                _mapData.SetCellToEmpty(block.BlockModel.XIndex, block.BlockModel.YIndex);
            }
        }
        

        private void OnEnemyKilled(EnemyFacade enemey) {
            _killedTankCount++;
            _onEnemyExplodedSignal.Fire();
            _enemySpawnController.RemoveKilledUnit(enemey);
            
            SendTankCountInfo();
            _enemySpawnController.SpawnOneUnit();
            CheckWinGondition();
        }

        private void SendTankCountInfo() {
            EnemyTankCount tankCount = new EnemyTankCount();
            tankCount.AllTanksCount = _enemySpawnController.UnitsCount;
            tankCount.InGameTanksCount = _killedTankCount;
            _setEnemyTankCountSignal.Fire(tankCount);
        }

        private void CheckWinGondition() {
            if (_enemySpawnController.UnitsCount == _killedTankCount) {
                _changeGameStateSignal.Fire(GameStates.GameOver, GameOverType.Win);
            }
        }

        private void OnPlayerKilled(PlayerFacade player) {
            _onPlayerExplodedSignal.Fire();
            player.Player.DecreaseLifes();
            _setPlayerLifesSignal.Fire(_player.Player.LifesCount);
            if (player.Player.LifesCount <= 0) {
                _changeGameStateSignal.Fire(GameStates.GameOver, GameOverType.Lose);
                return;
            }

            _player.Player.Position = _playerSpewnPoint;
        }

        private void GetMapFromTexture(Texture2D texture) {
            if (texture != null) {
                Block block = null;
                for (var y = 0; y < texture.height; y++) {
                    for (var x = 0; x < texture.width; x++) {
                        Color pixelColor = texture.GetPixel(x, y);

                        if (pixelColor == Color.red) {
                            block = _blockPool.Spawn(BlockType.Brick);
                            block.transform.position = Vector3.zero;
                            block.transform.Translate(x * CELL_SIZE, 0, y * CELL_SIZE);
                            block.BlockModel.XIndex = x;
                            block.BlockModel.YIndex = y;
                            _blocks.Add(block);
                        }

                        if (pixelColor == Color.white) {
                            block = _blockPool.Spawn(BlockType.Steel);
                            block.transform.position = Vector3.zero;
                            block.transform.Translate(x * CELL_SIZE, 0, y * CELL_SIZE);
                            block.BlockModel.XIndex = x;
                            block.BlockModel.YIndex = y;
                            _blocks.Add(block);
                        }

                        if (pixelColor == Color.green) {
                            block = _blockPool.Spawn(BlockType.Como);
                            block.transform.position = Vector3.zero;
                            block.transform.Translate(x * CELL_SIZE, 0, y * CELL_SIZE);
                            block.BlockModel.XIndex = x;
                            block.BlockModel.YIndex = y;
                            _blocks.Add(block);
                        }
                    }
                }
            }
        }

        public void Tick() {
        }

        public void Dispose() {
            for (int i = 0; i < _blocks.Count; i++) {
                if (_blocks[i])
                    _blockPool.Despawn(_blocks[i]);
            }

            _blocks.Clear();

            _killedTankCount = 0;
            SendTankCountInfo();
            _enemySpawnController.Dispose();
            _hqPool.Despawn(_headQuarter);
            _playerPool.Despawn(_player);
            _collidedWithBlockSignal -= onCollidedWithBlock;
        }
    }
}