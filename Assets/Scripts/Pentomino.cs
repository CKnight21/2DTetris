using UnityEngine;
using UnityEngine.Tilemaps;

public enum Pentomino
{
    I,
    O,
    T,
    J,
    L,
    S,
    Z,
}

//displays in editor
[System.Serializable]
public struct PentominoData
{
    public Pentomino pentomino;
    public Tile tile;
    public Vector2Int[] cells { get; private set; }
    public Vector2Int[,] wallKicks { get; private set; }

    public void Initialize()
    {
        this.cells = Data.Cells[this.pentomino];
        this.wallKicks = Data.WallKicks[this.pentomino];
    }
}