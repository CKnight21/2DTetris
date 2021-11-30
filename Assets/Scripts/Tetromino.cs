using UnityEngine;
using UnityEngine.Tilemaps;

public enum Pentomino
{
    I, J, L, O, S, T, Z
}

[System.Serializable]
public struct PentominoData
{
    public Tile tile;
    public Pentomino Pentomino;

    public Vector2Int[] cells { get; private set; }
    public Vector2Int[,] wallKicks { get; private set; }

    public void Initialize()
    {
        this.cells = Data.Cells[this.Pentomino];
        this.wallKicks = Data.WallKicks[this.Pentomino];
    }

}