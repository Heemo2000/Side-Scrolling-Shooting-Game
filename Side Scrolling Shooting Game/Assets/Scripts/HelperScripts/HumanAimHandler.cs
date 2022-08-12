using UnityEngine;
using UnityEngine.Animations.Rigging;

public class HumanAimHandler : MonoBehaviour
{
    [SerializeField]private Transform aimPointerPrefab;
    [SerializeField]private RigBuilder rigBuilder;
    [SerializeField]private MultiAimConstraint headIK;
    [SerializeField]private MultiAimConstraint spineIK;

    [SerializeField]private MultiAimConstraint rightHandIK;
    private PlayerMovement _movement;

    GameObject _aimPointerPrimitive;
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

    public void SetAimPosition(Vector3 aimPosition)
    {
        _aimPointerPrimitive.transform.position = aimPosition;
    }
}
