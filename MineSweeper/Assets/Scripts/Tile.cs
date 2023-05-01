using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private bool _hasMine = false, _revealed = false;

    // returns number of adjacent mines (horizontal, vertical, and diagonal)
    public int GetAdjacentMines()
    {
        return 0;
    }

    public bool GetMine() => _hasMine;
    public bool GetRevealed() => _revealed;

    public void SetMine(bool b) => _hasMine = b;
    public void SetRevealed(bool b) => _revealed = b;
}
