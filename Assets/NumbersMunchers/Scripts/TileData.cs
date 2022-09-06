using System;
using NumbersMunchers.Scripts.Questions;
using UnityEngine;
using UnityEngine.Events;

namespace NumbersMunchers.Scripts {
    public class TileData {

        public Vector2Int Index { get; }
        public Statement Statement;
        public bool Active;

        private UnityEvent<TileData> dataUpdateEvent;

        public TileData(Vector2Int vector2Int) {
            Index = vector2Int;
            dataUpdateEvent = new UnityEvent<TileData>();
        }

        public void Subscribe(UnityAction<TileData> OnUpdate) {
            dataUpdateEvent.AddListener(OnUpdate);
        }

        public void UpdateData() {
            dataUpdateEvent.Invoke(this);
        }
        
        public void UpdateExpression(Statement newStatement, bool active) {
            Statement = newStatement;
            Active = active;
            dataUpdateEvent.Invoke(this);
        }

        public void ClearStatement() {
            Active = false;
            dataUpdateEvent.Invoke(this);
        }
    }
}