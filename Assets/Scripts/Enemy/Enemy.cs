using UnityEngine;

namespace AnotherTankGame{
    public class Enemy{
        readonly MeshRenderer _renderer;
        readonly Collider _collider;
        readonly Rigidbody _rigidBody;
        private bool _isMovingLeft;
        private bool _isMovingRight;
        private bool _isMovingUp;
        private bool _isMovingDown;
        private bool _isFiring;
        private Vector3 _lookDir;
        private GameObject _rootObject;
        private bool _isPaused;

        public bool IsPaused {
            get { return _isPaused; }
            set { _isPaused = value; }
        }

        public GameObject RootObject {
            get { return _rootObject; }
            set { _rootObject = value; }
        }

        public Enemy(
            Rigidbody rigidBody,
            Collider collider,
            MeshRenderer renderer) {
            _renderer = renderer;
            _collider = collider;
            _rigidBody = rigidBody;
            IsDead = true;
            _lookDir = Vector3.down;
        }

        public bool IsDead { get; set; }

        public Vector3 LookDir {
            get { return _lookDir; }
        }

        public Quaternion Rotation {
            get { return _rigidBody.rotation; }
            set { _rigidBody.rotation = value; }
        }

        public Vector3 Position {
            get { return _rigidBody.position; }
            set {
                _rootObject.transform.position = value;
                _rigidBody.position = value;
            }
        }

        public Vector3 Velocity {
            get { return _rigidBody.velocity; }
            set { _rigidBody.velocity = value; }
        }

        public void AddForce(Vector3 force) {
            _rigidBody.AddForce(force, ForceMode.Impulse);
        }

        public void Rotate(float angle) {
            _rigidBody.gameObject.transform.rotation = Quaternion.Euler(0, angle, 0);
        }

        public void ResetMoving() {
            _isMovingDown = false;
            _isMovingLeft = false;
            _isMovingUp = false;
            _isMovingRight = false;
        }

        public bool IsMovingLeft {
            get { return _isMovingLeft; }
            set {
                _lookDir = Vector3.left;
                _isMovingLeft = value;
            }
        }

        public bool IsMovingRight {
            get { return _isMovingRight; }
            set {
                _lookDir = Vector3.right;
                _isMovingRight = value;
            }
        }

        public bool IsMovingUp {
            get { return _isMovingUp; }
            set {
                _lookDir = Vector3.forward;
                _isMovingUp = value;
            }
        }

        public bool IsMovingDown {
            get { return _isMovingDown; }
            set {
                _lookDir = Vector3.back;
                _isMovingDown = value;
            }
        }

        public void Dispose() {
            IsDead = true;
            _rigidBody.velocity = Vector3.zero;
            IsMovingDown = false;
            IsMovingLeft = false;
            IsMovingRight = false;
            IsMovingUp = false;
        }
    }
}