using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGenerator : MonoBehaviour
{
    [Min(0.8f)]
    [SerializeField]private float stopInterval = 2.0f;
    [Min(1.0f)]
    [SerializeField]private float fireInterval = 3.0f;
    [SerializeField]private ParticleSystem fire;

    [Min(1f)]
    [SerializeField]private float damage = 5f;

    [Min(1f)]
    [SerializeField]private float healthDamageInterval = 1.0f;

    private bool _isFireTriggerOn;

    private Coroutine _damageCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GenerateFire());
    }

    // Update is called once per frame
    /*
    void Update()
    {
        if(_isFireTriggerOn)
        {
            Debug.Log("Generating Fire.");
        }

        Debug.Log("Is damage coroutine null ? " + (_damageCoroutine == null));
    }
    */
    IEnumerator GenerateFire()
    {
        fire.Stop();
        while(this.enabled)
        {
            if(!_isFireTriggerOn)
            {
                fire.Play();
                _isFireTriggerOn = true;
                yield return new WaitForSeconds(fireInterval);
                
            }
            else
            {
                fire.Stop();
                _isFireTriggerOn = false;
                yield return new WaitForSeconds(stopInterval);
            }
        }

        yield return null;
    }


    IEnumerator DamageHealth(Health health)
    {
        while(health != null)
        {
            if(_isFireTriggerOn)
            {
                Debug.Log("Fire damaging health...");
                health.OnHealthDamaged?.Invoke(damage);
                yield return new WaitForSeconds(healthDamageInterval);
            }
            else
            {
                yield return null;
            }
        }
    }

    private void OnTriggerStay(Collider other) 
    {
        if(!_isFireTriggerOn)
        {
            return;
        }

        Health health = other.GetComponent<Health>();

         if(health != null && _damageCoroutine == null)
         {
            _damageCoroutine = StartCoroutine(DamageHealth(health));
         }
    }

    private void OnTriggerExit(Collider other) {
        
        //Debug.Log("Getting out of fire trigger.");
        if(_damageCoroutine != null)
        {
            StopCoroutine(_damageCoroutine);
            _damageCoroutine = null;
        }
    }
}
