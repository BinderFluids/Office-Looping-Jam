using EventBus;
using UnityEngine;

namespace LevelGeneration
{
    [CreateAssetMenu(menuName = "Procedural/Structure Layout")]
    public class StructureLayout : ScriptableObject
    {
        public GameObject prefab;

        [Tooltip("Width and height in floor tiles used for placement only. If sprites extend past this (pillars, shadows), increase footprint and/or depth below, or use world offset — the game never places into wall cells, but art can overlap them.")]
        public Vector2Int footprintInTiles = new Vector2Int(1, 1);
        
        [Header("Placement Rules")]
        public StructureRule placementRules;

        [Tooltip("If true, every footprint tile must have walkable floor on all four sides (one tile inward from the room edge).")]
        public bool interiorOnlyPlacement = true;

        [Header("Instantiation")]
        public GameObject spawnOnPlacement;
    }
}