using UnityEngine;
using UnityEngine.Tilemaps;

//defines a state to be used
public enum Pentomino
{
    I, J, L, O, S, T, Z
}

[System.Serializable]
//struct to build with collected data later
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