using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tictactoe
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public bool Playable { get; private set; } // Can players place symbols?

        [SerializeField] private GameObject[] _symbols;
        [SerializeField] private GameObject[] _tileContents = new GameObject[9];
        [SerializeField] private int[] _winCon;
        [SerializeField] private GameObject _gridManager, _board;
        [SerializeField] private bool _turn = false; // true = it's first player's turn, false = it's second player's turn
        private int _p1Score, _p2Score;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

            //DontDestroyOnLoad(gameObject);
        }
        private void Start()
        {
            OnSceneReload();
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
            //_turnCounter++;
            WinConCheck();
        }
        private void WinConCheck()
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
                            if (nextSymbols.Count == 3)
                            {
                                // Add the symbols to a list so that they don't get deleted when on EndGame
                                var wSymbols = new List<GameObject>();
                                for (int k = 0; k < 3; k++)
                                {
                                    wSymbols.Add(_tileContents[_winCon[i + k]]);
                                }
                                EndGame(symbolInTile, wSymbols); break;
                            }
                        }
                    }
                }
            }
        }
        private void EndGame(bool winner, List<GameObject> wSymbols)
        {
            Playable = false;

            if (!winner)
            {
                _p1Score++;
                Debug.Log("Player1 won");
            }
            else
            {
                _p2Score++;
                Debug.Log("Player2 won");
            }

            for (int i = 0; i < _tileContents.Length; i++)
            {
                if (_tileContents[i] != null && !wSymbols.Contains(_tileContents[i]))
                {
                    Destroy(_tileContents[i]);
                }
            }
            Destroy(_board);

            StartCoroutine(RestartScene(1));
        }

        private IEnumerator RestartScene(int s)
        {
            yield return new WaitForSeconds(s);

            // Get the name of the currently active scene
            string currentSceneName = SceneManager.GetActiveScene().name;

            // Reload the current scene
            SceneManager.LoadScene(currentSceneName);
            OnSceneReload();
        }

        private void OnSceneReload()
        {
            Playable = true;
            _turn = false;
        }
    }
}
