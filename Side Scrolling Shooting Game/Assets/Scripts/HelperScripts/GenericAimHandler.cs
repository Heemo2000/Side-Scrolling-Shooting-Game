using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
public class GenericAimHandler : MonoBehaviour
{
    
    [Range(1f,100.0f)]
    [SerializeField]private float aimingSpeed = 50f;
    [SerializeField]private Transform aimPointerPrefab;
    [SerializeField]private RigBuilder rigBuilder;

    [SerializeField]private MultiAimConstraint[] aimIKs;
    [SerializeField]private bool showAimPoint = true;
    private GameObject _aimPointerPrimitive;
    private Vector3 _targetAimPosition;

    private Vector3 _aimDirection;

    public Vector3 AimPosition
    {
        get => _aimPointerPrimitive.transform.position;
    }

    public void OnAimInput(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        _aimDirection = input;
    }

    private void Start() 
    {
        if(aimPointerPrefab == null)
        {
            _aimPointerPrimitive = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            _aimPointerPrimitive.GetComponent<SphereCollider>().enabled = false;
            _aimPointerPrimitive.GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            _aimPointerPrimitive = Instantiate(aimPointerPrefab.gameObject);
        }

        _aimPointerPrimitive.transform.position = transform.position;
        _aimPointerPrimitive.transform.rotation = Quaternion.identity;

        foreach(MultiAimConstraint constraint in aimIKs)
        {
            var sourceObjects = constraint.data.sourceObjects;
            sourceObjects.SetTransform(0,_aimPointerPrimitive.transform);
            constraint.data.sourceObjects = sourceObjects;
        }

        rigBuilder.Build();
    }

    private void Update()
    {
        _aimPointerPrimitive.GetComponent<MeshRenderer>().enabled = showAimPoint;
    }

    private void LateUpdate() 
    {
        _aimPointerPrimitive.transform.position = Vector3.Lerp(_aimPointerPrimitive.transform.position,
                                                               _targetAimPosition,aimingSpeed * Time.deltaTime);
    }
    public void SetAimPosition(Vector3 aimPosition)
    {
        _targetAimPosition = aimPosition;
    }

    private void OnDestroy() {
        Destroy(_aimPointerPrimitive);
    }
}
