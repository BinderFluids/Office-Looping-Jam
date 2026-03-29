using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LevelGeneration.Generation
{
    public class TilemapVisualizer : MonoBehaviour
    {
        [SerializeField] private Tilemap floorTilemap, wallTilemap;
        [SerializeField] private TileBase floorTile,
            wallTop,
            wallSideRight,
            wallSiderLeft,
            wallBottom,
            wallFull,
            wallInnerCornerDownLeft,
            wallInnerCornerDownRight,
            wallDiagonalCornerDownRight,
            wallDiagonalCornerDownLeft,
            wallDiagonalCornerUpRight,
            wallDiagonalCornerUpLeft;

        public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
        {
            PaintTiles(floorPositions, floorTilemap, floorTile);
        }
        
        public Vector3 GetWorldPosition(Vector2Int tilePos)
        {
            return floorTilemap.CellToWorld((Vector3Int)tilePos);
        }

        public Vector3 GetCellCenterWorld(Vector2Int tilePos)
        {
            return floorTilemap.GetCellCenterWorld((Vector3Int)tilePos);
        }

        /// <summary>Center of a rectangle of cells whose bottom-left tile is <paramref name="bottomLeftTile"/>.</summary>
        public Vector3 GetFootprintCenterWorld(Vector2Int bottomLeftTile, Vector2Int footprintSize)
        {
            Vector3Int bl = (Vector3Int)bottomLeftTile;
            Vector3Int tr = bl + new Vector3Int(footprintSize.x - 1, footprintSize.y - 1, 0);
            Vector3 bottomLeftCenter = floorTilemap.GetCellCenterWorld(bl);
            Vector3 topRightCenter = floorTilemap.GetCellCenterWorld(tr);
            return (bottomLeftCenter + topRightCenter) * 0.5f;
        }

        public Vector3 FloorCellSizeWorld => floorTilemap.cellSize;

        private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
        {
            foreach (var position in positions)
            {
                PaintSingleTile(tilemap, tile, position);
            }
        }

        internal void PaintSingleBasicWall(Vector2Int position, string binaryType)
        {
            int typeAsInt = Convert.ToInt32(binaryType, 2);
            TileBase tile = null;
            if (WallTypesHelper.wallTop.Contains(typeAsInt))
            {
                tile = wallTop;
            }
            else if (WallTypesHelper.wallSideRight.Contains(typeAsInt))
            {
                tile = wallSideRight;
            }
            else if (WallTypesHelper.wallSideLeft.Contains(typeAsInt))
            {
                tile = wallSiderLeft;
            }
            else if (WallTypesHelper.wallBottm.Contains(typeAsInt))
            {
                tile = wallBottom;
            }
            else if (WallTypesHelper.wallFull.Contains(typeAsInt))
            {
                tile = wallFull;
            }

            if (tile != null)
                PaintSingleTile(wallTilemap, tile, position);
        }

        private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
        {
            var cellPosition = new Vector3Int(position.x, position.y, 0);
            tilemap.SetTile(cellPosition, tile);
        }

        public void Clear()
        {
            floorTilemap.ClearAllTiles();
            wallTilemap.ClearAllTiles();
        }

        internal void PaintSingleCornerWall(Vector2Int position, string binaryType)
        {
            int typeASInt = Convert.ToInt32(binaryType, 2);
            TileBase tile = null;

            if (WallTypesHelper.wallInnerCornerDownLeft.Contains(typeASInt))
            {
                tile = wallInnerCornerDownLeft;
            }
            else if (WallTypesHelper.wallInnerCornerDownRight.Contains(typeASInt))
            {
                tile = wallInnerCornerDownRight;
            }
            else if (WallTypesHelper.wallDiagonalCornerDownLeft.Contains(typeASInt))
            {
                tile = wallDiagonalCornerDownLeft;
            }
            else if (WallTypesHelper.wallDiagonalCornerDownRight.Contains(typeASInt))
            {
                tile = wallDiagonalCornerDownRight;
            }
            else if (WallTypesHelper.wallDiagonalCornerUpRight.Contains(typeASInt))
            {
                tile = wallDiagonalCornerUpRight;
            }
            else if (WallTypesHelper.wallDiagonalCornerUpLeft.Contains(typeASInt))
            {
                tile = wallDiagonalCornerUpLeft;
            }
            else if (WallTypesHelper.wallFullEightDirections.Contains(typeASInt))
            {
                tile = wallFull;
            }
            else if (WallTypesHelper.wallBottmEightDirections.Contains(typeASInt))
            {
                tile = wallBottom;
            }

            if (tile != null)
                PaintSingleTile(wallTilemap, tile, position);
        }
    }
}