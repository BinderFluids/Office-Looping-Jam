using System.Collections.Generic;
using UnityEngine;
using LevelGeneration;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private List<StructureLayout> structures = new List<StructureLayout>();
    [SerializeField] private int structuresToSpawn = 3;

    private void Start()
    {
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        if (structures.Count == 0)
        {
            Debug.LogWarning("No structures assigned!");
            return;
        }

        for (int i = 0; i < structuresToSpawn; i++)
        {
            // Pick a random structure
            StructureLayout layout = structures[Random.Range(0, structures.Count)];

            if (layout.prefab != null)
            {
                Vector3 spawnPosition = new Vector3(
                    Mathf.Round(Random.Range(-15f, 15f)),
                    Mathf.Round(Random.Range(-15f, 15f)),
                    0
                );
                Instantiate(layout.prefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}