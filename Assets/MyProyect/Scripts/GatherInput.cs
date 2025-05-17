using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class GatherInput : MonoBehaviour
{ 
    private Controls controls; 
    [FormerlySerializedAs("_valueX")] [SerializeField]  private Vector2 _value;
  public Vector2 Value
  {
      get => _value;
  }

  [SerializeField] private bool _isJumping;

  public bool IsJumping
  {
      get => _isJumping;
      set => _isJumping = value;
  }
  void Awake()
    {
        controls = new Controls();
    }

    private void OnEnable()
    {
        controls.Player.Move.performed += StartMove;
        controls.Player.Move.canceled += StopMove;
        controls.Player.Jump.performed += StartJump;
        controls.Player.Jump.canceled += StopJump;
        controls.Player.Enable();
    }

    private void StartMove(InputAction.CallbackContext context)
    {
        _value = context.ReadValue<Vector2>().normalized;
    }

    private void StopMove(InputAction.CallbackContext context)
    {
        _value = Vector2.zero;
    }
    
    private void StartJump(InputAction.CallbackContext context)
    {
        _isJumping = true;
    }

    private void StopJump(InputAction.CallbackContext context)
    {
        _isJumping = false;
    }
    
    private void OnDisable()
    {
        controls.Player.Move.performed -= StartMove;
        controls.Player.Move.canceled -= StopMove;
        controls.Player.Jump.performed -= StartJump;
        controls.Player.Jump.canceled -= StopJump;
        controls.Player.Disable();
    }
}
