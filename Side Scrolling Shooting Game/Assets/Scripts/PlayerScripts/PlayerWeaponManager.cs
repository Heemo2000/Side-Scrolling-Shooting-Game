using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
public class PlayerWeaponManager : MonoBehaviour
{
    [SerializeField]private Gun gun;
    [SerializeField]private LayerMask ignoreMask;

    [SerializeField]private CinemachineImpulseSource shootImpulse;
    private bool _firePressed = false;
    

    private void Start() 
    {
        gun.OnFire += GenerateFireShake;
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

    private void OnDestroy() 
    {
        gun.OnFire -= GenerateFireShake;    
    }

    private void GenerateFireShake()
    {
        Vector3 fireShakeVelocity = Vector3.forward * Random.Range(0.0f,1.0f);
        shootImpulse.GenerateImpulse(fireShakeVelocity);
    }
}
