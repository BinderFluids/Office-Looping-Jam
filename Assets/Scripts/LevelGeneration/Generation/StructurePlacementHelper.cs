using System.Collections.Generic;
using UnityEngine;

namespace LevelGeneration.Generation
{
    public class StructurePlacementHelper : MonoBehaviour
    {
        [SerializeField] private List<StructureLayout> structures = new List<StructureLayout>();
        [SerializeField] private int structuresToSpawn = 3;
    }
}