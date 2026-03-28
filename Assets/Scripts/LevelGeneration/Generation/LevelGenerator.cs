using System.Collections.Generic;
using UnityEngine;
using LevelGeneration;
using LevelGeneration.Generation;
using UnityEngine.Tilemaps;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private List<StructureLayout> structures = new List<StructureLayout>();
    [SerializeField] private int structuresToSpawn = 3;
    
    [SerializeField] private LayoutGenerator layoutGenerator;
    [SerializeField] private TilemapVisualizer tilemapVisualizer;

    private void Start()
    {
        this.layoutGenerator.GenerateRooms();
        int[,] tileMapArray = this.layoutGenerator.RoomsToTileArray();
        
        List<Vector2Int> walkableTiles = new List<Vector2Int>();
        for (int x = 0; x < tileMapArray.GetLength(0); x++)
        {
            for (int y = 0; y < tileMapArray.GetLength(1); y++)
            {
                if (tileMapArray[x, y] == 0)
                {
                    walkableTiles.Add(new Vector2Int(x, y));
                }
            }
        }
        Debug.Log("Floor Tiles to paint" + walkableTiles.Count);
        this.tilemapVisualizer.PaintFloorTiles(walkableTiles);
    }
}