using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{   
    [SerializeField] private bool _hasMine = false, _revealed = false;

    private Vector2Int _coordinates;

    // returns number of adjacent mines (horizontal, vertical, and diagonal)

    public int GetAdjacentMines() => Board.GetAdjacentMines(this);
    public bool GetMine() => _hasMine;
    public bool GetRevealed() => _revealed;
    public Vector2Int GetCoordinates() => _coordinates;

    public void SetMine(bool b) => _hasMine = b;
    public void SetRevealed(bool b) => _revealed = b;
    public void SetCoordinates(Vector2Int v) => _coordinates = v;
}
