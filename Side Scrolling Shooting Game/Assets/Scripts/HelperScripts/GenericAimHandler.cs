using UnityEngine;
using UnityEngine.Animations.Rigging;

public class GenericAimHandler : MonoBehaviour
{

    [Range(1f,100.0f)]
    [SerializeField]private float aimingSpeed = 50f;
    [SerializeField]private Transform aimPointerPrefab;
    [SerializeField]private RigBuilder rigBuilder;

    [SerializeField]private MultiAimConstraint[] aimIKs;
    
    private GameObject _aimPointerPrimitive;
    private Vector3 _targetAimPosition;

    public Vector3 AimPosition
    {
        get => _aimPointerPrimitive.transform.position;
    }
    private void Awake() 
    {

        if(aimPointerPrefab == null)
        {
            _aimPointerPrimitive = new GameObject("Aim pointer");
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

    private void LateUpdate() 
    {
        _aimPointerPrimitive.transform.position = Vector3.Lerp(_aimPointerPrimitive.transform.position,
                                                               _targetAimPosition,aimingSpeed * Time.deltaTime);
    }
    public void SetAimPosition(Vector3 aimPosition)
    {
        _targetAimPosition = aimPosition;
    }
}
