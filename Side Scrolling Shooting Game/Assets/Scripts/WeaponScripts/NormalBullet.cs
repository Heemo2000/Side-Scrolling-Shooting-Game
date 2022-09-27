using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : Bullet
{
    
    private void FixedUpdate() 
    {
        base.BulletRB.AddForce(transform.forward * base.BulletData.moveSpeed,ForceMode.Impulse);
    }
}
