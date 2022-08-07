using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponManager : MonoBehaviour
{
    [SerializeField]private Gun gun;
    [SerializeField]private LayerMask ignoreMask;
    private bool _firePressed = false;

    private void Start() 
    {
        
    }
    private void Update() {
        if(_firePressed)
        {
            FireWeapon();
        }
    }
    public void OnShootInput(InputAction.CallbackContext context)
    {
        float input = context.ReadValue<float>();
        _firePressed = input == 1;
    }

    private void FireWeapon()
    {
        gun.Fire();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) 
    {
        BulletPowerup powerup = hit.gameObject.GetComponent<BulletPowerup>();
        if(powerup != null)
        {
            gun.SetBulletPrefab(powerup.GetBulletPrefab());
        }

        int hitLayerMaskValue = 1 << hit.gameObject.layer;
        if((hitLayerMaskValue & ignoreMask.value) == 0)
        {
            Destroy(hit.gameObject);
        }
            
    }
}
