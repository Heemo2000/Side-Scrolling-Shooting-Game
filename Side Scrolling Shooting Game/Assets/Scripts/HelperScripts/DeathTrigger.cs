using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPosition = GameManager.Instance.GetPlayerPosition();
        transform.position = new Vector3(playerPosition.x,transform.position.y,playerPosition.z);
    }


    private void OnTriggerEnter(Collider other) 
    {
        Player player = other.GetComponent<Player>();
        if(player != null)
        {
            player.PlayerHealth.OnDeath?.Invoke();
        }    
    }
}
