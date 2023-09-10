using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tictactoe
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public bool Playable { get; private set; } // Can players place symbols?

        [SerializeField] private GameObject[] _symbols;
        [SerializeField] private GameObject[] _tileContents = new GameObject[9];
        [SerializeField] private int[] _winCon;
        [SerializeField] private GameObject _gridManager, _board, _score;
        [SerializeField] private bool _turn = false; // true = it's first player's turn, false = it's second player's turn
        private int _p1Score, _p2Score, turnCounter;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
        private void Start()
        {
            OnGameRestart();
        }

        public void PlaceSymbol(Transform tile)
        {
            if (!Playable) return;
            Tile tileScript = tile.GetComponent<Tile>();
            tileScript.RemoveHighlight();
            if (!_turn) {
                var symbol = Instantiate(_symbols[0], tile.position, Quaternion.identity);
                tileScript.Contents = symbol;
                _turn = true;
            } else {
                var symbol = Instantiate(_symbols[1], tile.position, Quaternion.identity);
                tileScript.Contents = symbol;
                _turn = false;
            }
            turnCounter++;
            WinConCheck();
        }
        private void WinConCheck()
        {
            if (turnCounter < 5) return;
            else
            {
                for (int i = 0; i < 9; i++)
                {
                    _tileContents[i] = _gridManager.GetComponent<GridManager>().Tiles[i].GetComponent<Tile>().Contents;
                }
                // Cycle through the _winCon array, 3 tiles at a time
                for (int i = 0; i < _winCon.Length; i += 3)
                {
                    if (_tileContents[_winCon[i]] != null)
                    {
                        var nextSymbols = new List<bool>();
                        for (int j = i; j < (i + 3); j++)
                        {
                            if (_tileContents[_winCon[j]] == null) break;
                            else
                            {
                                bool symbolInTile; //false if X, true if O
                                _tileContents[_winCon[j]].TryGetComponent<X>(out var x);
                                if (x is X) symbolInTile = false;
                                else symbolInTile = true;
                                nextSymbols.Add(symbolInTile);
                                if (nextSymbols.Count > 0 && nextSymbols[0] != symbolInTile) break;
                                if (nextSymbols.Count == 3) // if 3 of the same symbol are detected in a row, call EndGame
                                {
                                    // Add the symbols to a list so that they don't get deleted when on EndGame
                                    var wSymbols = new List<GameObject>();
                                    for (int k = 0; k < 3; k++)
                                    {
                                        wSymbols.Add(_tileContents[_winCon[i + k]]);
                                    }
                                    EndGame(false, symbolInTile, wSymbols); break;
                                }
                            }
                        }
                        if (turnCounter == 9)
                        {
                            EndGame(true, false, null);
                        }
                    }
                }
            }
        }
        private void EndGame(bool tie, bool winner, List<GameObject> wSymbols)
        {
            Playable = false;
            turnCounter = 0;
            if (tie)
            {
                StartCoroutine(RestartGame(1));
            }
            else
            {
                if (!winner)
                {
                    _p1Score++;
                    _score.GetComponent<Score>().SetPlayerScore(winner, _p1Score);
                }
                else
                {
                    _p2Score++;
                    _score.GetComponent<Score>().SetPlayerScore(winner, _p2Score);
                }

                foreach (GameObject obj in _tileContents)
                {
                    if (!wSymbols.Contains(obj)) Destroy(obj);
                }

                StartCoroutine(RestartGame(1));
            }
        }

        private IEnumerator RestartGame(int s)
        {
            _board.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);

            yield return new WaitForSeconds(s);

            foreach (GameObject obj in _tileContents)
            {
                Destroy(obj);
            }

            _board.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);

            OnGameRestart();
        }

        private void OnGameRestart() // = on game restart
        {
            Playable = true;

            if ((_p1Score + _p2Score) % 2 == 0)
            {
                _turn = false;
            }
            else
            {
                _turn = true;
            }

            foreach (GameObject obj in _tileContents)
            {
                if (obj != null) Destroy(obj);
            }

            _tileContents = new GameObject[9];

            _gridManager = GameObject.Find("GridManager");
            _board = GameObject.Find("Board");
        }
    }
}
