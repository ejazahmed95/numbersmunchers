using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NumbersMunchers.Scripts.Questions;
using NumbersMunchers.Scripts.UI;
using RangerRPG.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NumbersMunchers.Scripts {
    public class LevelGenerator : SingletonBehaviour<LevelGenerator> {
        [SerializeField] private GameObject tileParent;
        [SerializeField] private TileView tileViewPrefab;
        [SerializeField] private Vector2Int gridSize;
        [SerializeField] private Vector2 tileSize;
        private List<List<TileView>> _tiles;
        private List<List<TileData>> _tilesData;
        [SerializeField] private PlayerController _player;
        [SerializeField] private QuestionManager questionManager;
        [SerializeField][Range(0.2f, 0.7f)] private float trueRatio;
        private ExpressionGenerator _expGen;
        private int currentLives = 3;

        private List<TileData> _emptyTiles;

        private void Start() {
            _tiles = new List<List<TileView>>();
            _tilesData = new List<List<TileData>>();
            _emptyTiles = new List<TileData>();
            CreateTiles();
            StartCoroutine(CreateNewQuestion());
            _expGen = ExpressionGenerator.Instance;
        }
        private IEnumerator CreateNewQuestion() {
            yield return new WaitForSeconds(2f);
            var question = questionManager.CreateNewQuestion();
            UIManager.Instance.InitQuestion(question);

            FillTiles(question);
        }
        
        private void FillTiles(QuestionInfo qInfo) {
            foreach (var rowTiles in _tilesData) {
                foreach (var tileData in rowTiles) {
                    if (Random.value < trueRatio) {
                        int index = Random.Range(0, qInfo.TrueNumbers.Count);
                        var expression = _expGen.GetExpression(qInfo.TrueNumbers[index]);
                        tileData.UpdateExpression(new Statement{Expression = expression, Correct = true}, true);
                    } else {
                        int index = Random.Range(0, qInfo.FalseNumbers.Count);
                        var expression = _expGen.GetExpression(qInfo.FalseNumbers[index]);
                        tileData.UpdateExpression(new Statement{Expression = expression, Correct = false}, true);
                    }
                }
            }
        }

        private void CreateTiles() {
            for (int row = 0; row < gridSize.y; row++) {
                List<TileView> rowTiles = new List<TileView>();
                List<TileData> rowTilesData = new List<TileData>();
                for (int col = 0; col < gridSize.x; col++) {
                    TileData data = new TileData(new Vector2Int(row, col));
                    int r = row, c = col;
                    TileView newTileView = Instantiate(tileViewPrefab, new Vector3(col * tileSize.x, row * tileSize.y), Quaternion.identity, tileParent.transform)
                        .Init(data, () => { OnTileClick(r, c); });
                    newTileView.gameObject.transform.localPosition = new Vector3(col * tileSize.x, row * tileSize.y);
                    rowTiles.Add(newTileView);
                    rowTilesData.Add(data);
                }
                _tiles.Add(rowTiles);
                _tilesData.Add(rowTilesData);
            }
        }

        private void OnTileClick(int row, int col) {
            Log.Info($"Tile Clicked! {col} {row}");
            var currentLoc = GetCellLocationForPosition(_player.gameObject.transform.localPosition);
            var paths = CreatePathPoints(currentLoc, new Vector2Int(col, row));
            _player.SetPaths(paths);
        }

        private Vector2Int GetCellLocationForPosition(Vector3 pos) {
            int col = (int)Math.Floor((pos.x + tileSize.x / 2) / tileSize.x);
            int row = (int)Math.Floor((pos.y + tileSize.y / 2) / tileSize.y);
            return new Vector2Int(col, row);
        }

        private List<Vector3> CreatePathPoints(Vector2Int from, Vector2Int to) {
            Log.Info($"Creating Path Points");
            List<Vector2Int> indexPaths = new List<Vector2Int>();
            int x = from.x;
            int y = from.y;

            while (x != to.x) {
                x += Math.Sign(to.x - from.x);
                indexPaths.Add(new Vector2Int(x, y));
            }

            while (y != to.y) {
                y += Math.Sign(to.y - from.y);
                indexPaths.Add(new Vector2Int(x, y));
            }
            
            List<Vector3> vecPaths = new List<Vector3>();
            foreach (var indexPath in indexPaths.Skip(0)) {
                vecPaths.Add(new Vector3(indexPath.x*tileSize.x, indexPath.y * tileSize.y));
                Log.Info($"Added to path Index={indexPath} && {new Vector3(indexPath.x*tileSize.x, indexPath.y * tileSize.y)}");
            }
            return vecPaths;
        }
        
        public void EatCandy() {
            var currentLoc = GetCellLocationForPosition(_player.gameObject.transform.localPosition);
            var tileData = _tilesData[currentLoc.y][currentLoc.x];
            if (!tileData.Active) {
                return;
            }
            if (tileData.Statement.Correct) {
                tileData.ClearStatement();
                _emptyTiles.Add(tileData);
                UIManager.Instance.UpdateScore(10);
            }
            else {
                tileData.ClearStatement();
                _emptyTiles.Add(tileData);
                UIManager.Instance.UpdateLives(--currentLives);
                if (currentLives == 0) {
                    CustomSceneLoader.LoadScene("GameOver");
                }
                _player.Respawn();
                
            }
        }
        
        public List<Vector3> GetRandomPath(Vector3 fromLoc) {
            var currentLoc = GetCellLocationForPosition(fromLoc);

            var tileData = _tilesData[Random.Range(0, gridSize.y)][Random.Range(0, gridSize.x)];
            if (_emptyTiles.Count > 0) {
                tileData = _emptyTiles[Random.Range(0, _emptyTiles.Count)];
            }
            Log.Debug($"Random Path for Enemy! = {currentLoc}; To={tileData.Index}");
            var paths = CreatePathPoints(currentLoc, new Vector2Int(tileData.Index.y, tileData.Index.x));
            return paths;
        }
        
        public void DigNumber(Vector3 fromLoc) {
            var currentLoc = GetCellLocationForPosition(fromLoc);
            TileData tileData = _tilesData[currentLoc.y][currentLoc.x];
            var qInfo = questionManager.currentQuestion;
            if (tileData.Active) return;
            if (Random.value < trueRatio) {
                int index = Random.Range(0, qInfo.TrueNumbers.Count);
                var expression = _expGen.GetExpression(qInfo.TrueNumbers[index]);
                tileData.UpdateExpression(new Statement{Expression = expression, Correct = true}, true);
            } else {
                int index = Random.Range(0, qInfo.FalseNumbers.Count);
                var expression = _expGen.GetExpression(qInfo.FalseNumbers[index]);
                tileData.UpdateExpression(new Statement{Expression = expression, Correct = false}, true);
            }
        }
    }
}