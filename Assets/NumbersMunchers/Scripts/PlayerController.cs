using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NumbersMunchers.Scripts {
    public class PlayerController : MonoBehaviour {
        private GridMovement _gridMovement;
        private bool dead = false;

        private void Start() {
            _gridMovement = GetComponent<GridMovement>();
        }
        
        public void SetPaths(List<Vector3> pathPoints) {
            _gridMovement.SetPaths(pathPoints);
        }

        private void Update() {
            if (dead) return;
            if(Input.GetKeyDown(KeyCode.Space)) {
                LevelGenerator.Instance.EatCandy();
            }
        }
        public void Respawn() {
            _gridMovement.Dead(true);
            dead = true;
            StartCoroutine(ComeAlive());
        }
        
        private IEnumerator ComeAlive() {
            yield return new WaitForSeconds(2f);
            _gridMovement.Dead(false);
            dead = false;
        }
        
    }
}