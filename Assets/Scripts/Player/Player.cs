using UnityEngine;


namespace AnotherTankGame{
    public class Player{
        private readonly Rigidbody _rigidBody;
        private bool _isMovingLeft;
        private bool _isMovingRight;
        private bool _isMovingUp;
        private bool _isMovingDown;
        private bool _isFiring;
        private Vector3 _lookDir;
        private bool _isPaused;
        private int _lifesCount;

        public int LifesCount {
            get { return _lifesCount; }
            set { _lifesCount = value; }
        }

        public bool IsPaused {
            get { return _isPaused; }
            set { _isPaused = value; }
        }

        public Player(Rigidbody rigidBody) {
            _rigidBody = rigidBody;
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
            set { _rigidBody.position = value; }
        }

        public Vector3 Velocity {
            get { return _rigidBody.velocity; }
            set { _rigidBody.velocity = value; }
        }

        public void DecreaseLifes() {
            _lifesCount--;
            if (_lifesCount <= 0) _lifesCount = 0;
        }

        public void AddForce(Vector3 force) {
            _rigidBody.AddForce(force, ForceMode.Impulse);
        }

        public void Rotate(float angle) {
            _rigidBody.gameObject.transform.rotation = Quaternion.Euler(0, angle, 0);
        }

        public void SetRoundPosition(Vector3 dirVector3) {
            Vector3 newPos = _rigidBody.gameObject.transform.position;
            if (dirVector3 == Vector3.left || dirVector3 == Vector3.right) {
                _rigidBody.gameObject.transform.position = new Vector3(newPos.x, 0.1f, Mathf.Round(newPos.z));
            }

            if (dirVector3 == Vector3.forward || dirVector3 == Vector3.back) {
                _rigidBody.gameObject.transform.position = new Vector3(Mathf.Round(newPos.x), 0.1f, newPos.z);
            }
        }

        public bool IsMovingLeft {
            get { return _isMovingLeft; }
            set {
                _isMovingLeft = value;
                if (_isMovingLeft) {
                    _lookDir = Vector3.left;
                }
            }
        }

        public bool IsMovingRight {
            get { return _isMovingRight; }
            set {
                _isMovingRight = value;
                if (_isMovingRight) {
                    _lookDir = Vector3.right;
                }
            }
        }

        public bool IsMovingUp {
            get { return _isMovingUp; }
            set {
                _isMovingUp = value;
                if (_isMovingUp) {
                    _lookDir = Vector3.forward;
                }
            }
        }

        public bool IsMovingDown {
            get { return _isMovingDown; }
            set {
                _isMovingDown = value;
                if (_isMovingDown) {
                    _lookDir = Vector3.back;
                }
            }
        }

        public bool IsFiring {
            get { return _isFiring; }
            set { _isFiring = value; }
        }
    }
}