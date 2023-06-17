using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InputStick : MonoBehaviour
{
    [SerializeField]private RectTransform knob;

    private RectTransform _rectTransform;

    public RectTransform RectTransform { get => _rectTransform; set => _rectTransform = value; }

    public RectTransform Knob { get => knob; set => knob = value; }
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }
    
}
