using UnityEngine;
using UnityEngine.Animations.Rigging;

public class HumanAimHandler : MonoBehaviour
{

    [Range(1f,100.0f)]
    [SerializeField]private float aimingSpeed = 50f;
    [SerializeField]private Transform aimPointerPrefab;
    [SerializeField]private RigBuilder rigBuilder;
    [SerializeField]private MultiAimConstraint headIK;
    [SerializeField]private MultiAimConstraint spineIK;

    [SerializeField]private MultiAimConstraint rightHandIK;
    private PlayerMovement _movement;

    private GameObject _aimPointerPrimitive;
    private Vector3 _targetAimPosition;
    private void Awake() 
    {
        _movement = GetComponent<PlayerMovement>();

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

        var headIKSourceObjects = headIK.data.sourceObjects;
        var spineIKSourceObjects = spineIK.data.sourceObjects;
        var rightHandIKSourceObjects = rightHandIK.data.sourceObjects;

        headIKSourceObjects.SetTransform(0,_aimPointerPrimitive.transform);
        spineIKSourceObjects.SetTransform(0,_aimPointerPrimitive.transform);
        rightHandIKSourceObjects.SetTransform(0,_aimPointerPrimitive.transform);

        headIK.data.sourceObjects = headIKSourceObjects;
        spineIK.data.sourceObjects = spineIKSourceObjects;
        rightHandIK.data.sourceObjects = rightHandIKSourceObjects;

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
