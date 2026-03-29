using System.Collections.Generic;
using UnityEngine;
using LevelGeneration;
using LevelGeneration.Generation;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private LayoutGenerator layoutGenerator;
    [SerializeField] private TilemapVisualizer tilemapVisualizer;
    [SerializeField] private StructurePlacementHelper structurePlacementHelper;

    private void Start()
    {
        this.layoutGenerator.GenerateRooms();
        int[,] tileMapArray = this.layoutGenerator.RoomsToTileArray();

        HashSet<Vector2Int> walkableTiles = new HashSet<Vector2Int>();
        HashSet<Vector2Int> cielingTiles = new HashSet<Vector2Int>();
        for (int x = 0; x < tileMapArray.GetLength(0); x++)
        {
            for (int y = 0; y < tileMapArray.GetLength(1); y++)
            {
                if (tileMapArray[x, y] == 0)
                {
                    walkableTiles.Add(new Vector2Int(x, y));
                } else if (tileMapArray[x, y] == 99)
                {
                    cielingTiles.Add(new Vector2Int(x, y));
                }
            }
        }
        
        this.tilemapVisualizer.PaintFloorTiles(walkableTiles);
        this.tilemapVisualizer.PaintCielingTiles(cielingTiles);
        WallGenerator.CreateWalls(walkableTiles, this.tilemapVisualizer);
        

        if (structurePlacementHelper != null)
            structurePlacementHelper.PlaceStructures(walkableTiles);
    }
}