using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tictactoe
{
    public class Tile : MonoBehaviour
    {
        public GameObject Contents; // contains the symbol one of the players placed in the tile
        public int Coords { get; private set; }

        [SerializeField] private SpriteRenderer _renderer;
        private GameObject _highlight;

        private void Awake()
        {
            _highlight = transform.GetChild(0).gameObject;
        }

        private void Start()
        {

        }

        private void Update()
        {
            
        }

        private void OnMouseEnter() // Activate the highligh object on mouse enter if the tile doesn't contain a symbol
        {
            if(GameManager.Instance.Playable && Contents == null) _highlight.SetActive(true);
        }
        private void OnMouseExit()
        {
            _highlight.SetActive(false);
        }

        private void OnMouseDown()
        {
            if (!_highlight.activeSelf) return;

            GameManager.Instance.PlaceSymbol(gameObject);

        }

        public void RemoveHighlight()
        {
            _highlight.SetActive(false);
        }

        public void SetCoords(int x, int y)
        {
            string coordsString = x.ToString() + y.ToString();
            Coords = Convert.ToInt32(coordsString);
        }

        public void WinConCheck()
        {

        }
    }
}