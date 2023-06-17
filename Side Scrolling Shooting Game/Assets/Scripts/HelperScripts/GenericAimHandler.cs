using UnityEngine;
using UnityEngine.Animations.Rigging;

public class GenericAimHandler : MonoBehaviour
{
    private const float MaxAimCircleSize = 2f;
    private const float AimCheckForwardOffSet = 2.5f;
    [Range(1f,100.0f)]
    [SerializeField]private float aimingSpeed = 50f;
    [SerializeField]private Transform aimPointerPrefab;
    [SerializeField]private RigBuilder rigBuilder;

    [SerializeField]private MultiAimConstraint[] aimIKs;

    [SerializeField]private float aimCheckRadius = 2f;

    [SerializeField]private Camera lookCamera;
    [SerializeField]private float aimCheckMaxDistance = 10f;
    [SerializeField]private LayerMask aimMask;

    [SerializeField]private bool showAimPoint = false;
    
    private GameObject _aimPointerPrimitive;
    private Vector3 _targetAimPosition;

    private Vector3 _aimDirection;
    private Plane _plane;

    public Vector3 AimPosition
    {
        get => _aimPointerPrimitive.transform.position;
    }

    public Camera LookCamera
    {
        get => lookCamera; set => lookCamera = value;
    }
    private void Awake() 
    {
        _plane = new Plane(Vector3.forward,transform.position);
        if(aimPointerPrefab == null)
        {
            _aimPointerPrimitive = GameObject.CreatePrimitive(PrimitiveType.Sphere);
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
        HandleAimPosition();
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

    public void SetAimDirection(Vector3 aimDirection)
    {
        _aimDirection = aimDirection;
    }

    private void HandleAimPosition()
    {
        
        _plane.SetNormalAndPosition(Vector3.forward,transform.position);
        
        Vector3 centreOfScreen = new Vector3(Screen.currentResolution.height/2, Screen.currentResolution.width/2);
        Vector3 tempPosition = centreOfScreen + new Vector3(_aimDirection.x, _aimDirection.y) * MaxAimCircleSize;
        Ray ray = lookCamera.ScreenPointToRay(tempPosition);

        Vector3 aimPos = centreOfScreen;
        if(_plane.Raycast(ray,out float distance))
        {
            aimPos = ray.origin + ray.direction * distance;            
            if(Physics.SphereCast(aimPos - Vector3.forward * AimCheckForwardOffSet,aimCheckRadius,Vector3.forward,out RaycastHit hit,aimCheckMaxDistance,aimMask.value))
            {
                aimPos = hit.transform.position;
            }
        }
        
        _targetAimPosition = aimPos;
    }

    private void OnDestroy() {
        Destroy(_aimPointerPrimitive);
    }
}
