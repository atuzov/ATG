using System.Linq;
using UnityEngine;
using Zenject;

namespace AnotherTankGame{
    public enum BulletOwnerType{
        Enemy,
        Player
    }

    public class Bullet : MonoBehaviour{
        [Inject] private BulletPool _bulletPool;
        [Inject] private CollidedWithBlock _collidedWithBlockSignal;
        [Inject] private GamePausedSignal _gamePausedSignal;
        [Inject] private ResumeGameSignal _resumeGameSignal;
        [Inject] private GameOverSignal _gameOverSignal;

        public delegate void OnBulletColide();

        public event OnBulletColide OnBulletColided;

        private float _speed = 18f;
        private bool _isFlying;
        private bool _isPaused;
        private Vector3 _moveDirection;

        [SerializeField] private Rigidbody _rigidBody;

        public bool IsPaused {
            get { return _isPaused; }
            set { _isPaused = value; }
        }

        public BulletOwnerType _bulletOwner;

        public BulletOwnerType BulletOwner {
            get { return _bulletOwner; }
            set { _bulletOwner = value; }
        }

        private void OnEnable() {
            _gamePausedSignal += OnPaused;
            _resumeGameSignal += OnResume;
            _gameOverSignal += OnGameOver;
        }

        private void OnDisable() {
            _gamePausedSignal -= OnPaused;
            _resumeGameSignal -= OnResume;
            _gameOverSignal -= OnGameOver;
        }

        public Vector3 Position {
            get { return _rigidBody.position; }
            set {
                transform.position = value;
                _rigidBody.position = value;
            }
        }

        public Vector3 MoveDirection {
            get { return _moveDirection; }
            set { _moveDirection = value; }
        }

        private void OnCollisionEnter(Collision collision) {
            var enemey = collision.gameObject.GetComponent<EnemyFacade>();

            if (enemey != null && BulletOwner == BulletOwnerType.Enemy) {
                Despawn();
                return;
            }

            Despawn();
            _collidedWithBlockSignal.Fire(collision);
        }

        public void FixedUpdate() {
            if (_isPaused) return;

            _rigidBody.position += MoveDirection * _speed * Time.fixedDeltaTime;
        }

        public void Dispose() {
            Position = Vector3.zero;
            _rigidBody.velocity = Vector3.zero;
        }

        private void Despawn() {
            if (!_bulletPool.InactiveItems.Contains(this)) {
                if (OnBulletColided != null) {
                    OnBulletColided.Invoke();
                }

                _bulletPool.Despawn(this);
            }
        }

        private void OnPaused() {
            _isPaused = true;
        }

        private void OnResume() {
            _isPaused = false;
        }

        private void OnGameOver(GameOverType gameOverType) {
            _bulletPool.Despawn(this);
        }
    }

    public class BulletPool : MonoMemoryPool<Bullet>{
        protected override void OnDespawned(Bullet bullet) {
            bullet.gameObject.SetActive(false);
            bullet.Dispose();
        }

        protected override void Reinitialize(Bullet bullet) {
            bullet.IsPaused = false;
        }
    }
}