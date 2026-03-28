using System.Collections.Generic;
using LevelGeneration;
using UnityEngine;

namespace LevelGeneration.Generation
{
    public class StructurePlacementHelper : MonoBehaviour
    {
        [SerializeField] private List<StructureLayout> structures = new List<StructureLayout>();
        [SerializeField] private TilemapVisualizer tilemapVisualizer;
        [SerializeField] private Transform structureParent;

        /// <summary>
        /// Spawns structure prefabs on walkable floor cells using each layout's <see cref="StructureRule"/>.
        /// Order the list so layouts with <see cref="StructureRule.mustBeAdjacentToStructures"/> come after
        /// layouts they must touch (or use empty adjacency lists for structures that must spawn first).
        /// </summary>
        public void PlaceStructures(HashSet<Vector2Int> walkableTiles)
        {
            HashSet<Vector2Int> occupiedTiles = new();

            foreach (var structure in structures)
            {
                Vector2Int size = Vector2Int.Max(structure.footprintInTiles, Vector2Int.one);

                Debug.Log($"Placing structure: {structure.name}, Size : {size}");

                bool placed = false;

                foreach (Vector2Int tile in walkableTiles)
                {
                    if (CanPlaceStructure(tile, size, walkableTiles, occupiedTiles, structure.interiorOnlyPlacement))
                    {
                        PlaceStructure(structure, tile);

                        for (int x = 0; x < size.x; x++)
                        {
                            for (int y = 0; y < size.y; y++)
                            {
                                occupiedTiles.Add(tile + new Vector2Int(x, y));
                            }
                        }

                        placed = true;
                        break;
                    }
                }

                if (!placed)
                {
                    Debug.LogWarning($"Could not place structure: {structure.name}");
                }
            }
        }

        private static bool CanPlaceStructure(
            Vector2Int origin,
            Vector2Int size,
            HashSet<Vector2Int> walkableTiles,
            HashSet<Vector2Int> occupiedTiles,
            bool interiorOnly)
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector2Int check = origin + new Vector2Int(x, y);

                    if (!walkableTiles.Contains(check)) return false;
                    if (occupiedTiles.Contains(check)) return false;

                    if (interiorOnly)
                    {
                        foreach (Vector2Int dir in Direction2D.cardinalDirectionsList)
                        {
                            if (!walkableTiles.Contains(check + dir))
                                return false;
                        }
                    }
                }
            }

            return true;
        }

        private void PlaceStructure(StructureLayout layout, Vector2Int origin)
        {
            Vector3 worldPos = tilemapVisualizer.GetWorldPosition(origin);
            Instantiate(layout.prefab, worldPos, Quaternion.identity, structureParent);
        }
    }
}
