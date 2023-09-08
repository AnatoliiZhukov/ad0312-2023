using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tictactoe
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private Grid _grid;
        [SerializeField] private GameObject _tilePrefab;

        private void Start()
        {
            GenerateGrid(); // Generate grid on start
        }

        private void GenerateGrid() // Generates a 3x3 grid and renames all the tiles accordingly
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    var worldPosition = _grid.GetCellCenterWorld(new Vector3Int(x, y));
                    var spawnedTile = Instantiate(_tilePrefab, worldPosition, Quaternion.identity);
                    spawnedTile.name = $"Tile {x} {y}";
                    spawnedTile.GetComponent<Tile>().SetCoords(x, y);
                }
            }
        }
    }
}