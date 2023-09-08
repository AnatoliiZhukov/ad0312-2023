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
        [SerializeField] private bool _turn = false; // true = it's first player's turn, false = it's second player's turn
        //[SerializeField] private int _turnCounter; // counts placed symbols
        private List<Transform> _oList = new List<Transform>();
        private List<Transform> _xList = new List<Transform>();

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
        }
        void Start()
        {
            Playable = true;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void PlaceSymbol(GameObject tile)
        {
            if (!Playable) return;
            Tile tileScript = tile.GetComponent<Tile>();
            tileScript.RemoveHighlight();
            if (!_turn) {
                var symbol = Instantiate(_symbols[0], tile.transform.position, Quaternion.identity);
                tileScript.Contents = symbol;
                _turn = true;
                _xList.Add(symbol.transform);
            } else {
                var symbol = Instantiate(_symbols[1], tile.transform.position, Quaternion.identity);
                tileScript.Contents = symbol;
                _turn = false;
                _oList.Add(symbol.transform);
            }
            tileScript.WinConCheck();
        }
    }
}
