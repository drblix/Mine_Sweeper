using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PresetButtons : MonoBehaviour
{
    [SerializeField] private TMP_InputField _widthField, _heightField, _mineField;
    [SerializeField] private Image _begStar, _interStar, _expStar;

    private Board _board;

    private void Start()
    {
        _board = FindObjectOfType<Board>();
    }

    public void PresetClicked(string name)
    {
        if (name.Equals("beginner"))
        {
            _board.CreateTiles(10, 10, 10);
        }
        else if (name.Equals("intermediate"))
        {
            _board.CreateTiles(15, 15, 40);
        }
        else if (name.Equals("expert"))
        {
            _board.CreateTiles(30, 16, 99);
        }
    }

    public void PlayerWon(int height, int width, int mines)
    {
        if (height == 10 && width == 10 && mines == 10)
        {
            _begStar.gameObject.SetActive(true);
        }
        else if (height == 15 && width == 15 && mines == 40)
        {
            _interStar.gameObject.SetActive(true);
        }
        else if (height == 16 && width == 30 && mines == 99)
        {
            _expStar.gameObject.SetActive(true);
        }
    }
}
