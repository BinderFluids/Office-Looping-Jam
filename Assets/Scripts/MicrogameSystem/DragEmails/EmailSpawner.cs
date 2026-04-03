using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MicrogameSystem.DragEmails
{
    public class EmailSpawner : MicrogameBehaviour<DragEmailsContext>
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private int emailsToSpawn = 10;
        [SerializeField] private float spawnRange; 
        [SerializeField] EmailItem emailPrefab;
        
        public override void OnMicrogameStart()
        {
            for (int i = 0; i < emailsToSpawn; i++)
            {
                Vector2 spawnPosition = Random.insideUnitCircle * spawnRange + _transform.position.ToVector2();
                
                EmailItem newEmail = Instantiate(emailPrefab, spawnPosition, Quaternion.identity);
                newEmail.Init();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue; 
            Gizmos.DrawWireSphere(_transform.position, spawnRange);
        }
    }
}