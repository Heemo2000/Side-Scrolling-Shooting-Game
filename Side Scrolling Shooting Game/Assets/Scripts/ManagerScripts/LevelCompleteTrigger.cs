using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteTrigger : MonoBehaviour
{
    private bool _triggered = false;
    private void OnTriggerEnter(Collider other) 
    {
        Debug.Log("Level trigger triggered.");
        if(!_triggered)
        {
            Player player = other.transform.GetComponent<Player>();
            if(player != null)
            {
                GameManager.Instance.OnLevelComplete?.Invoke();
                _triggered = true;    
            }
        }
    }
    
}
