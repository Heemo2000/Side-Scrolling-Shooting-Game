using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
    [SerializeField]private Gun gun;
    private bool _firePressed = false;
    private void Update() {
        if(_firePressed)
        {
            gun.Fire();
        }
    }
    public void OnShootInput(InputAction.CallbackContext context)
    {
        float input = context.ReadValue<float>();
        Debug.Log("LMB Input : " + input);
        _firePressed = input == 1;
    }
}
