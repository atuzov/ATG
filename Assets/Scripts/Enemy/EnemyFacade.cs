using UnityEngine;
using Zenject;

namespace AnotherTankGame{
    public class EnemyFacade : MonoBehaviour{

        [Inject] private OnCollisionEnterSignal _collisionEnterSignal;
        [Inject] private OnCollisionStaySignal _onCollisionStaySignal;

        [Inject] private GamePausedSignal _gamePausedSignal;
        [Inject] private ResumeGameSignal _resumeGameSignal;
        
        private Enemy _enemy;
        private EnemyAIController _aiController;

        public MapData MapData {
            get { return _aiController.MapData; }
            set { _aiController.MapData = value; }
        }

        public Vector3 Position {
            get { return _enemy.Position; }
            set {
                gameObject.transform.position = value;
                _enemy.Position = value;
            }
        }

        [Inject]
        public void Construct(Enemy enemy, EnemyAIController aiController) {
            _enemy = enemy;
            _enemy.RootObject = gameObject;
            _aiController = aiController;
        }

        private void OnCollisionEnter(Collision collision) {
            if (_enemy.IsDead) return;
            _collisionEnterSignal.Fire(collision);
        }

        private void OnCollisionStay(Collision collision) {
            if (_enemy.IsDead) return;
            _onCollisionStaySignal.Fire(collision);
        }

        public bool IsDead {
            get { return _enemy.IsDead; }
            set { _enemy.IsDead = value; }
        }

        private void FixedUpdate() {
            if (_enemy.IsPaused) return;
            gameObject.transform.position = _enemy.Position;
        }

        private void OnPaused() {
            _enemy.IsPaused = true;
        }

        private void OnResume() {
            _enemy.IsPaused = false;
        }

        public void AddSignals() {
            _gamePausedSignal += OnPaused;
            _resumeGameSignal += OnResume;
            _enemy.IsPaused = false;
        }

        public void RemoveSignal() {
            _gamePausedSignal -= OnPaused;
            _resumeGameSignal -= OnResume;
        }

        public void Dispose() {
            _enemy.Dispose();
            _aiController.Dispose();
        }
    }

    public class EnemyPool : MonoMemoryPool<EnemyFacade>{
        protected override void Reinitialize(EnemyFacade enemy) {
            enemy.AddSignals();
            enemy.Position = new Vector3(-10, -10, -10);
        }

        protected override void OnDespawned(EnemyFacade enemy) {
            enemy.IsDead = true;
            enemy.Dispose();
            enemy.Position = new Vector3(-10, -10, -10);
            enemy.RemoveSignal();
            enemy.gameObject.SetActive(false);
        }
    }
}