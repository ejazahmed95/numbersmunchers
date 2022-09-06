using System;
using RangerRPG.Core;
using TMPro;
using UnityEngine;

namespace NumbersMunchers.Scripts {
    public class TileView : MonoBehaviour {
        private Vector2Int _index;
        private Action _onClickHandle;
        [SerializeField] private GameObject pillRef;
        [SerializeField] private TMP_Text _expressionText;

        public TileView Init(TileData tileData, Action onClickCallback) {
            _index = tileData.Index;
            tileData.Subscribe(SyncNewData);
            _onClickHandle = onClickCallback;
            SyncNewData(tileData);
            return this;
        }

        private void SyncNewData(TileData data) {
            Log.Info($"Tile Data Changed for Index={_index}");

            _expressionText.text = data.Active ? data.Statement.Expression : "";
            pillRef.SetActive(data.Active);
        }

        private void OnMouseDown() {
            _onClickHandle.Invoke();
        }
        
    }
}