using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private GameObject _tile;

    [SerializeField] private int _width = 10, _height = 10;
    private Vector2 _startPos = Vector2.zero;

    private GameObject[,] _mineBoard;

    // (rows / columns - 1) / 2
    private void Awake()
    {
        float tileSize = _tile.GetComponent<RectTransform>().rect.width;
        Debug.Log(_startPos);

        _startPos = new Vector2(_height * tileSize / 2f, ((_width - 1) * tileSize) / 2f);

        GameObject start = new GameObject("Starting Position");
        start.transform.SetParent(transform);
        start.transform.localPosition = _startPos;

        _mineBoard = new GameObject[_width, _height];

        for (int x = 0; x < _mineBoard.GetLength(0); x++)
        {
            for (int y = 0; y < _mineBoard.GetLength(1); y++)
            {
                GameObject newTile = Instantiate(_tile);
                newTile.transform.SetParent(transform);

                newTile.transform.localPosition = new Vector2(_startPos.x + x * tileSize, _startPos.y + y * tileSize);
                _startPos = new Vector2(_height * tileSize / 2f, ((_width - 1) * tileSize) / 2f);

                _mineBoard[x, y] = newTile;
           }
        }
    }
}
