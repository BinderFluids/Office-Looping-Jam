using System;
using ScriptableVariables;
using UnityEngine;
using UnityUtils;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader input;
    [SerializeField] private Transform _transform;
    [SerializeField] private Rigidbody2D rb; 
    [SerializeField] private BoxCollider2D col;

    [Header("Settings")]
    [SerializeField] private FloatVariable movementSpeed;
    
    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 moveDir = input.Direction.normalized;
        rb.linearVelocity = moveDir * movementSpeed.Value;
    }
}
