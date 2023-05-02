using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Board : MonoBehaviour
{
    public bool GameOver { get; private set; } = true;
    public bool MinesGenerated { get; private set; } = false;

    private Player _player;

    [SerializeField] private GameObject _tile;

    [SerializeField] private int _width = 10, _height = 10, _mineAmount = 20;

    private static Tile[,] _mineBoard;
    private static float _tileSize;

    [SerializeField] private Sprite[] _sprites;

    [SerializeField] TMP_InputField _widthIn, _heightIn, _minesIn;
    [SerializeField] TextMeshProUGUI _timerDisplay;
    [SerializeField] private Image _emoticon;

    private int _revealedSafeTiles = 0, _safeTiles;
    private float _timer = 0f;

    public enum Sprites
    {
        Tile,
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Mine,
        Flag,
        Empty,
        Happy,
        Win,
        Dead
    }

    // MAX SIZE 40 width and 14 height

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
        _tileSize = _tile.GetComponent<RectTransform>().rect.width;

        _mineBoard = new Tile[_width, _height];

        // CreateTiles();
    }

    private void Update()
    {
        if (MinesGenerated && !GameOver)
        {
            _timer += Time.deltaTime;
            _timerDisplay.SetText(Mathf.FloorToInt(_timer).ToString("000"));
        }
    }

    /// <summary>
    /// Creates a board of tiles
    /// </summary>
    public void CreateTiles()
    {
        GameOver = false;
        ClearBoard();

        _timer = 0f;
        _emoticon.sprite = GetSprite(Sprites.Happy);

        try
        {
            _width = int.Parse(_widthIn.text);
            _height = int.Parse(_heightIn.text);
            _mineAmount = int.Parse(_minesIn.text);
        }
        catch (System.FormatException)
        {
            _width = _height = _mineAmount = 10;
        }

        _mineBoard = new Tile[_width, _height];

        _revealedSafeTiles = 0;
        _safeTiles = _width * _height - _mineAmount;



        // Iterating through 2D array and instantiating a tile for each position
        for (int x = 0; x < _mineBoard.GetLength(0); x++)
        {
            for (int y = 0; y < _mineBoard.GetLength(1); y++)
            {
                GameObject newTile = Instantiate(_tile);

                // Setting coordinates in tile class attached to object
                newTile.GetComponent<Tile>().SetCoordinates(new Vector2Int(x, y));

                // Parenting the tile to the canvas because UI element
                // worldPositionStays being set to false stops some weird scaling issues that can happen
                newTile.transform.SetParent(transform.parent, false);
                newTile.transform.localPosition = new Vector2(x * _tileSize, y * _tileSize);

                newTile.name = $"{x}, {y}";
                _mineBoard[x, y] = newTile.GetComponent<Tile>();
            }
        }

        CenterTiles();
    }

    /// <summary>
    /// Parents all tiles to an object that is then centered to the screen
    /// </summary>
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

        // PlantMines();
    }

    /// <summary>
    /// Interates through board array and plants mines randomly
    /// </summary>
    /// <param name="exception">Tile that should not have a mine placed (typically the one the player first clicks)</param>
    public void PlantMines(Tile exception)
    {
        if (MinesGenerated) return;

        if (_mineAmount > _width * _height)
        {
            Debug.LogWarning("Planting mines error! Too many mines for the provided field size. Assigning new value.");
            _mineAmount = _width * _height - 4;
        }

        int count = 0;

        while (count < _mineAmount)
        {
            for (int x = 0; x < _mineBoard.GetLength(0); x++)
            {
                for (int y = 0; y < _mineBoard.GetLength(1); y++)
                {
                    if (count == _mineAmount)
                        break;

                    Tile tile = _mineBoard[x, y];

                    if (Random.Range(0, 9) == 0 && !tile.GetMine() && tile != exception)
                    {
                        tile.SetMine(true);
                        count++;
                    }
                }
            }
        }

        MinesGenerated = true;
    }

    /// <summary>
    /// Clears board of all tiles
    /// </summary>
    private void ClearBoard()
    {
        MinesGenerated = false;

        for (int x = 0; x < _mineBoard.GetLength(0); x++)
        {
            for (int y = 0; y < _mineBoard.GetLength(1); y++)
            {
                if (_mineBoard[x, y]) Destroy(_mineBoard[x, y].gameObject);
            }
        }
    }

    /// <summary>
    /// Reveals all tiles with mines on the board, ignoring those without
    /// </summary>
    public void RevealBoard()
    {
        for (int x = 0; x < _mineBoard.GetLength(0); x++)
        {
            for (int y = 0; y < _mineBoard.GetLength(1); y++)
            {
                if (_mineBoard[x, y].GetMine())
                    _mineBoard[x, y].Reveal();
            }
        }
    }
    
    /// <summary>
    /// Reveals all tiles regardless if they have mines or not (used in inspector only)
    /// </summary>
    public void RevealBoard_Debug()
    {
        GameOver = true;

        for (int x = 0; x < _mineBoard.GetLength(0); x++)
        {
            for (int y = 0; y < _mineBoard.GetLength(1); y++)
            {
                if (_mineBoard[x, y]) _mineBoard[x, y].Reveal();
            }
        }
    }

    /// <summary>
    /// Reveals all adjacent tiles
    /// </summary>
    /// <param name="tile">Tile to check all other tiles relative to</param>
    public void RevealAdjacentTiles(Tile tile)
    {
        // Debug.Log(tile.name + " is revealing its nearby tiles!");
        Vector2Int coords = tile.GetCoordinates();

        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                int xPos = coords.x + x, yPos = coords.y + y;
                
                // Ensuring the coordinate is within bounds of the array and is not itself
                if ((xPos != coords.x || yPos != coords.y) && (xPos >= 0 && xPos < _mineBoard.GetLength(0)) &&
                (yPos >= 0 && yPos < _mineBoard.GetLength(1)))
                {
                    Tile otherTile = _mineBoard[xPos, yPos];

                    if (!otherTile.GetMine() && !otherTile.GetRevealed())
                        otherTile.Reveal();
                }
            }
        }
    }

    public void TileRevealed(Tile tile)
    {
        if (tile.GetMine())
        {
            PlayerLose();
        }
        else
        {
            _revealedSafeTiles++;
            if (_safeTiles == _revealedSafeTiles)
                PlayerWin();
        }
    }

    public void PlayerWin()
    {
        GameOver = true;
        _player.PlaySound(Player.SoundClips.Tada, Player.Sources.Board);
        _emoticon.sprite = GetSprite(Sprites.Win);
    }

    public void PlayerLose()
    {
        GameOver = true;
        _player.PlaySound(Player.SoundClips.Explosion, Player.Sources.Board);
        _emoticon.sprite = GetSprite(Sprites.Dead);

        RevealBoard();
    }

    /// <summary>
    /// Gets number of mines adjacent to tile
    /// </summary>
    /// <param name="tile">Tile to check around</param>
    /// <returns>Number of adjacent mines</returns>
    public static int GetAdjacentMines(Tile tile)
    {
        Vector2Int coords = tile.GetCoordinates();

        int count = 0;

        // Iterating through all adjacent tiles (not including the calling tile) and checking if they have mines
        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                int xPos = coords.x + x, yPos = coords.y + y;
                
                // Ensuring the coordinate is within bounds of the array and is not itself
                if ((xPos != coords.x || yPos != coords.y) && (xPos >= 0 && xPos < _mineBoard.GetLength(0)) &&
                (yPos >= 0 && yPos < _mineBoard.GetLength(1)))
                {
                    if (_mineBoard[xPos, yPos].GetMine())
                        count++;
                }
            }
        }

        return count;
    }


    public Sprite GetSprite(Sprites sprite) => _sprites[(int)sprite];
    public Sprite GetSprite(int i) => _sprites[i];
}
