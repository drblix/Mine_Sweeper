using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private GameObject _tile;

    [SerializeField] private int _width = 10, _height = 10, _mineAmount = 20;

    private static Tile[,] _mineBoard;

    private static float _tileSize;

    private void Awake()
    {
        _tileSize = _tile.GetComponent<RectTransform>().rect.width;

        _mineBoard = new Tile[_width, _height];

        CreateTiles();
        CenterTiles();

        _mineBoard[1, 1].GetAdjacentMines();
    }

    private void CreateTiles()
    {
        // Iterating through 2D array and instantiating a tile for each position
        for (int x = 0; x < _mineBoard.GetLength(0); x++)
        {
            for (int y = 0; y < _mineBoard.GetLength(1); y++)
            {
                GameObject newTile = Instantiate(_tile);

                // Setting coordinates in tile class attached to object
                newTile.GetComponent<Tile>().SetCoordinates(new Vector2Int(x, y));

                // Parenting the tile to the canvas because UI element
                newTile.transform.SetParent(transform.parent);
                newTile.transform.localPosition = new Vector2(x * _tileSize, y * _tileSize);

                _mineBoard[x, y] = newTile.GetComponent<Tile>();
            }
        }
    }

    private void CenterTiles()
    {
        // Formula gets the center of the grid that was created previously
        // If we didn't subtract by the tile size divided by 2, center would be on an edge of a tile
        transform.localPosition = new Vector2((_width * _tileSize / 2f) - _tileSize / 2f, (_height * _tileSize / 2f) - _tileSize / 2f);

        // Assigning each tile's parent to be the board, then centering the board to the center of the screen
        for (int x = 0; x < _mineBoard.GetLength(0); x++)
            for (int y = 0; y < _mineBoard.GetLength(1); y++)
                _mineBoard[x, y].transform.SetParent(transform);

        transform.localPosition = Vector2.zero;
    }

    public static int GetAdjacentMines(Tile tile)
    {
        Vector2Int coords = tile.GetCoordinates();

        int count = 0;

        Debug.Log(_mineBoard.GetLength(0));
        Debug.Log(_mineBoard.GetLength(1));
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; x <= 1; y++)
            {
                Debug.Log(coords.x + x);
                Debug.Log(coords.y + y);
                if (x != 0 && y != 0 && _mineBoard[coords.x + x, coords.y + y].GetMine())
                {
                    Debug.Log(coords.x + x);
                    Debug.Log(coords.y + y);
                    count++;
                }
            }
        }

        return 0;
    }
}
