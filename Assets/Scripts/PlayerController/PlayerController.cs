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
    
    [Header("Settings")]
    [SerializeField] private FloatVariable movementSpeed;
    
    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 moveDir = input.Direction;
        rb.linearVelocity = moveDir * movementSpeed.Value;
    }
}
