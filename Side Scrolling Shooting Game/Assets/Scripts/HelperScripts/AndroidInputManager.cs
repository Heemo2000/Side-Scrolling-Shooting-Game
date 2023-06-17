using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;
public class AndroidInputManager : MonoBehaviour
{
    [SerializeField]private Vector2 joystickSize = new Vector2(50,50);
    private InputStick _stick;
    public InputStick Stick{ set => _stick = value; }
    public Vector2 MovementInput { get => _movementInput;}
    private ETouch.Finger _movementFinger;
    private Vector2 _movementInput;

    // Start is called before the first frame update
    void Start()
    {
        ETouch.EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += OnFingerDown;
        ETouch.Touch.onFingerUp += OnFingerUp;
        ETouch.Touch.onFingerMove += OnFingerMove;
        _stick.RectTransform.sizeDelta = joystickSize;
    }

    private void OnDestroy()
    {
        ETouch.EnhancedTouchSupport.Disable();
        ETouch.Touch.onFingerDown -= OnFingerDown;
        ETouch.Touch.onFingerUp -= OnFingerUp;
        ETouch.Touch.onFingerMove -= OnFingerMove;
    }
    

    private void OnFingerDown(ETouch.Finger finger)
    {
        if(_movementFinger == null)
        {
            _movementFinger = finger;
            _movementInput = Vector2.zero;
        }    
    }

    private void OnFingerUp(ETouch.Finger finger)
    {
        if(_movementFinger == finger)
        {
            _movementFinger = null;
        }
    }

    private void OnFingerMove(ETouch.Finger finger)
    {
        if(_movementFinger == finger)
        {
            Vector2 knobPosition;
            float maxMovement = joystickSize.x/2f;
            ETouch.Touch currentTouch = finger.currentTouch;

            if(Vector2.SqrMagnitude(currentTouch.screenPosition - _stick.RectTransform.anchoredPosition) > maxMovement * maxMovement)
            {
                knobPosition = (currentTouch.screenPosition - _stick.RectTransform.anchoredPosition).normalized * maxMovement;
                //_movementInput = knobPosition;
            }
            else
            {
                knobPosition = currentTouch.screenPosition;
                
            }
            _stick.Knob.anchoredPosition = knobPosition;
            _movementInput = (currentTouch.screenPosition - _stick.RectTransform.anchoredPosition).normalized;
        }
    }

}


