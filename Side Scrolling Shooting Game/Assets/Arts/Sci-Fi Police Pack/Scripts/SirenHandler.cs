using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SirenHandler : MonoBehaviour
{
    public float speed; 
    
    // Start is called before the first frame update
    void Start()
    {
        speed = 100;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, speed * Time.deltaTime,0);
    }
}
