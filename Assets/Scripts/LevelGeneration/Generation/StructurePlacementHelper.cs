using System.Collections.Generic;
using LevelGeneration;
using UnityEngine;
using UnityUtils;

namespace LevelGeneration.Generation
{
    public class StructurePlacementHelper : MonoBehaviour
    {
        [SerializeField] private List<StructureLayout> structures = new List<StructureLayout>();
        [SerializeField] private TilemapVisualizer tilemapVisualizer;
        [SerializeField] private Transform structureParent;

        /// <summary>
        /// Spawns prefabs for structures, we will randomly select walkable tiles and then first place the minimums of all structures
        /// and then continually go down the list picking 25 random tiles to attempt to spawn one, then movoe onto the next structure
        /// We do this until we have unsuccessfully placed 150 times in a row
        /// </summary>
        public void PlaceStructures(HashSet<Vector2Int> walkableTiles)
        {
            if (walkableTiles == null || walkableTiles.Count == 0 || structures.Count == 0)
                return;

            HashSet<Vector2Int> occupiedTiles = new HashSet<Vector2Int>();
            Dictionary<StructureLayout, int> placedCounts = new Dictionary<StructureLayout, int>();

            foreach (StructureLayout layout in structures)
            {
                if (layout != null && !placedCounts.ContainsKey(layout))
                    placedCounts.Add(layout, 0);
            }

            foreach (StructureLayout layout in structures)
            {
                if (layout == null)
                    continue;

                int minimumCount = Mathf.Max(0, layout.placementRules?.minCount ?? 0);
                while (placedCounts[layout] < minimumCount)
                {
                    if (TryPlaceStructure(layout, walkableTiles, occupiedTiles, placedCounts, 250))
                        continue;

                    Debug.LogWarning($"Could not place minimum count for structure '{layout.name}'.");
                    break;
                }
            }

            int consecutiveFailures = 0;
            while (consecutiveFailures < 1500)
            {
                bool canStillPlaceAnything = false;

                foreach (StructureLayout layout in structures)
                {
                    if (layout == null)
                        continue;

                    int maxCount = Mathf.Max(0, layout.placementRules?.maxCount ?? int.MaxValue);
                    if (placedCounts[layout] >= maxCount)
                        continue;

                    canStillPlaceAnything = true;

                    if (TryPlaceStructure(layout, walkableTiles, occupiedTiles, placedCounts, 25))
                        consecutiveFailures = 0;
                    else
                        consecutiveFailures++;

                    if (consecutiveFailures >= 150)
                    {
                        Debug.LogWarning("Stopped structure placement due to too many consecutive failures.");
                        break;
                    }
                }

                if (!canStillPlaceAnything)
                    break;
            }
        }

        private Vector2Int PickRandomTile(HashSet<Vector2Int> walkableTiles)
        {
            return walkableTiles.Random();
        }

        private static bool CanPlaceStructure(
            Vector2Int origin,
            Vector2Int size,
            HashSet<Vector2Int> walkableTiles,
            HashSet<Vector2Int> occupiedTiles,
            bool interiorOnly,
            bool avoidOverlap)
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector2Int check = origin + new Vector2Int(x, y);

                    if (!walkableTiles.Contains(check)) return false;
                    if (avoidOverlap && occupiedTiles.Contains(check)) return false;

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

        private bool TryPlaceStructure(
            StructureLayout layout,
            HashSet<Vector2Int> walkableTiles,
            HashSet<Vector2Int> occupiedTiles,
            Dictionary<StructureLayout, int> placedCounts,
            int attempts)
        {
            if (layout.prefab == null || walkableTiles.Count == 0)
                return false;

            int maxCount = Mathf.Max(0, layout.placementRules?.maxCount ?? int.MaxValue);
            if (placedCounts[layout] >= maxCount)
                return false;

            Vector2Int size = GetFootprint(layout);
            bool avoidOverlap = layout.placementRules?.avoidOverlap ?? true;

            for (int attempt = 0; attempt < attempts; attempt++)
            {
                Vector2Int origin = PickRandomTile(walkableTiles);
                if (!CanPlaceStructure(origin, size, walkableTiles, occupiedTiles, layout.interiorOnlyPlacement, avoidOverlap))
                    continue;

                PlaceStructure(layout, origin, walkableTiles, occupiedTiles, size);
                placedCounts[layout]++;
                return true;
            }

            return false;
        }

        private static Vector2Int GetFootprint(StructureLayout layout)
        {
            return new Vector2Int(
                Mathf.Max(1, layout.footprintInTiles.x),
                Mathf.Max(1, layout.footprintInTiles.y));
        }

        private static bool IsInsideFootprint(Vector2Int tile, Vector2Int origin, Vector2Int size)
        {
            return tile.x >= origin.x
                && tile.x < origin.x + size.x
                && tile.y >= origin.y
                && tile.y < origin.y + size.y;
        }

        private static bool TryGetNearbySpawnTile(
            Vector2Int origin,
            Vector2Int size,
            HashSet<Vector2Int> walkableTiles,
            HashSet<Vector2Int> occupiedTiles,
            out Vector2Int spawnTile)
        {
            const int maxSearchRadius = 4;
            List<Vector2Int> candidates = new List<Vector2Int>();

            for (int radius = 1; radius <= maxSearchRadius; radius++)
            {
                candidates.Clear();

                int minX = origin.x - radius;
                int maxX = origin.x + size.x - 1 + radius;
                int minY = origin.y - radius;
                int maxY = origin.y + size.y - 1 + radius;

                for (int x = minX; x <= maxX; x++)
                {
                    for (int y = minY; y <= maxY; y++)
                    {
                        bool isPerimeter = x == minX || x == maxX || y == minY || y == maxY;
                        if (!isPerimeter)
                            continue;

                        Vector2Int candidate = new Vector2Int(x, y);
                        if (IsInsideFootprint(candidate, origin, size))
                            continue;
                        if (!walkableTiles.Contains(candidate))
                            continue;
                        if (occupiedTiles.Contains(candidate))
                            continue;

                        candidates.Add(candidate);
                    }
                }

                if (candidates.Count > 0)
                {
                    spawnTile = candidates[Random.Range(0, candidates.Count)];
                    return true;
                }
            }

            spawnTile = default;
            return false;
        }

        private void PlaceStructure(
            StructureLayout layout,
            Vector2Int origin,
            HashSet<Vector2Int> walkableTiles,
            HashSet<Vector2Int> occupiedTiles,
            Vector2Int size)
        {
            Vector3 worldPos = tilemapVisualizer.GetWorldPosition(origin);
            Instantiate(layout.prefab, worldPos, Quaternion.identity, structureParent);

            if (layout.spawnOnPlacement != null
                && TryGetNearbySpawnTile(origin, size, walkableTiles, occupiedTiles, out Vector2Int spawnTile))
            {
                Vector3 spawnWorldPos = tilemapVisualizer.GetCellCenterWorld(spawnTile);
                Instantiate(layout.spawnOnPlacement, spawnWorldPos, Quaternion.identity, structureParent);
            }

            // Mark all tiles in the footprint as occupied
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    occupiedTiles.Add(origin + new Vector2Int(x, y));
                }
            }
        }
    }
}
