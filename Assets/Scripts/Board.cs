using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public PentominoData[] pentominoes;
    public Piece activePiece { get; private set; }
    public Vector3Int spawnPosition;

    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();

        for (int i=0; i < this.pentominoes.Length; i++)
        {
            this.pentominoes[i].Initialize();
        }
    }

    private void Start()
    {
        SpawnPiece(); 
    }

    public void SpawnPiece()
    {
        int random = Random.Range(0, this.pentominoes.Length);
        PentominoData data = this.pentominoes[random];

        this.activePiece.Initialize(this, this.spawnPosition, data);
        Set(this.activePiece);
    }

    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }
}
