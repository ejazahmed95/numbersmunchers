using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NumbersMunchers.Scripts {
    public class TrogglerBasic : MonoBehaviour {
        private GridMovement _gridMovement;
        private Vector2Int _targetLocation;
        private bool _active = false;

        private void Start() {
            _gridMovement = GetComponent<GridMovement>();
            StartCoroutine(GoToNewLocation());
        }
        
        private IEnumerator GoToNewLocation() {
            yield return new WaitForSeconds(4f);
            var paths = LevelGenerator.Instance.GetRandomPath(transform.localPosition);
            _gridMovement.SetPaths(paths);
            _active = true;
        }

        public void SetPaths(List<Vector3> pathPoints) {
            _gridMovement.SetPaths(pathPoints);
        }

        private void Update() {
            if (!_active) return;
            if (_gridMovement.Moving) return;

            _active = false;
            StartCoroutine(GoToNewLocation());
            LevelGenerator.Instance.DigNumber(transform.localPosition);
        }
    }
}