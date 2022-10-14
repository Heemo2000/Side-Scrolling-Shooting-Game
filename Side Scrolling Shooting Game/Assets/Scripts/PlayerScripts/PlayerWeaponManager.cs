using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
public class PlayerWeaponManager : MonoBehaviour
{
    [SerializeField]private Gun gun;
    [SerializeField]private LayerMask ignoreMask;

    [SerializeField]private CinemachineImpulseSource shootImpulse;

    [SerializeField]private PlayerMovement player;
    private bool _firePressed = false;
    
    private Coroutine _bulletDataCoroutine;

    private void Start() 
    {
        gun.OnBulletShoot.AddListener(GenerateFireShake);
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

    public void ChangeBulletData(CommonBulletData bulletData)
    {
        if(_bulletDataCoroutine == null)
        {
            _bulletDataCoroutine = StartCoroutine(gun.SetBulletDataCoroutine(bulletData));
        }
    }
    private void OnDestroy() 
    {
        gun.OnBulletShoot.RemoveAllListeners();    
    }

    private void GenerateFireShake()
    {
        Vector3 fireShakeVelocity = Vector3.forward * Random.Range(0.0f,1.0f);
        shootImpulse.GenerateImpulse(fireShakeVelocity);
    }
}
