using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public Piece activePiece { get; private set; }

    public PentominoData[] Pentominoes;
    public Vector2Int boardSize = new Vector2Int(10, 20);
    public Vector3Int spawnPosition = new Vector3Int(-1, 8, 0);

    [SerializeField] Text pointText;

    int points = 0;

    public int Points
    {
        get
        {
            return points;
        }
        set
        {
            points = value;
            UpdateHUD();
        }
    }
    //updates text field in scene with point counter value
    private void UpdateHUD()
    {
        pointText.text = points.ToString();
    }
    //actual number counter for points
    private IEnumerator CountPoints()
    {
        //adds ten points in perpetuity
        while (true)
        {
            Points += 10;

            yield return new WaitForSeconds(1);
            //resets to 0 points when text is set to "0"
            if (pointText.text == "0")
            {
                Points = 0;
            }
        }
    }


    //creating the rectangle for the board
    public RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2);
            return new RectInt(position, this.boardSize);
        }
    }

    private void Awake()
    {
        //uses getcomponent to pass data
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();
        //starts counting points
        StartCoroutine(CountPoints());
        UpdateHUD();
        for (int i = 0; i < this.Pentominoes.Length; i++)
        {
            this.Pentominoes[i].Initialize();
        }
    }

    private void Start()
    {
        SpawnPiece();
    }
    //uses random and pentdata struct to build a random piece
    public void SpawnPiece()
    {
        int random = Random.Range(0, this.Pentominoes.Length);
        PentominoData data = this.Pentominoes[random];

        this.activePiece.Initialize(this, this.spawnPosition, data);
        //checks to see if theres room for piece
        if (!IsValidPosition(this.activePiece, this.spawnPosition))
        {
            GameOver();
        }
        else
        {
            Set(this.activePiece);
        }
    }

    public void GameOver()
    {
        //clears tiles
        this.tilemap.ClearAllTiles();
        //resets point text in editor to 0
        pointText.text = "0";
    }

    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, null);
        }
    }

    public bool IsValidPosition(Piece piece, Vector3Int position)
    {
        RectInt bounds = this.Bounds;

        //position is only valid if every cell is valid
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + position;

            //out of bounds tile is invalid
            if (!bounds.Contains((Vector2Int)tilePosition))
            {
                return false;
            }

            //tile already occupies the position, thus invalid
            if (this.tilemap.HasTile(tilePosition))
            {
                return false;
            }
        }

        return true;
    }

    public void ClearLines()
    {
        RectInt bounds = this.Bounds;
        int row = bounds.yMin;

        //clear from bottom to top
        while (row < bounds.yMax)
        {
            //advance to the next row if the current is not cleared
            if (IsLineFull(row))
            {
                LineClear(row);
            }
            else
            {
                row++;
            }
        }
    }

    public bool IsLineFull(int row)
    {
        RectInt bounds = this.Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            //line is not full if a tile is missing
            if (!this.tilemap.HasTile(position))
            {
                return false;
            }
        }

        return true;
    }

    public void LineClear(int row)
    {
        RectInt bounds = this.Bounds;

        //clear all tiles in the row
        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            this.tilemap.SetTile(position, null);
        }

        //shift every row above down one
        while (row < bounds.yMax)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row + 1, 0);
                TileBase above = this.tilemap.GetTile(position);

                position = new Vector3Int(col, row, 0);
                this.tilemap.SetTile(position, above);
            }

            row++;
        }
    }

}