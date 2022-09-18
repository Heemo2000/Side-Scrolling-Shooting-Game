using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [Range(10f,100f)]
    [SerializeField]private float rotateSpeed = 10f;

    [SerializeField]private Transform powerupGraphic;

    protected virtual void FixedUpdate() 
    {
        powerupGraphic.Rotate(Vector3.up * -rotateSpeed * Time.fixedDeltaTime);    
    }
}
