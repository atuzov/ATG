using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace AnotherTankGame{
    enum ChangeDirectionMode{
        Random,
        LookForBase
    }

    public class EnemyAIController : IFixedTickable, ITickable{
        [Inject] private OnCollisionEnterSignal _collisionEnterSignal;
        [Inject] private OnCollisionStaySignal _onCollisionStaySignal;

        private Enemy _enemy;
        private Vector3[] _directions;
        private List<Vector3> _openDirections;
        private bool _doCollision = true;
        private float _coolDown = 2.0f;
        private bool _coolDowmTimerStarted;
        private ChangeDirectionMode _changeDirectionMode;
        private Vector3 _prevDir;
        private MapData _mapData;
        private Vector3 prevDir;
        private int _xTile = -1;
        private int _yTile = -1;
        private bool _isStartMoving;

        public Vector3 PrevDir {
            get { return prevDir; }
            set { prevDir = value; }
        }

        public MapData MapData {
            get { return _mapData; }
            set { _mapData = value; }
        }

        public float XTile {
            get { return _xTile; }
            set {
                int xValue = FloorPositionToCellIndex(value);
                if (xValue < 0) return;
                if (_xTile == xValue) return;
                _xTile = xValue;
                //  Debug.Log("Change _xTile " + _xTile);
                ChangeDirection();
            }
        }

        public float YTile {
            get { return _yTile; }
            set {
                int yValue = FloorPositionToCellIndex(value);
                if (yValue < 0) return;
                if (_yTile == yValue) return;
                _yTile = yValue;
                //  Debug.Log("!!!Change _yTile "+_yTile);
                ChangeDirection();
            }
        }

        private void ChangeDirection() {
            if (_xTile < 0 || _yTile < 0) return;
            if (_isStartMoving) {
                SetDirection(GetRandomDirection());
                _isStartMoving = false;
            }

            switch (_changeDirectionMode) {
                case ChangeDirectionMode.Random:
                    if (Random.Range(0, 100) < 20) {
                        SetDirection(GetRandomDirection());
                    }
                    else {
                        if (!_mapData.CheckForQuadCellIsEmpty(_xTile + (int) _enemy.LookDir.x,
                            _yTile + (int) _enemy.LookDir.z)) {
                            SetDirection(GetRandomDirection());
                        }
                    }
                    break;

                case ChangeDirectionMode.LookForBase:
                    SetDirection(GetPriorityDirectionForBase());
                    break;
            }
        }

        [Inject]
        public void Construct(Enemy enemy) {
            _enemy = enemy;
            _directions = new[] {
                Vector3.right,
                Vector3.left,
                Vector3.forward,
                Vector3.back
            };
            _isStartMoving = true;
            _openDirections = new List<Vector3>();

            _collisionEnterSignal += OnCollisionEnter;
            _onCollisionStaySignal += OnCollisionStay;

            _changeDirectionMode = ChangeDirectionMode.Random;
        }

        private int FloorPositionToCellIndex(float value) {
            float normalizedValue = value / 4;
            int integer = (int) normalizedValue;
            float fract = normalizedValue - integer;

            if (_xTile < 0 || _yTile < 0)
                return Mathf.FloorToInt(normalizedValue);

            if (fract < 0.26f) return integer;
            if (fract > 0.75f) return integer + 1;

            return -1;
        }

        private void StartTimer() {
            _doCollision = false;
            _coolDowmTimerStarted = true;
            _coolDown = 1.0f;
        }

        public void TimerTick() {
            if (!_coolDowmTimerStarted) return;
            _coolDown -= Time.deltaTime;
            if (_coolDown <= 0.0f) {
                _coolDowmTimerStarted = false;
                SetDirection(GetRandomDirection());
                _doCollision = true;
            }
        }

        public void OnCollisionEnter(Collision collision) {
            if (_xTile < 0 || _yTile < 0) return;
            if (!_doCollision) return;
            var block = collision.gameObject.transform.parent.gameObject.GetComponent<Block>();
            if (block == null) {
                SetDirection(GetRandomDirection());
            }
            else {
                StartTimer();
            }
        }

        public void OnCollisionStay(Collision collision) {
            if (!_doCollision) return;
            if (_xTile < 0 || _yTile < 0) return;
            SetDirection(GetRandomDirection());
        }

        public void CheckTile() {
            XTile = _enemy.Position.x;
            YTile = _enemy.Position.z;
        }

        private void SetDirection(Vector3 lookVector) {
            _enemy.ResetMoving();
            if (lookVector == Vector3.left) {
                _enemy.Position = new Vector3(_enemy.Position.x, 0, (_yTile * 4f) + 1);
                _enemy.IsMovingLeft = true;
            }

            if (lookVector == Vector3.right) {
                _enemy.Position = new Vector3(_enemy.Position.x, 0, (_yTile * 4f) + 1);
                _enemy.IsMovingRight = true;
            }

            if (lookVector == Vector3.forward) {
                _enemy.Position = new Vector3((_xTile * 4f) + 1, 0, _enemy.Position.z);
                _enemy.IsMovingUp = true;
            }

            if (lookVector == Vector3.back) {
                _enemy.Position = new Vector3((_xTile * 4f) + 1, 0, _enemy.Position.z);
                _enemy.IsMovingDown = true;
            }
        }

        private Vector3 GetRandomDirection() {
            _openDirections.Clear();
            foreach (var direction in _directions) {
                if (_mapData.CheckForQuadCellIsEmpty(_xTile + (int) direction.x, _yTile + (int) direction.z)) {
                    _openDirections.Add(direction);
                }
            }

            int index = Random.Range(0, _openDirections.Count);
            Vector3 lookVector = _openDirections[index];

            return lookVector;
        }

        private Vector3 GetPriorityDirectionForBase() {
            if (_mapData == null) return prevDir;

            Debug.Log("---------");
            Debug.Log("Мы в тайле x=" + _xTile + " y=" + _yTile);
            Debug.Log("Пробуем двигаться вниз");
            int xt = _xTile + (int) Vector3.back.x;
            int yt = _yTile + (int) Vector3.back.z;

            Debug.Log("Проверяем тайл xt=" + xt + " yt=" + yt);
            if (_mapData.CheckForQuadCellIsEmpty(xt, yt) && prevDir != Vector3.forward) {
                Debug.Log("Двигаемся вниз");
                prevDir = Vector3.back;
                return Vector3.back;
            }
            else {
                Debug.Log("двигаться вниз невозможно");
            }

            Debug.Log("Пробуем двигаться влево");
            xt = _xTile + (int) Vector3.left.x;
            yt = _yTile + (int) Vector3.left.z;

            Debug.Log("Проверяем тайл xt=" + xt + " yt=" + yt);
            if (_mapData.CheckForQuadCellIsEmpty(xt, yt) && prevDir != Vector3.right) {
                Debug.Log("Двигаемся влево");
                prevDir = Vector3.left;
                return Vector3.left;
            }
            else {
                Debug.Log("двигаться влево невозможно");
            }

            Debug.Log("Пробуем двигаться вправо");
            xt = _xTile + (int) Vector3.right.x;
            yt = _yTile + (int) Vector3.right.z;

            Debug.Log("Проверяем тайл xt=" + xt + " yt=" + yt);
            if (_mapData.CheckForQuadCellIsEmpty(xt, yt) && prevDir != Vector3.left) {
                Debug.Log("Двигаемся вправо");
                prevDir = Vector3.right;
                return Vector3.right;
            }
            else {
                Debug.Log("двигаться вправо невозможно");
            }

            Debug.Log("Пробуем двигаться вверх");
            xt = _xTile + (int) Vector3.forward.x;
            yt = _yTile + (int) Vector3.forward.z;

            Debug.Log("Проверяем тайл xt=" + xt + " yt=" + yt);
            if (_mapData.CheckForQuadCellIsEmpty(xt, yt) && prevDir == Vector3.back) {
                Debug.Log("Двигаемся вверх");
                prevDir = Vector3.forward;
                return Vector3.forward;
            }
            else {
                Debug.Log("двигаться вверх невозможно");
            }

            Debug.Log("prevDir " + -prevDir);
            Debug.Log("prevDir1 " + Vector3.forward);
            prevDir = -prevDir;
            return -prevDir;
        }

        public void FixedTick() {
            if (_enemy.IsDead || _enemy.IsPaused) return;
            CheckTile();
        }

        public void Tick() {
            if (_enemy.IsDead || _enemy.IsPaused) return;
            TimerTick();
        }

        public void Dispose() {
            _xTile = -1;
            _yTile = -1;
            _openDirections.Clear();
        }
    }
}