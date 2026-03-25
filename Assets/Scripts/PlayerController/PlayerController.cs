using System;
using ScriptableVariables;
using UnityEngine;
using UnityUtils;

public class PlayerController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private InputReader input;
    [SerializeField] private Transform _transform; 
    
    [Header("Physics")]
    [SerializeField] private BoxCollider2D col;
    [SerializeField] private LayerMask wallLayer;

    [Header("Settings")]
    [SerializeField] private FloatVariable movementSpeed;
    
    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 moveDir = input.Direction;
        if (IsPositionInWall(_transform.position + moveDir.ToVector3())) return; 
        
        transform.Translate( movementSpeed.Value * Time.deltaTime * moveDir);
    }

    private bool IsPositionInWall(Vector2 pos) =>
        Physics2D.OverlapBox(pos, col.size, 0f, wallLayer); 
}
