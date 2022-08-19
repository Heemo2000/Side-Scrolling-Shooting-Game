using UnityEngine;
using UnityEngine.UI;

public class BarsUI : MonoBehaviour
{
    [SerializeField]private Image fillImage;
    [SerializeField]private Vector3 offset;
    
    private Transform _followTarget;

    public Transform FollowTarget { get => _followTarget; set => _followTarget = value; }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        transform.position = _followTarget.position + offset;
    }

    public void SetFillAmount(float currentValue,float maxValue)
    {
        fillImage.fillAmount = currentValue/maxValue;
    }
}
