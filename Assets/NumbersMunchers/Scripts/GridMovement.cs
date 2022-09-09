using System.Collections.Generic;
using RangerRPG.Core;
using UnityEngine;

namespace NumbersMunchers {
    public class GridMovement: MonoBehaviour {
        private List<Vector3> _paths;
        private float elapsedDistance = 0;
        [SerializeField] private float _moveSpeed = 2f;
        private Vector3 targetPos;
        private int targetIndex = 0;
        private bool _moving = false;
        
        public bool Moving {
            get => _moving;
            set => _moving = value;
        }
        private bool _isDead = false;

        private Transform _objectTransform;
        
        private void Start() {
            _objectTransform = GetComponent<Transform>();
            _paths = new List<Vector3>();
        }

        public void SetPaths(List<Vector3> newPaths) {
            _paths.Clear();
            _paths = newPaths;
            targetIndex = -1;
            targetPos = _paths[0];
            _moving = true;
        }
        
        public void Dead(bool dead) {
            _isDead = dead;
        }

        private void Update() {
            if (!_moving || _isDead) return;
            elapsedDistance += Time.deltaTime * _moveSpeed;
            if (_objectTransform.localPosition != targetPos) {
                // Log.Debug($"Local Position {_objectTransform.localPosition}");
                _objectTransform.localPosition = Vector3.Lerp(_objectTransform.localPosition, targetPos, elapsedDistance);
            } else {
                elapsedDistance = 0;
                if (targetIndex < _paths.Count - 1) {
                    targetIndex++;
                    targetPos = _paths[targetIndex];
                } else {
                    _moving = false;
                }
            }
            
        }
    }
}