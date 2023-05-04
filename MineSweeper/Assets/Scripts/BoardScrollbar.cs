using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardScrollbar : MonoBehaviour
{
    [SerializeField] private RectTransform _boardRect;
    private Board _board;
    private float _prevValue = .5f;
    private Vector3 _start, _end;

    private void Awake() 
    {
        _board = FindObjectOfType<Board>();
    }

    public void ScrollbarChanged(float newValue)
    {
        float boardWidth = _board.GetBoardWidth();
        float boardHeight = _board.GetBoardHeight();
        float change = Mathf.Sign(newValue - _prevValue);
        _prevValue = newValue;

        
        _boardRect.Translate(Vector3.right * Time.deltaTime * change);
        Debug.Log(change);
        // Debug.Log(boardHeight);
        // Debug.Log(boardWidth);
    }
}
